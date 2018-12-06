using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Utility
{
    class Animation
    {
        private InputState inputState = new InputState();
        private double alpha;
        private double alphaPlus;
        private float positionPuls;

        private float baseAlpha;
        private float maxAlpha;
        private float minAlpha;
        private float filAlpha;
        private bool overAlphaMax;
        private bool overAlphaMin = true;

        private void Initialize()
        {
        }

        /// <summary>
        /// フェードインのアニメーション
        /// </summary>
        /// <param name="complete">最終的に表示させたい透明度</param>
        /// <param name="motionTime">何フレームで完成に持っていくか</param>
        /// <returns></returns>
        public double Fade_in(double complete, double motionTime)
        {
            motionTime *= 2;
            alphaPlus = complete / motionTime;  // １フレームどのくらいalpha値を足せばいいのか計算
            if (alpha <= complete)
            {
                return alpha += alphaPlus;
            }
            else                                // 完成以上の値になったらcompleteを入れて値を固定
            {
                return alpha = complete;
            }
        }

        /// <summary>
        /// ゆっくり上に上げるアニメーション
        /// </summary>
        /// <param name="basePosition">元のロゴの座標</param>
        /// <param name="complete">止めたい座標</param>
        /// <param name="motionTime">何フレームで完成に持っていくか</param>
        /// <returns></returns>
        public float Fade_up(float basePosition, float complete, float motionTime)
        {
            motionTime *= 2;
            positionPuls = complete / motionTime;
            if (basePosition > complete)
            {
                return basePosition -= positionPuls;
            }
            else
            {
                return basePosition = complete;
            }
        }

        /// <summary>
        /// 薄くしたり濃くしたりさせるもの
        /// </summary>
        /// <param name="baseAlpha">最初のalpha値</param>
        /// <param name="maxAlpha">透明度の最大値</param>
        /// <param name="minAlpha">透明度の最小値</param>
        /// <param name="filAlpha">１フレームどれだけ透明度を変更させるか</param>
        /// <returns></returns>
        public void InitializeAlphaMortion(float baseAlpha, float maxAlpha ,float minAlpha ,float filAlpha)
        {
            this.baseAlpha = baseAlpha;
            this.maxAlpha = maxAlpha;
            this.minAlpha = minAlpha;
            this.filAlpha = filAlpha;
        }
        public float AlphaUpdate()
        {
            if (baseAlpha <= maxAlpha && overAlphaMin)
            {
                baseAlpha += filAlpha;
                overAlphaMax = true;
                overAlphaMin = false;
            }
            else if (baseAlpha >= minAlpha && overAlphaMax)
            {
                baseAlpha -= filAlpha;
                overAlphaMin = true;
                overAlphaMax = false;
            }
            return baseAlpha;
        }

    }
}
