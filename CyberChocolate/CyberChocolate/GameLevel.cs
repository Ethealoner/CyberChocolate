using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using TiledSharp;

namespace CyberChocolate
{
    class GameLevel : DrawableGameComponent
    {
        private List<Obstacle> Obstacles = new List<Obstacle>();
        private List<Obstacle> Backgroundobs = new List<Obstacle>();
        private List<Obstacle> KeyCards = new List<Obstacle>();
        private List<HpPotion> HpPotions = new List<HpPotion>();
        private List<Enemy> Enemies = new List<Enemy>();
        public List<Door> Doors = new List<Door>();
        public ControlPanel controlPanel;
        private SortedList<int, Texture2D> Texture = new SortedList<int, Texture2D>();
        private ParticleEngine particleEngine;        
        private Vector2 playerPosition;
        private GameTime time;
        public PlayerComponent player;
        SpriteBatch spriteBatch;
        Matrix matrix;
        TmxMap map;
        public int Row;
        private int playerKeys;
        private bool loaded;

        public static GameLevel CurrentGameLevel { get; private set; }
        public GameLevel(Game game, int row) : base(game)
        {
            Row = row;
            GameLevel.CurrentGameLevel = this;
            player = new PlayerComponent(this.Game);

        }

        public void getMatrix(Matrix matrix)
        {
            this.matrix = matrix;
        }
        protected override void LoadContent()
        {

            base.LoadContent();
            player.Load();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            map = new TmxMap("Content/GameMap-level-1.tmx");
            LoadTexture();
            LoadMap();
            LoadObjects();
            foreach (var Enemy in Enemies)
            {
                Enemy.Load();
            }
            foreach(var Door in Doors)
            {
                Door.Load();
            }
            foreach(var HpPotion in HpPotions)
            {
                HpPotion.Load();
            }
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Game.Content.Load<Texture2D>("Elidia Spriter/Elidia Parts side view/spark"));
            textures.Add(Game.Content.Load<Texture2D>("Elidia Spriter/Elidia Parts side view/spark2"));
            List<Texture2D> textures2 = new List<Texture2D>();
            textures2.Add(Game.Content.Load<Texture2D>("Particle/fireSpark1"));
            textures2.Add(Game.Content.Load<Texture2D>("Particle/fireSpark2"));
            particleEngine = new ParticleEngine(textures,textures2);
            loaded = true;

        }
        public override void Draw(GameTime time)
        {
            
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, matrix);
            controlPanel.Draw(spriteBatch);
            foreach (var Enemy in Enemies)
            {
                Enemy.GetMatrix(matrix);
                Enemy.Draw(time, spriteBatch);
            }

            foreach (var Door in Doors)
            {
                Door.GetMatrix(matrix);
                Door.Draw(spriteBatch);
            }
            foreach (var Obstacle in Obstacles)
            {
                Obstacle.Draw(spriteBatch);
            }
            foreach (var Obstacle in Backgroundobs)
            {
                Obstacle.Draw(spriteBatch);
            }                   
            foreach(var HpPotion in HpPotions)
            {
                if(!HpPotion.isTaken)
                HpPotion.Draw(spriteBatch);
            }
            foreach (var keyCard in KeyCards)
            {
                if (!keyCard.isTaken)
                {
                    keyCard.Draw(spriteBatch);
                }
            }
            player.Draw(time, spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.Additive, null, null, null, null, matrix);
            particleEngine.Draw(spriteBatch);
            spriteBatch.End();
        }
        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            EnemyParticle();
            controlPanel.Update(gameTime,playerPosition);
            foreach (var Enemy in Enemies)
            {
                Enemy.Update(gameTime);
                if(Enemy.state != Enemy.characterState.Dead)
                {
                    Enemy.PlayerSpotted((int)playerPosition.X);
                }
                
            }
            foreach(Door door in Doors)
            {
                door.Update(gameTime,playerPosition);
            }
           // if (hackingConsole.isBeingHacked)
           // {
           //     hackingConsole.Update(gameTime,playerPosition);
           // }
            particleEngine.Update();
          //  PlayerAction(gameTime);   
        }
        public bool HasRoomForRectangle(Rectangle rectangleToCheck) {
            foreach (var tile in Obstacles)
            {
                if (!Object.ReferenceEquals(tile, null))
                {
                    if (tile.Bounds.Intersects(rectangleToCheck))
                    {
                        
                        return false;
                    }
                }

            }
            return true;
        }
        public bool IsInGround(Rectangle rectangleToCheck)
        {
            foreach(var tile in Obstacles)
            {
                if (!Object.ReferenceEquals(tile, null))
                {
                    if(rectangleToCheck.Bottom > tile.Bounds.Bottom && rectangleToCheck.Location.Y > tile.Bounds.Location.Y)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool BulletHit(Rectangle bulletRectangle)
        {
            foreach (var tile in Obstacles)
            {
                if (!Object.ReferenceEquals(tile, null))
                {
                    if (tile.Bounds.Intersects(bulletRectangle))
                    {
                        particleEngine.GenerateParticles(new Vector2(bulletRectangle.X, bulletRectangle.Y));
                        return true;
                    }
                }

            }
            foreach(var Enemy in Enemies)
            {
                if (!Object.ReferenceEquals(Enemy, null))
                {
                    if (Enemy.bounds.Intersects(bulletRectangle))
                    {
                        particleEngine.GenerateParticles(new Vector2(bulletRectangle.X, bulletRectangle.Y));
                        if(Enemy.state != Enemy.characterState.Dead)
                        {
                            Enemy.WasHit();
                        }
                        
                        return true;
                    }
                }
            }
            foreach(var Door in Doors)
            {
                if (!Object.ReferenceEquals(Door, null))
                {
                    if (Door.IsDoorClosed())
                    {
                        if (Door.GetRectangle().Intersects(bulletRectangle))
                        {
                            particleEngine.GenerateParticles(new Vector2(bulletRectangle.X, bulletRectangle.Y));
                            return true;
                        }

                    }
                }
            }
            return false;

        }
        public bool PlayerHit(Rectangle rectangleToCheck)
        {
            foreach(Enemy enemy in Enemies)
            {
                if (enemy.firing)
                {
                    
                    
                    if (enemy.PlayerHit(rectangleToCheck))
                    {
                        particleEngine.GenerateParticles(new Vector2(rectangleToCheck.X + 100, rectangleToCheck.Y + 100));
                        return true;
                    }
                    
                }
            }
            return false;
        }
        public bool DoorColision(Rectangle rectangleToCheck)
        {
            foreach(Door door in Doors)
            {
                if (!Object.ReferenceEquals(door, null))
                {
                    if (door.IsDoorClosed())
                    {
                        if (door.GetRectangle().Intersects(rectangleToCheck))
                        {
                            
                            return false;
                        }
                    }
                   
                }
            }
            
            return true;

        }
        public bool HpPotionCollision(Rectangle rectangleToCheck)
        {
            foreach (HpPotion hpPotion in HpPotions)
            {
                if (!Object.ReferenceEquals(hpPotion, null))
                {
                    if (!hpPotion.isTaken)
                    {
                        if (hpPotion.Bounds.Intersects(rectangleToCheck))
                        {
                            hpPotion.isTaken = true;
                            return true;
                        }
                    }

                }
            }
            return false;

        }
        public bool KeysCardCollision(Rectangle rectangleToCheck)
        {
            foreach (Obstacle keyCard in KeyCards)
            {
                if (!Object.ReferenceEquals(keyCard, null))
                {
                    if (!keyCard.isTaken)
                    {
                        if (keyCard.Bounds.Intersects(rectangleToCheck))
                        {
                            keyCard.isTaken = true;
                            return true;
                        }
                    }

                }
            }
            return false;
        }
        public void GetPlayerPosition(Vector2 playerPosition)
        {
            this.playerPosition = playerPosition;
        }
        public int KeysNeedToBeTaken(int playerKeys)
        {
            this.playerKeys = playerKeys;
            return playerKeys;
        }
        public void LoadMap()
        {
            int tileWidth;
            int tileHeight;
            int tilesetTilesWide;
            int tilesetTilesHigh;
            tileWidth = map.Tilesets[0].TileWidth;
            tileHeight = map.Tilesets[0].TileHeight;
            tilesetTilesWide = Texture.Values.ElementAt(0).Width / tileWidth;
            tilesetTilesHigh = Texture.Values.ElementAt(0).Height / tileHeight;
            for (int i = 0; i < map.Layers.Count; i++)
            {
                for(int j = 0; j < map.Layers[i].Tiles.Count; j++)
                {
                    int gid = map.Layers[i].Tiles[j].Gid;
                    if(gid != 0)
                    {
                        int tileFrame = gid - 1;
                        
                    int column = tileFrame % tilesetTilesWide;
                        int row = (int)Math.Floor((double)tileFrame / (double)tilesetTilesWide);
                        float x = (j % map.Width) * map.TileWidth;
                        float y = (float)Math.Floor(j / (double)map.Width) * map.TileHeight;
                        if (map.Layers[i].Name.Equals("Layer1"))
                        {
                            Obstacles.Add(new Obstacle(new Vector2(x, y), Texture.Values.ElementAt(tileFrame)));
                        }
                        else if (map.Layers[i].Name.Equals("Layer2"))
                        {
                            Backgroundobs.Add(new Obstacle(new Vector2(x, y), Texture.Values.ElementAt(tileFrame)));
                        }
                        else if (map.Layers[i].Name.Equals("Layer4"))
                        {
                            Enemies.Add(new Enemy(this.Game,"police_robot",(int)x,(int)y,35,120));
                        }else if (map.Layers[i].Name.Equals("Layer5"))
                        {
                            Doors.Add(new Door(this.Game, "door", new Vector2(x, y)));
                        }else if (map.Layers[i].Name.Equals("Layer6"))
                        {
                            HpPotions.Add(new HpPotion(this.Game, new Vector2(x, y)));
                        }else if (map.Layers[i].Name.Equals("Layer7"))
                        {
                            KeyCards.Add(new Obstacle(new Vector2(x, y), Texture.Values.ElementAt(tileFrame)));
                        }
                    }
                }
            }
          
        }
        public void LoadTexture()
        {
            for(int i = 0; i < map.Tilesets[0].Tiles.Count; i++)
            {
                Texture.Add(i, Game.Content.Load<Texture2D>(GetSource(map, i)));
            }
        }
        public void LoadObjects()
        {
            TmxObjectGroup objectGroup = map.ObjectGroups[0];
            Texture2D tex;
            for(int i = 0; i < objectGroup.Objects.Count; i++)
            {
                tex = Game.Content.Load<Texture2D>(objectGroup.Objects[i].Properties["path"]);
                Backgroundobs.Add(new Obstacle(new Vector2((float)objectGroup.Objects[i].X, (float)objectGroup.Objects[i].Y - tex.Bounds.Height), tex));
            }
            objectGroup = map.ObjectGroups[1];
            for(int i = 0; i < objectGroup.Objects.Count; i++)
            {
                tex = Game.Content.Load<Texture2D>("Devices/controlPanel");
                controlPanel = new ControlPanel(this.Game, new Vector2((float)objectGroup.Objects[i].X, (float)objectGroup.Objects[i].Y),tex);
            }
            controlPanel.LoadContent();
        }
        public void getTime(GameTime time)
        {
            this.time = time;
        }
        public String GetSource(TmxMap tmxmap, int index)
        {

            TmxTileset tileset = tmxmap.Tilesets[0];
            String source = tileset.Tiles[index].Image.Source;
            source = source.Remove(source.Length - 4);
            source = source.Remove(0, 8);
            source = "Obstacles/" + source;
            return source;
        }
        public void UnLoad()
        {
            Obstacles.Clear();
            Backgroundobs.Clear();
            Enemies.Clear();
            Doors.Clear();
            HpPotions.Clear();
            KeyCards.Clear();
            Texture.Clear();
            particleEngine.Clear();
            loaded = false;
            controlPanel.score = 0;
        }
        public bool isLoaded()
        {
            return loaded;
        }
        public void LoadGameLevel()
        {
            LoadContent();
        }
        public SpriteBatch getSpriteBatch()
        {
            return spriteBatch;
        }
        public void EnemyParticle()
        {
            foreach(Enemy enemy in Enemies)
            {
                if(enemy.state == Enemy.characterState.Dead)
                {
                    particleEngine.GenerateFireParticles(enemy.EnemyPosition());
                }
            }
        }
        public int getScore()
        {
            return controlPanel.score;
        }
        
    }
}
