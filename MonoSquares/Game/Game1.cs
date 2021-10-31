using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace MonoSquares
{

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;

        GameObject[,] walls;
        Player player;
        Monster monster;
        private SpriteFont font;

        Camera Cam = new Camera();
        PhysicsEngine Engine = new PhysicsEngine();
        const int SCREEN_WIDTH = 1280;
        const int SCREEN_HEIGHT = 720;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            _graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            _graphics.ApplyChanges();

            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("font");

            Cam.Scene = spriteBatch;
            Cam.Device = _graphics.GraphicsDevice;
            Cam.Pos = new Vector2(500, 500);

            walls = new GameObject[7, 10];
            int tileSize = 100;
            for (int y = 0; y < 7; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if(y == 0 || y == 6 || x == 0 || x == 9)
                    {
                        walls[y, x] = new GameObject();
                        walls[y, x].Body = new Rectangle(0, 0, tileSize, tileSize);
                        walls[y, x].TexturePath = "Floor1";
                        walls[y, x].Body = new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize);
                        walls[y, x].PhysicsType = 0;
                        walls[y, x].IsSolid = true;
                    }
                    else
                    {
                        walls[y, x] = new GameObject();
                        walls[y, x].TexturePath = "Tile_12";
                        walls[y, x].Body = new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize);
                        walls[y, x].PhysicsType = 0;
                        walls[y, x].IsSolid = false;
                    }
                    

                    Cam.BindObject(walls[y, x]);
                    Engine.BindEntity(walls[y, x]);
                }
            }


            player = new Player();
            player.TexturePath = "3";
            player.Body = new Rectangle(300, 300, 30, 30);
            player.showScore = true;
            Engine.BindEntity(player);
            Cam.BindObject(player);

            monster = new Monster();
            monster.TexturePath = "3";
            monster.Body = new Rectangle(500, 500, 30, 30);
            monster.Engine = Engine;
            monster.showScore = true;
            Controller monsterControl = new Controller(monster.Control, 3);
            Engine.BindEntity(monster);
            Cam.BindObject(monster);

            Cam.LoadTextures(Content);
                        
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            if(Keyboard.GetState().IsKeyDown(Keys.Space))
                Cam.Zoom -= 0.001f;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
                Engine.AdditativeImpact(player, player.MaxSpeed / (Math.Abs(player.Velocity.Y) + 1) * player.Acceleration, -Math.PI / 2);

            if (Keyboard.GetState().IsKeyDown(Keys.S))
                Engine.AdditativeImpact(player, player.MaxSpeed / (Math.Abs(player.Velocity.Y) + 1) * player.Acceleration, Math.PI / 2);

            if (Keyboard.GetState().IsKeyDown(Keys.A))
                Engine.AdditativeImpact(player, player.MaxSpeed / (Math.Abs(player.Velocity.X) + 1) * player.Acceleration, Math.PI);

            if (Keyboard.GetState().IsKeyDown(Keys.D))
                Engine.AdditativeImpact(player, player.MaxSpeed / (Math.Abs(player.Velocity.X) + 1) * player.Acceleration, Math.PI * 2);


            // TODO: Add your update logic here
            Engine.Think();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Cam.pos.X = player.Body.X;
            Cam.pos.Y = player.Body.Y;
            Cam.Update();

            foreach (GameObject ent in Engine.Entities)
            {
                if (ent.showScore)
                {
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font, "Health " + ent.Health, new Vector2(ent.Body.X, ent.Body.Y), Color.Black);
                    spriteBatch.End();
                }

                if (ent.Health <= 0)
                {
                    Cam.Bodies.Remove(ent);
                    Engine.Entities.Remove(ent);
                    break;
                }
            }


            base.Draw(gameTime);
        }
    }
}
