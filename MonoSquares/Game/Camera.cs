using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoSquares
{
    class Camera : Game
    {
        public SpriteBatch Scene;
        public GraphicsDevice Device;
        public double X, Y;
        public float Rotation;
        private List<IGraphicsBody> Bodies = new List<IGraphicsBody>();

        public Camera(GraphicsDevice graphicsDevice)
        {
            Scene = new SpriteBatch(graphicsDevice);
        }

        public void BindObject(IGraphicsBody obj)
        {
            Bodies.Add(obj);
        }

        public void LoadTexture()
        {
            foreach (var Body in Bodies)
            {
                Body.Texture=Content.Load<Texture2D>(Body.TexturePath);
            }
        }

        public void Update()
        {
            Scene.Begin();

            foreach (var body in Bodies)
            {
                Rectangle tempRectangle = new Rectangle(body.Y, body.X, body.Width, body.Height);

                tempRectangle.X += (int)(tempRectangle.Width * .5f);
                tempRectangle.Y += (int)(tempRectangle.Height * .5f);
                Vector2 toff = new Vector2(tempRectangle.X + tempRectangle.Width * .5f, tempRectangle.Y + tempRectangle.Height * .5f);

                Scene.Draw(body.Texture, tempRectangle, new Rectangle(body.Y, body.X, body.Width, body.Height), Color.White, Rotation, toff, SpriteEffects.None, 0);

            }

            Scene.End();
        }

    }
}
