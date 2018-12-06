using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Utility;
using EasingDict;

namespace Agar.io_modoki_
{
    /// <summary>
    /// 大元のプレイヤー
    /// </summary>
    class Player : GameObject
    {
        /// <summary>
        /// スケールをいじるイージングでの処理
        /// </summary>
        protected enum Motion
        {
            Moving,
            Stopped,
        }

        private Vector2 radius;
        private Vector2 futureScale;    // なりたい大きさ（イージングのため）
        private Vector2 currentScale;   // 今の大きさ
        private float subScale;
        private Motion scaleMotion;   // イージング処理をするか決定するためのやつ
        private int scaleAddTime = 10;// 吸収してデカくなる時のデカくなる時間
        private int elapsedTime;      // 経過時間カウント用
        protected float speed;
        private int absorptionCount;  // 吸収インターバルカウント

        public Player(GameManager gameManager)
            : base(gameManager)
        {
            id = ObjectID.Character;
            Tag = CharacterID.Player;
            drawStruct.textureName = CharacterID.Player;
            radius = new Vector2(35f / 2f);
            transform.Position = new Vector2(Screen.MapHalfWidth, Screen.MapHalfHeight);
            currentScale = new Vector2(1f);
            futureScale = currentScale;

            scaleMotion = Motion.Stopped;

        }

        public override void Initialize()
        {
            speed = 2f;
        }

        public override void Update(GameTime gameTime)
        {
            // 中心を新しくセット
            transform.CenterPosition = radius;

            // スケールを毎フレーム代入(transform.Scaleがプロパティでrefが使えないため)
            transform.Scale = currentScale;
            EagingScale();

            collision = new CircleCollision(CompVector2.ScaleConversion(currentScale, radius).X, transform);

            // プレイヤーの移動
            PlayerMove();

            // 任意の分裂
            Division(ref futureScale, ref currentScale, transform.Position);

            // クランプ
            transform.Position = Clamp(transform.Position, CompVector2.ScaleConversion(currentScale, radius));


            // デバッグ用
#if DEBUG
            if (input.IsKeyDown(Keys.T))
            {
                // スケール係数を１ずつ足す
                futureScale += new Vector2(1f);
            }
#endif
        }

        /// <summary>
        /// プレイヤーの移動に関するもの
        /// </summary>
        private void PlayerMove()
        {
            // S押してると止まるよ(デバッグ用)
            if (input.IsKeyPush(Keys.S)) { return; }

            // 自分の座標とマウスの座標が2ドット以上の場合マウスに付いてくる
            if (Vector2.Distance(transform.Position, input.WorldMouseVector2()) >= 2)
            {
                transform.Position -= Vector2.Normalize(transform.Position - input.WorldMouseVector2()) * Speed(speed, currentScale.X);
                GetPlayerSpeed = Speed(speed, currentScale.X);
            }
            // 自分とマウスの座標が同じになったらマウスにピッタリくっ付く
            else
            {
                transform.Position = input.WorldMouseVector2();
            }
            GetPos = transform.Position;
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
        /// デカくなる時の緩急処理
        /// </summary>
        private void EagingScale()
        {
            if(transform.Scale.X < futureScale.X)
            {
                scaleMotion = Motion.Moving;
                scaleAddTime /= (int)transform.Scale.X;
            }
            else
            {
                scaleMotion = Motion.Stopped;
            }

            if (scaleMotion == Motion.Stopped)
            {
                elapsedTime = 0;
            }
            else
            {
                if(elapsedTime < scaleAddTime)
                {
                    // 時間経過を０～１で表す
                    float timeRate = (float)elapsedTime / scaleAddTime;
                    // イージング処理
                    timeRate = Easing.EaseOutQuint(timeRate);
                    // ラープ処理
                    currentScale = Vector2.Lerp(currentScale, futureScale, timeRate);
                }
                else
                {
                    currentScale = futureScale;

                    subScale += currentScale.X / 2;

                    scaleMotion = Motion.Stopped;
                }
                // 経過時間をカウント
                elapsedTime++;
            }
        }

        /// <summary>
        /// 自分の体に入ってこようとする自分の分裂から逃げる処理
        /// </summary>
        protected Vector2 BlockInvasion(Vector2 position, float speed, bool absorptionCheck, GameObject other)
        {
            // 吸収されてもいい個体ならめり込んでいく
            if (absorptionCheck)
            {
                return new Vector2(0.0f);
            }
            else
            {
                return Vector2.Normalize(position - other.transform.Position) / 2 * speed;
            }
        }
        /// <summary>
        /// 大きさに応じたスピードの増減の計算
        /// </summary>
        protected float Speed(float speed, float scale)
        {
            // y = ax + b
            speed = (-0.02f * scale) + 2;
            return speed;
        }

        /// <summary>
        /// プレイヤーがスペースキーを押したときに分裂する処理
        /// </summary>
        protected void Division(ref Vector2 futureScale, ref Vector2 currentScale, Vector2 position)
        {
            // スペースが押されたら　＆＆　大きさが２以上
            if (input.IsKeyDown(Keys.Space) && currentScale.X > 2f && GameObjectManager.FindAll(CharacterID.Player).Count < 8)
            {
                // 分裂すると大きさが半分になる
                currentScale /= 2;
                futureScale /= 2;

                // 大きさが１を下回ったら１を代入（大きさが１より下に行かないように）
                if (currentScale.X < 1f)
                {
                    currentScale = new Vector2(1f);
                    futureScale = currentScale;
                }
                GameObjectManager.Add(new DivisionPlayer(currentScale, radius, Speed(speed, currentScale.X), position, gameManager));
            }
        }

        // エネミーに当たった時の反応
        protected void EnemyHit(GameObject other, ref Vector2 scale)
        {
            // 相手と自分の大きさの差が0.45f以下なら何もしない
            if (CompVector2.U_DifferenceValue(other.transform.Scale.X, scale.X) <= 0.45f) return;

            // 相手より自分の方が大きければ吸収
            if (other.transform.Scale.X < scale.X)
            {
                // エネミーのスケールを自分のスケールに足す(相手のスケールを5で割る：バランス調整)
                scale += other.transform.Scale / 5;
            }
            else
            {
                isDead = true;
            }
        }

        public override void React(GameObject other)
        {
            // 当たったもの
            switch (other.Tag)
            {
                case CharacterID.Player:
                    // 吸収のカウント
                    ++absorptionCount;
                    transform.Position += BlockInvasion(transform.Position, Speed(speed, currentScale.X), AbsorptionCheck, other);
                    if (other.AbsorptionCheck && absorptionCount >= 5)
                    {
                        absorptionCount = 0;
                        currentScale += other.transform.Scale;
                        other.AbsorptionCheck = false;
                        other.isDead = true;
                    }
                    break;

                // 針（画面にある緑色のイガイガみたいなの）
                case CharacterID.Needle:
                    //isDead = true;
                    break;

                // エネミーに当たった時の
                case CharacterID.Enemy:
                    EnemyHit(other, ref currentScale);
                    break;

                case CharacterID.Food:
                    futureScale += Food.AddScale;
                    break;
            }
        }

        // プレイヤーのスピードを取得
        protected float GetPlayerSpeed { get; private set; } 

        // 座標取得 
        public static Vector2 GetPos { get; private set; }
    }
}
