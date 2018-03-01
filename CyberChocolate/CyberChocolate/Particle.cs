using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CyberChocolate
{
    class Particle
    {
        private Texture2D texture;
        private Vector2 position;
        private Vector2 velocity;
        private Color color;
        private float angle;
        private float angularVelocity;
        private float size;
        public int lifeTime;

        public Particle(Texture2D texture,Vector2 position,Vector2 velocity, Color color, float angle,float angularVelocity,float size,int lifeTime)
        {
            this.texture = texture;
            this.position = position;
            this.velocity = velocity;
            this.color = color;
            this.angle = angle;
            this.angularVelocity = angularVelocity;
            this.size = size;
            this.lifeTime = lifeTime;
        }

        public void Update()
        {
            lifeTime--;
            position += velocity;
            angle += angularVelocity;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            spriteBatch.Draw(texture, position, sourceRectangle, color,
        angle, origin, size, SpriteEffects.None, 0f);
        }
    }
}
