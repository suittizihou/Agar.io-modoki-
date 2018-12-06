using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using EasingDict;
using Utility;

namespace Agar.io_modoki_
{
    /// <summary>
    /// 分裂したときに出てくるプレイヤー
    /// </summary>
    class DivisionPlayer : Player
    {
        private Vector2 radius;
        private float bornRadian;   // 生まれた時にマウスがあった方向に進むためのもの
        private Vector2 course;
        private Vector2 needleHitReact; // 針に当たった時の飛び散る方向
        private Vector2 futureScale;    // なりたい大きさ（イージングのため）5
        private Vector2 currentScale;   // 今の大きさ
        private Motion scaleMotion;
        private int scaleAddTime = 10;
        private int elapsedTime;
        private float mySpeed;
        private bool needleHitbone;
        private int divisionCount;      // 分裂してからの時間経過をカウントする処理
        private int absorptionCount;

        private CompVector2 compVector2;

        public DivisionPlayer(Vector2 scale, Vector2 radius, float speed, Vector2 position, GameManager gameManager, float x = 0, float y = 0, bool needleHitbone = false)
            : base(gameManager)
        {
            compVector2 = new CompVector2();

            id = ObjectID.Character;
            Tag = CharacterID.Player;
            drawStruct.textureName = CharacterID.Player;
            this.radius = radius;
            transform.Position = position;
            mySpeed = (speed * scale.X) * 4f;
            bornRadian = (float)compVector2.Radian(position, input.WorldMouseVector2());
            course = new Vector2((float)Math.Cos(bornRadian),
                                 (float)Math.Sin(bornRadian));
            currentScale = scale;
            futureScale = currentScale;

            needleHitReact = new Vector2(x, y);
            this.needleHitbone = needleHitbone;

            scaleMotion = Motion.Stopped;
        }
        public override void Update(GameTime gameTime)
        {
            transform.CenterPosition = radius;

            // 大きさ
            transform.Scale = currentScale;
            EagingScale();

            // 半径が変わるので当たり判定も１フレーム毎にチェック
            collision = new CircleCollision(CompVector2.ScaleConversion(currentScale, radius).X, transform);

            // 移動
            Move();

            // 任意の分裂
            Division(ref futureScale, ref currentScale, transform.Position);

            // クランプ
            transform.Position = Clamp(transform.Position, CompVector2.ScaleConversion(currentScale, radius));

#if DEBUG
            // デバッグ用
            if (input.IsKeyDown(Keys.A))
            {
                isDead = true;
            }
#endif
        }

        /// <summary>
        /// 移動
        /// </summary>
        private void Move()
        {
            // 針に当たって生まれたときはHitMoveの処理を先にする
            if (needleHitbone) NeedleHitMove();

            // 生まれた時はマウスの方向に行こうとする力がプレイヤーよりも強い
            if (mySpeed > Speed(speed, currentScale.X))
            {
                transform.Position += course * mySpeed;

                // どんどん力を弱めていく
                mySpeed -= mySpeed / 12;
            }
            // 自分のスピードが大きさに応じた速さ以下になったら
            else
            {
                // 大きさに応じたスピードを直接入れる
                mySpeed = Speed(speed, currentScale.X);

                // 自分の座標とマウスの座標が5ドット以上の場合マウスに付いてくる
                if (Vector2.Distance(transform.Position, input.WorldMouseVector2()) >= 2)
                {
                    transform.Position -= Vector2.Normalize(transform.Position - input.WorldMouseVector2()) * mySpeed;
                }
                // 1ドット以下になるとマウスの座標に直接くっつく
                else
                {
                    transform.Position = input.WorldMouseVector2();
                }
            }
        }

        /// <summary>
        /// デカくなる時の緩急処理
        /// </summary>
        private void EagingScale()
        {
            if (transform.Scale.X < futureScale.X)
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
                if (elapsedTime < scaleAddTime)
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

                    scaleMotion = Motion.Stopped;
                }
                // 経過時間をカウント
                elapsedTime++;
            }
        }

        private void NeedleHitMove()
        {
            transform.Position -= Vector2.Normalize(transform.Position - needleHitReact) * mySpeed;
        }

        /// <summary>
        /// 吸収される処理
        /// </summary>
        private void Absorption()
        {
            if(divisionCount >= 600)
            {
                AbsorptionCheck = true;
            }
        }

        ///// <summary>
        ///// 自分の分裂は一定時間たたないと入ることができなくなる
        ///// </summary>
        //private void BlockInvasion(GameObject other)
        //{
        //    transform.Position += Vector2.Normalize(transform.Position - other.transform.Position) * Speed(speed, currentScale.X);
        //}

        public override void React(GameObject other)
        {
            switch (other.Tag)
            {
                // 自分達と当たった時の反応
                case CharacterID.Player:
                    ++divisionCount;
                    ++absorptionCount;
                    transform.Position += BlockInvasion(transform.Position, Speed(speed, currentScale.X), AbsorptionCheck, other);
                    Absorption();
                    // 吸収されてもいいフラグが建ってる＆カウントが5以上
                    if (other.AbsorptionCheck && absorptionCount >= 5)
                    {
                        absorptionCount = 0;
                        currentScale += other.transform.Scale;
                        other.AbsorptionCheck = false;
                        other.isDead = true;
                    }
                    break;

                // 敵と当たった時の反応
                case CharacterID.Enemy:
                    EnemyHit(other, ref futureScale);
                    break;

                case CharacterID.Food:
                    futureScale += Food.AddScale;
                    break;

            }
        }
    }
}
