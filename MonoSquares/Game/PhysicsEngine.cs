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
        private List<IPhysics> Entities;
        private List<IPhysics> NonThinkingEntities;

        public PhysicsEngine()
        {
            Entities = new List<IPhysics>();

        }

        public void BindEntity(IPhysics entity)
        {
            if(entity.PhysicsType==0)
                NonThinkingEntities.Add(entity);
            else if (entity.PhysicsType == 1)
                Entities.Add(entity);
        }

        public void Think()
        {
            foreach(var entity in Entities)
            {
                foreach (var collidableEntity in GetCollidableEntities())
                {
                    if (entity.Body.Intersects(collidableEntity.Body) && entity!=collidableEntity && collidableEntity.IsSolid && entity.IsSolid)
                    {
                        Debug.WriteLine("Collided!");

                        if(entity.PhysicsType==1)
                        {
                            CreateCollision(entity, collidableEntity);
                        }
                    }
                }
            }
        }

        private void CreateCollision(IPhysics entity1, IPhysics entity2)
        {
            if (IsCollisionVertical(entity1, entity2))
            {
                bool verticalCollision = true;
            }

                Vector2 tempVelocity = entity1.Velocity;
                double tempSpeed = Math.Sqrt(Math.Pow(tempVelocity.X, 2) + Math.Pow(tempVelocity.Y, 2));
                double tempDir = GetDirection(entity1);
                
                //Debug.WriteLine(tempSpeed);

                entity1.Velocity = Vector2.Zero;
                AdditativeImpactReflective(entity1, 5, tempDir + Math.PI);
                //player.Velocity *= -2;
                entity1.Collided = true;
                //player.Acceleration = 0.01f;

                Debug.WriteLine($"COLLISION AT X {entity1.Body.X} | Y {entity1.Body.Y}");
               
        }

        public bool IsCollisionVertical(IPhysics entity1, IPhysics entity2)
        {
            /* Used to determine what corner overlaps
            bool top=false, left=false;

            if(entity1.Body.X > entity2.Body.Center.X) //Entity 1 is on the right side
                left = false;

            if (entity1.Body.X<entity2.Body.Center.X) //Entity 1 is on the left side
                left = true;

            if (entity1.Body.Y<entity2.Body.Center.Y) //Entity 1 is on the top side
                top = true;

            if (entity1.Body.Y > entity2.Body.Center.Y) //Entity 1 is on the bottom side
                top = false;
            */

            if(Rectangle.Intersect(entity1.Body, entity2.Body).Width > Rectangle.Intersect(entity1.Body, entity2.Body).Height)
            {
                return true;
            }
            else
            {
                return false;
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
            entity.Position += entity.Velocity;
            //body.X = (int)Position.X;
            //body.Y = (int)Position.Y;
        }

        public void AdditativeImpact(IPhysics entity, double amount, double direction)
        {
            entity.Velocity += new Vector2((float)(amount * Math.Cos(direction)), (float)(amount * Math.Sin(direction)));
        }

        public void AdditativeImpactReflective(IPhysics entity, double amount, double direction)
        {
            entity.Velocity += new Vector2((float)(amount * Math.Cos(direction)*-1), (float)(amount * Math.Sin(direction)));
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
