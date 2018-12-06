//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Xna.Framework;
//using Utility;

//namespace Agar.io_modoki_
//{
//    class Needle : GameObject
//    {
//        private float radius;

//        public Needle(GameManager gameManager)
//            : base(gameManager)
//        {
//            id = ObjectID.Needle;
//            Tag = CharacterID.Needle;
//            drawStruct.textureName = Tag;

//            radius = 64f;

//            transform.Position = new Vector2(random.Next((int)radius, Screen.MapWidth), random.Next((int)radius, Screen.MapHeight));
//            transform.CenterPosition = new Vector2(radius);

//            collision = new CircleCollision(radius, transform);
//        }

//        public override void Update(GameTime gameTime)
//        {
            
//        }
//        ///// <summary>
//        ///// プレイヤーが針に当たった時に分裂する時の処理
//        ///// </summary>
//        //protected void NeedleDivision(GameObject other)
//        //{
//        //    // 針じゃないならリターン
//        //    if (!(other is Needle)) return;

//        //    if (currentScale.X > 2.1f)
//        //    {
//        //        int n = 8;
//        //        float[] x = new float[n];
//        //        float[] y = new float[n];
//        //        float[] vx = new float[n];
//        //        float[] vy = new float[n];
//        //        float space = 0;

//        //        for (int i = 0; i < n; i++)
//        //        {
//        //            vx[i] = (float)Math.Cos(space) * speed;
//        //            vy[i] = (float)Math.Sin(space) * speed;
//        //            GameObjectManager.Add(new DivisionPlayer(currentScale, radius, speed, transform.Position, gameManager, vx[i], vy[i], true));
//        //            space += 3.14f * 2 / n;
//        //        }
//        //    }
//        //    else
//        //    {
//        //        for (int i = 0; i < 16; i++)
//        //        {

//        //        }
//        //    }
//        //}

//        /// <summary>
//        /// 細胞が当たった時にどんどん大きさが増えていく
//        /// </summary>
//        private void AddScale()
//        {
//            transform.Scale += new Vector2(1.2f);
//            radius += 76.8f;

//            collision = new CircleCollision(radius, transform);
//        }

//        public override void React(GameObject other)
//        {
//            switch (other.Tag)
//            {
//                case CharacterID.BigFood:
//                    AddScale();
//                    break;

//                case CharacterID.Player:
//                    if (other.transform.Scale.X >= 2f)  { isDead = true; }
//                        break;
//            }
//        }
//    }
//}
