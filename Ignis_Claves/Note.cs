using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IgnisClaves
{
    public class Note
    {
        public HitSound HitSound;
        public Stave.KeysEnum Line;


        public float PositionY;

        public Note(HitSound hitSound, Stave.KeysEnum line, SpriteBatch spriteBatch, float posY)
        {
            HitSound = hitSound;
            Line = line;
            //PositionY = -IgnisRender.GetAbsoluteY(0.1f, spriteBatch);
            PositionY = posY;
        }

        public void Draw(SpriteBatch spriteBatch, float relativeX, Vector2 relativeSize)
        {
            Color color = Line is Stave.KeysEnum.InnerLeft or Stave.KeysEnum.InnerRight ? IgnisGame.InnerKeyEnabledColor : IgnisGame.OuterKeyEnabledColor;

            //IgnisRender.DrawRectangleByRelative(spriteBatch,
            //    color,
            //    new Vector2(relativeX + relativeSize.X * ((int)Line - 2), PositionY),
            //    //new Vector2(relativeX + relativeLineWidth * i, 0.8f),
            //    //new Vector2(relativeLineWidth, 1f));
            //    relativeSize);
            var size = IgnisRender.GetAbsolutePosition(relativeSize.X, relativeSize.Y, spriteBatch);

            IgnisRender.DrawRectangle(spriteBatch, color,
                new Vector2(IgnisRender.GetAbsoluteX(relativeX + relativeSize.X * ((int)Line - 2), spriteBatch), PositionY),
                size, 
                Vector2.Zero, 
                (int)GameSession.GameDrawOrder.Notes / 100f);
        }
    }
}
