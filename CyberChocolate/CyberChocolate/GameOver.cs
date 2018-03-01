using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CyberChocolate
{
    class GameOver : PauseMenu
    {
        public GameOver(Game game) : base(game)
        {

        }

        public override void LoadContent()
        {
            returnToMenu = game.Content.Load<Texture2D>("GameOver/returnToMenu");
            returnToMenuActive = game.Content.Load<Texture2D>("GameOver/returnToMenuActive");
            resumeGame = game.Content.Load<Texture2D>("GameOver/restartGame");
            resumeGameActive = game.Content.Load<Texture2D>("GameOver/restartGameActive");
            background = game.Content.Load<Texture2D>("GameOver/gameover");
            returnToMenuRectangle = returnToMenu.Bounds;
            resumeGameRectangle = resumeGame.Bounds;
            returnToMenuRectangle.Location = new Point(200, 300);
            resumeGameRectangle.Location = new Point(400, 300);
        }
    }
}
