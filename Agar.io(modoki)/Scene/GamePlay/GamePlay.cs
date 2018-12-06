using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Utility;

namespace Agar.io_modoki_
{
    class GamePlay : Scene
    {
        private Player player;
        private Score score;
        private int x = 0;
        private int enemyAddCount;  // エネミーが時間がたつと一匹出てくるやつのカウンター
        private const int enemyAddInterval = 5000;   // エネミーが追加されるまでの時間(5秒)
        private const int foodMaxCount = 400;        // 餌の最大個数

        private int foodAddInterval = 500;          // GameTime換算で0.5秒
        private int foodAddCount;

        public GamePlay(GameManager gameManager)
            : base(gameManager)
        {
        }
        public override void Initialize()
        {
            nextScene = SceneID.GameOver;
            isEnd = false;

            GameObjectManager.Add(new Ranking(gameManager));

            player = (Player)GameObjectManager.Add(new Player(gameManager));

            // エネミーを生成
            for (int i = 0; i < 60; i++)
            {
                GameObjectManager.Add(new Enemy(gameManager));
            }

            // 餌を生成
            for (int i = 0; i < 200; i++)
            {
                GameObjectManager.Add(new Food(gameManager));
            }

            //// 針を生成
            //for(int i = 0; i < 5; i++)
            //{
            //    GameObjectManager.Add(new Needle(gameManager));
            //}
        }

        /// <summary>
        /// 餌を定期的に追加
        /// </summary>
        /// <returns></returns>
        /// 
        //private void RunTask()
        //{
        //    TaskFactory taskFactory = new TaskFactory();
        //    CancellationTokenSource tokenSource = new CancellationTokenSource();

        //    Task task = taskFactory.StartNew(() =>
        //    {
        //        while (true)
        //        {
        //            // キャンセル要求が来ていたらOperationCanceledException例外をスロー
        //            tokenSource.Token.ThrowIfCancellationRequested();

        //            GameObjectManager.Add(new Food(gameManager));

        //            Task.Delay(500);
        //        }
        //    }, tokenSource.Token);

        //    if (sceneEndCheck)
        //    {
        //        try
        //        {
        //            // キャンセル要求出す
        //            tokenSource.Cancel();

        //            // タスクがキャンセルされるまで待機
        //            task.Wait();
        //        }
        //        catch (AggregateException)
        //        {
        //            // タスクがキャンセルされるとここが実行される
        //        }
        //    }
        //}

        // 更新
        public override void Update(GameTime gameTime)
        {
            CameraGroup();

            // 敵を定期的に追加する処理を実行
            EnemyAdd(gameTime);
            // 餌を定期的に追加する処理を実行
            FoodAdd(gameTime);

            PlayerNullCheck();

            Console.WriteLine(CompVector2.SumScale(CharacterID.Player));
        }


        public override void MatrixDraw(GameTime gameTime)
        {
            renderer.DrawTexture(CharacterID.Grid, Vector2.Zero);
        }

        /*////////////////////////////////////////////
         * 
         * 
         *ここから下はUpdataに書いた処理が書かれてます
         * 
         * 
        /* /////////////////////////////////////////*/

        /// <summary>
        /// カメラのズームと動き
        /// </summary>
        private void CameraGroup()
        {
            // カメラの初期値
            Camera.Position = Screen.HalfScreen;

            if (CompVector2.SumScale(CharacterID.Player).X <= 17.0f)
            {
                if (!input.IsKeyPush(Keys.Space))
                {
                    // 初期カメラズーム値 / キャラクターのScaleの合計でカメラのズームを決める
                    Camera.Zoom = new Vector2(4f) / CompVector2.SumScale(CharacterID.Player);
                }
            }
            else
            {
                Camera.Zoom = new Vector2(0.23f);
            }

            // カメラを移動させる
            camera.MoveVector2(-(Screen.HalfScreen - CompVector2.ObjectsCenterPos(CharacterID.Player)));
        }

        /// <summary>
        /// エネミーが一定時間ごとに追加される処理
        /// </summary>
        /// <param name="gameTime"></param>
        private void EnemyAdd(GameTime gameTime)
        {
            // エネミーが一定時間で追加される
            if (enemyAddInterval <= enemyAddCount)
            {
                GameObjectManager.Add(new Enemy(gameManager));
                enemyAddCount = 0;
            }
            // カウンタ
            enemyAddCount += gameTime.ElapsedGameTime.Milliseconds;
        }

        private void PlayerNullCheck()
        {
            if(GameObjectManager.FindAll(CharacterID.Player).Count == 0)
            {
                GameObjectManager.Clear();
                isEnd = true;
            }
        }

        /// <summary>
        /// 餌を定期的に追加
        /// </summary>
        /// <returns></returns>
        private void FoodAdd(GameTime gameTime)
        {
            if (foodAddInterval <= foodAddCount)
            {
                GameObjectManager.Add(new Food(gameManager));
                foodAddCount = 0;
            }
            foodAddCount += gameTime.ElapsedGameTime.Milliseconds;
        }

        /*////////////////////////////////////////////
         * 
         * 
         *ここまでがUpdatetに書かれた処理です
         * 
         * 
        /* /////////////////////////////////////////*/

        // 終了処理
        public override void End()
        {
            GameObjectManager.Clear();
        }

    }
}
