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
        /*                  __Поля и свойства__                 */



        /*                  Конструкторы                    */
        public MenuSession(List<BeatMap> beatMapsList)
        {
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

        public MenuSession()
        {
            BeatMapsList = new List<BeatMap>();
            SelectedBeatMap = null;
        }
        /*                  __Конструкторы__                    */



        /*                  Логика                  */
        public override void Start()
        {
            // TODO Загрузить карты
            // TODO Пройтись по всем битмапам и подгрузить всё, что им нужно

            BackgroundTexture = IgnisRender.LoadTexture2D("default\\ralsei_3");
        }

        public override void Update(IgnisGame ignisGame)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                ignisGame.Exit();

            MenuSession session = (MenuSession) ignisGame.CurrentSession;

            if (Keyboard.GetState().IsKeyDown(Keys.Enter)) // TODO надо проверять нажатие и отпускание (сейчас он возвращает true потому что кнопка не нажата :) )
            {
                ignisGame.CurrentSession = new GameSession(session.GetSelectedBeatMap());
                ignisGame.CurrentSession.Start();
                // TODO наверное, нужно что-то вручную почистить или написать Dispose()
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                session.SelectPreviousIndex();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                session.SelectNextIndex();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            IgnisRender.DrawBackground(BackgroundTexture, spriteBatch, IgnisRender.ScalingMode.Stretch);

            spriteBatch.End();
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
