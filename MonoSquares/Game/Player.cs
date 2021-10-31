using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace MonoSquares
{
    class Player : GameObject
    {
        public Player()
        {
            Acceleration = 0.8f;
            MaxSpeed = 2.0f;
            Friction = 0.90f;
            PhysicsType = 1;
            IsSolid = true;
            Damage = 3;
            Health = 1000;
        }


        public override void OnTouch(object entity1, object entity2, EventArgs e)
        {
            GameObject ent1 = (GameObject)entity1;
            GameObject ent2 = (GameObject)entity2;

            if (ent2.PhysicsType == 1)
            {
                Health -= ent2.Damage;

                Debug.WriteLine($"Health: {ent1.Health}");

                Debug.WriteLine($"Health: {ent2.Health}");
            }


                
        }
    }

 
}
