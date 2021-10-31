using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoSquares
{
    class GameObject : IGraphicsBody, IPhysics
    {
        public Rectangle Body { get; set; } = new Rectangle(0, 0, 50, 50);
        public Texture2D Texture { get; set; }
        public string TexturePath { get; set; }

        public Vector2 tempVelocity { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Position { get; set; }

        public float Acceleration { get; set; }
        public float MaxSpeed { get; set; }
        public float Friction { get; set; }
        public bool IsSolid { get; set; }
        public bool Collided { get; set; }

        public int PhysicsType { get; set; }
    }
}
