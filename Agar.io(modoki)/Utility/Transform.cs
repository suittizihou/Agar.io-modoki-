using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Utility
{
    class Transform
    {
        public Transform Parent { get; set; }
        /// <summary>
        /// 座標
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// 画像の中心点
        /// </summary>
        public Vector2 CenterPosition { get; set; }
        /// <summary>
        /// 画像の大きさ
        /// </summary>
        public Vector2 Scale { get; set; }
        /// <summary>
        /// 角度
        /// </summary>
        public float Angle { get; set; }
        
        /// <summary>
        ///  コンストラクタ
        /// </summary>
        public Transform()
        {
            this.Position = Vector2.Zero;
            this.CenterPosition = Vector2.Zero;
            this.Scale = Vector2.One;
            this.Angle = 0.0f;
        }

        public Transform(Transform copy)
        {
            this.Position = copy.Position;
            this.CenterPosition = copy.CenterPosition;
            this.Scale = copy.Scale;
            this.Angle = copy.Angle;
        }
    }
}
