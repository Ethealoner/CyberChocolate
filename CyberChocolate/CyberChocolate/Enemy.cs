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
    class Enemy : DrawableGameComponent
    {
        private List<Bullet> bullets = new List<Bullet>();
        public GameAnimator animator;
        public Rectangle bounds;
        private String animationPath;
        private Matrix matrix;
        private Vector2 gravity;
        private SpriteFont debugFont;
        private SpriteBatch spriteBatch;
        private int positionX;
        private int positionY;
        private int correctionX, correctionY;
        private int destination;
        private int spottingDistance;
        private int distance;
        private int maxBullets;
        private int bulletsFired;
        private int prevBullet;
        private float time;
        private float shootInterval;
        private bool facingRight;
        private bool prevFacingRight;
        private bool reached;
        public bool firing;
        public enum characterState { Idle, Walking, Shooting, Dead, };
        public characterState state;
        private characterState prevState;


        private static readonly Config config = new Config
        {
            MetadataEnabled = false,
            EventsEnabled = true,
            PoolingEnabled = true,
            TagsEnabled = true,
            VarsEnabled = true,
            SoundsEnabled = false
        };

        public Enemy(Game game,String animationPath,int positionX, int positionY,int correctionX,int correctionY) : base(game)
        {
            this.animationPath = "Enemies/" + animationPath;
            this.positionX = positionX;
            this.positionY = positionY;
            this.correctionX = correctionX;
            this.correctionY = correctionY;
            reached = false;
            facingRight = false;
            prevFacingRight = facingRight;
            firing = false;
            gravity = new Vector2(0);
            time = 0;
            destination = 0;
            maxBullets = 5;
            bulletsFired = 0;
            prevBullet = bulletsFired;
            spottingDistance = 400;
            shootInterval = 0;
            state = characterState.Idle;
            prevState = state;

        }
        public void Load()
        {
            //base.LoadContent();
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            DefaultProviderFactory<ISprite, SoundEffect> factory = new DefaultProviderFactory<ISprite, SoundEffect>(config, true);
            SpriterContentLoader loader = new SpriterContentLoader(Game.Content, animationPath);
            loader.Fill(factory);
            animator = new GameAnimator(loader.Spriter.Entities[0], 1300, 3500, Game.GraphicsDevice, factory);
            Vector2 scale = new Vector2((float)0.07);
            animator.Scale = scale;
            animator.getCorrection(correctionX, correctionY);
            animator.Position = new Vector2(positionX, positionY);
            debugFont = Game.Content.Load<SpriteFont>("Font");
            animator.Play("idle_reverse");
            for (int i = 0; i < maxBullets; i++)
            {
                bullets.Add(new Bullet(Game.Content.Load<Texture2D>("Elidia Spriter/Elidia Parts side view/Bullet")));
            }
        }
        public void GetMatrix(Matrix matrix)
        {
            this.matrix = matrix;
        }
        public  void Draw(GameTime gametime,SpriteBatch batch)
        {

                animator.getMatrix(matrix);
                animator.Draw(batch);
                bounds = animator.getRectangle();       
            foreach(Bullet bullet in bullets)
            {
                if (bullet.fired)
                {
                    bullet.Draw(batch, matrix);
                }
            }
        }
        public bool IsOnFirmGround()
        {
            Rectangle onePixelLower = animator.getRectangle();
            onePixelLower.Offset(0, 5);
            if (!GameLevel.CurrentGameLevel.HasRoomForRectangle(onePixelLower))
            {
                return true;
            }
            else
                return false;
        }
        public void GravityForce()
        {
            if (!IsOnFirmGround())
            {
                gravity += new Vector2(0, 0.10f);

                animator.Position += gravity;
            }
            else if (IsOnFirmGround())
            {
                gravity = new Vector2(0, 0);
            }

        }
        public override void Update(GameTime gameTime)
        {
            shootInterval += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(shootInterval > 20)
            {
                shootInterval = 8;
            }
            EnemyAI(gameTime);
            EnemyState(gameTime);
            Vector2 oldPosition = animator.Position;
            animator.Update(gameTime.ElapsedGameTime.Milliseconds);
            if (!GameLevel.CurrentGameLevel.HasRoomForRectangle(animator.getRectangle()))
            {
                animator.Position = oldPosition;
            }
            GravityForce();
            //  if(state != characterState.Idle)
            //  {
            //      state = characterState.Idle;
            //      EnemyState();
            //  }
            foreach(Bullet bullet in bullets)
            {
                if (bullet.fired)
                {
                    bullet.Update(gameTime);
                }
            }
           
        }
        // Obsluga stanow
        public void EnemyState(GameTime time)
        {
            switch (state)
            {
                case characterState.Idle:                                    
                    break;
                case characterState.Walking:
                    if (facingRight == true)
                    {
                        animator.Position += new Vector2(1, 0) * 50 * (float)time.ElapsedGameTime.TotalSeconds;
                    }else
                    {
                        animator.Position += new Vector2(-1, 0) * 50 * (float)time.ElapsedGameTime.TotalSeconds;
                    }                     
                    break;
                case characterState.Shooting:
                    Attacking();
                    break;
            }
        }
        // Warunki zmiany stanów
        public void EnemyAI(GameTime gameTime)
        {
            switch (state)
            {
                case characterState.Idle:
                    time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if(time > 5)
                    {
                        state = characterState.Walking;
                        destination = (int)animator.Position.X;
                        facingRight = !facingRight;
                        reached = false;
                        time = 0;
                        if(facingRight == true)
                        {
                            animator.Play("walking");
                        }else
                        {
                            animator.Play("walking_reverse");
                        }
                    }
                    break;
                case characterState.Walking:
                    /*
                    if(facingRight == false)
                    {
                        
                        if(destination > animator.Position.X)
                      // if((distance - 200) < destination)
                        {
                            reached = true;
                            distance = 0;
                            state = characterState.Idle;
                        }else if (destination > animator.Position.X)
                      // else if((distance+200) > destination)
                        {
                            distance = 0;
                            reached = true;
                            state = characterState.Idle;
                        }
                    }
                    */
                    if (facingRight == false)
                    {
                        if (destination - 200 > animator.Position.X)
                        {
                            reached = true;
                            state = characterState.Idle;
                            animator.Play("idle_reverse");
                        }
                    } else if (facingRight == true)
                    {
                        if (destination + 200 < animator.Position.X)
                        {
                            reached = true;
                            state = characterState.Idle;
                            animator.Play("idle");
                        }
                    }
                        break;
                case characterState.Dead:
                    if (animator.Progress > 0.9)
                    {
                        if (facingRight)
                        {
                            animator.Play("dead");
                        }else
                        {
                            animator.Play("dead_reverse");
                        }
                    }
                    
                        //  }
                        break;
                case characterState.Shooting:
                    if (animator.Progress > 0.9)
                    {
                        if (facingRight)
                        {
                            animator.Play("shooting");
                        }
                        else
                        {
                            animator.Play("shooting_reverse");
                        }
                    }
                    break;
                
            }
        }
        public void WasHit()
        {
            animator.Progress = 1;
            state = characterState.Dead;
           
        }
        public void PlayerSpotted(int playerPosition)
        {
            if (facingRight)
            {
                distance = playerPosition - (int)animator.Position.X;
                if(distance < spottingDistance && !(distance < 0))
                {
                    if(state != characterState.Shooting)
                    {
                        prevState = state;
                        prevFacingRight = facingRight;
                        state = characterState.Shooting;
                        animator.Progress = 1;
                        firing = true;
                    }

                    
                }else if (state == characterState.Shooting)
                {
                    if (((int)animator.Position.X - playerPosition) < spottingDistance)
                    {
                        facingRight = !facingRight;
                    }else
                    {
                        facingRight = prevFacingRight;
                        state = prevState;
                        firing = false;
                    }
                    
                }
            }else if(!facingRight)
            {
                distance = (int)animator.Position.X - playerPosition;
                if (distance < spottingDistance && !(distance < 0))
                {
                    if(state != characterState.Shooting)
                    {
                        prevState = state;
                        prevFacingRight = facingRight;
                        state = characterState.Shooting;
                        animator.Progress = 1;
                        firing = true;
                    }
                    
                }else if (state == characterState.Shooting)
                {
                    if ((playerPosition - (int)animator.Position.X) < spottingDistance)
                    {
                        facingRight = !facingRight;
                    }else
                    {
                        facingRight = prevFacingRight;
                        state = prevState;
                        firing = false;
                    }
                    
                }
            }
        }
        public void FireBullet()
        {
            if (bulletsFired < maxBullets)
            {
                bullets[bulletsFired].Fire(facingRight, animator.Position,30);
            }
            else
            {
                bulletsFired = 0;
                bullets[bulletsFired].Fire(facingRight, animator.Position,30);
            }
        }
        public void Attacking()
        {

                if (animator.Progress > 0.1 && animator.Progress < 0.2)
                {
                    if(shootInterval > 0.5)
                 {
                    shootInterval = 0;
                    FireBullet();
                    prevBullet = bulletsFired;
                    bulletsFired++;
                 }
                    
                }
        }
        public bool PlayerHit(Rectangle playerRectangle)
        {
            if (bullets[prevBullet].Bounds.Intersects(playerRectangle) && bullets[prevBullet].fired != false)
            {
                bullets[prevBullet].fired = false;
                return true;
            }else
            {
                return false;
            }
       
        }
        public Vector2 EnemyPosition()
        {
            return animator.Position;
        }

    }
}
