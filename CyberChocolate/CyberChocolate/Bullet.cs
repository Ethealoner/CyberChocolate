using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace CyberChocolate
{
    class Bullet
    {
        public Vector2 position;
        private Texture2D texture;
        private float destination;
        public bool fired;
        public bool facingRight;
        private int direction;
        private int maxDistance = 600;
        private int speed = 250;
        
        public Bullet(Texture2D texture)
        {
            this.texture = texture;
        }
        public void Fire(bool facingRight,Vector2 position,int correctionY)
        {
            this.position = position;
            this.position.Y -= correctionY;
            this.facingRight = facingRight;
            fired = true;
            if (facingRight)
            {
                direction = 2;
                destination = position.X + maxDistance;
            }else
            {
                direction = -2;
                destination = position.X - maxDistance;
            }
        }
        public void Reached()
        {
            if (facingRight)
            {
                if(position.X > destination)
                {
                    fired = false;
                }
            }else
            {
                if(position.X < destination)
                {
                    fired = false;
                }
            }
        }
        public void Draw(SpriteBatch spritebatch,Matrix matrix)
        {
            if (fired)
            {
               // spritebatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null,matrix);
                spritebatch.Draw(texture, position, Color.White);
               // spritebatch.End();
            }
        }
        public void Update(GameTime time)
        {
            if (fired)
            {
                position += new Vector2(direction, 0) * speed * (float)time.ElapsedGameTime.TotalSeconds;
            }
            Reached();
            
        }
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y,
                  texture.Width, texture.Height);
            }
        }
    }
}
