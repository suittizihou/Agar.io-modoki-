using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Utility;

namespace Agar.io_modoki_
{
    class SceneManager
    {
        private Dictionary<SceneID, Scene> scenes = new Dictionary<SceneID, Scene>();
        private Scene _scene;
        private GameManager gameManager;
        private Renderer renderer;
        private InputState input;
        private Sound sound;
        private Motion motion = new Motion();
        private bool isChange = false;

        public SceneManager(GameManager gameManager)
        {
            this.gameManager = gameManager;
            input = gameManager.InputState;
            renderer = gameManager.Renderer;
            sound = GameManager.Sound;
        }
        public void Update(GameTime gameTime)
        {
            if (isChange)    // IsEndからtrueが返ってきたら
            {
                isChange = false;
            }
            input.Update();
            sound.VolumeUpdate();
            _scene.Update(gameTime);
            GameObjectManager.Update(gameTime);
            Scene_End();
        }

        public void Change(SceneID id)
        {
            GameObject.GetScene = id;
            if (_scene != null) { if (_scene.IsGameObjectClear) GameObjectManager.Clear(); }
            if (id == SceneID.None) gameManager.IsExit = true;
            _scene = scenes[id];
            _scene.Initialize();
            isChange = true;
        }
        public void DrawLine()
        {
            renderer.LineDraw();
        }
        public void Draw(GameTime gameTime)
        {
            // カメラの効果を受ける描画
            renderer.MatixBegin();

            _scene.MatrixDraw(gameTime);
            GameObjectManager.MatrixDraw(gameTime);

            renderer.End();
            //////////////////////////////////////////////////////////////////////////////////

            // カメラの効果を受けない描画
            renderer.Begin();

            _scene.Draw(gameTime);
            GameObjectManager.Draw(gameTime);

            renderer.End();
        }
        public void AddScene(SceneID id, Scene scene)
        {
            scenes.Add(id, scene);
        }

        private void Scene_End()
        {
            if(_scene.IsEnd && _scene.GetNextSceneID != SceneID.None)
            {
                _scene.End();
                Change(_scene.GetNextSceneID);
            }
        }
    }
}
