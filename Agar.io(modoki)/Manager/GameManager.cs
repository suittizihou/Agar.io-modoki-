using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Utility;

namespace Agar.io_modoki_
{
    /// <summary>
    /// SoundやRendererなどの機能を簡単に実装させる為の物的な
    /// </summary>
    class GameManager
    {
        private static int _hiScore;
        public bool IsExit = false;

        /// <summary>
        /// 入力クラス
        /// </summary>
        public InputState InputState { get; private set; }

        /// <summary>
        /// ゲームパッドスティック
        /// </summary>
        public GamePadThumbSticks Sticks { get; private set; }

        /// <summary>
        /// ゲームパッド
        /// </summary>
        public GamePadState PadState { get; private set; }

        /// <summary>
        /// 描画クラス
        /// </summary>
        public Renderer Renderer { get; private set; }

        /// <summary>
        /// 音楽クラス
        /// </summary>
        public static Sound Sound { get; private set; }

        /// <summary>
        /// ゲームタイム
        /// </summary>
        public GameTime GameTime { get; private set; }

        /// <summary>
        /// 乱数
        /// </summary>
        public Random Random { get; private set; }

        /// <summary>
        /// スコア
        /// </summary>
        public static int Score { get; set; }

        /// <summary>
        /// カメラ
        /// </summary>
        public Camera Camera { get; private set; }

        /// <summary>
        /// スコアのセット
        /// </summary>
        /// <param name="score"></param>
        public static void SetScore(int score)
        {
            Score += score;
        }

        /// <summary>
        /// ハイスコアの取得
        /// </summary>
        /// <returns></returns>
        private static int GetHiScore()
        {
            return _hiScore;
        }

        /// <summary>
        /// ハイスコアをセット
        /// </summary>
        /// <param name="hiScore"></param>
        public static void SetHiScore(int hiScore)
        {
            _hiScore = hiScore;
        }

        // コンストラクタ
        public GameManager(ContentManager content, GraphicsDevice graphics) // GameManagerを呼べばいつでもここに書いてある機能を使えるようにする。
        {
            InputState = new InputState();
            Sticks = new GamePadThumbSticks();
            PadState = new GamePadState();
            Renderer = new Renderer(content,graphics);
            Sound = new Sound(content);
            GameTime = new GameTime();
            Random = new Random();
            Camera = new Camera(Player.GetPos);
        }

        /// <summary>
        /// コンテンツのロード
        /// </summary>
        public void LoadContent()   // データの読み込み
        {
            // キャラクター
            Renderer.LoadTexture(CharacterID.Player);
            Renderer.LoadTexture(CharacterID.debugPlayer);
            Renderer.LoadTexture(CharacterID.Enemy);

            // フィールド
            Renderer.LoadTexture(CharacterID.Grid);
            Renderer.LoadTexture(CharacterID.CrossLine);

            // 餌
            Renderer.LoadTexture(CharacterID.Food);

            // 針
            Renderer.LoadTexture(CharacterID.Needle);

            // UI
            Renderer.LoadTexture(CharacterID.Number);

            // タイトル
            Renderer.LoadTexture(CharacterID.titleBack);

            // テキスト
            Renderer.LoadTexture(CharacterID.gamePlay);
            Renderer.LoadTexture(CharacterID.TitleReturn);

            // BGM
            // Sound.LoadBGM("");

            // SE
            // Sound.LoadSE("");


            // シェーダー
            Renderer.LoadShader("File");

            // ラインの頂点データのロード
            Renderer.VertexLoad();
        }

        /// <summary>
        /// コンテンツの削除
        /// </summary>
        public void UnloadContent()     // いらなくなったデータを処分
        {
            Renderer.UnloadContent();
            Sound.UnloadContent();
        }
    }
}
