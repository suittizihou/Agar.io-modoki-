using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agar.io_modoki_
{
    class Collision_Manager
    {
        /// <summary>
        /// 円の当たり判定のみ
        /// </summary>
        /// <param name="one">オブジェクト１</param>
        /// <param name="two">オブジェクト２</param>
        public void OnCollide(Collision one, Collision two)
        {
            Collide collide = new Circle_And_Circle();
            IsCollide = collide.OnCollide(one, two);
        }

        /// <summary>
        /// 計算を行った結果をboolで報告、それに伴って各Reactが実行される
        /// </summary>
        public bool IsCollide { get; private set; }
    }
}
