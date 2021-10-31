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
        private SpriteBatch _spriteBatch;
        GameObject[,] gameobject;
        Player player;
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
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Cam.Scene = _spriteBatch;
            Cam.Device = _graphics.GraphicsDevice;
            Cam.Pos = new Vector2(500, 500);

            

            
            gameobject = new GameObject[7, 10];
            
            int tileSize = 100;
            for (int y = 0; y < 7; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if(y == 0 || y == 6 || x == 0 || x == 9)
                    {
                        gameobject[y, x] = new GameObject();
                        gameobject[y, x].TexturePath = "Floor1";
                        gameobject[y, x].Body = new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize);
                        gameobject[y, x].PhysicsType = 0;
                        gameobject[y, x].IsSolid = true;
                    }
                    else
                    {
                        gameobject[y, x] = new GameObject();
                        gameobject[y, x].TexturePath = "Tile_12";
                        gameobject[y, x].Body = new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize);
                        gameobject[y, x].PhysicsType = 0;
                        gameobject[y, x].IsSolid = false;
                    }
                    

                    Cam.BindObject(gameobject[y, x]);
                    Engine.BindEntity(gameobject[y, x]);
                }

                player = new Player();
                player.TexturePath = "3";
                player.PhysicsType = 1;
                player.Body = new Rectangle(300, 300, 30, 30);

            }

            


            Engine.BindEntity(player);
            Cam.BindObject(player);

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

            // TODO: Add your drawing code here
            Cam.Update();

            base.Draw(gameTime);
        }
    }
}
