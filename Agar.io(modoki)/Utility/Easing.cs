using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EasingDict
{
    static class Easing
    {
        // 序盤遅く、徐々に加速
        public static float EaseInQuad(float time)
        {
            return time * time;
        }

        // 序盤遅く、徐々に加速
        public static float EaseInCubic(float time)
        {
            return time * time * time;
        }

        // 序盤遅く、徐々に加速
        public static float EaseInQuart(float time)
        {
            return time * time * time * time;
        }

        // 序盤遅く、徐々に加速
        public static float EaseInQuint(float time)
        {
            return time * time * time * time * time;
        }

        // 序盤遅く、徐々に加速
        public static float EaseInExpo(float time)
        {
            return (float)(Math.Pow(2, 10 * (time - 1)));
        }

        // 序盤遅く、中盤速く、終盤また遅く
        public static float EaseInOutQuad(float time)
        {
            time *= 2f;
            if (time < 1) return time * time * 0.5f;
            time -= 1f;
            return (time * (time - 2) - 1) * -0.5f;
        }

        // 序盤遅く、中盤速く、終盤また遅く
        public static float EaseInOutCubic(float time)
        {
            time *= 2f;
            if (time < 1f) return 0.5f * time * time * time;
            time -= 2f;
            return (time * time * time + 2f) * 0.5f;
        }

        // 序盤遅く、中盤速く、終盤また遅く
        public static float EaseInOutQuart(float time)
        {
            time *= 2f;
            if (time < 1) return 0.5f * time * time * time * time;
            time -= 2f;
            return -0.5f * (time * time * time * time - 2f);
        }

        // 序盤遅く、中盤速く、終盤また遅く
        public static float EaseInOutQuint(float time)
        {
            time *= 2f;
            if (time < 1f) return 0.5f * time * time * time * time * time;
            time -= 2f;
            return 0.5f * (time * time * time * time * time + 2f);
        }

        /// <summary>
        /// 序盤遅く、中盤速く、終盤遅い
        /// </summary>
        /// <param name="time"></param>
        /// <param name="start"></param>
        /// <param name="difference"></param>
        /// <param name="totalTime"></param>
        /// <returns></returns>
        public static float EaseInOutExponential(float time)
        {
            time *= 2f;
            if (time < 1f) return (float)(0.5 * Math.Pow(2, 10 * (time - 1)));
            time -= 1f;
            return (float)(0.5 * (-Math.Pow(2, -10 * time) + 2));
        }

        // 序盤速く、徐々に減速
        public static float EaseOutCubic(float time)
        {
            time -= 1f;
            return time * time * time + 1;
        }

        /// <summary>
        /// 序盤速く、徐々に減速
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static float EaseOutQuart(float time)
        {
            time -= 1f;
            return -1f * (time * time * time * time - 1);
        }

        // 序盤速く、徐々に減速
        public static float EaseOutQuint(float time)
        {
            time -= 1f;
            return time * time * time * time * time + 1f;
        }

        // 序盤速く、徐々に減速
        public static float EaseOutExpo(float time)
        {
            return (float)(-Math.Pow(2, -10 * time) + 1);
        }

        // 序盤から速い。最後は一度、目標値を通り越してから、目標値に戻る。
        // overshoot : 通り越す量
        public static float EaseOutBack(float time, float overshoot = 1f)
        {
            float s = 1.70158f * overshoot;
            time -= 1f;
            return (time * time * ((s + 1f) * time + s) + 1f);
        }

        // 一度、開始値から逆走してから、目標値に向かって一気に動く。後ろにバックしてパワーを溜めてからはじけるようなイメージ。
        // overshoot : 開始時にバックする量
        public static float EaseInBack(float time, float overshoot = 1f)
        {
            float s = 1.70158f * overshoot;
            return time * time * ((s + 1f) * time - s);
        }

        // EaseOutBackとEaseInBackの両方の特徴をあわせ持つ。
        // 後ろにバックしてパワーを溜めて、はじけるように移動し、最後は一旦目標値を通り越してから、目標値に戻る。
        // overshoot : 開始時にバックする量、および終了時に通り越す量
        public static float EaseInOutBack(float time, float overshoot = 1f)
        {
            float s = 1.70158f * overshoot;
            time *= 2f;
            if (time < 1f)
            {
                s *= (1.525f) * overshoot;
                return 0.5f * (time * time * ((s + 1f) * time - s));
            }
            time -= 2f;
            s *= (1.525f * overshoot);
            return 0.5f * (time * time * ((s + 1f) * time + s) + 2f);
        }

        // スーパーボールのように、目標地点でバウンドするような動き。
        public static float EaseOutBounce(float time)
        {
            if (time < (1f / 2.75f))
            {
                return 7.5625f * time * time;
            }
            else if (time < (2f / 2.75f))
            {
                time -= (1.5f / 2.75f);
                return 7.5625f * (time) * time + .75f;
            }
            else if (time < (2.5f / 2.75))
            {
                time -= (2.25f / 2.75f);
                return 7.5625f * (time) * time + .9375f;
            }
            else
            {
                time -= (2.625f / 2.75f);
                return 7.5625f * (time) * time + .984375f;
            }
        }

        // バネのようにビヨビヨーンとなる動き。
        // overshoot : 振れ幅の大きさ。
        // period : 周波数。値が小さいほど小刻みに揺れ、値が大きいほどゆったりと揺れる。
        public static float EaseOutElastic(float time, float overshoot = 1f, float period = 1f)
        {
            period /= 4f;
            float s = period / 4f;

            if (overshoot > 1f && time < 0.4f)
                overshoot = 1f + (time / 0.4f * (overshoot - 1f));

            return (float)(1 + Math.Pow(2, -10 * time) * Math.Sin((time - s) * (2 * Math.PI) / period) * overshoot);
        }



        // ライセンス

        /*
        Terms of Use: Easing Functions (Equations)
        Open source under the MIT License and the 3-Clause BSD License.

        MIT License
        Copyright © 2001 Robert Penner

        Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

        The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

        THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

        BSD License
        Copyright © 2001 Robert Penner

        Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

        Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
        Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
        Neither the name of the author nor the names of contributors may be used to endorse or promote products derived from this software without specific prior written permission.
        THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
        */
    }
}
