using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Principal;
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

        public string Username;

        public BeatMap TestBeatMap;

        private FrameCounter _frameCounter = new FrameCounter();

        // Конструктор
        public IgnisGame()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        // Инициализация
        protected override void Initialize()
        {
            // Фключает фулскрин
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.IsFullScreen = true;
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.ApplyChanges();

            IsFixedTimeStep = false;

            CurrentKeyboardState = OldKeyboardState = Keyboard.GetState();

            IgnisRender.Initialize(this);

            CurrentSession = new MenuSession(this, spriteBatch);

            Username = IgnisUtils.GetUsername();
            TestBeatMap = new BeatMap(100)
            {
                Name = "??Hello,< How Are You?",
                TPS = 5, 
                Notes = new Dictionary<uint, KeyValuePair<byte, HitSound>[]>()
            {
                {
                    0, new KeyValuePair<byte , HitSound>[]
                    {new (0, HitSound.HiHat), new (1, HitSound.HiHat)}

                },

                {
                    10, new KeyValuePair<byte , HitSound>[]
                    {new (0, HitSound.HiHat), new (2, HitSound.HiHat), new (3, HitSound.HiHat)}
                }
            }
        };

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
            CurrentSession.Update(gameTime);


            base.Update(gameTime);
        }

        // Отрисовка
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(255, 255, 255));

            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            // Отрисовка текущей сессии
            CurrentSession.Draw(gameTime);


            // Отрисовка FPS
            {
                var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                _frameCounter.Update(deltaTime);
                var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);
                Vector2 fpsBox = IgnisRender.GetAbsoluteMeasureString(spriteBatch, IgnisRender.DefaultFont, fps, 0.04f);

                // Фон
                IgnisRender.DrawRectangle(spriteBatch,
                    new Color(IgnisRender.DarkGrayColor, 200),
                    IgnisRender.GetAbsolutePosition(0f, 1f, spriteBatch) - new Vector2(0f, fpsBox.Y), fpsBox,
                    Vector2.Zero, 0.95f);

                // Текст
                IgnisRender.DrawText(spriteBatch,
                    fps,
                    IgnisRender.GetAbsolutePosition(0f, 1f, spriteBatch) - new Vector2(0f, fpsBox.Y),
                    IgnisRender.DarkBrownColor,
                    IgnisRender.GetAbsoluteFontScale(spriteBatch, IgnisRender.DefaultFont, 0.04f),
                    0.96f);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }


        public bool IsKeyReleased(Keys key)
        {
            return OldKeyboardState.IsKeyDown(key) && CurrentKeyboardState.IsKeyUp(key);
        }

        public bool IsKeyPressed(Keys key)
        {
            return OldKeyboardState.IsKeyUp(key) && CurrentKeyboardState.IsKeyDown(key);
        }
    }
}