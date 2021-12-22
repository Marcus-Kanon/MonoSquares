using System.Numerics;
using System.Drawing;
using static MonoSquares.Physics.IPhysics;

namespace MonoSquaresServer.Physics
{
    class GameObject : IPhysics
    {
        public Rectangle Body { get; set; }
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
        public bool showScore { get; set; } = false;
        public TouchDelegate TouchAction { get; set; }
        public ThinkDelegate ThinkAction { get; set; }

        protected virtual void OnTouch(object entity1, object entity2, EventArgs e)
        {
            if(TouchAction!=null)
                TouchAction(entity1, entity2, e);
        }
        protected virtual void OnThink(object entity1, EventArgs e)
        {
            if (ThinkAction != null)
                ThinkAction(entity1, e);
        }
    }
}
