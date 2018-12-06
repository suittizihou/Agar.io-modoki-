//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;
//using Agar.io_modoki_;

//namespace Utility
//{
//    class Pause : GameObject
//    {
//        private bool sceneStop;

//        / <summary>
//        / ポーズ（Stage1に一度だけAdd。以降殺さず隠す）
//        / </summary>
//        / <param name = "gameManager" ></ param >
//        public Pause(GameManager gameManager)
//            : base(gameManager)
//        {

//        }
//        public override void Initialize()
//        {
//            id = ObjectID.Pause;
//        }
//        public override void Update(GameTime gameTime)
//        {
//        }

//        public override void Draw(GameTime gameTime)
//        {
//            if (sceneStop)
//            {
//                renderer.DrawTexture("pauseBack", Vector2.Zero);
//                renderer.DrawTexture("pause", new Vector2(Screen.Width / 2 - 111.5f, Screen.Height / 7));
//            }
//        }
//        public bool StopTime()
//        {
//            これだけでon off切り替えてる（イツキ式）
//            if (input.IsKeyDown(Keys.T) || input.IsPadDown(Buttons.Start)) sceneStop = !sceneStop;
//            if (!sceneStop) input.StopVivration();
//            GetPause = sceneStop;
//            return sceneStop;
//        }
//        public static bool GetPause { get; set; }
//    }
//}
