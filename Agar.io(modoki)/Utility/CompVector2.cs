using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Agar.io_modoki_;

namespace Utility
{
    /// <summary>
    /// Vector2クラスの補完クラス
    /// </summary>
    class CompVector2
    {
        private static Random rand = new Random();
        private static Vector2 lastCenterPos;   // 最後の群れの中心を入れるための変数
        private static Vector2 lastScaleSum;    // 最後のScaleの大きさを入れるための変数

        private static Vector2 currentValue;
        private static Vector2 previousValue;

        /// <summary>
        /// 2Dでの外積を求める（XY平面）
        /// a * b = a1b2 - a2b1
        /// 2D的に計算。z軸は無視したものと考える
        /// 外積 = 0 のとき、両ベクトルは平行（0または180度）
        /// 外積 ＞ 0 のとき、transform.up を基準にすると左側
        ///  外積 ＜ 0 のとき、transform.up を基準にすると右側
        /// </summary>
        /// <param name="value1">基準の方向ベクトル</param>
        /// <param name="value2">対象の方向ベクトル</param>
        /// <returns>2Dの外積</returns>
        public static float Cross2D(Vector2 value1, Vector2 value2)
        {
            return value1.X * value2.Y - value1.Y * value2.X;
        }

        /// <summary>
        /// 二点間からラジアンを求める
        /// </summary>
        /// <returns></returns>
        public double Radian(Vector2 value1, Vector2 value2)
        {
            double y = value2.Y - value1.Y;
            double x = value2.X - value1.X;

            return Math.Atan2(y, x);
        }

        public double Degrees(Vector2 value1, Vector2 value2)
        {
            return MathHelper.ToDegrees((float)Radian(value1, value2));
        }

        /// <summary>
        /// 指定された同じIDのオブジェクト座標の平均から中心点を求める
        /// </summary>
        /// <param name="tag">群の中心を知りたいオブジェクトのID</param>
        /// <returns></returns>
        public static Vector2 ObjectsCenterPos(CharacterID tag)
        {
            // 見つけてきたキャラクターのX,Y座標を入れるためのもの
            List<float> possX = new List<float>();
            List<float> possY = new List<float>();

            foreach(GameObject chara in GameObjectManager.FindAll(tag))
            {
                possX.Add(chara.transform.Position.X);
                possY.Add(chara.transform.Position.Y);
            }

            // 見つからなかったら最後の値を返す
            if (GameObjectManager.FindAll(tag).Count == 0)
            {
                return lastCenterPos;
            }
            else
            {
                // X,Yの平均をVector2にまとめて返す
                return lastCenterPos = new Vector2(possX.Average(), possY.Average());
            }
        }

        /// <summary>
        /// 指定されたキャラクターのScaleを全部足して返す
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static Vector2 SumScale(CharacterID tag)
        {
            List<float> scaleX = new List<float>();
            List<float> scaleY = new List<float>();

            foreach (GameObject chara in GameObjectManager.FindAll(tag))
            {
                scaleX.Add(chara.transform.Scale.X);
                scaleY.Add(chara.transform.Scale.Y);
            }

            // 見つからなかったら最後の値を返す
            if (GameObjectManager.FindAll(tag).Count == 0)
            {
                return lastScaleSum;
            }
            else
            {
                // X,Yの平均をVector2にまとめて返す
                return lastScaleSum = new Vector2(scaleX.Sum(), scaleY.Sum());
            }
        }

        /// <summary>
        /// 自分の座標と比べてX,Y軸のうち小さいほうを返す(LINQ用)
        /// </summary>
        /// <param name="position"></param>
        /// <param name="valueX"></param>
        /// <param name="valueY"></param>
        /// <returns></returns>
        public static float MinVector2(Vector2 position , float valueX, float valueY)
        {
            // 同じなら
            if(position.X - valueX == position.Y - valueY)
            {
                // Xの値を返す
                return valueX;
            }
            else if(position.X - valueX < position.Y - valueY)
            {
                return valueX;
            }
            else
            {
                return valueY;
            }
        }

        /// <summary>
        /// 自分の座標と比べてX,Y軸のうち大きいほうを返す(LINQ用)
        /// </summary>
        /// <param name="position"></param>
        /// <param name="valueX"></param>
        /// <param name="valueY"></param>
        /// <returns></returns>
        public static float MaxVector2(Vector2 position , float valueX, float valueY)
        {
            // 同じなら
            if(position.X - valueX == position.Y - valueY)
            {
                 // Xの値を返す
                return valueX;
            }
            else if(position.X - valueX > position.Y - valueY)
            {
                return valueX;
            }
            else
            {
                return valueY;
            }
        }

        /// <summary>
        /// 引数は(, を ＜＝ と置き換えて)Value1がValue2より画面左上に近ければtrue(x, yどっちかでもvalue2より小さいとtrue)
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static bool LeftUpCheck(Vector2 value1, Vector2 value2)
        {
            bool x = value1.X <= value2.X;
            bool y = value1.Y <= value2.Y;

            return x || y;
        }

        /// <summary>
        /// 引数は(, を ＞＝ と置き換えて)Value1がValue2より画面右下に近ければtrue(x, yどっちかでもvalue2より大きいとtrue)
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static bool RightDownCheck(Vector2 value1, Vector2 value2)
        {
            bool x = value1.X >= value2.X;
            bool y = value1.Y >= value2.Y;

            return x || y;
        }

        /// <summary>
        /// 四角内にvalue1が入ってればtrue
        /// </summary>
        /// <param name="position"></param>
        /// <param name="leftUp"></param>
        /// <param name="rightDown"></param>
        /// <returns></returns>
        public static bool RectInCheck(Vector2 position, Vector2 leftUp, Vector2 rightDown)
        {
            return !LeftUpCheck(position, leftUp) && !RightDownCheck(position, rightDown);
        }

        /// <summary>
        /// １フレーム前から値が変更されていたらtrueを返す
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ChangeValueCheck(Vector2 value)
        {
            // 今の値を入れる
            previousValue = currentValue;
            
            // 今の値に引数を入れる
            currentValue = value;

            // 前の値と今の値が同じならfalse
            if (previousValue == currentValue)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 符号なしの差
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static float U_DifferenceValue(float value1, float value2)
        {
            float value = value1 - value2;

            // 符号が付いてるなら無くす
            return Math.Abs(value);
        }

        /// <summary>
        /// 左辺の値よりも右辺の値が指定した数より大きければtrue、小さければfalse
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static bool BoolDifference(float value1, float value2, float difference)
        {
            float value = value1 - value2;
            bool boolValue = false;

            if (value < difference)
            {
                boolValue = false;
            }
            else
            {
                boolValue = true;
            }

            if(value1 < value2)
            {
                return boolValue;
            }
            else
            {
                return !boolValue;
            }
        }

        /// <summary>
        /// スケール係数を半径に変換
        /// </summary>
        public static Vector2 ScaleConversion(Vector2 scale, Vector2 radius)
        {
            return scale * radius;
        }

        /// <summary>
        /// 二つの値のうちどっちかランダムな値を返す
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static float RandomValue(float value1, float value2)
        {
            if(rand.Next(0, 1) == 0)
            {
                return value1;
            }
            else
            {
                return value2;
            }
        }
    }
}
