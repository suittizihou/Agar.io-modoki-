using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Utility;

namespace Agar.io_modoki_
{
    /// <summary>
    /// フィールドにいっぱいある小さな餌
    /// </summary>
    class Food : GameObject
    {
        //private int addListPos; // このオブジェクトの座標が保存されたリストの番地を入れるための物
        private float radius;

        public Food(GameManager gameManager)
            : base(gameManager)
        {
            id = ObjectID.Food;
            Tag = CharacterID.Food;
            drawStruct.textureName = CharacterID.Food;

            // Foodの色を決定
            drawStruct.color = new Color(random.Next(0,255), random.Next(0,255), random.Next(0,255));
            radius = 4f;
            
            // 中心座標を決定
            drawStruct.centerPos = new Vector2(radius);

            // 初期座標を設定
            transform.Position = new Vector2(random.Next((int)radius, Screen.MapWidth), random.Next((int)radius, Screen.MapHeight));

            // 丸い当たり判定を付ける
            collision = new CircleCollision(radius, transform);
        }
        public override void Update(GameTime gameTime)
        {
            // 更新処理はしない
        }

        public override void React(GameObject other)
        {
            switch (other.Tag)
            {
                case CharacterID.Player:
                case CharacterID.Enemy:
                    // エネミーとプレイヤーに当たると死ぬ
                    isDead = true;
                    break;
            }
        }

        // 初期状態から餌を24個食べると分裂できるようになる数値（分裂は大きさ２以上でできる）
        public static Vector2 AddScale { get => new Vector2(0.021f); }
    }
}
