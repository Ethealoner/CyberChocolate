using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace CyberChocolate
{
    class MainMenu
    {
        private Texture2D startGame;
        private Texture2D startGameActive;
        private Texture2D endGame;
        private Texture2D endGameActive;
        private Texture2D background;
        private Rectangle buttonStartGame;
        private Rectangle buttonEndGame;
        private Game game;
        private MouseState mouseState;
        private Point mousePosition;
        private Vector2 startButtonPosition;
        private Vector2 endButtonPosition;

        public MainMenu(Game game)
        {
            this.game = game;           
            startButtonPosition = new Vector2(320, 220);
            endButtonPosition = new Vector2(320, 400);

        }

        public void LoadContent()
        {
            startGame = game.Content.Load<Texture2D>("MainMenu/startGame");
            startGameActive = game.Content.Load<Texture2D>("MainMenu/startGameActive");
            endGame = game.Content.Load<Texture2D>("MainMenu/exitGame");
            endGameActive = game.Content.Load<Texture2D>("MainMenu/exitGameActive");
            background = game.Content.Load<Texture2D>("MainMenu/MainMenu");
            buttonStartGame = startGame.Bounds;
            buttonEndGame = endGame.Bounds;
            buttonStartGame.Location = startButtonPosition.ToPoint();
            buttonEndGame.Location = endButtonPosition.ToPoint();

        }
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Begin();
            spritebatch.Draw(background, Vector2.Zero, Color.White);
            if(isHover(mousePosition, buttonStartGame))
            {
                spritebatch.Draw(startGameActive, startButtonPosition, Color.White);
            }else
            {
                spritebatch.Draw(startGame, startButtonPosition, Color.White);
            }

            if (isHover(mousePosition, buttonEndGame))
            {
                spritebatch.Draw(endGameActive, endButtonPosition, Color.White);
            }
            else
            {
                spritebatch.Draw(endGame, endButtonPosition, Color.White);
            }
            spritebatch.End();
        }
        public void Update()
        {
            mouseState = Mouse.GetState();
            mousePosition = mouseState.Position;

        }
        public bool isHover(Point mousePosition,Rectangle buttonPosition)
        {
            if (buttonPosition.Contains(mousePosition))
            {
                return true;
            }else
            {
                return false;
            }
        }
        public bool StartGameClicked()
        {
            if(mouseState.LeftButton == ButtonState.Pressed)
            {
                if(isHover(mousePosition, buttonStartGame))
                {
                    return true;
                }
            }
            return false;
        }
        public bool EndGameClicked()
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (isHover(mousePosition, buttonEndGame))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
