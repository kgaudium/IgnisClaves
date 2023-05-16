using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IgnisClaves
{
    public class Stave
    {
        private bool[] keyStates = new bool[4];

        public Stave()
        {
            
        }

        public void Draw(SpriteBatch spriteBatch, float relativeX, float relativeLineWidth)
        {
            // Background
            IgnisRender.DrawRectangleByRelative(spriteBatch,
                IgnisRender.LightBlackColor,
                new Vector2(relativeX - 2 * relativeLineWidth, 0f),
                new Vector2(relativeLineWidth * 4, 1f),
                Vector2.Zero, (int)GameSession.GameDrawOrder.StaveBackground / 100f);



            for (int i = -2; i < 2; i++)
            {
                Color color;

                if (keyStates[i + 2])
                {
                    color = i is -1 or 0 ? IgnisRender.GrayColor : IgnisRender.PinkColor;

                    // TODO Белые полосы
                    //IgnisRender.DrawRectangleByRelative(spriteBatch,
                    //    new Color(IgnisRender.GrayColor, 3),
                    //    new Vector2(relativeX + relativeLineWidth * i, 0.6f),
                    //    new Vector2(relativeLineWidth, 0.2f));
                }
                else
                {
                    color = i is -1 or 0 ? IgnisRender.DarkerGrayColor : IgnisRender.DarkPinkColor;
                }

                // Нижние полосы (Keys)
                IgnisRender.DrawRectangleByRelative(spriteBatch,
                    color,
                    new Vector2(relativeX + relativeLineWidth * i, 0.8f),
                    new Vector2(relativeLineWidth, 0.2f),
                    Vector2.Zero, 
                    (int)GameSession.GameDrawOrder.Keys / 100f);
            }

        }

        public void UpdateKeyStates(KeyboardState state)
        {
            keyStates = new[]
            {
                Keyboard.GetState().IsKeyDown(Keys.D),
                Keyboard.GetState().IsKeyDown(Keys.F),
                Keyboard.GetState().IsKeyDown(Keys.J),
                Keyboard.GetState().IsKeyDown(Keys.K)
            };
        }
    }
}
