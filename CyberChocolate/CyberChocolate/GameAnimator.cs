using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpriterDotNet.MonoGame;
using SpriterDotNet;
using SpriterDotNet.MonoGame.Content;
using SpriterDotNet.Providers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace CyberChocolate
{
    class GameAnimator : MonoGameAnimator
    {
        private Rectangle rectangle;
        private readonly Texture2D whiteDot;
        private int width, height;
        private Matrix matrix;
        private int cox, coy;
        public Color DebugColor { get; set; } = Color.Red;

        public GameAnimator ( SpriterEntity entity, int width,int height,GraphicsDevice graphicsDevice,IProviderFactory<ISprite, SoundEffect> providerFactory = null) : base(entity, providerFactory)
        {
            whiteDot = TextureUtil.CreateRectangle(graphicsDevice, 1, 1, Color.White);
            this.width = width;
            this.height = height;
            cox = 0;
            coy = 0;
        }

        public void getMatrix(Matrix matrix)
        {
            this.matrix = matrix;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, matrix);          
            base.Draw(spriteBatch);          
            //TextureUtil.DrawRectangle(spriteBatch, getRectangle(), Color.Red, 1);
           // spriteBatch.End();
        }
        public void getCorrection(int cox,int coy)
        {
            this.cox = cox;
            this.coy = coy;
        }

        public Rectangle getRectangle()
        {
            return rectangle = new Rectangle((int)Position.X - cox, (int)Position.Y - coy, (int)(width * Scale.X), (int)(height * Scale.Y));                     
        }
        public void DrawRectangle(SpriteBatch spriteBatch,Rectangle rec)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, matrix);
            TextureUtil.DrawRectangle(spriteBatch, rec, Color.Blue, 1);
            spriteBatch.End();
        }
    }
}
