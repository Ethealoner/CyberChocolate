using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace CyberChocolate
{
    class HpPotion : DrawableGameComponent
    {
        private Texture2D texture;
        private Vector2 position;
        public bool isTaken;
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y,
                  texture.Width, texture.Height);
            }
        }
        public HpPotion(Game game,Vector2 position) : base(game)
        {
            this.position = position;
            isTaken = false;
        }
        public void Load()
        {
            texture = Game.Content.Load<Texture2D>("Obstacles/CyberChocolate");

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White,
        40, Vector2.Zero, 0.2f, SpriteEffects.None, 0f);
        }
    }
}
