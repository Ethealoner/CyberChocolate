using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CyberChocolate
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Camera cam;
        GameLevel box;
        SpriteFont debugFont;
        GameTime secondTime;
        MainMenu mainMenu;
        PauseMenu pauseMenu;
        GameOver gameOver;
        Intro intro;
        enum GameState { intro,mainMenu,playingLevel,pause,gameOver}
        GameState gameState;
        private float time;





        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            box = new GameLevel(this, 10);
            mainMenu = new MainMenu(this);
            pauseMenu = new PauseMenu(this);
            gameOver = new GameOver(this);
            intro = new Intro(this);
            Components.Add(box);
            graphics.PreferredBackBufferWidth = 800; 
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            gameState = GameState.intro;
            this.IsMouseVisible = true;

        }
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            cam = Camera.GetInstance(GraphicsDevice.Viewport);
            base.Initialize();
        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            debugFont = Content.Load<SpriteFont>("Font");
            intro.LoadContent();
            mainMenu.LoadContent();
            pauseMenu.LoadContent();
            gameOver.LoadContent();
            // TODO: use this.Content to load your game content here
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            secondTime = gameTime;
            time += (float)secondTime.ElapsedGameTime.TotalSeconds;
          
            switch (gameState)
            {
                case GameState.intro:
                    intro.Update(gameTime);
                    if (intro.IntroFinished())
                    {
                        gameState = GameState.mainMenu;
                    }
                    break;
                case GameState.mainMenu:
                    mainMenu.Update();
                    if (mainMenu.EndGameClicked())
                    {
                        Exit();
                    }else if (mainMenu.StartGameClicked())
                    {
                        gameState = GameState.playingLevel;
                        if (!box.isLoaded())
                        {
                            box.LoadGameLevel();
                        }
                    }
                    break;
                case GameState.playingLevel:
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        gameState = GameState.pause;
                    }
                    cam.Update(box.player.getPosition());
                    box.player.getMatrix(cam.Transform);
                    box.getMatrix(cam.Transform);
                    box.GetPlayerPosition(box.player.getPosition());                  
                    base.Update(gameTime);
                    if (box.player.isDead())
                    {
                        gameState = GameState.gameOver;
                    }
                    if(box.getScore() == 3)
                    {
                        gameState = GameState.mainMenu;
                        box.UnLoad();
                        box.player.Reset();
                    }
                    break;
                case GameState.pause:
                    pauseMenu.Update();
                    if (pauseMenu.ResumeGameClicked())
                    {
                        gameState = GameState.playingLevel;
                    }else if (pauseMenu.ReturnToMenuClicked())
                    {
                        gameState = GameState.mainMenu;
                        box.UnLoad();
                        box.player.Reset();
                    }
                    break;
                case GameState.gameOver:
                    gameOver.Update();
                    if (gameOver.ReturnToMenuClicked())
                    {
                        gameState = GameState.mainMenu;
                        box.UnLoad();
                        box.player.Reset();
                    }
                    if (gameOver.ResumeGameClicked())
                    {
                        box.UnLoad();
                        box.player.Reset();
                        box.LoadGameLevel();
                        gameState = GameState.playingLevel;
                    }
                    break;
            }
            

            



        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(Color.Black);
            switch (gameState)
            {
                case GameState.intro:
                    intro.Draw(spriteBatch);
                    break;
                case GameState.mainMenu:
                    mainMenu.Draw(spriteBatch);
                    break;
                case GameState.playingLevel:
                    base.Draw(gameTime);                  
                    break;
                case GameState.pause:
                    pauseMenu.Draw(spriteBatch);
                    break;
                case GameState.gameOver:
                    gameOver.Draw(spriteBatch);
                    break;
            }        
            
        }
    }
}
