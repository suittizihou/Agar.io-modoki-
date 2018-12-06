using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Agar.io_modoki_
{
    abstract class Collision
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="transform">トランスフォームの情報</param>
        public Collision(Transform transform)
        {
            GetTransform = transform;
        }
        public Transform GetTransform { get; }
    }
}
