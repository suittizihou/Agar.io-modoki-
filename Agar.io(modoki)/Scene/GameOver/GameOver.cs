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
    class GameOver : Scene
    {
        public GameOver(GameManager gameManager)
            : base(gameManager)
        {

        }

        public override void Initialize()
        {
            nextScene = SceneID.GameTitle;
            isEnd = false;
        }


        public override void Update(GameTime gameTime)
        {
            if (input.IsKeyDown(Keys.Space))
            {
                isEnd = true;
            }
        }

        public override void MatrixDraw(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            renderer.DrawTexture(CharacterID.TitleReturn, Screen.HalfScreen - new Vector2(282.0f, 0.0f));
        }

        public override void End()
        {
            GameObjectManager.Clear();
        }
    }
}
