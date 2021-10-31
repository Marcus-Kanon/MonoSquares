using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace MonoSquares
{

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        GameObject[,] gameobject;
        Camera Cam = new Camera();
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

            gameobject = new GameObject[100, 100];

            int tileSize = 100;
            for (int y = 0; y < 100; y++)
            {
                for (int x = 0; x < 100; x++)
                {
                    gameobject[y, x] = new GameObject();
                    gameobject[y, x].TexturePath = "Floor1";
                    gameobject[y, x].Body = new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize);

                    Cam.BindObject(gameobject[y, x]);
                }
            }
            


            Cam.LoadTextures(Content);
                        
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            if(Keyboard.GetState().IsKeyDown(Keys.Space)
                Cam.Zoom += 0.1f;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
                Cam.pos.Y -= 1f;

            if (Keyboard.GetState().IsKeyDown(Keys.S))
                Cam.pos.Y += 1f;

            // TODO: Add your update logic here

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
