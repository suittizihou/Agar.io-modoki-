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
    /// 2D用カメラ
    /// </summary>
    class Camera
    {
        // カメラの位置
        private static Vector2 cameraPos;

        // カメラのズーム値
        private Vector2 cameraZoom = Vector2.One;

        // 可視領域
        private static Rectangle visible;

        // どのくらい回転しているか
        private float rotation = 0.0f;

        // 画面の中心点
        private Vector2 screenPos = Vector2.Zero;

        public Camera(Vector2 screen)
        {
            visible = new Rectangle(0, 0, (int)screen.X, (int)screen.Y);
            Position = cameraPos;
            screenPos = new Vector2(screen.X / 2, screen.Y / 2);
        }

        /// <summary>
        /// カメラの場所を指定
        /// </summary>
        public static Vector2 Position
        {
            get { return cameraPos; }
            set
            {
                cameraPos = value;
                visible.X = (int)(cameraPos.X - visible.Width / 2);
                visible.Y = (int)(cameraPos.Y - visible.Height / 2);
            }
        }

        public static Vector2 ScreenPos { get; set; }

        /// <summary>
        /// カメラの移動
        /// </summary>
        /// <param name="moveFloatX"></param>
        /// <param name="moveFloatY"></param>
        public void MoveFloat(float moveFloatX, float moveFloatY)
        {
            CameraMove = new Vector2(moveFloatX, moveFloatY);
            Position = new Vector2(cameraPos.X + moveFloatX, cameraPos.Y + moveFloatY);
        }

        public void MoveVector2(Vector2 moveVector2)
        {
            CameraMove = moveVector2;
            Position = cameraPos + moveVector2;
        }

        /// <summary>
        /// カメラの移動量を返す
        /// </summary>
        public static Vector2 CameraMove { get; private set; }

        /// <summary>
        /// カメラのズーム
        /// </summary>
        public static Vector2 Zoom { get; set; } = new Vector2(1f);

        public void Angle(float angle)
        {
            rotation = angle;
        }

        // 行列
        public virtual Matrix GetMatrix()
        {
            Vector2 center0fScreen = Screen.HalfScreen;
            Vector2 offset = (center0fScreen * Zoom) - center0fScreen;
            offset /= Zoom;
            offset += Position;
            offset -= center0fScreen;

            cameraZoom = Zoom;

            return 
                Matrix.CreateTranslation(-new Vector3(offset, 0)) * // カメラの移動
                Matrix.CreateScale(cameraZoom.X, cameraZoom.Y, 1.0f) *  // ズーム
                Matrix.CreateRotationZ(rotation) *  // 回転
                Matrix.Identity;    // 中心
        }

        /// <summary>
        /// スクリーン座標をワールド座標に変換
        /// </summary>
        /// <param name="screenWidth">画面の横幅</param>
        /// <param name="ScreenHeight">画面の縦幅</param>
        /// <param name="screenPoint">変換したい座標</param>
        /// <param name="cameraPosition">カメラの位置</param>
        /// <param name="zoom">ズーム倍率</param>
        /// <returns></returns>
        public static Vector2 ScreenPointWorldPoint(int screenWidth, int ScreenHeight, Vector2 screenPoint, Vector2 cameraPosition, float zoom)
        {
            Vector2 center0fScreen = new Vector2(screenWidth / 2, ScreenHeight / 2);
            return (screenPoint - center0fScreen) / zoom + cameraPosition;
        }
    }
}
