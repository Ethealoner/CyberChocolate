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
    class HackingConsole
    {
        private Texture2D texture;
        private Random random;
        private SpriteFont messageFont;
        private String answer;
        private Vector2 playerLocation;
        private int variableA;
        private int variableB;
        private int sum;
        private int playerAnswer;
        private float answerInterval;
        private float displayTime;
        public float firewallTime;
        public bool firewallDetected;
        public bool answerConfirmed;
        public bool isBeingHacked;
        private bool isSuccesful;
        public bool pointGiven;
        Game game;     

        public HackingConsole(Game game)
        {
            this.game = game;
            random = new Random();
            answerConfirmed = false;
            isBeingHacked = false;
            displayTime = 0;
            firewallTime = 0;
            pointGiven = false;
        }
        public void Load()
        {
            texture = game.Content.Load<Texture2D>("Devices/HackingPanel");
            messageFont = game.Content.Load<SpriteFont>("Font");
        }
        public bool GetKeyboardState(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key);
        }
        public void LoadPanel()
        {
            answer = "";
            variableA = random.Next(10);
            variableB = random.Next(20);
            sum = variableA + variableB;
            firewallTime = 0;
            pointGiven = false;
        }
        public void DrawMessage(SpriteBatch spriteBatch)
        {
            firewallTime = (float)System.Math.Round(firewallTime, 2);
            if (answerConfirmed)
            {
                if (isSuccesful)
                {
                    spriteBatch.DrawString(messageFont, variableA + " + " + variableB + " = " + answer, new Vector2(playerLocation.X + 110, playerLocation.Y + 125), Color.Green, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);
                    spriteBatch.DrawString(messageFont, Convert.ToString(firewallTime), new Vector2(playerLocation.X + 255, playerLocation.Y + 125), Color.Green, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);
                }
                else
                {
                    spriteBatch.DrawString(messageFont, variableA + " + " + variableB + " = " + answer, new Vector2(playerLocation.X + 110, playerLocation.Y + 125), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);
                    spriteBatch.DrawString(messageFont, Convert.ToString(firewallTime), new Vector2(playerLocation.X + 255, playerLocation.Y + 125), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);
                }
            }
            else
            {
                spriteBatch.DrawString(messageFont, variableA + " + " + variableB + " = " + answer, new Vector2(playerLocation.X + 110, playerLocation.Y + 125), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);
                spriteBatch.DrawString(messageFont,Convert.ToString(firewallTime) , new Vector2(playerLocation.X + 255, playerLocation.Y + 125), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);
            }      
            

        }
        public void DrawMessage(SpriteBatch spriteBatch,int score)
        {
            firewallTime = (float)System.Math.Round(firewallTime, 2);
            if (answerConfirmed)
            {
                if (isSuccesful)
                {
                    spriteBatch.DrawString(messageFont, variableA + " + " + variableB + " = " + answer, new Vector2(playerLocation.X + 110, playerLocation.Y + 125), Color.Green, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);
                    spriteBatch.DrawString(messageFont, Convert.ToString(firewallTime), new Vector2(playerLocation.X + 255, playerLocation.Y + 125), Color.Green, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);
                    spriteBatch.DrawString(messageFont, Convert.ToString(score), new Vector2(playerLocation.X + 315, playerLocation.Y + 125), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);
                }
                else
                {
                    spriteBatch.DrawString(messageFont, variableA + " + " + variableB + " = " + answer, new Vector2(playerLocation.X + 110, playerLocation.Y + 125), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);
                    spriteBatch.DrawString(messageFont, Convert.ToString(firewallTime), new Vector2(playerLocation.X + 255, playerLocation.Y + 125), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);
                    spriteBatch.DrawString(messageFont, Convert.ToString(score), new Vector2(playerLocation.X + 315, playerLocation.Y + 125), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);
                }
            }
            else
            {
                spriteBatch.DrawString(messageFont, variableA + " + " + variableB + " = " + answer, new Vector2(playerLocation.X + 110, playerLocation.Y + 125), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);
                spriteBatch.DrawString(messageFont, Convert.ToString(firewallTime), new Vector2(playerLocation.X + 255, playerLocation.Y + 125), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);
                spriteBatch.DrawString(messageFont, Convert.ToString(score), new Vector2(playerLocation.X + 315, playerLocation.Y + 125), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);
            }
        }
        public void GetValues()
        {
            if(answerInterval > 0.1)
            {
                if (GetKeyboardState(Keys.D1))
                    answer += 1;
                if (GetKeyboardState(Keys.D2))
                    answer += 2;
                if (GetKeyboardState(Keys.D3))
                    answer += 3;
                if (GetKeyboardState(Keys.D4))
                    answer += 4;
                if (GetKeyboardState(Keys.D5))
                    answer += 5;
                if (GetKeyboardState(Keys.D6))
                    answer += 6;
                if (GetKeyboardState(Keys.D7))
                    answer += 7;
                if (GetKeyboardState(Keys.D8))
                    answer += 8;
                if (GetKeyboardState(Keys.D9))
                    answer += 9;
                if (GetKeyboardState(Keys.D0))
                    answer += 0;
                if (GetKeyboardState(Keys.Enter))
                {
                    TranslateAnswer();
                    answerConfirmed = true;
                }
                    
                answerInterval = 0;
            }
            

        }
        public bool Confirm()
        {
          if(playerAnswer == sum)
            {
                isSuccesful = true;
                return true;
            }
            else
            {
                isSuccesful = false;
                return false;
            }
        }
        public void TranslateAnswer()
        {
            playerAnswer = Int32.Parse(answer);
        }
        public void Update(GameTime time, Vector2 playerLocation)
        {
            this.playerLocation = playerLocation;            
            if (isBeingHacked)
            {
                answerInterval += (float)time.ElapsedGameTime.TotalSeconds;
                firewallTime += (float)time.ElapsedGameTime.TotalSeconds;
            }
            GetValues();

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin(SpriteSortMode.FrontToBack);
            DrawMessage(spriteBatch);
            spriteBatch.Draw(texture, new Vector2(playerLocation.X + 100, playerLocation.Y + 100), null,
        Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.3f);
            
          //  spriteBatch.End();
        }
        public void Draw(SpriteBatch spriteBatch,int score)
        {
            //spriteBatch.Begin(SpriteSortMode.FrontToBack);
            DrawMessage(spriteBatch,score);
            spriteBatch.Draw(texture, new Vector2(playerLocation.X + 100, playerLocation.Y + 100), null,
        Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.3f);

            //  spriteBatch.End();
        }
        public void ResetState(GameTime gameTime)
        {
            displayTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            firewallDetected = false;
            if (displayTime > 2)
            {
                isSuccesful = false;
                answerConfirmed = false;
                isBeingHacked = false;               
                displayTime = 0;
                firewallTime = 0;
                pointGiven = true;
                
            }
           
        }
        public void IntrusionDetected()
        {
            firewallDetected = true;
            answerConfirmed = true;
            isSuccesful = false;
        }
 

    }
}
