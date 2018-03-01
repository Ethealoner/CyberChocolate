using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CyberChocolate
{
    class ParticleEngine
    {
        private Random random;
        private List<Particle> particles;
        private List<Particle> fireParticles;
        private List<Texture2D> textures;
        private List<Texture2D> fireTextures;
        private int maxParticles;

        public ParticleEngine(List<Texture2D> textures,List<Texture2D> fireTextures)
        {
            this.textures = textures;
            this.fireTextures = fireTextures;
            particles = new List<Particle>();
            fireParticles = new List<Particle>();
            random = new Random();
            maxParticles = 20;
        }
        private Particle GenerateNewParticle(Vector2 emiterLocation)
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = emiterLocation;
            Vector2 velocity = new Vector2(1f * (float)(random.NextDouble() * 5 - 1),
                                           1f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            Color color = Color.White;
            float size = 0.2f;
            int lifeTime = 20 + random.Next(40);
            return new Particle(texture, position, velocity, color, angle, angularVelocity, size, lifeTime);
        }
        private Particle GenerateNewFireParticle(Vector2 emiterLocation)
        {
            Texture2D texture = fireTextures[random.Next(fireTextures.Count)];
            Vector2 position = emiterLocation + new Vector2(random.Next(-20,20),random.Next(-20,20));
            Vector2 velocity = new Vector2(1f * (float)(random.NextDouble() * 4 - 1),
                                           1f * (float)(random.NextDouble() * 1 - 2));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            Color color = Color.White;
            float size = 0.2f;
            int lifeTime = 20 + random.Next(40);
            
            return new Particle(texture, position, velocity, color, angle, angularVelocity, size, lifeTime);
        }
        public void GenerateParticles(Vector2 location)
        {
            for(int i = 0; i < maxParticles; i++)
            {
                particles.Add(GenerateNewParticle(location));
            }
        }
        public void GenerateFireParticles(Vector2 location)
        {
            for(int i = 0; i < maxParticles; i++)
            {
                fireParticles.Add(GenerateNewFireParticle(location));
            }
        }
        public void Update()
        {           

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].lifeTime <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
            for(int particle = 0; particle < fireParticles.Count; particle++)
            {
                fireParticles[particle].Update();
                if(fireParticles[particle].lifeTime <= 0)
                {
                    fireParticles.RemoveAt(particle);
                    particle--;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
            foreach(Particle fireParticle in fireParticles)
            {
                fireParticle.Draw(spriteBatch);
            }
        }
        public void Clear()
        {
            textures.Clear();
            fireTextures.Clear();
        }
    }
}
