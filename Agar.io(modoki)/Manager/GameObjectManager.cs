using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Utility;

namespace Agar.io_modoki_
{
    class GameObjectManager
    {
        /// <summary>
        /// オブジェリスト
        /// </summary>
        private static List<GameObject> objList = new List<GameObject>();
        private static List<GameObject> objTemp = new List<GameObject>();
        private static List<GameObject> find = new List<GameObject>();

        /// <summary>
        /// GameObjectの更新
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Update(GameTime gameTime)
        {
            objList.AddRange(objTemp);
            objTemp.Clear();
            objList.Sort((a, b) => (int)a.GetObjectID - (int)b.GetObjectID);
            objList.ForEach(obj => obj.Update(gameTime));   // それぞれのObjectが持つUpdateを実行
            objList.ForEach(obj => obj.ListRemove());
            Collide();      // 当たり判定
            HitBehavior();  // React実行
            OutRemove();    // 画面外にでたら消す処理
            objList.ForEach(obj => obj.RankingUpdate());
            IsDead();
        }

        /// <summary>
        /// GameObjectの描画
        /// </summary>
        /// <param name="gameTime"></param>
        public static void MatrixDraw(GameTime gameTime)
        {
            objList.ForEach(obj => obj.MatrixDraw(gameTime));
        }

        public static void Draw(GameTime gameTime)
        {
            objList.ForEach(obj => obj.Draw(gameTime));
        }

        /// <summary>
        /// GameObjectのIsDeadがTrueになったら削除
        /// </summary>
        public static void IsDead()
        {
            // isDeadがtrueになってるものを見つけてきてfindに入れる
            var find = objList.FindAll(obj => obj.IsDead);
            // それを全部回す
            find.ForEach(
                obj =>
                {
                    // 親クラスと繋がってる子供は全部消す
                    objList.RemoveAll(child => child.transform.Parent == obj.transform);

                    // objに入ってるやつを消す
                    objList.Remove(obj);
                });
        }

        /// <summary>
        /// GameObjectの当たり判定(colllisionがnullなら判定しない)
        /// </summary>
        private static void Collide()
        {
            var collider = objList.FindAll(obj => obj.GetCollision != null);
            for (int i = 0; i < collider.Count; i++)
            {
                collider[i].CollideDecision(collider);
            }
        }

        /// <summary>
        /// 全部の当たり判定処理が終わった後にReactを実行するためのもの
        /// </summary>
        private static void HitBehavior()
        {
            var collider = objList.FindAll(obj => obj.GetCollision != null);
            for (int i = 0; i < collider.Count; i++)
            {
                if (collider[i].Flag)   // フラグがtrueになっていれば中身が実行されReactの処理が行われる。
                {
                    collider[i].Collide();
                }
            }
        }
        /// <summary>
        /// Tagの名前が一致しているのもを1つ取得する
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>見つからなければnullを返す</returns>
        public static GameObject Find(CharacterID tag)
        {
            find.AddRange(objTemp);
            find.AddRange(objList);
            GameObject find_obj = find.Find(obj => obj.Tag == tag);
            find.Clear();
            return find_obj;
        }
        /// <summary>
        /// Class名が一致しているもの一つを取得する
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static GameObject FindClass(string className)
        {
            find.AddRange(objTemp);
            find.AddRange(objList);
            GameObject find_obj = find.Find(obj => obj.GetType().Name == className);
            find.Clear();
            return find_obj;
        }
        /// <summary>
        /// Tagの名前が一致しているすべてを取得する。
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>見つからなければ空のリストを返す</returns>
        public static List<GameObject> FindAll(CharacterID tag)
        {
            find.AddRange(objTemp);
            find.AddRange(objList);
            List<GameObject> find_list = find.FindAll(obj => obj.Tag == tag);
            find.Clear();
            return find_list;
        }

        //public static List<Vector2> CharacterPos(CharacterID tag)
        //{
        //    find.AddRange(objTemp);
        //    find.AddRange(objList);
        //    List<GameObject> find_pos = find.FindAll(obj => obj.Tag == tag);
            

        //    return 
        //}

        //public static void IsDead()
        //{
        //    var find = objList.FindAll(obj => obj.IsDead);
        //    find.ForEach(
        //        obj =>
        //        {
        //            objList.RemoveAll(child => child.transform.Parent == obj.transform);
        //            objList.Remove(obj);
        //        });
        //}


        public static List<GameObject> Children(Transform parent)
        {
            find.AddRange(objTemp);
            find.AddRange(objList);
            List<GameObject> find_list = find.FindAll(child => child.transform.Parent == parent);
            find.Clear();
            return find_list;
        }

        /// <summary>
        /// GameObjectの追加
        /// </summary>
        /// <param name="add">GameObject</param>
        /// <param name="parent">親のGameObject</param>
        /// <returns></returns>
        public static GameObject Add(GameObject add, Transform parent = null)
        {
            add.transform.Parent = parent;
            objTemp.Add(add);
            add.Initialize();
            return add;
        }

        /// <summary>
        /// IsNotClearがtrueのもの以外をすべて削除
        /// </summary>
        public static void Clear()
        {
            objList.RemoveAll(obj => !obj.IsNotClear);
        }

        /// <summary>
        /// 同じタグ名のオブジェを一つ死亡させる。
        /// </summary>
        /// <param name="tag"></param>
        public static void Remove(CharacterID tag)
        {
            GameObject gameObject = objList.Find(obj => obj.Tag == tag);
            gameObject.Dead();
        }

        /// <summary>
        /// 画面外に一定量出たら消すよ
        /// </summary>
        public static void OutRemove()
        {
            int outLine = 50;
            var find = objList.FindAll(
                obj => obj.transform.Position.X < (0 - outLine) || (Screen.MapWidth + outLine) < obj.transform.Position.X ||
                      (obj.transform.Position.Y < (0 - outLine) || (Screen.MapHeight + outLine) < obj.transform.Position.Y));
            find.ForEach(
                obj =>
                {
                    objList.RemoveAll(child => child.transform.Parent == obj.transform);
                    objList.Remove(obj);
                });
        }

        /// <summary>
        /// 同じタグ名のオブジェを全て死亡させる。
        /// </summary>
        /// <param name="tag"></param>
        public static void RemoveAll(CharacterID tag)
        {
            var find = FindAll(tag);
            find.ForEach(obj => obj.Dead());
        }

        /// <summary>
        /// 全て削除する
        /// </summary>
        public static void ClearAll()
        {
            objList.Clear();
        }

    }
}