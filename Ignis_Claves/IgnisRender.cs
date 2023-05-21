using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IgnisClaves;

public static class IgnisRender
{
    public enum ScalingMode
    {
        Stretch, // заполнение (весь экран заполнен, но картинка может обрезаться)
        Fit // по размеру (картинку видно целиком, но возможны чёрные полосы)
    }

    public static SpriteFont DefaultFont;
    public static Texture2D DefaultBackground;
    public static Texture2D BlankTexture;

    public static Color DarkerGrayColor = new(90, 90, 90);
    public static Color DarkGrayColor = new(150, 150, 150);
    public static Color GrayColor = new(214, 214, 214);
    public static Color LightBlackColor = new(36, 36, 36);

    public static Color DarkerGreenColor = new(19, 79, 46);
    public static Color DarkGreenColor = new(41, 171, 130);
    public static Color GreenColor = new(52, 219, 127);

    public static Color DarkBrownColor = new(61, 51, 51);

    public static Color PinkColor = new(232, 99, 182);
    public static Color DarkPinkColor = new(84, 36, 66);

    public static void Initialize(IgnisGame ignisGame)
    {
        DefaultFont = ignisGame.Content.Load<SpriteFont>("default\\spaceage");
        DefaultBackground = ignisGame.Content.Load<Texture2D>("default\\ralsei_3");

        BlankTexture = new Texture2D(ignisGame.GraphicsDevice, 1, 1);
        BlankTexture.SetData(new[] { Color.White });
    }

    public static float GetScaleToFitScreen(Texture2D texture, GraphicsDevice graphicsDevice)
    {
        return (float)texture.Width / texture.Height >
               (float)graphicsDevice.Viewport.Width / graphicsDevice.Viewport.Height
            ? (float)graphicsDevice.Viewport.Width / texture.Width
            : (float)graphicsDevice.Viewport.Height / texture.Height;
    }

    public static float GetScaleToStretchScreen(Texture2D texture, GraphicsDevice graphicsDevice)
    {
        return (float)texture.Width / texture.Height >
               (float)graphicsDevice.Viewport.Width / graphicsDevice.Viewport.Height
            ? (float)graphicsDevice.Viewport.Height / texture.Height
            : (float)graphicsDevice.Viewport.Width / texture.Width;
    }

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
        spriteBatch.Draw(BlankTexture, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y),
            color);
    }

    public static void DrawRectangle(SpriteBatch spriteBatch, Color color, Vector2 position, Vector2 size,
        Vector2 origin)
    {
        spriteBatch.Draw(BlankTexture, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), null,
            color, 0f, origin, SpriteEffects.None, 0.5f);
    }

    public static void DrawRectangle(SpriteBatch spriteBatch, Color color, Vector2 position, Vector2 size,
        Vector2 origin, float layerDepth)
    {
        spriteBatch.Draw(BlankTexture, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), null,
            color, 0f, origin, SpriteEffects.None, layerDepth);
    }

    public static void DrawRectangleByRelative(SpriteBatch spriteBatch, Color color, Vector2 position, Vector2 size)
    {
        DrawRectangle(spriteBatch, color, GetAbsolutePosition(position.X, position.Y, spriteBatch),
            GetAbsolutePosition(size.X, size.Y, spriteBatch));
    }

    public static void DrawRectangleByRelative(SpriteBatch spriteBatch, Color color, Vector2 position, Vector2 size,
        Vector2 origin)
    {
        DrawRectangle(spriteBatch, color, GetAbsolutePosition(position.X, position.Y, spriteBatch),
            GetAbsolutePosition(size.X, size.Y, spriteBatch), origin);
    }

    public static void DrawRectangleByRelative(SpriteBatch spriteBatch, Color color, Vector2 position, Vector2 size,
        Vector2 origin, float layerDepth)
    {
        DrawRectangle(spriteBatch, color, GetAbsolutePosition(position.X, position.Y, spriteBatch),
            GetAbsolutePosition(size.X, size.Y, spriteBatch), origin, layerDepth);
    }

    public static void DrawText(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color,
        float scale, float layerDepth)
    {
        spriteBatch.DrawString(font, text, position, color, 0, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
    }

    public static void DrawText(SpriteBatch spriteBatch, string text, Vector2 position, Color color, float scale,
        float layerDepth)
    {
        spriteBatch.DrawString(DefaultFont, text, position, color, 0, Vector2.Zero, scale, SpriteEffects.None,
            layerDepth);
    }

    public static void DrawCenteredText(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 centerPosition,
        Color color, float scale, float layerDepth)
    {
        spriteBatch.DrawString(font, text, centerPosition, color, 0f, font.MeasureString(text) / 2f, scale,
            SpriteEffects.None, layerDepth);
    }

    public static void DrawCenteredText(SpriteBatch spriteBatch, string text, Vector2 centerPosition,
        Color color, float scale, float layerDepth)
    {
        spriteBatch.DrawString(DefaultFont, text, centerPosition, color, 0f, DefaultFont.MeasureString(text) / 2f,
            scale, SpriteEffects.None, layerDepth);
    }

    public static float GetAbsoluteX(float percentX, SpriteBatch spriteBatch)
    {
        return spriteBatch.GraphicsDevice.Viewport.Width * percentX;
    }

    public static float GetAbsoluteY(float percentY, SpriteBatch spriteBatch)
    {
        return spriteBatch.GraphicsDevice.Viewport.Height * percentY;
    }

    /// <summary>
    ///     Возвращает позицию из процентов от экрана
    /// </summary>
    /// <param name="percentX"></param>
    /// <param name="percentY"></param>
    /// <param name="spriteBatch"></param>
    /// <returns></returns>
    public static Vector2 GetAbsolutePosition(float percentX, float percentY, SpriteBatch spriteBatch)
    {
        return new Vector2(spriteBatch.GraphicsDevice.Viewport.Width * percentX,
            spriteBatch.GraphicsDevice.Viewport.Height * percentY);
    }

    /// <summary>
    ///     Возвращает размер шрифта, при котором он будет занимать заданное количество процентов от высоты экрана.
    /// </summary>
    /// <param name="spriteBatch">SpriteBatch с помощью которого будет отрисовываться</param>
    /// <param name="font">Шрифт</param>
    /// <param name="relativeScale">Нужный процент от экрана</param>
    /// <returns></returns>
    public static float GetAbsoluteFontScale(SpriteBatch spriteBatch, SpriteFont font, float relativeScale)
    {
        float height = font.MeasureString("A").Y;

        return spriteBatch.GraphicsDevice.Viewport.Height / height * relativeScale;
    }

    /// <summary>
    ///     Возврщает размеры, занимаемые заданной строкой в заданном масштабе
    /// </summary>
    /// <param name="relativeScale">Размер шрифта в процентах от экрана</param>
    /// <returns></returns>
    public static Vector2 GetAbsoluteMeasureString(SpriteBatch spriteBatch, SpriteFont font, string text,
        float relativeScale)
    {
        return font.MeasureString(text) * GetAbsoluteFontScale(spriteBatch, font, relativeScale);
    }

    public static string CutStringToFitWidth(string text, SpriteFont font, float relativeFontScale, float width,
        SpriteBatch spriteBatch)
    {
        if (GetAbsoluteMeasureString(spriteBatch, font, text, relativeFontScale).X <= width) return text;

        int length = text.Length;
        while (true)
        {
            float textWidth = GetAbsoluteMeasureString(spriteBatch, font, text.Substring(0, --length) + "...",
                relativeFontScale).X;

            if (textWidth <= width) return text.Substring(0, length) + "...";

            if (length <= 0) return "";
        }
    }

    public static string CutStringToFitWidth(string text, float relativeFontScale, float width, SpriteBatch spriteBatch)
    {
        return CutStringToFitWidth(text, DefaultFont, relativeFontScale, width, spriteBatch);
    }

    //public static Vector2 GetCenteredPosition(SpriteBatch spriteBatch, )
}