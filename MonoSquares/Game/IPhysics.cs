using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoSquares
{
    interface IPhysics
    {
        public Rectangle Body { get; set; }
        public Vector2 tempVelocity { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Position { get; set; }

        public float Acceleration { get; set; }
        public float MaxSpeed { get; set; }
    }
}
