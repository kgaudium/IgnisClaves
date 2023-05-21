using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IgnisClaves;

public class Stave
{
    public enum KeysEnum
    {
        OuterLeft,
        InnerLeft,
        InnerRight,
        OuterRight
    }

    public static float StaveRelativeHeight = 0.8f;
    private bool[] keyStates = new bool[4];

    public void Draw(SpriteBatch spriteBatch, float relativeX, float relativeLineWidth)
    {
        // Background
        IgnisRender.DrawRectangleByRelative(spriteBatch,
            IgnisGame.StaveBackgroundColor,
            new Vector2(relativeX - 2 * relativeLineWidth, 0f),
            new Vector2(relativeLineWidth * 4, 1f),
            Vector2.Zero, (int)GameSession.GameDrawOrder.StaveBackground / 100f);


        for (int i = -2; i < 2; i++)
        {
            Color color;

            if (keyStates[i + 2])
                color = i is -1 or 0 ? IgnisGame.InnerKeyEnabledColor : IgnisGame.OuterKeyEnabledColor;
            // TODO Белые полосы
            //IgnisRender.DrawRectangleByRelative(spriteBatch,
            //    new Color(IgnisRender.GrayColor, 3),
            //    new Vector2(relativeX + relativeLineWidth * i, 0.6f),
            //    new Vector2(relativeLineWidth, 0.2f));
            else
                color = i is -1 or 0 ? IgnisGame.InnerKeyDisabledColor : IgnisGame.OuterKeyDisabledColor;

            // Нижние полосы (Keys)
            IgnisRender.DrawRectangleByRelative(spriteBatch,
                color,
                new Vector2(relativeX + relativeLineWidth * i, StaveRelativeHeight),
                new Vector2(relativeLineWidth, 1f - StaveRelativeHeight),
                Vector2.Zero,
                (int)GameSession.GameDrawOrder.Keys / 100f);
        }
    }

    public void UpdateKeyStates()
    {
        keyStates = new[]
        {
            Keyboard.GetState().IsKeyDown(IgnisGame.Keybinds[KeysEnum.OuterLeft]),
            Keyboard.GetState().IsKeyDown(IgnisGame.Keybinds[KeysEnum.InnerLeft]),
            Keyboard.GetState().IsKeyDown(IgnisGame.Keybinds[KeysEnum.InnerRight]),
            Keyboard.GetState().IsKeyDown(IgnisGame.Keybinds[KeysEnum.OuterRight])
        };
    }
}