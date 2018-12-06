using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Agar.io_modoki_;

namespace Utility
{
    class Option
    {
        private static float bgmVolume = 0.5f;      // BGMの初期値
        private static float seVolume = 0.5f;       // SEの初期値
        private static float voiceVolume = 0.5f;    // VOICEの初期値
        private float volumeControl = 0.05f;        // 音量の下げ幅、上げ幅
        private readonly int maxVolumePos = 676;    // ボリュームカーソルの最大ｘ座標
        private readonly int minVolumePos = 391;    // ボリュームカーソルの最小ｘ座標
        private static int choiceOption;            // どのオプションが選ばれているか
        private static int choiceManu;              // どのメニューが選ばれたか
        private static int choiceBGM = 534;         // ボリュームカーソルのX座標初期値
        private static int choiceSE = 534;
        private static int choiceVoice = 140;
        private Sound sound;

        // 呼び出し元で指定しなくてもこっちで指定できるようにしたいな
        //private InputState input = new InputState();

        public Option()
        {
            sound = GameManager.Sound;
        }
        public void Initialize()
        {
            choiceManu = 0;
        }

        /// <summary>
        /// オプションを表示してるかしてないか
        /// </summary>
        public bool OptionCheck { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="upKeyCheck">上矢印キーが押されているかどうか</param>
        /// <param name="downKeyCheck">下矢印キーが押されているかどうか</param>
        /// <param name="arrowPosition">矢印の初期座標</param>
        /// <param name="maxUpPosition">矢印が上がれる一番上の座標</param>
        /// <param name="minDownPosition">一番下の座標</param>
        /// <param name="moveY">矢印が一度に動く値</param>
        /// <returns></returns>
        /// このクラスを使う場合は使いたいところで矢印のデフォルトのY座標の値が入っている変数に代入する必要がある
        public int OptionArrowChoice(bool upKeyCheck, bool downKeyCheck, int arrowPosition, int maxUpPosition, int minDownPosition, int moveY)
        {
            // 矢印キーで選択
            if (upKeyCheck && arrowPosition > maxUpPosition)
            {
                arrowPosition -= moveY;
                choiceOption--;
                sound.PlaySE("SE_Select2");
            }
            else if (downKeyCheck && arrowPosition < minDownPosition)
            {
                arrowPosition += moveY;
                choiceOption++;
                sound.PlaySE("SE_Select2");
            }
            return arrowPosition;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="upKeyCheck">上矢印キーが押されているかどうか</param>
        /// <param name="downKeyCheck">下矢印キーが押されているかどうか</param>
        /// <param name="arrowPosition">矢印の初期座標</param>
        /// <param name="maxUpPosition">矢印が上がれる一番上の座標</param>
        /// <param name="minDownPosition">一番下の座標</param>
        /// <param name="moveY">矢印が一度に動く値</param>
        /// <returns></returns>
        /// このクラスを使う場合は使いたいところで矢印のデフォルトのY座標の値が入っている変数に代入する必要がある
        public int ArrowChoice(bool upKeyCheck, bool downKeyCheck, int arrowPosition, int maxUpPosition, int minDownPosition, int moveY)
        {
            // 矢印キーで選択
            if (upKeyCheck && arrowPosition > maxUpPosition)
            {
                arrowPosition -= moveY;
                choiceManu--;
                sound.PlaySE("SE_Select2");
            }
            else if (downKeyCheck && arrowPosition < minDownPosition)
            {
                arrowPosition += moveY;
                choiceManu++;
                sound.PlaySE("SE_Select2");
            }
            return arrowPosition;
        }
        public int Manu()
        {
            return choiceManu;
        }

        /// <summary>
        /// BGMのオプション
        /// </summary>
        /// <param name="r">Right</param>
        /// <param name="l">Left</param>
        /// <returns></returns>
        public int BGMBarPosition(bool r, bool l, int moveX)
        {
            if (choiceOption == 0)
            {
                if (r && choiceBGM <= maxVolumePos)
                {
                    choiceBGM += moveX;
                    bgmVolume += volumeControl;
                    if (bgmVolume > 1.0f)
                    {
                        bgmVolume = 1.0f;
                    }
                }
                if (l && choiceBGM >= minVolumePos)
                {
                    choiceBGM -= moveX;
                    bgmVolume -= volumeControl;
                    if (bgmVolume < 0.0f)
                    {
                        bgmVolume = 0.0f;
                    }
                }
            }
            return choiceBGM;
        }

        /// <summary>
        /// SEのオプション
        /// </summary>
        /// <param name="r">Right</param>
        /// <param name="l">Left</param>
        /// <returns></returns>
        public int SEBarPosition(bool r, bool l, int moveX)
        {
            if (choiceOption == 1)
            {
                if (r && choiceSE <= maxVolumePos)
                {
                    choiceSE += moveX;
                    seVolume += volumeControl;
                    if(seVolume > 1.0f)
                    {
                        seVolume = 1.0f;
                    }
                    sound.PlaySE("SE_Select2");
                }
                if (l && choiceSE >= minVolumePos)
                {
                    choiceSE -= moveX;
                    seVolume -= volumeControl;
                    if(seVolume < 0.0f)
                    {
                        seVolume = 0.0f;
                    }
                    sound.PlaySE("SE_Select2");
                }
            }
            return choiceSE;
        }

        /// <summary>
        /// VOISEのオプション
        /// </summary>
        /// <param name="r">Right</param>
        /// <param name="l">Left</param>
        /// <returns></returns>
        public int VOICEBarPosition(bool r, bool l)
        {
            if (choiceOption == 2)
            {
                if (r && choiceVoice <= maxVolumePos)
                {
                    choiceVoice += 10;
                    voiceVolume += volumeControl;
                    if (voiceVolume > 1.0f)
                    {
                        voiceVolume = 1.0f;
                    }
                }
                if (l && choiceVoice >= minVolumePos)
                {
                    choiceVoice -= 10;
                    voiceVolume -= volumeControl;
                    if (voiceVolume < 0.0f)
                    {
                        voiceVolume = 0.0f;
                    }
                }
            }
            return choiceVoice;
        }

        /// <summary>
        /// ボリュームの値を返す
        /// </summary>
        /// <param name="volumeID"> どのボリュームを返すか </param>
        /// <returns></returns>
        public float Volume(VolumeID volumeID)
        {
            switch (volumeID)
            {
                case 0:
                    return bgmVolume;

                case (VolumeID)1:
                    return seVolume;

                default:
                    return voiceVolume;
            }
        }
    }
}
