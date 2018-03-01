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
using Microsoft.Xna.Framework.Content;
namespace CyberChocolate
{
    class Door : DrawableGameComponent
    {
        private GameAnimator animator;
        public HackingConsole hackingConsole;
        private Vector2 position;
        private String animationPath;
        private Matrix matrix;
        private SpriteFont debugFont;
        SpriteBatch spriteBatch;
        enum state { closed,opened,closing,opening};
        private state doorState;
        private state prevDoorState;
        private bool isClosed;
        private bool isUnlocked;
        private int maxActionDistance;



        private static readonly Config config = new Config
        {
            MetadataEnabled = false,
            EventsEnabled = true,
            PoolingEnabled = true,
            TagsEnabled = true,
            VarsEnabled = true,
            SoundsEnabled = false
        };

        public Door(Game game, String animationPath, Vector2 position) : base(game)
        {
            //Poprawa wspolrzednych
            this.position = position + new Vector2(25,11);
            this.animationPath = "Devices/" + animationPath;
            maxActionDistance = 200;
            doorState = state.closed;
            prevDoorState = doorState;
            isClosed = true;
            isUnlocked = false;
            hackingConsole =  new HackingConsole(this.Game);
        }
        public bool IsDoorClosed()
        {
            return isClosed;
        }
        public void Load()
        {

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            debugFont = Game.Content.Load<SpriteFont>("Font");
            DefaultProviderFactory<ISprite, SoundEffect> factory = new DefaultProviderFactory<ISprite, SoundEffect>(config, true);
            SpriterContentLoader loader = new SpriterContentLoader(Game.Content, animationPath);
            loader.Fill(factory);
            animator = new GameAnimator(loader.Spriter.Entities[0], 60, 288, Game.GraphicsDevice, factory);
            animator.Position = position;
            //Poprawa hitboxow
            animator.getCorrection(25, 140);
            animator.Play("Opened");
            hackingConsole.Load();
        }
        public void GetMatrix(Matrix matrix)
        {
            this.matrix = matrix;
        }
        public void Draw(SpriteBatch batch)
        {
            if (hackingConsole.isBeingHacked)
            {
                hackingConsole.Draw(batch);
            }
            animator.getMatrix(matrix);
            animator.Draw(batch);
           // DrawDebug();              
        }
        public void DrawDebug()
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(debugFont, "Door state: " + doorState, new Vector2(10, 200), Color.Red);
            spriteBatch.DrawString(debugFont, " Door position: " + animator.Position, new Vector2(10, 220), Color.Red);
            spriteBatch.DrawString(debugFont, "Max Distance: " + (animator.Position.X - maxActionDistance), new Vector2(10, 240), Color.Red);
            spriteBatch.DrawString(debugFont, "Prev Door State: " + prevDoorState, new Vector2(10, 260), Color.Red);
            spriteBatch.DrawString(debugFont, "Door is unlocked: " + isUnlocked, new Vector2(10, 280), Color.Red);
            spriteBatch.End();

        }
        public void Update(GameTime gameTime,Vector2 playerPosition)
        {
            if (hackingConsole.isBeingHacked)
            {
                hackingConsole.Update(gameTime, playerPosition);
            }
            if (doorState != prevDoorState)
            {
                DoorState();
            }
            CheckingState();
           
            animator.Update(gameTime.ElapsedGameTime.Milliseconds);
            HackingResult(gameTime);
        }
        public Rectangle GetRectangle()
        {
            return animator.getRectangle();
        }
        public int DoorAction(Vector2 playerPosition,int Keys)
        {
            
            if (IsPlayerCloseHorizontal(playerPosition) && IsPlayerCloseVertical(playerPosition))
            {
                if(Keys > 0 && !isUnlocked)
                {

                    isUnlocked = true;
                    if (doorState == state.closed)
                    {
                        doorState = state.opening;
                    }
                    else if (doorState == state.opened)
                    {
                        doorState = state.closing;
                    }
                    Keys--;
                    return Keys;
                }
                else if (isUnlocked)
                {
                   // isUnlocked = true;
                    if (doorState == state.closed)
                    {
                        doorState = state.opening;
                    }
                    else if (doorState == state.opened)
                    {
                        doorState = state.closing;
                    }
                    return Keys;
                }
                return Keys;
                
            }
            return Keys;
        }
        public bool HackingDoor(Vector2 playerPosition)
        {
            if (IsPlayerCloseHorizontal(playerPosition) && IsPlayerCloseVertical(playerPosition))
            {                
                if (isUnlocked)
                {
                   // isUnlocked = true;
                    if (doorState == state.closed)
                    {
                        doorState = state.opening;
                    }
                    else if (doorState == state.opened)
                    {
                        doorState = state.closing;
                    }
                }else if (!isUnlocked)
                {                   
                    hackingConsole.isBeingHacked = true;
                    hackingConsole.LoadPanel();
                    return true;

                }
            }
            return false;
        }
        public void HackingResult(GameTime time)
        {
            if (hackingConsole.answerConfirmed)
            {
                if (hackingConsole.Confirm())
                {
                    isUnlocked = true;
                    hackingConsole.ResetState(time);
                    //hackingConsole.answerConfirmed = false;
                   // hackingConsole.isBeingHacked = false;
                }else
                {
                    hackingConsole.ResetState(time);
                    //  hackingConsole.answerConfirmed = false;
                    //  hackingConsole.isBeingHacked = false;
                }
            }else if(hackingConsole.firewallTime > 10)
            {
                hackingConsole.IntrusionDetected();
                hackingConsole.ResetState(time);
            }
        }
        public bool IsPlayerCloseHorizontal(Vector2 playerPosition)
        {

            if((int)animator.Position.X > playerPosition.X)
            {
                if((playerPosition.X >= animator.Position.X - maxActionDistance))
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            else if ((int)animator.Position.X < playerPosition.X)
            {
                if ((playerPosition.X <= animator.Position.X + maxActionDistance))
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
            if ((int)animator.Position.Y > playerPosition.Y)
            {
                if ((playerPosition.Y >= animator.Position.Y - maxActionDistance))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else if ((int)animator.Position.Y < playerPosition.Y)
            {
                if ((playerPosition.Y <= animator.Position.Y + maxActionDistance))
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
        public void DoorState()
        {
            switch (doorState)
            {
                case state.closed:
                    animator.Play("Opened");
                    prevDoorState = state.closed;
                    break;
                case state.closing:
                    animator.Play("Closing");
                    prevDoorState = state.closing;
                    isClosed = true;
                    break;
                case state.opened:
                    animator.Play("Closed");
                    prevDoorState = state.opened;
                    break;
                case state.opening:
                    animator.Play("Openning");
                    prevDoorState = state.opening;
                    isClosed = false;
                    break;

            }
        }
        public void CheckingState()
        {
            if(doorState == state.closing)
            {
                if(animator.Progress > 0.99)
                {
                    doorState = state.closed;
                }
            }else if (doorState == state.opening)
            {
                if (animator.Progress > 0.99)
                {
                    doorState = state.opened;
                }
            }
        }
    }
}
