using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Utility
{
    static class Screen
    {
        public static readonly int ScreenWidth = 1920;  // 画面横幅
        public static readonly int ScreenHeight = 1080; // 画面縦幅

        public static readonly int ScreenHalfWidth = ScreenWidth / 2;   // 画面横幅の半分
        public static readonly int ScreenHalfHeight = ScreenHeight / 2; // 画面縦幅の半分

        public static readonly Vector2 HalfScreen = new Vector2(ScreenHalfWidth, ScreenHalfHeight); // 画面半分
        public static readonly Vector2 FullScreen = new Vector2(ScreenWidth, ScreenHeight);         // 画面フル

        public static readonly int MapWidth = 4096;    // マップ横幅
        public static readonly int MapHeight = 4096;    // マップ縦幅

        public static readonly int MapHalfWidth = MapWidth / 2;       // マップ横幅の半分
        public static readonly int MapHalfHeight = MapHeight / 2;     // マップ縦幅の半分

        public static readonly Vector2 MapHalf = new Vector2(MapHalfWidth, MapHalfHeight);     // マップ半分
        public static readonly Vector2 MapFull = new Vector2(MapWidth, MapHeight);             // マップフル
    }
}
