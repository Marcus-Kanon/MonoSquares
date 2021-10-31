using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace MonoSquares
{
    class Player : IGraphicsBody, IPhysics
    {
        public Rectangle Body { get; set; }
        public Texture2D Texture { get; set; }
        public String TexturePath { get; set; }
        public Vector2 tempVelocity { get; set; }
        public Vector2 Velocity { get; set; }

        public float Acceleration { get; set; } = 0.5f;
        public float MaxSpeed { get; set; } = 1.0f;
        public float Friction { get; set; } = 0.95f;
        public bool IsSolid { get; set; } = true;
        public bool Collided { get; set; }
        public int PhysicsType { get; set; } = 1; // 0: Non-thinking 1: Thinking
    }
}
