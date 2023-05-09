using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IgnisClaves
{
    public class IgnisGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Keys[] Keybinds = new[] { Keys.D, Keys.F, Keys.J, Keys.K }; // TODO подгружать из файла настроек

        #nullable enable
        public Session CurrentSession;
        //public GameSession? CurrentGameSession;
        //public MenuSession? CurrentMenuSession;

        // Конструктор
        public IgnisGame()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            graphics.IsFullScreen = true;
        }

        // Инициализация
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.ApplyChanges();

            CurrentSession = new MenuSession();

            base.Initialize();
        }

        // Подгрузка контента
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Загрузить карты в меню (или вызвать старт у меню)
            CurrentSession.Start();
        }

        // Апдейт
        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            CurrentSession.Update(this);
            

            base.Update(gameTime);
        }

        // Отрисовка
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(87,87,87));


            // TODO: Add your drawing code here

            
            //if (CurrentMenuSession != null)
            //{
            //    CurrentMenuSession.Draw(spriteBatch);
            //}
            CurrentSession.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}