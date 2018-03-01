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
    class PlayerComponent : DrawableGameComponent
    {
        private GameAnimator currentAnimator;
        private SpriteBatch spriteBatch;
        private IList<GameAnimator> animators = new List<GameAnimator>();
        private List<Bullet> bullets = new List<Bullet>();
        private Vector2 movement;
        private Vector2 gravity;
        private SpriteFont debugFont;
        private Matrix matrix;
        private Rectangle bottomRectangle;
        private Rectangle upperRectangle;
        private String scml = "Elidia Spriter/Elidia";
        private Texture2D guiHP;
        private Texture2D guiKeys;
        private Texture2D shootingBar;
        private Texture2D shootingBarActive;
        private bool facingRight;
        private bool running;
        private bool hasJumped;
        private bool firing;
        private bool hacking;
        private bool isReadyToFire;
        private int speed = 80;
        private int jumpHeight = 0;
        private int bulletsFired = 0;
        private int maxBullets = 10;
        private int hp = 3;
        public int numberOfKeys = 0;
        private float actionInterval = 0;
        private float shootInterval = 0;
        private float defendingTime = 0;

        enum characterState { idle, walking, running, shooting , shielding, };
        private characterState state;
        public void getMatrix(Matrix matrix)
        {
            this.matrix = matrix;
        }
        public Vector2 getPosition()
        {
            return currentAnimator.Position;
        }
        public PlayerComponent(Game game) : base(game)
        {
           state = characterState.idle;
            facingRight = true;
            running = true;
            hasJumped = false;
            firing = false;
            hacking = false;
            movement = new Vector2();
            gravity = new Vector2();          
        }
        private static readonly IList<string> Scmls = new List<string>
        {
            "Elidia Spriter/Elidia"
        };
        private static readonly Config config = new Config
        {
            MetadataEnabled = false,
            EventsEnabled = true,
            PoolingEnabled = true,
            TagsEnabled = true,
            VarsEnabled = true,
            SoundsEnabled = false
        };

        public override void Initialize()
        {
            base.Initialize();
            

        }
        protected override void LoadContent()
        {       
            base.LoadContent();
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            debugFont = Game.Content.Load<SpriteFont>("Font");
            guiHP = Game.Content.Load<Texture2D>("Gui/GUI_Elidia");
            guiKeys = Game.Content.Load<Texture2D>("Gui/KeyCard");
            shootingBar = Game.Content.Load<Texture2D>("Gui/shootingBar");
            shootingBarActive = Game.Content.Load<Texture2D>("Gui/shootingBarActive");
            DefaultProviderFactory<ISprite, SoundEffect> factory = new DefaultProviderFactory<ISprite, SoundEffect>(config, true);
            SpriterContentLoader loader = new SpriterContentLoader(Game.Content, scml);
            Stack<SpriteDrawInfo> drawInfoPool = new Stack<SpriteDrawInfo>();
            loader.Fill(factory);
            currentAnimator = new GameAnimator(loader.Spriter.Entities[0],1183,3772, Game.GraphicsDevice,factory);
            currentAnimator.Play("Idle");
            Vector2 scale = new Vector2 ((float)0.07);
            currentAnimator.getCorrection(35, 70);
            currentAnimator.Scale = scale;
            currentAnimator.Position = new Vector2 (400,500);
            for (int i = 0; i < maxBullets; i++)
            {
                bullets.Add(new Bullet(Game.Content.Load<Texture2D>("Elidia Spriter/Elidia Parts side view/Bullet")));
            }

        }
        public void  Draw(GameTime gametime , SpriteBatch batch)
        {
            currentAnimator.getMatrix(matrix);
            currentAnimator.Draw(batch);
         //   currentAnimator.DrawRectangle(spriteBatch, bottomRectangle);
          //  currentAnimator.DrawRectangle(spriteBatch, upperRectangle);
            foreach (Bullet bullet in bullets)
            {
                if (bullet.fired)
                {
                    bullet.Draw(batch, matrix);
                }
            }
            DrawGui(batch);
            //DrawDebug();

        }
        public bool IsOnFirmGround()
        {

            bottomRectangle = new Rectangle(new Point(currentAnimator.getRectangle().Location.X-3, currentAnimator.getRectangle().Bottom), new Point(currentAnimator.getRectangle().Width- 5,5));

            bottomRectangle.Offset(5, 0);
            if (!GameLevel.CurrentGameLevel.HasRoomForRectangle(bottomRectangle))
            {
                return true;
            }                
            else               
                return false;
        }
        public bool IsTouchingCeiling()
        {
            upperRectangle = new Rectangle(new Point(currentAnimator.getRectangle().Location.X, currentAnimator.getRectangle().Top), new Point(currentAnimator.getRectangle().Width, 5));
            if (!GameLevel.CurrentGameLevel.HasRoomForRectangle(upperRectangle))
            {
                return true;
            }
            else
                return false;
        }
        public void DrawDebug()
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(debugFont, "Elidia's Position: " + currentAnimator.Position, new Vector2(10, 0), Color.Red);
            spriteBatch.DrawString(debugFont, "Elidia's State: " + state, new Vector2(10, 20), Color.Red);
            spriteBatch.DrawString(debugFont, "Facing Right: " + facingRight, new Vector2(10, 40), Color.Red);
            spriteBatch.DrawString(debugFont, "Bullets Fired: " + bulletsFired, new Vector2(10, 60), Color.Red);
            spriteBatch.DrawString(debugFont, "HP: " + hp, new Vector2(10, 80), Color.Red);
            spriteBatch.DrawString(debugFont, "Defending Time: " + defendingTime, new Vector2(10, 100), Color.Red);
            spriteBatch.DrawString(debugFont, "Is on firm ground: " + IsOnFirmGround(), new Vector2(10, 120), Color.Red);
            spriteBatch.DrawString(debugFont, "Has jumped: " + hasJumped, new Vector2(10, 140), Color.Red);
            spriteBatch.DrawString(debugFont, "Number of keys: " + numberOfKeys, new Vector2(10, 160), Color.Red);
            spriteBatch.DrawString(debugFont, "Is hacking: " + hacking, new Vector2(10, 180), Color.Red);
            spriteBatch.End();

        }
        public void DrawGui(SpriteBatch spriteBatch)
        {
            // spriteBatch.Begin();
            spriteBatch.Draw(guiHP, new Vector2(currentAnimator.Position.X - 350,currentAnimator.Position.Y - 280), null, Color.White, 0, Vector2.Zero, 0.1f, SpriteEffects.None, 0.3f);
            //spriteBatch.Draw(guiHP,new Vector2(10,10), null, Color.White, 0, Vector2.Zero, 0.1f, SpriteEffects.None, 0.9f);
            spriteBatch.DrawString(debugFont, "X" + hp, new Vector2(currentAnimator.Position.X - 270, currentAnimator.Position.Y - 240), Color.White);
            spriteBatch.Draw(guiKeys, new Vector2(currentAnimator.Position.X - 330, currentAnimator.Position.Y - 180), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.3f);
            spriteBatch.DrawString(debugFont, "X" + numberOfKeys, new Vector2(currentAnimator.Position.X - 270, currentAnimator.Position.Y - 180), Color.White);
            DrawShootingBar(spriteBatch);
            
            //spriteBatch.End();
        }
        public void DrawShootingBar(SpriteBatch spriteBatch)
        {
            if (isReadyToFire)
            {

                spriteBatch.Draw(shootingBarActive, new Vector2(currentAnimator.Position.X - 380, currentAnimator.Position.Y - 130), null, Color.White, 0, Vector2.Zero, new Vector2(1f, 0.3f), SpriteEffects.None, 0.3f);
            }
            else if (!isReadyToFire)
            {
                spriteBatch.Draw(shootingBar, new Vector2(currentAnimator.Position.X - 380, currentAnimator.Position.Y - 130), null, Color.White, 0, Vector2.Zero, new Vector2(CalculateScale(), 0.3f), SpriteEffects.None, 0.3f);
            }
        }
        private void PlayerState()
        {
            switch (state)
            {
                case characterState.idle:
                    if (facingRight)
                    {
                        currentAnimator.Play("Idle");
                    }else
                    {
                        currentAnimator.Play("Idle_Reverse");
                    }
                        
                    break;
                case characterState.walking:
                    if (facingRight)
                    {
                        currentAnimator.Play("Walking");
                    }else
                    {
                        currentAnimator.Play("Walking_Reverse");
                    }                      
                    break;
                case characterState.running:
                    if (facingRight)
                    {
                        currentAnimator.Play("Running");
                    }
                    else
                    {
                        currentAnimator.Play("Running_Reverse");
                    }                      
                    break;
                case characterState.shooting:
                    if (facingRight)
                    {
                        currentAnimator.Play("Shooting");
                    }else
                    {
                        currentAnimator.Play("Shooting_Reverse");
                    }
                    break;
                case characterState.shielding:
                    if (facingRight)
                    {
                        currentAnimator.Play("Defending");
                    }else
                    {
                        currentAnimator.Play("Defending_Reverse");
                    }
                        break;           
                                    
            }

        }
        public bool GetKeyboardState(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key);
        }
        public void UpdateMovement(GameTime time)
        {
            currentAnimator.Position += movement * speed * (float)time.ElapsedGameTime.TotalSeconds;

            shootInterval += (float)time.ElapsedGameTime.TotalSeconds;
            actionInterval += (float)time.ElapsedGameTime.TotalSeconds;
            if (shootInterval > 10)
                shootInterval = 2;
            movement = new Vector2();

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                running = !running;              
            }
            
            if (GetKeyboardState(Keys.D) && running == false && !GetKeyboardState(Keys.A))
            {
                facingRight = true;
                if (state != characterState.walking)
                {
                    state = characterState.walking;
                    PlayerState();
                }
                movement = new Vector2(1, 0);
            }
            
            if (GetKeyboardState(Keys.D) && running == true &&!GetKeyboardState(Keys.A))
            {
                facingRight = true;
                if (state != characterState.running)
                {
                    state = characterState.running;
                    PlayerState();
                }
                movement = new Vector2(2, 0);
            }
            
            if (GetKeyboardState(Keys.A) && running == true && !GetKeyboardState(Keys.D))
            {
                facingRight = false;
                if (state != characterState.running)
                {                  
                    state = characterState.running;
                    PlayerState();
                }
                movement = new Vector2(-2, 0);
            }
            
            if (GetKeyboardState(Keys.A) && running == false && !GetKeyboardState(Keys.D))
            {
                facingRight = false;
                if (state != characterState.walking)
                {                   
                    state = characterState.walking;
                    PlayerState();
                }
                movement = new Vector2(-1, 0);
            }
            if (GetKeyboardState(Keys.F))
            {
                
                    if (actionInterval >= 0.5)
                    {
                        foreach (Door door in GameLevel.CurrentGameLevel.Doors)
                        {
                            numberOfKeys = door.DoorAction(currentAnimator.Position, numberOfKeys);

                        }
                        actionInterval = 0;
                    }
                
            }
            if (GetKeyboardState(Keys.H))
            {
                if (actionInterval >= 0.5)
                {
                    foreach (Door door in GameLevel.CurrentGameLevel.Doors)
                    {
                        if (door.HackingDoor(currentAnimator.Position)) {
                            door.hackingConsole.LoadPanel();
                            hacking = true;
                        } 

                    }
                    if (GameLevel.CurrentGameLevel.controlPanel.HackingPanel(currentAnimator.Position))
                    {
                        hacking = true;
                    }
                    actionInterval = 0;
                }
            }
            if (GetKeyboardState(Keys.W) && IsOnFirmGround())
            {
                hasJumped = true;
            }          
            else
            {
                if (((!GetKeyboardState(Keys.A) && !GetKeyboardState(Keys.D)) || (GetKeyboardState(Keys.A) && GetKeyboardState(Keys.D))) && state != characterState.shooting && state != characterState.shielding )
                {
                    if (state != characterState.idle)
                    {
                            state = characterState.idle;                       
                        PlayerState();
                    }

                }

            }
            
    

        }
        public void GravityForce(Vector2 oldPosition)
        {
            if (IsTouchingCeiling())
            {
                gravity = new Vector2(0, 0);
                jumpHeight = 0;
                hasJumped = false;
            }

            if (!IsOnFirmGround() && !hasJumped) {
                gravity += new Vector2(0, 0.10f);

                currentAnimator.Position += gravity;
            }else if(IsOnFirmGround() && !hasJumped)
            {
                gravity = new Vector2(0, 0);
               
            }
            else if (hasJumped)
            {

                gravity = new Vector2(0, -5f);
                jumpHeight += (int)gravity.Y;
                currentAnimator.Position += gravity;
                if(jumpHeight < 2)
                {
                    jumpHeight = 0;
                    hasJumped = false;
                }
            }


        }
        public override void Update(GameTime gameTime)
        {
            
            Vector2 oldPosition = currentAnimator.Position;
            Attacking();
            Defending(gameTime);            
            PlayerHit();
            
            if (!firing && !hacking)
            {
                UpdateMovement(gameTime);
            }
            if (hacking)
            {
                GetHackingResult();
            }
              
             if (!GameLevel.CurrentGameLevel.HasRoomForRectangle(currentAnimator.getRectangle()) || !GameLevel.CurrentGameLevel.DoorColision(currentAnimator.getRectangle()))
              {
                  currentAnimator.Position = oldPosition;
                if (GameLevel.CurrentGameLevel.IsInGround(currentAnimator.getRectangle()) && IsOnFirmGround())
                {
                    currentAnimator.Position -= new Vector2(0, 1);
                }           
               }
                GravityForce(oldPosition);
               foreach(Bullet bullet in bullets)
            {
                if (bullet.fired)
                {
                    bullet.Update(gameTime);
                }
            }
            if (GameLevel.CurrentGameLevel.HpPotionCollision(currentAnimator.getRectangle()))
            {
                hp++;
            }
            if (GameLevel.CurrentGameLevel.KeysCardCollision(currentAnimator.getRectangle()))
            {
                numberOfKeys = numberOfKeys + 1;
            }
            BulletCollision();        
            currentAnimator.Update(gameTime.ElapsedGameTime.Milliseconds);
            
        }
        public void Attacking()
        {
            
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {

                    if (shootInterval > 1.0f)
                    {
                        firing = true;
                        if (state != characterState.shooting)
                        {
                            state = characterState.shooting;
                            PlayerState();
                        }
                        shootInterval = 0;
                        FireBullet();
                        bulletsFired++;
                    isReadyToFire = false;
                }

                }
                if(shootInterval > 1.0f)
            {
                isReadyToFire = true;
            }
            
            
            if (state == characterState.shooting)
            {
                if (currentAnimator.Progress > 0.9)
                {
                    state = characterState.idle;
                    firing = false;
                    PlayerState();
                }
            }
        }
        public void Defending(GameTime gameTime)
        {
            defendingTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(defendingTime > 20)
            {
                defendingTime = 0;
            }
            if ((Keyboard.GetState().IsKeyDown(Keys.E)) && !firing)
            {
                if(state != characterState.shielding)
                {
                    defendingTime = 0;
                    state = characterState.shielding;
                    PlayerState();
                }
            }
            if(defendingTime > 2 && state == characterState.shielding)
            {
                
                state = characterState.idle;
                PlayerState();
            }

        }
        public void FireBullet()
        {
            if(bulletsFired < maxBullets)
            {
                bullets[bulletsFired].Fire(facingRight,currentAnimator.Position,20);
            }else
            {               
                bulletsFired = 0;
                bullets[bulletsFired].Fire(facingRight, currentAnimator.Position,20);
            }
        }
        public void BulletCollision()
        {
            foreach(Bullet bullet in bullets)
            {
                if (bullet.fired)
                {
                    if (GameLevel.CurrentGameLevel.BulletHit(bullet.Bounds))
                    {
                        bullet.fired = false;
                    }
                }
            }
        }
        public void PlayerHit()
        {
            if (GameLevel.CurrentGameLevel.PlayerHit(currentAnimator.getRectangle()))
            {
                if(state == characterState.shielding)
                {
                    if (facingRight)
                    {
                        currentAnimator.Play("Defending_Hit");
                    }
                    else
                    {
                        currentAnimator.Play("Defending_Hit_Reverse");
                    }
                    
                }else
                {
                    hp--;
                }
                
            }
        }
        public void GetHackingResult()
        {
            foreach(Door door in GameLevel.CurrentGameLevel.Doors)
            {
                if (door.hackingConsole.isBeingHacked)
                {
                    if (door.hackingConsole.answerConfirmed)
                    {
                        if (door.hackingConsole.Confirm())
                        {
                            hacking = false;
                        }
                        else
                        {
                            hacking = false;
                            hp--;
                        }
                    }
                }
            }
            if (GameLevel.CurrentGameLevel.controlPanel.console.isBeingHacked)
            {
                if (GameLevel.CurrentGameLevel.controlPanel.console.answerConfirmed)
                {
                    if (GameLevel.CurrentGameLevel.controlPanel.console.Confirm())
                    {
                        hacking = false;
                    }
                    else
                    {
                        hacking = false;
                        hp--;
                    }
                }
            }
           
            
            
        }
        public void Reset()
        {
            currentAnimator.Position = new Vector2(400, 500);
            hp = 3;
            numberOfKeys = 0;
        }
        public void Load()
        {
            LoadContent();
        }
        public bool isDead()
        {
            if (hp <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public float CalculateScale()
        {
            return ((shootInterval * (shootingBar.Width)) / 1.0f)/ (shootingBar.Width);
        }

    }
}
