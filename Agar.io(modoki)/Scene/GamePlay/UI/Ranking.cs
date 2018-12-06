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
    /// <summary>
    /// プレイヤーの順位を調べて描画するクラス
    /// </summary>
    class Ranking : GameObject
    {

        private int playerRank;

        public Ranking(GameManager gameManager)
            : base(gameManager)
        {

        }

        public override void Initialize()
        {
            Tag = CharacterID.Number;
            drawStruct.textureName = CharacterID.Number;
            id = ObjectID.UI;

            // 初期座標S
            transform.Position = new Vector2(Screen.ScreenHalfWidth + 800, 50);
        }
        public override void Update(GameTime gameTime)
        {
            // キャラクター達の更新とタイミングをズラすため、こちらのアップデートには記述しない
        }
        public override void RankingUpdate()
        {
            // enemysにエネミーのデータを入れる
            List<Vector2> scale = new List<Vector2>();

            // エネミー一体一体の大きさを個別に保存
            foreach (var enemy in GameObjectManager.FindAll(CharacterID.Enemy))
            {
                scale.Add(enemy.transform.Scale);
            }

            // スケールでソート
            var scaleVar = scale.OrderByDescending(s => s.X).ToList();

            // Rankingの初期値はエネミーの数
            playerRank = scaleVar.Count;

            // プレイヤーのランクを決定
            for (int enemyNumber = 0; enemyNumber < scaleVar.Count; enemyNumber++)
            {
                // プレイヤー全体の大きさが今調べているエネミーの大きさよりも大きかったら
                if ((CompVector2.SumScale(CharacterID.Player).X > scaleVar[enemyNumber].X))
                {
                    // 現在調べているエネミーのナンバーをプレイヤーに入れる
                    playerRank = enemyNumber;
                    
                    // それ以降は処理しない
                    break;
                }
            }
        }

        public override void MatrixDraw(GameTime gameTime)
        {
            DrawUpdate();
        }

        public override void Draw(GameTime gameTime)
        {
            // プレイヤーのランクを描画
            renderer.DrawNumber(drawStruct, playerRank + 1);
        }
    }
}
