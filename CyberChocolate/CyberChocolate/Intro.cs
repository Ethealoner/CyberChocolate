using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CyberChocolate
{
    class Intro
    {
        private Texture2D evilCake;
        private Game game;
        private float timer = 0;
        private float alpha = 0;
        private float increment = 0.05f;
        private bool finished = false;
        private bool faded = false;
        public Intro (Game game)
        {
            this.game = game;
        }

        public void LoadContent()
        {
            evilCake = game.Content.Load<Texture2D>("MainMenu/Evil_Cake");
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(evilCake,Vector2.Zero,Color.White * alpha);
            spriteBatch.End();
        }
        public void Update(GameTime time)
        {
            timer += (float)time.ElapsedGameTime.TotalSeconds;
            Fading();

        }
        public void Fading()
        {
            if(timer > 0.1f)
            {
                alpha += increment;
                timer = 0;
                if (alpha > 1)
                {
                    increment *= -1;
                    faded = true;
                }
                if (faded && alpha < 0)
                {
                    finished = true;
                }
            }
            
        }
        public bool IntroFinished()
        {
            return finished;
        }
    }
}
