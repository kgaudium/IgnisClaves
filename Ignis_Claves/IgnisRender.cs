using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IgnisClaves
{
    public static class IgnisRender
    {
        public enum ScalingMode
        {
            Stretch, // заполнение
            Fit     // по размеру
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
            float ScreenHeight = spriteBatch.GraphicsDevice.Viewport.Height;
            float ScreenWidth = spriteBatch.GraphicsDevice.Viewport.Width;

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

            float TextureHeigth = texture.Height * scale;
            float TextureWidth = texture.Width * scale;

            spriteBatch.Draw(texture,
                new Vector2(ScreenWidth / 2f - TextureWidth / 2f, ScreenHeight / 2f - TextureHeigth / 2f), null,
                Color.White, 0f, Vector2.Zero, scale,
                SpriteEffects.None, 1f);
        }

        public static Texture2D LoadTexture2D(string assetName)
        {
            return AppController.Ignis.Content.Load<Texture2D>(assetName);
        }
    }
}
