using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Utility;

namespace Agar.io_modoki_
{
    abstract class Scene
    {
        protected GameManager gameManager;
        protected Renderer renderer;
        protected Sound sound;
        protected InputState input;
        protected Option option;
        protected bool isGameObjClear = false;
        protected SceneID nextScene = SceneID.None;
        protected bool isEnd = false;
        protected Motion motion = new Motion();
        protected Camera camera;

        public Scene(GameManager gameManager)
        {
            this.gameManager = gameManager;
            renderer = gameManager.Renderer;
            sound = GameManager.Sound;
            input = gameManager.InputState;

            option = new Option();
            camera = gameManager.Camera;
        }

        public virtual void Initialize() { }

        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// カメラの影響を受ける描画
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void MatrixDraw(GameTime gameTime);

        /// <summary>
        /// カメラの影響を受けない描画
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Draw(GameTime gameTime) { }

        public virtual void End() { }

        public bool IsGameObjectClear { get => isGameObjClear; }

        public SceneID GetNextSceneID { get => nextScene; }

        public bool IsEnd { get => isEnd; }

        public static string SelectStage { get; set; }

    }
}
