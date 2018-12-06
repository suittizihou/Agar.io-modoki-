using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Agar.io_modoki_;

namespace Utility
{
    class Renderer
    {
        private ContentManager contentManager;
        private SpriteBatch spriteBatch;
        private GraphicsDevice graphicsDevice;
        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        private Dictionary<string, Effect> effects = new Dictionary<string, Effect>();
        private Camera camera;

        private VertexBuffer lineListVertexBuffer = null;
        private BasicEffect basicEffect = null;

        public Renderer(ContentManager content, GraphicsDevice graphics)
        {
            contentManager = content;
            contentManager.RootDirectory = "Content";
            graphicsDevice = graphics;
            spriteBatch = new SpriteBatch(graphics);
            camera = new Camera(Player.GetPos);
        }

        public void LoadTexture(CharacterID characterID)
        {
            textures[characterID.ToString()] = contentManager.Load<Texture2D>(characterID.ToString());
        }

        public void VertexLoad()
        {
            // エフェクトを作成
            basicEffect = new BasicEffect(graphicsDevice);

            // エフェクトで頂点カラーを有効にする
            basicEffect.VertexColorEnabled = true;

            // ビューマトリックスをあらかじめ設定((0, 0, 15)から原点を見る)
            basicEffect.View = Matrix.CreateLookAt(
                new Vector3(0.0f, 0.0f, 0.0f),
                Vector3.Zero,
                Vector3.Up);

            // プロジェクションマトリックスをあらかじめ設定
            basicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45.0f),
                (float)graphicsDevice.Viewport.Width /
                (float)graphicsDevice.Viewport.Height,
                1.0f,
                100.0f);

            // 頂点の数
            int vertexCount = 6;

            // LineList用頂点バッファ作成
            lineListVertexBuffer = new VertexBuffer(graphicsDevice,
                typeof(VertexPositionColor), vertexCount, BufferUsage.None);

            // LineList用頂点バッファを作成する
            VertexPositionColor[] lineListVertices = new VertexPositionColor[vertexCount];

            lineListVertices[0] = new VertexPositionColor(new Vector3(-2.5f, 2.5f, 0.0f), Color.Black);
            lineListVertices[1] = new VertexPositionColor(new Vector3(100.0f, 2.5f, 0.0f), Color.Black);

            lineListVertexBuffer.SetData(lineListVertices);
        }

        // エフェクトの読み込み
        public void LoadShader(string name)
        {
            effects[name] = contentManager.Load<Effect>(name);
        }
        public void UnloadContent()
        {
            textures.Clear();
            effects.Clear();
        }
        public void LineDraw()
        {
            // 描画に使用する頂点バッファをセットします
            graphicsDevice.SetVertexBuffer(lineListVertexBuffer);

            // パスの数だけ繰り返し描画
            foreach(EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                // パスの開始
                pass.Apply();

                // LineListで描画する
                graphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, 3);
            }
        }
        public void MatixBegin()
        {
            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                camera.GetMatrix());
        }
        public void Begin()
        {
            spriteBatch.Begin();
        }
        // シェーダー用のBegin
        public void BeginShader()
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        }
        public void End()
        {
            spriteBatch.End();
        }
        public void DrawTexture(CharacterID name, Vector2 position)
        {
            spriteBatch.Draw(textures[name.ToString()], position, Color.White);
        }
        public void DrawTexture(CharacterID name, Vector2 position, Color color)
        {
            spriteBatch.Draw(textures[name.ToString()], position, color);
        }
        public void DrawTexture(string name, Vector2 position, Rectangle rectangle, float alpha = 1.0f)
        {
            spriteBatch.Draw(textures[name], position, rectangle, Color.White * alpha);
        }

        // 画像のサイズを持ってくる
        public Vector2 GetHalfSize(CharacterID characterID)
        {
            return new Vector2(textures[characterID.ToString()].Width / 2, textures[characterID.ToString()].Height);
        }
        public void DrawTexture(DrawStruct draw)
        {
            if (NotImage(draw.textureName.ToString())) return;
            spriteBatch.Draw(
                textures[draw.textureName.ToString()],  // 画像の名前
                draw.position,                          // 座標
                draw.rectangle,                         // nullなら区切らず画像をそのまま表示
                draw.color * draw.alpha,                // 透明度
                MathHelper.ToRadians(draw.angle),       // 回転
                draw.centerPos,                         // 中心座標
                draw.scale,                             // 大きさ
                draw.effect,                            // 反転
                0.0f);
        }
        public bool NotImage(string textureName)
        {
            return (textureName.Length == 0);
        }

        public void DrawNumber(DrawStruct drawStruct, int number)
        {
            if (NotImage(drawStruct.textureName.ToString())) return;
            foreach(var n in number.ToString())
            {
                spriteBatch.Draw(textures[drawStruct.textureName.ToString()],
                    drawStruct.position,
                    new Rectangle((n - '0') * 32, 0, 32, 64),
                    drawStruct.color * drawStruct.alpha,
                    MathHelper.ToRadians(drawStruct.angle),
                    drawStruct.centerPos,
                    drawStruct.scale,
                    drawStruct.effect,
                    0.0f);
                drawStruct.position.X += 32;
            }
        }
    }
}
