using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Utility;

namespace Agar.io_modoki_
{
    class CircleCollision : Collision
    {
        public float Radius { get; set; }
        public CircleCollision(float radius, Transform transform)
            : base(transform)
        {
            Radius = radius;
        }
    }
}
