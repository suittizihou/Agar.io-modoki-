using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Utility;

namespace Agar.io_modoki_
{
    class Enemy : GameObject
    {
        enum EnemyMoveMode
        {
            NormalMove,
            EatMove,
        }

        private Vector2 randPos;    // ランダムで行く先を決定するときの入れ物
        private Vector2 radius;     // 半径
        private Vector2 scale;      // 大きさ
        private float speed;        // スピード
        private int randCount;      // 行く先を決定するまでのインターバルをランダムで決定
        private int findFoodInterval;// 餌を探すインターバル
        private int findFoodCount;  // 餌を探すインターバルカウント
        private int intervalCount;  // 単純なカウンタ
        private EnemyMoveMode mode; // エネミーがどの行動をするか決定するもの

        private Vector2 findFoodPos;    // 見つけた餌の座標を入れておくためのリスト

        public Enemy(GameManager gameManager)
            : base(gameManager)
        {
            id = ObjectID.Character;
            Tag = CharacterID.Enemy;
            drawStruct.textureName = Tag;
            drawStruct.color = new Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
            transform.Position = new Vector2(random.Next(0, Screen.MapWidth), random.Next(0, Screen.MapHeight));
            radius = new Vector2(16f);
            speed = 2f;
            scale = new Vector2((float)random.NextDouble() * (5 - 1) + 1);

            mode = EnemyMoveMode.NormalMove;

            findFoodInterval = random.Next(180, 600);    // ３～１０秒の間で固有のインターバルを決定

            //// ゲームプレイで呼ばれたら非同期処理開始
            //if(GetScene == SceneID.GamePlay)
            //{
            //    FindFood();
            //}
        }

        public override void Update(GameTime gameTime)
        {
            collision = new CircleCollision(CompVector2.ScaleConversion(scale, radius).X, transform);

            transform.CenterPosition = radius;

            transform.Scale = scale;


            if (findFoodInterval <= findFoodCount && GetScene == SceneID.GamePlay)
            {
                // 餌が近くにないか探す
                FindFood();

                // インターバル + 0.1秒した値よりカウントが大きくなったらリセット
                if (findFoodInterval + 10 <= findFoodCount) { findFoodCount = 0; }
            }
            findFoodCount++;

            // エネミーのモードによって行動を決定
            switch (mode)
            {
                // 通常行動
                case EnemyMoveMode.NormalMove:
                    EnemyMove(gameTime);
                    break;

                // 近くに食べ物が見つかればこの行動に移る
                case EnemyMoveMode.EatMove:
                    FoodEatMove();
                    break;
            }

            transform.Position = Clamp(transform.Position, CompVector2.ScaleConversion(scale, radius));
        }

        /// <summary>
        /// エネミーの動きに関するもの
        /// </summary>
        /// <param name="gameTime"></param>
        private void EnemyMove(GameTime gameTime)
        {
            if (randCount < intervalCount)
            {
                // １秒 ～ １０秒の間でインターバルを決定
                randCount = random.Next(1000, 10000);
                randPos = Vector2.Normalize(
                    transform.Position - new Vector2(
                        random.Next(0, Screen.MapWidth - (int)(CompVector2.ScaleConversion(scale, radius).X)),
                        random.Next(0, Screen.MapHeight - (int)(CompVector2.ScaleConversion(scale, radius).Y))));
                intervalCount = 0;
            }
            transform.Position -= randPos * Speed(speed, scale.X);

            // インターバルをカウント
            intervalCount += gameTime.ElapsedGameTime.Milliseconds;
        }

        /// <summary>
        /// 餌を見つける
        /// </summary>
        private void FindFood()
        {
            List<Vector2> FoodPos = new List<Vector2>();

            // 餌を入れる
            foreach (GameObject food in GameObjectManager.FindAll(CharacterID.Food))
            {
                // 餌と自分との距離が３０１ドット以上離れているならリストに入れない(軽くするため、、、たぶん軽くなるはず)
                if(Vector2.Distance(food.transform.Position, transform.Position + CompVector2.ScaleConversion(scale, radius)) >= 301f) { continue; }

                // 餌の座標を個別に保存する
                FoodPos.Add(food.transform.Position);
            }

            if (FoodPos.Count != 0)
            {
                // ソート(バグあり)
                FoodPos = FoodPos.OrderBy(o => CompVector2.MinVector2(transform.Position, o.X, o.Y)).ToList();

                // 見つけた餌の座標の自分から一番近いところを入れる
                findFoodPos = FoodPos[0];

                // 餌との距離が0ドット未満になったら中をクリアする
                if (Vector2.Distance(transform.Position, FoodPos[0]) < 0)
                {
                    FoodPos.RemoveAt(0);
                }
            }

            // 近くに餌があったら食べに行く
            if (FoodPos.Count != 0)
            {
                mode = EnemyMoveMode.EatMove;
            }
        }

        /// <summary>
        /// 自分の近くに餌があったらそれを取りに行く動き
        /// </summary>
        private void FoodEatMove()
        {
            // どんどん餌に近づいていく
            transform.Position -= Vector2.Normalize(transform.Position - findFoodPos) * Speed(speed, scale.X);

            // 自分の座標と餌の座標が５ドット以下なら通常行動に移行
            if(Vector2.Distance(transform.Position, findFoodPos) <= 5f)
            {
                mode = EnemyMoveMode.NormalMove;
            }
        }

        // 大きさに応じたスピード
        protected float Speed(float speed, float scale)
        {
            speed = (-0.02f * scale) + 2;
            return speed;
        }

        /// <summary>
        /// クランプ
        /// 注意：クランプは戻り値で値を制限するから、ポジションに=で入れる必要がある。
        /// </summary>
        protected Vector2 Clamp(Vector2 position, Vector2 radius)
        {
            return Vector2.Clamp(position, radius, Screen.MapFull - radius);
        }


        /// <summary>
        /// プレイヤーと当たった時の処理
        /// </summary>
        /// <param name="other"></param>
        private void CharacterHit(GameObject other, ref Vector2 scale)
        {
            // 相手と自分の大きさの差が0.004f以下なら何もしない
            if (CompVector2.U_DifferenceValue(other.transform.Scale.X, scale.X) <= 0.45f) return;

            // 相手より自分の方が大きければ吸収
            if (other.transform.Scale.X < scale.X)
            {
                // エネミーのスケールを自分のスケールに足す(相手のスケールを２で割る：バランス調整)
                scale += other.transform.Scale / 5;

                // 将来的に敵の半径分を自機の半径に足す機構も作る
                //radius += new Vector2(((CircleCollision)other.GetCollision).Radius) / 2;
            }
            else
            {
                isDead = true;
            }
        }

        /// <summary>
        /// エネミーが針に当たった時に分裂する時の処理
        /// </summary>
        protected void NeedleDivision()
        {

        }

        public override void React(GameObject other)
        {
            switch (other.Tag)
            {
                case CharacterID.Player:
                case CharacterID.Enemy:
                    CharacterHit(other, ref scale);
                    break;

                case CharacterID.Food:
                    scale += Food.AddScale;
                    break;

                case CharacterID.Needle:
                    NeedleDivision();
                    break;
            }
        }
    }
}
