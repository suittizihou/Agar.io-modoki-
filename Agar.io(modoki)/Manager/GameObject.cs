using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Utility;

namespace Agar.io_modoki_
{
    abstract class GameObject
    {
        public readonly Transform transform = new Transform();
        protected DrawStruct drawStruct = new DrawStruct(CharacterID.None, Vector2.Zero);
        private Collision_Manager collisionManager = new Collision_Manager();
        private List<GameObject> HitObject = new List<GameObject>();
        //protected Pause pause;            // 今回のゲームには使用しない(たぶん)
        protected GameManager gameManager;
        protected InputState input;
        protected Renderer renderer;
        protected Sound sound;
        protected GamePadState padSate;
        protected Random random;
        protected Collision collision;
        protected Motion motion = new Motion();
        protected Animation animation = new Animation();
        public bool isDead = false;
        protected bool isNotClear = false;
        protected ObjectID id;

        // 衝突フラグ
        public bool Flag { get; set; }

        /// <summary>
        /// UnityのTagと一緒
        /// </summary>
        public CharacterID Tag { get; set; }

        public GameObject(GameManager gameManager)
        {
            this.gameManager = gameManager;
            renderer = gameManager.Renderer;
            sound = GameManager.Sound;
            input = gameManager.InputState;
            random = gameManager.Random;
            padSate = gameManager.PadState;
        }
        public virtual void Initialize() { }
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// ランキングアップデート(キャラクターのアップデートとタイミングをずらすため別個で記述)
        /// </summary>
        public virtual void RankingUpdate() { }
        public void ListRemove()
        {
            HitObject.Clear();
        }

        /// <summary>
        /// カメラ処理の影響を受ける描画処理
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void MatrixDraw(GameTime gameTime)
        {
            MatrixDrawUpdate();
            renderer.DrawTexture(drawStruct);
        }

        /// <summary>
        /// カメラ処理の影響を受けない描画処理
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Draw(GameTime gameTime) { }

        /// <summary>
        /// 当たったオブジェクトを決定
        /// </summary>
        /// <param name="other"></param>
        public void CollideDecision(List<GameObject> other)
        {
            foreach(var i in other)
            {
                if (i == this) continue;
                collisionManager.OnCollide(collision, i.collision);
                if (collisionManager.IsCollide)
                {
                    Flag = true;
                    HitObject.Add(i);
                }
            }
        }

        /// <summary>
        /// 当たったキャラクターのReactを実行
        /// </summary>
        public void Collide()
        {
            foreach(var i in HitObject)
            {
                collisionManager.OnCollide(collision, i.collision);
                if (collisionManager.IsCollide)
                {
                    React(i);
                    i.React(this);
                }
            }
        }
        public void Dead() => isDead = true;
        public virtual void React(GameObject other) { }
        public bool IsDead { get => isDead; }
        public bool IsNotClear { get => isNotClear; }
        public Collision GetCollision { get => collision; }
        public ObjectID GetObjectID { get => id; }
        public static SceneID GetScene { get; set; }
        /// <summary>
        /// 吸収していいかのチェック
        /// </summary>
        public bool AbsorptionCheck { get; set; }

        /// <summary>
        /// トランスフォームの情報をDrawStruct構造体に入れる(カメラ影響受けるやつ)
        /// </summary>
        protected void MatrixDrawUpdate()
        {
            drawStruct.position = transform.Position;
            drawStruct.centerPos = transform.CenterPosition;
            drawStruct.scale = transform.Scale;
            drawStruct.angle = transform.Angle;
        }

        /// <summary>
        /// 同上(カメラ影響受けない)
        /// </summary>
        protected void DrawUpdate()
        {
            drawStruct.position = transform.Position;
            drawStruct.centerPos = transform.CenterPosition;
            drawStruct.scale = transform.Scale;
            drawStruct.angle = transform.Angle;
        }
    }
}
