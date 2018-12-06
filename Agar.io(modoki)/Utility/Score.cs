using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Agar.io_modoki_;

namespace Utility

{
    class Score 
    {
        private int score;


        public Score()
        {
            Initialize();
        }
        public void Initialize()
        {
            score = GameManager.Score;
        }
        public void Add()
        {
            score = GameManager.Score;
        }
        public void Draw(Renderer renderer,Vector2 position)
        {
            //スコアの文字
            //renderer.DrawTexture("score", position);
            //スコアの数字
            //renderer.DrawNumber("number", position, score);
        }
    }
}