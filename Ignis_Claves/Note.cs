using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IgnisClaves;

public class Note
{
    public HitSound HitSound;
    public Stave.KeysEnum Line;


    public float PositionY;

    public Note(HitSound hitSound, Stave.KeysEnum line, float posY)
    {
        HitSound = hitSound;
        Line = line;
        PositionY = posY;
    }

    public void Draw(SpriteBatch spriteBatch, float relativeX, Vector2 relativeSize)
    {
        Color color = Line is Stave.KeysEnum.InnerLeft or Stave.KeysEnum.InnerRight
            ? IgnisGame.InnerKeyEnabledColor
            : IgnisGame.OuterKeyEnabledColor;

        Vector2 size = IgnisRender.GetAbsolutePosition(relativeSize.X, relativeSize.Y, spriteBatch);

        IgnisRender.DrawRectangle(spriteBatch, color,
            new Vector2(IgnisRender.GetAbsoluteX(relativeX + relativeSize.X * ((int)Line - 2), spriteBatch), PositionY),
            size,
            Vector2.Zero,
            (int)GameSession.GameDrawOrder.Notes / 100f);
    }
}