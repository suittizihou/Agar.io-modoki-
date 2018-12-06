using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Agar.io_modoki_;

namespace Utility
{
    class DrawStruct
    {
        public CharacterID textureName;
        public Vector2 position;
        public Rectangle? rectangle;
        public Color color;
        public float angle;
        public Vector2 centerPos;
        public Vector2 scale;
        public float alpha;
        public SpriteEffects effect;

        /// <summary>
        /// 最小構成
        /// </summary>
        /// <param name="name">画像の名前</param>
        /// <param name="position">画像の位置(左上)</param>
        public DrawStruct(CharacterID name, Vector2 position)
        : this(name, position, Color.White, Vector2.Zero, Vector2.One) { }

        /// <summary>
        /// 中心座標の設定
        /// </summary>
        /// <param name="name">画像の名前</param>
        /// <param name="position">centerPosを中心(0,0)にした座標</param>
        /// <param name="centerPos">画像自体の中心座標,(0,0)は画像の左上</param>
        public DrawStruct(CharacterID name, Vector2 position, Vector2 centerPos)
        : this(name, position, Color.White, centerPos, Vector2.One) { }

        /// <summary>
        /// 画像の大きさを変える
        /// </summary>
        /// <param name="name">画像の名前</param>
        /// <param name="position">centerPosを中心(0,0)にした座標</param>
        /// <param name="centerPos">画像自体の中心座標,(0,0)は画像の左上</param>
        /// <param name="scale">画像の大きさ 1.0 = 100%</param>
        public DrawStruct(CharacterID name, Vector2 position, Vector2 centerPos, Vector2 scale)
        : this(name, position, Color.White, centerPos, scale) { }

        /// <summary>
        /// 画像を区切る
        /// </summary>
        /// <param name="name">画像の名前</param>
        /// <param name="position">centerPosを中心(0,0)にした座標</param>
        /// <param name="centerPos">画像自体の中心座標
        /// <param name="rectangle">区切る</param>
        public DrawStruct(CharacterID name, Vector2 position, Vector2 centerPos, Rectangle? rectangle)
        : this(name, position, Color.White, centerPos, Vector2.One, rectangle) { }

        /// <summary>
        /// 画像を回転する
        /// </summary>
        /// <param name="name">画像の名前</param>
        /// <param name="position">画像自体の中心座標,(0,0)は画像の左上</param>
        /// <param name="centerPos">画像自体の中心座標
        /// <param name="angle">回転角度(度数法)</param>
        public DrawStruct(CharacterID name, Vector2 position, Vector2 centerPos, float angle)
        : this(name, position, Color.White, centerPos, Vector2.One, null, angle) { }

        /// <summary>
        /// 中心座標の設定
        /// 画像の大きさの変更
        /// 画像の回転
        /// </summary>
        /// <param name="name">画像の名前</param>
        /// <param name="position">centerPosを中心(0,0)にした座標</param>
        /// <param name="centerPos">画像自体の中心座標,(0,0)は画像の左上</param>
        /// <param name="scale">画像の大きさ 1.0 = 100%</param>
        /// <param name="rectangle">nullなら区切らず画像をそのまま表示</param>
        /// <param name="angle">回転角度(度数法)</param>
        public DrawStruct(CharacterID name, Vector2 position, Vector2 centerPos, Vector2 scale, Rectangle? rectangle, float angle)
        : this(name, position, Color.White, centerPos, scale, rectangle, angle) { }

        /// <summary>
        /// 全て
        /// </summary>
        /// <param name="name">画像の名前</param>
        /// <param name="position">centerPosを中心(0,0)にした座標</param>
        /// <param name="color">画像の色を変える</param>
        /// <param name="centerPos">画像自体の中心座標,(0,0)は画像の左上</param>
        /// <param name="scale">画像の大きさ 1.0 = 100%</param>
        /// <param name="rectangle">nullなら区切らず画像をそのまま表示</param>
        /// <param name="angle">回転角度(度数法)</param>
        /// <param name="effect">画像の上下反転</param>
        public DrawStruct(CharacterID textureName, Vector2 position, Color color, Vector2 centerPos, Vector2 scale, Rectangle? rectangle = null, float angle = 0.0f, float alpha = 1.0f, SpriteEffects effect = SpriteEffects.None)
        {
            this.textureName = textureName;
            this.position = position;
            this.rectangle = rectangle;
            this.color = color;
            this.angle = angle;
            this.centerPos = centerPos;
            this.scale = scale;
            this.alpha = alpha;
            this.effect = effect;
        }
    }
}
