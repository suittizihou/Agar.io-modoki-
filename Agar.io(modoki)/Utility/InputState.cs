using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Agar.io_modoki_;

namespace Utility
{
    class InputState
    {
        private KeyboardState currentKey;
        private KeyboardState previousKey;
        private Vector2 keyVelocity = Vector2.Zero;

        private GamePadState currentPad;
        private GamePadState previousPad;

        private Vector2 leftStickVelocity;
        private Vector2 padVelocity;

        //private int count;    // バイブレーションをインターバルで止めるよう

        public InputState() { }
        public bool IsKeyDown(Keys key)
        {
            bool current = currentKey.IsKeyDown(key);
            bool previous = previousKey.IsKeyDown(key);

            return current && !previous;
        }
        public bool IsKeyPush(Keys key)
        {
            return currentKey.IsKeyDown(key);
        }
        public Vector2 KeyVelocity()//ゲッター
        {
            return keyVelocity;
        }
        private void UpdateKey(KeyboardState keyState)
        {
            previousKey = currentKey;
            currentKey = keyState;
        }
        private void UpdateKeyVelocity(KeyboardState keyState)
        {
            keyVelocity = Vector2.Zero;

            if (keyState.IsKeyDown(Keys.Right))     // 右
            {
                keyVelocity.X = +1.0f;
            }
            if (keyState.IsKeyDown(Keys.Left))      // 左
            {
                keyVelocity.X = -1.0f;
            }
            if (keyState.IsKeyDown(Keys.Down))      // 下
            {
                keyVelocity.Y = +1.0f;
            }
            if (keyState.IsKeyDown(Keys.Up))        // 上
            {
                keyVelocity.Y = -1.0f;
            }

            if (keyVelocity.Length() != 0.0f) 
            {
                keyVelocity.Normalize();
            }

        }

        public bool IsPadDown(Buttons button)
        {
            bool current = currentPad.IsButtonDown(button);
            bool previous = previousPad.IsButtonDown(button);

            return current && !previous;
        }
        public bool IsPadPush(Buttons button)
        {
            return currentPad.IsButtonDown(button);
        }
        public Vector2 PadVelocity()
        {
            return padVelocity;
        }
        private void UpdatePad(GamePadState padState)
        {
            previousPad = currentPad;
            currentPad = padState;
        }

        /// <summary>
        /// 方向パッド
        /// </summary>
        /// <param name="padState"></param>
        private void UpdatePadVelocity(GamePadState padState)
        {
            padVelocity = Vector2.Zero;

            if (padState.DPad.Up == ButtonState.Pressed)
            {
                padVelocity.Y = -1.0f;
            }
            if (padState.DPad.Down == ButtonState.Pressed)
            {
                padVelocity.Y = +1.0f;
            }
            if (padState.DPad.Left == ButtonState.Pressed)
            {
                padVelocity.X = -1.0f;
            }
            if (padState.DPad.Right == ButtonState.Pressed)
            {
                padVelocity.X = 1.0f;
            }

            if (padVelocity.Length() != 0.0f)
            {
                padVelocity.Normalize();
            }
        }

        /// <summary>
        /// 左スティック
        /// </summary>
        /// <param name="stickID"></param>
        /// <returns></returns>
        public bool IsLeftSticksPush(StickID stickID)
        {
            bool current = false;
            if(stickID == StickID.Up)
            {
                if(1.0f == GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y)
                {
                    current = true;
                }
            }
            if(stickID == StickID.Down)
            {
                if(-1.0f == GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y)
                {
                    current = true;
                }
            }
            if(stickID == StickID.Left)
            {
                if(-1.0f == GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X)
                {
                    current = true;
                }
            }
            if(stickID == StickID.Right)
            {
                if(1.0f == GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X)
                {
                    current = true;
                }
            }
            return current;
        }

        public Vector2 StickLeftVelocity()
        {
            return leftStickVelocity;
        }
        /// <summary>
        /// 上下左右斜めの８方向対応
        /// </summary>
        private void UpdateLeftStickVelocity()
        {
            leftStickVelocity = Vector2.Zero;

            leftStickVelocity.Y -= GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y;
            leftStickVelocity.X += GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;

            if (leftStickVelocity.Length() != 0.0f)
            {
                leftStickVelocity.Normalize();
            }
        }
        /// <summary>
        /// バイブレーション
        /// </summary>
        /// <param name="lowViv">低周波モーター</param>
        /// <param name="heightViv">高周波モーター</param>
        /// <param name="interval">止まる時間</param>
        public void Vivration(float lowViv = 0, float heightViv = 0, int interval = 0)
        {
            intervalPropaty = interval;
            GamePad.SetVibration(PlayerIndex.One, lowViv, heightViv);
        }

        /// <summary>
        /// バイブレーションのストップ用メソッド
        /// </summary>
        public void StopVivration()
        {
            GamePad.SetVibration(PlayerIndex.One, 0, 0);
        }

        /// <summary>
        /// マウスの左クリックが押されたらtrue
        /// </summary>
        public bool LeftClick() { return Mouse.GetState().LeftButton == ButtonState.Pressed; }

        /// <summary>
        ///  マウスの座標取得
        /// </summary>
        /// <returns></returns>
        public Vector2 MouseVector2()
        {
            MouseState mouseState = Mouse.GetState();
            return new Vector2(mouseState.X, mouseState.Y);
        }

        /// <summary>
        ///  マウスのワールド座標取得
        /// </summary>
        /// <returns></returns>
        public Vector2 WorldMouseVector2()
        {
            MouseState mouseState = Mouse.GetState();
            return new Vector2(mouseState.X + Camera.CameraMove.X, mouseState.Y + Camera.CameraMove.Y);
        }


        private int intervalPropaty { get; set; }
        public void Update()
        {
            GamePadState padState = GamePad.GetState(PlayerIndex.One);
            UpdatePad(padState);
            UpdatePadVelocity(padState);
            //VivrationUpdate();

            KeyboardState keyState = Keyboard.GetState();
            UpdateKey(keyState);
            UpdateKeyVelocity(keyState);

            UpdateLeftStickVelocity();
        }
    }
}
