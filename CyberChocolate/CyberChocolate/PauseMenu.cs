using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CyberChocolate
{
    class PauseMenu
    {
        protected Texture2D returnToMenu;
        protected Texture2D returnToMenuActive;
        protected Texture2D resumeGame;
        protected Texture2D resumeGameActive;
        protected Texture2D background;
        protected Rectangle returnToMenuRectangle;
        protected Rectangle resumeGameRectangle;
        protected Game game;
        private MouseState mouseState;
        private Point mousePosition;

        public PauseMenu(Game game)
        {
            this.game = game;
        }

        public virtual void LoadContent()
        {
            returnToMenu = game.Content.Load<Texture2D>("Pause/returnToMenu");
            returnToMenuActive = game.Content.Load<Texture2D>("Pause/returnToMenuActive");
            resumeGame = game.Content.Load<Texture2D>("Pause/resumeGame");
            resumeGameActive = game.Content.Load<Texture2D>("Pause/resumeGameActive");
            background = game.Content.Load<Texture2D>("Pause/Pause");
            returnToMenuRectangle = returnToMenu.Bounds;
            resumeGameRectangle = resumeGame.Bounds;
            returnToMenuRectangle.Location = new Point(200,300);
            resumeGameRectangle.Location = new Point(400, 300);
        }
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Begin();
            spritebatch.Draw(background, Vector2.Zero, Color.White);
            if (isHover(mousePosition, returnToMenuRectangle))
            {
                spritebatch.Draw(returnToMenuActive, returnToMenuRectangle.Location.ToVector2(), Color.White);
            }
            else
            {
                spritebatch.Draw(returnToMenu, returnToMenuRectangle.Location.ToVector2(), Color.White);
            }

            if (isHover(mousePosition, resumeGameRectangle))
            {
                spritebatch.Draw(resumeGameActive, resumeGameRectangle.Location.ToVector2(), Color.White);
            }
            else
            {
                spritebatch.Draw(resumeGame, resumeGameRectangle.Location.ToVector2(), Color.White);
            }
            spritebatch.End();
        }
        public void Update()
        {
            mouseState = Mouse.GetState();
            mousePosition = mouseState.Position;

        }
        public bool isHover(Point mousePosition, Rectangle buttonPosition)
        {
            if (buttonPosition.Contains(mousePosition))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ReturnToMenuClicked()
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (isHover(mousePosition, returnToMenuRectangle))
                {
                    return true;
                }
            }
            return false;
        }
        public bool ResumeGameClicked()
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (isHover(mousePosition, resumeGameRectangle))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
