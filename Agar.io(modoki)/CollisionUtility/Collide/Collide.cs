using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agar.io_modoki_
{
    abstract class Collide
    {
        public abstract bool OnCollide(Collision one, Collision two);
    }
}
