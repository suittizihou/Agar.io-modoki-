using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Agar.io_modoki_
{
    class Circle_And_Circle : Collide
    {
        private CircleCollision one, two;
        public override bool OnCollide(Collision one, Collision two)
        {
            this.one = (CircleCollision)one;
            this.two = (CircleCollision)two;
            return Vector2.Distance(this.one.GetTransform.Position, this.two.GetTransform.Position) < this.one.Radius + this.two.Radius;
        }
    }
}
