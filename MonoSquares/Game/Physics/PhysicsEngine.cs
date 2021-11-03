using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace MonoSquares.Physics
{
    class PhysicsEngine
    {
        public delegate void Handler(IPhysics ent1, IPhysics ent2, EventArgs e);
        public event Handler Touch;
        public event EventHandler Think;

        private List<IPhysics> Entities;
        private List<IPhysics> NonThinkingEntities;

        public PhysicsEngine()
        {
            Entities = new List<IPhysics>();
            NonThinkingEntities = new List<IPhysics>();
        }

        public void BindEntity(IPhysics entity)
        {
            if (entity.PhysicsType == 0)
            {
                NonThinkingEntities.Add(entity);
            }
            else if (entity.PhysicsType == 1)
            {
                Entities.Add(entity);
            }

            this.Touch += new Handler(entity.OnTouch);
            this.Think += new EventHandler(entity.OnThink);
        }

        public void Process()
        {
            foreach(var entity in Entities)
            {
                IPhysics collidableEntity = HasCollided(entity, true);
                if (collidableEntity!=null)
                {
                    if (entity.PhysicsType == 1)
                    {
                        CreateCollision(entity, collidableEntity);
                    }
                }

                entity.Collided=false;
                UpdatePosition(entity);
            }
        }

        public IPhysics HasCollided(IPhysics entity, bool lookAhead=false)
        {
            foreach (var collidableEntity in GetCollidableEntities())
            {
                if(lookAhead)
                {
                    Rectangle nextPosition = GetNextPosition(entity);

                    if (nextPosition.Intersects(collidableEntity.Body) && entity != collidableEntity && collidableEntity.IsSolid && entity.IsSolid && !entity.Collided)
                    {

                        OnTouch(entity, collidableEntity);
                        return collidableEntity;
                    }

                }
                else
                {
                    if (entity.Body.Intersects(collidableEntity.Body) && entity != collidableEntity && collidableEntity.IsSolid && entity.IsSolid && !entity.Collided)
                    {

                        OnTouch(entity, collidableEntity);
                        return collidableEntity;
                    }
                }
                
            }

            return null;
        }

        private void CreateCollision(IPhysics entity1, IPhysics entity2)
        {
            Vector2 tempVelocity = entity1.Velocity;
            double tempSpeed = Math.Sqrt(Math.Pow(tempVelocity.X, 2) + Math.Pow(tempVelocity.Y, 2))*0.8;
            double tempDir = GetDirection(entity1);

            entity1.Velocity = Vector2.Zero;
            if(IsCollisionVertical(entity1, entity2))
            {
                AdditativeImpactVertical(entity1, tempSpeed, tempDir + Math.PI);
            }
            else
            {
                AdditativeImpactHorizontal(entity1, tempSpeed, tempDir + Math.PI);
            }
            AdditativeImpact(entity2, tempSpeed, tempDir);

            entity1.Collided = true;
        }

        public bool IsCollisionVertical(IPhysics entity1, IPhysics entity2)
        {
            Rectangle nextPosition = GetNextPosition(entity1);

            if (Rectangle.Intersect(nextPosition, entity2.Body).Width > Rectangle.Intersect(nextPosition, entity2.Body).Height)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public IEnumerable<IPhysics> GetCollidableEntities()
        {
            IEnumerable<IPhysics> combined = NonThinkingEntities.Concat(Entities).Concat(NonThinkingEntities).ToList();

            foreach(var entity in combined)
            {
                yield return entity;
            }

            
        }

        public Rectangle GetNextPosition(IPhysics entity)
        {
            Vector2 velocity = new Vector2(entity.Velocity.X * entity.Friction, entity.Velocity.Y * entity.Friction);

            return new Rectangle((int)(Math.Round(entity.Body.X + velocity.X)), (int)(Math.Round(entity.Body.Y + velocity.Y)), entity.Body.Width, entity.Body.Height);
        }

        public void UpdatePosition(IPhysics entity)
        {
            entity.Velocity = new Vector2(entity.Velocity.X * entity.Friction, entity.Velocity.Y * entity.Friction);
            //Debug.WriteLine(entity.Body.X);
            entity.Body = GetNextPosition(entity);//new Rectangle((int)(entity.Body.X + entity.Velocity.X), (int)(entity.Body.Y + entity.Velocity.Y), entity.Body.Width, entity.Body.Height);
            
        }

        public void AdditativeImpact(IPhysics entity, double amount, double direction)
        {
            //amount *= 2;
            entity.Velocity += new Vector2((float)(amount * Math.Cos(direction)), (float)(amount * Math.Sin(direction)));
        }

        public void AdditativeImpactVertical(IPhysics entity, double amount, double direction)
        {
            amount *= 2;
            entity.Velocity += new Vector2((float)(amount * Math.Cos(direction)), (float)(amount * Math.Sin(direction)*-1));
            //Debug.WriteLine("Vertical Collision");
        }

        public void AdditativeImpactHorizontal(IPhysics entity, double amount, double direction)
        {
            amount *= 2;
            entity.Velocity += new Vector2((float)(amount * Math.Cos(direction)), (float)(amount * Math.Sin(direction)));
            //Debug.WriteLine("Horizontal Collision");
        }

        public double GetSpeed(IPhysics entity)
        {
            return Math.Sqrt(Math.Pow(entity.Velocity.X, 2) + Math.Pow(entity.Velocity.Y, 2));
        }

        public double GetDirection(IPhysics entity)
        {
            return Math.Atan2(entity.Velocity.Y, entity.Velocity.X);
        }

        public void OnTouch(IPhysics entity1, IPhysics entity2)
        {
            Handler handler = Touch;

            if (null != handler) handler(entity1, entity2, EventArgs.Empty);
        }

        public void OnThink(IPhysics entity1, IPhysics entity2)
        {
            EventHandler handler = Think;

            if (null != handler) handler(entity1, EventArgs.Empty);
        }
    }
}
