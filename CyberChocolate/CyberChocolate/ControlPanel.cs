using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace CyberChocolate
{
    class ControlPanel
    {
        private Game game;
        public Texture2D texture;
        public HackingConsole console;
        private Vector2 position;
        private int maxActionDistance;
        public int score;


        public ControlPanel(Game game,Vector2 position, Texture2D texture)
        {
            maxActionDistance = 200;
            score = 0;
            this.game = game;
            console = new HackingConsole(game);
            this.texture = texture;
            this.position = new Vector2(position.X,position.Y - texture.Bounds.Height);
        }
        public void LoadContent()
        {           
            console.Load();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null,
        Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.3f);
            if (console.isBeingHacked)
            {
                console.Draw(spriteBatch,score); 
            }

        }
        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
            if (console.isBeingHacked)
            {
                console.Update(gameTime,playerPosition);
            }
            HackingResult(gameTime);
        }
        public bool IsPlayerCloseHorizontal(Vector2 playerPosition)
        {

            if ((int)position.X > playerPosition.X)
            {
                if ((playerPosition.X >= position.X - maxActionDistance))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else if ((int)position.X < playerPosition.X)
            {
                if ((playerPosition.X <= position.X + maxActionDistance))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        public bool IsPlayerCloseVertical(Vector2 playerPosition)
        {
            if ((int)position.Y > playerPosition.Y)
            {
                if ((playerPosition.Y >= position.Y - maxActionDistance))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else if ((int)position.Y < playerPosition.Y)
            {
                if ((playerPosition.Y <= position.Y + maxActionDistance))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        public bool HackingPanel(Vector2 playerPosition)
        {
            if (IsPlayerCloseHorizontal(playerPosition) && IsPlayerCloseVertical(playerPosition))
            {
                console.isBeingHacked = true;
                console.LoadPanel();
                return true;
            }
            return false;
        }
        public void HackingResult(GameTime time)
        {
            if (console.answerConfirmed)
            {
                if (!console.Confirm())
                {
                    score--;
                }
                console.ResetState(time);
                if (console.pointGiven)
                {
                    score++;
                }
            }
            else if (console.firewallTime > 10)
            {
                console.IntrusionDetected();
                console.ResetState(time);
            }
        }
    }
}
