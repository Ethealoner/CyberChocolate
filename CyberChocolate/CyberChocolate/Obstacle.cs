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
    class Obstacle
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { set; get; }
        public bool isTaken;
        private static Texture2D rect;
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y,
                  Texture.Width, Texture.Height);
            }
        }
        public Obstacle(Vector2 position, Texture2D texture)
        {
             Texture = texture;
             Position = position;
            isTaken = false;
        }       
        private void DrawRectangle(Rectangle coords, Color color, GraphicsDevice device, SpriteBatch spriteBatch)
        {
            if (rect == null)
            {
                rect = new Texture2D(device, 1, 1);
                rect.SetData(new[] { Color.White });
            }
            spriteBatch.Draw(rect, coords, color);
        }
        public  void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null,
        Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.7f);
           // spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
