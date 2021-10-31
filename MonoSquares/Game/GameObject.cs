using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoSquares
{
    class GameObject : IGraphicsBody
    {
        public Rectangle Body { get; set; } = new Rectangle(0, 0, 50, 50);
        public Texture2D Texture { get; set; }
        public string TexturePath { get; set; }


    }
}
