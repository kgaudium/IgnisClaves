using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IgnisClaves
{
    public static class IgnisRender
    {
        public static SpriteFont DefaultFont;
        public static Texture2D BlankTexture;

        

        public enum ScalingMode
        {
            Stretch, // заполнение (весь экран заполнен, но картинка может обрезаться)
            Fit     // по размеру (картинку видно целиком, но возможны чёрные полосы)
        }

        public static void Initialize(IgnisGame ignisGame)
        {
            DefaultFont = ignisGame.Content.Load<SpriteFont>("default\\spaceage");

            BlankTexture = new Texture2D(ignisGame.GraphicsDevice, 1, 1);
            BlankTexture.SetData(new Color[] { Color.White });
        }

        public static float GetScaleToFitScreen(Texture2D texture, GraphicsDevice graphicsDevice) =>
            (float)texture.Width / texture.Height >
            (float)graphicsDevice.Viewport.Width / graphicsDevice.Viewport.Height
                ? (float)graphicsDevice.Viewport.Width / texture.Width
                : (float)graphicsDevice.Viewport.Height / texture.Height;

        public static float GetScaleToStretchScreen(Texture2D texture, GraphicsDevice graphicsDevice) =>
            (float)texture.Width / texture.Height >
            (float)graphicsDevice.Viewport.Width / graphicsDevice.Viewport.Height
                ? (float)graphicsDevice.Viewport.Height / texture.Height
                : (float)graphicsDevice.Viewport.Width / texture.Width;

        public static void DrawBackground(Texture2D texture, SpriteBatch spriteBatch, ScalingMode scalingMode)
        {
            float screenHeight = spriteBatch.GraphicsDevice.Viewport.Height;
            float screenWidth = spriteBatch.GraphicsDevice.Viewport.Width;

            float scale;

            switch (scalingMode)
            {
                case ScalingMode.Stretch:
                    scale = GetScaleToStretchScreen(texture, spriteBatch.GraphicsDevice);
                    break;
                case ScalingMode.Fit:
                    scale = GetScaleToFitScreen(texture, spriteBatch.GraphicsDevice);
                    break;
                default:
                    scale = 0;
                    break;
            }

            float textureHeight = texture.Height * scale;
            float textureWidth = texture.Width * scale;

            spriteBatch.Draw(texture,
                new Vector2(screenWidth / 2f - textureWidth / 2f, screenHeight / 2f - textureHeight / 2f), null,
                Color.White, 0f, Vector2.Zero, scale,
                SpriteEffects.None, 0f);
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, Color color, Vector2 position, Vector2 size)
        {
            spriteBatch.Draw(BlankTexture, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), color);
        }
        public static void DrawRectangle(SpriteBatch spriteBatch, Color color, Vector2 position, Vector2 size, Vector2 origin)
        {
            spriteBatch.Draw(BlankTexture, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), null, color, 0f, origin, SpriteEffects.None, 0.5f);
        }

        public static void DrawRectangleByRelative(SpriteBatch spriteBatch, Color color, Vector2 position, Vector2 size)
        {
            DrawRectangle(spriteBatch, color, GetAbsolutePosition(position.X, position.Y, spriteBatch), GetAbsolutePosition(size.X, size.Y, spriteBatch));
        }

        public static void DrawText(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, float scale, float layerDepth)
        {
            spriteBatch.DrawString(font, text, position, color, 0, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
        }
        public static void DrawText(SpriteBatch spriteBatch, string text, Vector2 position, Color color, float scale, float layerDepth)
        {
            spriteBatch.DrawString(DefaultFont, text, position, color, 0, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
        }

        public static void DrawCenteredText(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 centerPosition,
            Color color, float scale, float layerDepth)
        {
            spriteBatch.DrawString(font, text, centerPosition, color, 0f, font.MeasureString(text) / 2f, scale, SpriteEffects.None, layerDepth);
        }
        public static void DrawCenteredText(SpriteBatch spriteBatch, string text, Vector2 centerPosition,
            Color color, float scale, float layerDepth)
        {
            spriteBatch.DrawString(DefaultFont, text, centerPosition, color, 0f, DefaultFont.MeasureString(text) / 2f, scale, SpriteEffects.None, layerDepth);
        }

        public static string GetUsername()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? WindowsIdentity.GetCurrent().Name : "default";
        }

        public static float GetAbsoluteX(float percentX, SpriteBatch spriteBatch)
        {
            return spriteBatch.GraphicsDevice.Viewport.Width * percentX;
        }
        
        public static float GetAbsoluteY(float percentY, SpriteBatch spriteBatch)
        {
            return spriteBatch.GraphicsDevice.Viewport.Height * percentY;
        }

        public static Vector2 GetAbsolutePosition(float percentX, float percentY, SpriteBatch spriteBatch)
        {
            return new Vector2(spriteBatch.GraphicsDevice.Viewport.Width * percentX, spriteBatch.GraphicsDevice.Viewport.Height * percentY);
        }

        public static float GetAbsoluteFontScale(SpriteBatch spriteBatch, SpriteFont font, float relativeScale)
        {
            float height = font.MeasureString("A").Y;

            return (spriteBatch.GraphicsDevice.Viewport.Height / height) * relativeScale;
        }

        public static Vector2 GetAbsoluteMeasureString(SpriteBatch spriteBatch, SpriteFont font, string text, float relativeScale)
        {
            return font.MeasureString(text) * GetAbsoluteFontScale(spriteBatch, font, relativeScale);
        }

        //public static Point Vector2ToPoint(Vector2 vector2)
        //{
        //    return new Point(vector2.X, vector2.Y);
        //}

        //public static Vector2 GetCenteredPosition(SpriteBatch spriteBatch, )
    }
}
