using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Utility
{
    class Motion
    {

        private int minIndex;   //範囲の最初
        private int maxIndex;   //範囲の最後+1
        private int interval;   //モーションの間隔
        private int counter;    //カウンター
        private int currentIndex;   //今のモーション番号
        private bool reverse;
        private Rectangle rectangle;
        //表示位置を番号で管理する
        //Dictionaryを使えば登録の順番を気にしなくてもよい
        private Dictionary<int, Rectangle> rectangles = new Dictionary<int, Rectangle>();
        private int minIndex2;   //範囲の最初
        private int maxIndex2;   //範囲の最後+1
        private int interval2;   //モーションの間隔
        private int counter2;    //カウンター
        private int currentIndex2;   //今のモーション番号
        private bool reverse2;
        private Rectangle rectangle2;
        //表示位置を番号で管理する
        //Dictionaryを使えば登録の順番を気にしなくてもよい
        private Dictionary<int, Rectangle> rectangles2 = new Dictionary<int, Rectangle>();

        //newした状態ではアニメーションしない
        public Motion()
        {
            Initialize(0, 0, 0);
            Initialize2(0, 0, 0);
        }
        //指定した番号から番号の間を順に表示していく
        //間隔は60で１秒
        public void Initialize(int minIndex, int maxIndex, int interval)
        {
            this.minIndex = minIndex;
            this.maxIndex = maxIndex;
            this.interval = interval;
            counter = interval;		//カウンター
            currentIndex = minIndex;	//今のモーション番号
            //AddRectangle();
        }
        public void Initialize2(int minIndex, int maxIndex, int interval)
        {
            this.minIndex2 = minIndex;
            this.maxIndex2 = maxIndex;
            this.interval2 = interval;
            counter2 = interval;		//カウンター
            currentIndex2 = minIndex;	//今のモーション番号
            //AddRectangle();
        }
        //表示範囲の登録
        public void AddRectangle(int width = 64, int height = 64)
        {
            for(int i = 0; i < maxIndex; i++)
            {
                rectangle = new Rectangle(width * i, 0, width, height);
                rectangles[i] = rectangle;
            }
            //rectangles[index] = rectangle;
        }
        //表示範囲の登録
        public void AddRectangle2(int width = 64, int height = 64)
        {
            for(int i = 0; i < maxIndex2; i++)
            {
                rectangle2 = new Rectangle(width * i, 0, width, height);
                rectangles2[i] = rectangle2;
            }
            //rectangles[index] = rectangle;
        }
        /// <summary>
        /// 多段式用のモーションメソッド
        /// </summary>
        /// <param name="picture_Y_Sheets">Y軸にある画像の枚数</param>
        /// <param name="picture_X_Sheets">X軸にある画像の枚数</param>
        /// <param name="width">画像の幅</param>
        /// <param name="height">画像の高さ</param>
        public void AddDoubleRectangle(int picture_Y_Sheets, int picture_X_Sheets, int width = 64, int height = 64)
        {
            int z = 0;
            for(int i = 0; i < picture_Y_Sheets; i++) // 何段あるか
            {
                for(int j = 0; j < picture_X_Sheets; j++) // X軸にある画像の枚数
                {
                    rectangle = new Rectangle(width * j, height * i, width, height);
                    rectangles[z] = rectangle;
                    z++;
                }
            }
        }
        public void AddDoubleRectangle2(int picture_Y_Sheets, int picture_X_Sheets, int width = 64, int height = 64)
        {
            int z = 0;
            for(int i = 0; i < picture_Y_Sheets; i++) // 何段あるか
            {
                for(int j = 0; j < picture_X_Sheets; j++) // X軸にある画像の枚数
                {
                    rectangle2 = new Rectangle(width * j, height * i, width, height);
                    rectangles2[z] = rectangle2;
                    z++;
                }
            }
        }
        //カウンターとモーション番号の処理
        public void Update()
        {
            if (minIndex >= maxIndex) return;//変化しない
            counter--;
            if (counter <= 0)//番号を増やす
            {
                counter = interval;
                currentIndex++;
                if (currentIndex >= maxIndex)//番号を元に戻す
                {
                    currentIndex = minIndex;
                }
            }
        }
        public void Update2()
        {
            if (minIndex2 >= maxIndex2) return;//変化しない
            counter2--;
            if (counter2 <= 0)//番号を増やす
            {
                counter2 = interval2;
                currentIndex2++;
                if (currentIndex2 >= maxIndex2)//番号を元に戻す
                {
                    currentIndex2 = minIndex2;
                }
            }
        }
        
        public void ReverseUpdate()
        {
            if (minIndex >= maxIndex) return;
            counter--;
            if(counter <= 0)
            {
                counter = interval;
                if(!reverse)
                {
                    currentIndex++;
                    if(currentIndex == maxIndex)
                    {
                        reverse = true;
                    }
                }
                else if (reverse)
                {
                    currentIndex--;
                    if(currentIndex == minIndex)
                    {
                        reverse = false;
                    }
                }
            }
        }
        public void ReverseUpdate2()
        {
            if (minIndex2 >= maxIndex2) return;
            counter2--;
            if(counter2 <= 0)
            {
                counter2 = interval2;
                if(!reverse2)
                {
                    currentIndex2++;
                    if(currentIndex2 == maxIndex2)
                    {
                        reverse2 = true;
                    }
                }
                else if (reverse2)
                {
                    currentIndex2--;
                    if(currentIndex2 == minIndex2)
                    {
                        reverse2 = false;
                    }
                }
            }
        }
        //今の表示範囲を取り出す。
        public Rectangle CurrentRectangle()
        {
            return rectangles[currentIndex];
        }
        public Rectangle CurrentRectangle2()
        {
            return rectangles2[currentIndex2];
        }
        public Rectangle ReverseRectangle()
        {
            return rectangles[currentIndex];
        }
        public Rectangle ReverseRectangle2()
        {
            return rectangles2[currentIndex2];
        }
    }
}
