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
    class GameTitle : Scene
    {
        private static int colorValue;  // マウスが重なった時にボタンの色が暗くなるやつ
        public GameTitle(GameManager gameManager)
            : base(gameManager)
        {

        }

        public override void Initialize()
        {
            nextScene = SceneID.GamePlay;
            isEnd = false;

            // エネミーを30体出す
            for (int i = 0; i < 60; i++)
            {
                GameObjectManager.Add(new Enemy(gameManager));
            }

            // 餌を300個数
            for (int i = 0; i < 300; i++)
            {
                GameObjectManager.Add(new Food(gameManager));
            }

            //// 針を5個設置
            //for (int i = 0; i < 20; i++)
            //{
            //    GameObjectManager.Add(new Needle(gameManager));
            //}

            Camera.Position = Screen.MapHalf;

            Camera.Zoom = new Vector2(0.5f);
        }

        public override void Update(GameTime gameTime)
        {
            //CompVector2.RectInCheck(input.MouseVector2(), Screen.HalfScreen - new Vector2(295.0f, 0.0f), Screen.HalfScreen + new Vector2(295.0f, 189.0f))
            if (CompVector2.RectInCheck(input.MouseVector2(), Screen.HalfScreen - new Vector2(295.0f, 0.0f), Screen.HalfScreen + new Vector2(295.0f, 189.0f)))
            {
                colorValue = 100;
                if (input.LeftClick())
                {
                    isEnd = true;
                }
            }
            else
            {
                colorValue = 0;
            }
        }

        public override void MatrixDraw(GameTime gameTime)
        {
            renderer.DrawTexture(CharacterID.Grid, Vector2.Zero);
        }

        public override void Draw(GameTime gameTime)
        {
            renderer.DrawTexture(CharacterID.gamePlay, Screen.HalfScreen - new Vector2(295, 0), new Color(255 - colorValue, 255 - colorValue, 255 - colorValue));
        }

        public override void End()
        {
            GameObjectManager.Clear();
        }
    }
}
