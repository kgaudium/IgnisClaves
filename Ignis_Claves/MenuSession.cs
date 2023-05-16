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
    public class MenuSession : Session
    {
        /*                  Поля и свойства                 */
        //public override IgnisGame CurrentIgnisGame { get; set; }
        public List<BeatMap> BeatMapsList;

        private int? selectedBeatMap;
        public int? SelectedBeatMap
        {
            get => selectedBeatMap;
            private set
            {
                if (value < 0)
                {
                    throw new IndexOutOfRangeException($"The index must be greater than or equal to zero! {value} < 0");
                }

                if (value == null && BeatMapsList.Any())
                {
                    throw new Exception("BeatMap List has elements, index cannot be null!");
                }

                selectedBeatMap = value;
            }
        }

        enum MenuDrawOrder : int
        {
            TextBackground = 49,
            Text = 50,
        }
        

        public string Username;

        public Vector2 BeatMapRelativeSize = new Vector2(30f, 5f); // Проценты от ширины экрана (высота прямоугольника тоже зависит от ширины экрана)
        public Vector2 BeatMapRelativeSizeSelected = new Vector2(45f, 7.5f);

        /*                  __Поля и свойства__                 */



        /*                  Конструкторы                    */
        public MenuSession(IgnisGame ignisGame, SpriteBatch spriteBatch, List<BeatMap> beatMapsList)
        {
            SessionIgnisGame = ignisGame;
            SessionSpriteBatch = spriteBatch;

            BeatMapsList = beatMapsList;

            if (BeatMapsList.Any())
            {
                SelectedBeatMap = 0;
            }
            else
            {
                SelectedBeatMap = null;
            }
        }

        public MenuSession(IgnisGame ignisGame, SpriteBatch spriteBatch)
        {
            SessionIgnisGame = ignisGame;
            SessionSpriteBatch = spriteBatch;
            BeatMapsList = new List<BeatMap>();
            SelectedBeatMap = null;
        }
        /*                  __Конструкторы__                    */



        /*                  Логика                  */
        public override void Start()
        {
            // TODO Пройтись по всем битмапам и подгрузить всё, что им нужно
            LoadBeatMaps();

            //foreach (var map in BeatMapsList)
            //{
            //    map.SaveBeatMap("Beatmaps\\");
            //}

            Username = SessionIgnisGame.Username;
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                SessionIgnisGame.Exit();

            //MenuSession session = (MenuSession)SessionIgnisGame.CurrentSession;

            if (SessionIgnisGame.IsKeyReleased(Keys.Enter))
            {
                SessionIgnisGame.CurrentSession = new GameSession(SessionIgnisGame, SessionSpriteBatch, GetSelectedBeatMap());
                SessionIgnisGame.CurrentSession.Start();

                // TODO наверное, нужно что-то вручную почистить или написать Dispose()
            }
            else if (SessionIgnisGame.IsKeyReleased(Keys.Up))
            {
                SelectPreviousIndex();
            }
            else if (SessionIgnisGame.IsKeyReleased(Keys.Down))
            {
                SelectNextIndex();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            IgnisRender.DrawBackground(IgnisRender.DefaultBackground, SessionSpriteBatch, IgnisRender.ScalingMode.Stretch);

            //IgnisRender.DrawRectangle(SessionSpriteBatch,
            //    DarkGrayColor, Vector2.Zero,
            //    IgnisRender.GetAbsoluteMeasureString(SessionSpriteBatch, IgnisRender.DefaultFont, Username, 0.05f) + new Vector2(55f, 25f));

            // Фон для юзернейма
            IgnisRender.DrawRectangle(SessionSpriteBatch, 
                new Color(IgnisRender.DarkBrownColor, 200), Vector2.Zero,
                IgnisRender.GetAbsoluteMeasureString(SessionSpriteBatch, IgnisRender.DefaultFont, Username, 0.05f) + new Vector2(50f, 20f),
                Vector2.Zero, (int)MenuDrawOrder.TextBackground / 100f);

            // Юсернейм
            IgnisRender.DrawText(SessionSpriteBatch,
                Username,
                new Vector2(25f, 10f), 
                new Color(IgnisRender.DarkGreenColor, 220),
                IgnisRender.GetAbsoluteFontScale(SessionSpriteBatch, IgnisRender.DefaultFont, 0.05f),
                (int)MenuDrawOrder.Text / 100f);

            
            // Отрисовывает список карт
            if (SelectedBeatMap != null)
            {
                float beatMapPosHeight = 0f;
                for (int i = (int)(SelectedBeatMap == 0 ? 0 : SelectedBeatMap - 1); i < BeatMapsList.Count; i++)
                {
                    Vector2 pos = IgnisRender.GetAbsolutePosition(0.45f, 0, SessionSpriteBatch);

                    DrawBeatMap(BeatMapsList[i],
                        pos + new Vector2(0, beatMapPosHeight),
                        Vector2.Zero, i == SelectedBeatMap);

                    float percX = IgnisRender.GetAbsoluteX(0.01f, SessionSpriteBatch);
                    beatMapPosHeight += i == SelectedBeatMap
                        ? BeatMapRelativeSizeSelected.Y * percX
                        : BeatMapRelativeSize.Y * percX;

                    if (beatMapPosHeight > SessionSpriteBatch.GraphicsDevice.Viewport.Height)
                    {
                        break;
                    }
                }
            }
        }
        /*                  __Логика__                  */



        /*                  Вспомогательные функции                 */
        // Подгружает битмапы из файлов
        public void LoadBeatMaps()
        {
            BeatMapsList.Clear();

            // TODO Загрузить карты из файлов
            BeatMapsList.Add(SessionIgnisGame.TestBeatMap);
            BeatMapsList.Add(new BeatMap("Osu mania sucks!", 500, 25));
            BeatMapsList.Add(new BeatMap("Ti kto takoyu", 228, 14));
            BeatMapsList.Add(new BeatMap("4tobi eto delat", 1337, 88));
            BeatMapsList.Add(new BeatMap("Nyeh heh heh heh", 666, 15));
            BeatMapsList.Add(new BeatMap("Osu taiko cool!", 500, 25));
            BeatMapsList.Add(new BeatMap("Keuda ti zvonishj", 228, 14));
            BeatMapsList.Add(new BeatMap("Slava Ukraine", 1337, 88));
            BeatMapsList.Add(new BeatMap("Keys go brrrrr", 666, 1));
            BeatMapsList.Add(new BeatMap("Popa shavala trusi", 666, 15));
            BeatMapsList.Add(new BeatMap("Never gonna give you up", 666, 15));
            BeatMapsList.Add(new BeatMap("Invisible BeatMap", 666, 15));

            //BeatMapsList.Add(BeatMap.FromFile("Beatmaps\\Keys_go_brrrrr.icbm"));

            if (BeatMapsList.Any())
            {
                SelectedBeatMap = 0;
            }
            else
            {
                SelectedBeatMap = null;
            }
        }

        // Получение выбраной битмапы
        public BeatMap GetSelectedBeatMap()
        {
            if (!BeatMapsList.Any())
                throw new Exception("BeatMap List is Empty! You cannot select any.");

            if (SelectedBeatMap == null)
                throw new Exception("Selected BeatMap is null!");

            return BeatMapsList[(int) SelectedBeatMap];
        }

        // Функции управления индексом
        // Выбрать следующий индекс
        public void SelectNextIndex()
        {
            if (BeatMapsList.Count <= 0)
                return;

            if (selectedBeatMap + 1 < BeatMapsList.Count)
            {
                SelectedBeatMap++;
            }
            else
            {
                SelectedBeatMap = 0;
            }
        }

        // Выбрать предыдущий индекс
        public void SelectPreviousIndex()
        {
            if (BeatMapsList.Count <= 0)
                return;

            if (selectedBeatMap - 1 >= 0)
            {
                SelectedBeatMap--;
            }
            else
            {
                SelectedBeatMap = BeatMapsList.Count - 1;
            }
        }

        // Отрисовать 
        private void DrawBeatMap(BeatMap map, Vector2 position, Vector2 origin, bool isSelected)
        {
            Vector2 rightCorner = position - origin;
            float percX = IgnisRender.GetAbsoluteX(0.01f, SessionSpriteBatch);

            // Фон
            if (isSelected)
            {
                IgnisRender.DrawRectangle(
                    SessionSpriteBatch,
                    new Color(IgnisRender.GreenColor, 220),
                    position,
                    BeatMapRelativeSizeSelected * percX, origin, (int)MenuDrawOrder.TextBackground / 100f);
            }
            else
            {
                IgnisRender.DrawRectangle(
                    SessionSpriteBatch,
                    new Color(IgnisRender.DarkGreenColor, 220),
                    position,
                    BeatMapRelativeSize * percX, origin, (int)MenuDrawOrder.TextBackground / 100f);
            }

            float currentHeight = 0f;
            float boxHeight = isSelected ? BeatMapRelativeSizeSelected.Y : BeatMapRelativeSize.Y;
            float boxWidth = isSelected ? BeatMapRelativeSizeSelected.X : BeatMapRelativeSize.X;

            // Название карты
            float relativeNameFontScale = SessionSpriteBatch.GraphicsDevice.Viewport.Height / (5f * percX) * 0.3f / 100f;
            IgnisRender.DrawText(SessionSpriteBatch,
                IgnisRender.CutStringToFitWidth(map.Name, relativeNameFontScale, boxWidth * percX * 0.97f, SessionSpriteBatch),
                rightCorner + new Vector2(boxWidth * percX * 0.02f, boxHeight * percX * 0.1f),
                new Color(IgnisRender.DarkBrownColor, 220),
                IgnisRender.GetAbsoluteFontScale(SessionSpriteBatch, IgnisRender.DefaultFont, relativeNameFontScale),
                (int)MenuDrawOrder.Text / 100f);

            currentHeight += boxHeight * percX * 0.1f +
                             relativeNameFontScale * SessionSpriteBatch.GraphicsDevice.Viewport.Height;

            // Длительность карты
            string mapInfo = $"Duration: {TimeSpan.FromSeconds(map.Duration / map.TPS)}";
            IgnisRender.DrawText(SessionSpriteBatch,
                IgnisRender.CutStringToFitWidth(mapInfo, relativeNameFontScale * 0.8f, boxWidth * percX * 0.97f, SessionSpriteBatch),
                rightCorner + new Vector2(boxWidth * percX * 0.02f, currentHeight),
                new Color(IgnisRender.DarkBrownColor, 220),
                relativeNameFontScale * 0.8f,
                (int)MenuDrawOrder.Text / 100f);

            //IgnisRender.DrawRectangle(SessionSpriteBatch, Color.Red, rightCorner + new Vector2(0, currentHeight), new Vector2(25, 25), Vector2.Zero);
        }
        /*                  __Вспомогательные функции__                 */


    }
}
