using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace MonoSquares
{
    class PhysicsEngine
    {
        public delegate void Handler(IPhysics ent1, IPhysics ent2, EventArgs e);

        public List<IPhysics> Entities;
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

                //Debug.WriteLine($"Bind NON thinking {entity}");
            }
            else if (entity.PhysicsType == 1)
            {
                Entities.Add(entity);

                //Debug.WriteLine($"Bind {entity}");
            }

            this.Touch += new Handler(entity.OnTouch);
        }

        public void Think()
        {
            foreach(var entity in Entities)
            {
                //Debug.WriteLine($"THINKING {entity}");
                
                foreach (var collidableEntity in GetCollidableEntities())
                {
                    if (entity.Body.Intersects(collidableEntity.Body) && entity!=collidableEntity && collidableEntity.IsSolid && entity.IsSolid && !entity.Collided)
                    {
                        //Debug.WriteLine($"Collided! X {collidableEntity.Body.X} | Y {collidableEntity.Body.Y} | {collidableEntity}");

                        if(entity.PhysicsType==1)
                        {
                            CreateCollision(entity, collidableEntity);
                        }
                    }
                }
                
                entity.Collided=false;
                UpdatePosition(entity);
            }
        }

        private void CreateCollision(IPhysics entity1, IPhysics entity2)
        {

            Vector2 tempVelocity = entity1.Velocity;
            double tempSpeed = Math.Sqrt(Math.Pow(tempVelocity.X, 2) + Math.Pow(tempVelocity.Y, 2));
            double tempDir = GetDirection(entity1);
                
            //Debug.WriteLine(tempSpeed);

            entity1.Velocity = Vector2.Zero;
            if(IsCollisionVertical(entity1, entity2))
            {
                AdditativeImpactVertical(entity1, 5, tempDir + Math.PI);
            }
            else
            {
                AdditativeImpactHorizontal(entity1, 5, tempDir + Math.PI);
            }
            
            //player.Velocity *= -2;
            entity1.Collided = true;
            //player.Acceleration = 0.01f;

            OnTouch(entity1, entity2);

            Debug.WriteLine($"COLLISION AT X {entity1.Body.X} | Y {entity1.Body.Y}");
               
        }

        public event Handler Touch;
        public void OnTouch(IPhysics entity1, IPhysics entity2)
        {
            Handler handler = Touch;

            if (null != handler) handler(entity1, entity2, EventArgs.Empty);
        }

        public bool IsCollisionVertical(IPhysics entity1, IPhysics entity2)
        {
            if(Rectangle.Intersect(entity1.Body, entity2.Body).Width > Rectangle.Intersect(entity1.Body, entity2.Body).Height)
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

        public void UpdatePosition(IPhysics entity)
        {
            entity.Velocity = new Vector2(entity.Velocity.X * entity.Friction, entity.Velocity.Y * entity.Friction);

            //Debug.WriteLine(entity.Body.X);
            entity.Body = new Rectangle((int)(entity.Body.X + entity.Velocity.X), (int)(entity.Body.Y + entity.Velocity.Y), entity.Body.Width, entity.Body.Height);
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
            Debug.WriteLine("Vertical Collision");
        }

        public void AdditativeImpactHorizontal(IPhysics entity, double amount, double direction)
        {
            amount *= 2;
            entity.Velocity += new Vector2((float)(amount * Math.Cos(direction)*-1), (float)(amount * Math.Sin(direction)));
            Debug.WriteLine("Horizontal Collision");
        }

        public double GetSpeed(IPhysics entity)
        {
            return Math.Sqrt(Math.Pow(entity.Velocity.X, 2) + Math.Pow(entity.Velocity.Y, 2));
        }

        public double GetDirection(IPhysics entity)
        {
            return Math.Atan2(entity.Velocity.Y, entity.Velocity.X);
        }

        


        public bool RectOverlap(Vector2 positionA, Vector2 sizeA, Vector2 positionB, Vector2 sizeB)
        {
            if (positionA.X < sizeB.X && sizeA.X > positionB.X && positionA.Y > sizeB.Y && sizeA.Y < positionB.Y)
                return true;
            else
                return false;
        }
    }
}
