using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static System.Collections.Specialized.BitVector32;

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

        public Texture2D BackgroundTexture;

        public string Username;
        
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
            // TODO Загрузить карты
            // TODO Пройтись по всем битмапам и подгрузить всё, что им нужно

            BackgroundTexture = SessionIgnisGame.Content.Load<Texture2D>("default\\ralsei_3");

            Username = IgnisRender.GetUsername();
        }

        public override void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                SessionIgnisGame.Exit();

            MenuSession session = (MenuSession)SessionIgnisGame.CurrentSession;

            if (SessionIgnisGame.IsKeyReleased(Keys.Enter))
            {
                SessionIgnisGame.CurrentSession = new GameSession(session.GetSelectedBeatMap());
                SessionIgnisGame.CurrentSession.Start();

                // TODO наверное, нужно что-то вручную почистить или написать Dispose()
            }
            else if (SessionIgnisGame.IsKeyReleased(Keys.Up))
            {
                session.SelectPreviousIndex();
            }
            else if (SessionIgnisGame.IsKeyReleased(Keys.Down))
            {
                session.SelectNextIndex();
            }
        }

        public override void Draw()
        {
            IgnisRender.DrawBackground(BackgroundTexture, SessionSpriteBatch, IgnisRender.ScalingMode.Stretch);

            IgnisRender.DrawRectangle(SessionSpriteBatch, 
                new Color(214, 214, 214), 
                IgnisRender.GetAbsolutePosition(0.05f, 0.05f, SessionSpriteBatch),
                IgnisRender.GetAbsoluteMeasureString(SessionSpriteBatch, IgnisRender.DefaultFont, Username, 0.05f));

            IgnisRender.DrawText(SessionSpriteBatch,
                Username,
                //IgnisRender.GetAbsoluteFontScale(SessionSpriteBatch, IgnisRender.DefaultFont, 0.5f).ToString(),
                IgnisRender.GetAbsolutePosition(0.05f, 0.05f, SessionSpriteBatch), 
                Color.Gray, 
                IgnisRender.GetAbsoluteFontScale(SessionSpriteBatch, IgnisRender.DefaultFont, 0.05f), 
                0.5f);

            //IgnisRender.DrawText(SessionSpriteBatch, Username, IgnisRender.GetAbsolutePosition(0.05f, 0.05f, SessionSpriteBatch), Color.Gray, );
        }
        /*                  __Логика__                  */



        /*                  Вспомогательные функции                 */
        // Подгружает битмапы из файлов
        public void LoadBeatMaps()
        {
            BeatMapsList.Clear();

            // TODO Загрузить карты из файлов

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
        /*                  __Вспомогательные функции__                 */


    }
}
