using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static System.Net.Mime.MediaTypeNames;

namespace IgnisClaves
{
    public class IgnisGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Keys[] Keybinds = new[] { Keys.D, Keys.F, Keys.J, Keys.K }; // TODO подгружать из файла настроек

        #nullable enable
        public Session CurrentSession;

        public KeyboardState CurrentKeyboardState;
        public KeyboardState OldKeyboardState;

        // Конструктор
        public IgnisGame()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        // Инициализация
        protected override void Initialize()
        {
            // Фключает фулскрин
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            CurrentKeyboardState = OldKeyboardState = Keyboard.GetState();

            IgnisRender.Initialize(this);

            CurrentSession = new MenuSession(this, spriteBatch);

            base.Initialize();
        }

        // Подгрузка контента
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            CurrentSession.SessionSpriteBatch = spriteBatch;

            // Загрузить карты в меню (или вызвать старт у меню)
            CurrentSession.Start();
        }

        // Апдейт
        protected override void Update(GameTime gameTime)
        {
            OldKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();

            // Апдейтит текущую сессию
            CurrentSession.Update();


            base.Update(gameTime);
        }

        // Отрисовка
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(87,87,87));

            spriteBatch.Begin();

            // Отрисовывает текущую сессию
            CurrentSession.Draw();


            //// Finds the center of the string in coordinates inside the text rectangle
            //Vector2 textMiddlePoint = IgnisRender.DefaultFont.MeasureString("MonoGame Font Test") / 2;
            //// Places text in center of the screen
            //Vector2 position = new Vector2(Window.ClientBounds.Width / 2f, Window.ClientBounds.Height / 2f);
            //spriteBatch.DrawString(IgnisRender.DefaultFont, "MonoGame Font Test", new Vector2(0f, 0f),
            //    Color.White, 0, Vector2.Zero, IgnisRender.GetAbsoluteFontScale(spriteBatch, IgnisRender.DefaultFont, 100f),
            //    SpriteEffects.None, 0.5f);

            IgnisRender.DrawCenteredText(spriteBatch, IgnisRender.GetUsername(), IgnisRender.GetAbsolutePosition(0.5f,0.5f,spriteBatch), Color.Red, IgnisRender.GetAbsoluteFontScale(spriteBatch, IgnisRender.DefaultFont, 0.1f), 0.5f);
            //IgnisRender.DrawRectangle(spriteBatch, Color.Red, new Vector2(10f, 10f), new Vector2(50f, 10f));

            spriteBatch.End();
            base.Draw(gameTime);
        }


        public bool IsKeyReleased(Keys key)
        {
            return OldKeyboardState.IsKeyDown(key) && CurrentKeyboardState.IsKeyUp(key);
        }
    }
}