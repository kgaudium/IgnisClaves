using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.XPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IgnisClaves
{
    public class GameSession : Session
    {
        public uint CurrentTick = 0;
        private TimeSpan lastUpdateTime;
        private TimeSpan targetElapsedTime;

        public GameSessionState State = GameSessionState.Stop;
        public int PlayerScore = 0;
        public float NoteSpeed;
        
        public BeatMap CurrentBeatMap;
        private Stave stave;

        public Dictionary<Note, uint> SpawnedNotes = new();

        public enum GameSessionState
        {
            Stop,
            Going,
            Paused,
            Ended
        }

        public enum GameDrawOrder
        {
            StaveBackground = 10,
            Notes = 15,
            Keys = 20,

            TextBackground = 49,
            Text = 50,
        }


        public GameSession(IgnisGame ignisGame, SpriteBatch spriteBatch, BeatMap beatMap)
        {
            CurrentBeatMap = beatMap;

            SessionIgnisGame = ignisGame;
            SessionSpriteBatch = spriteBatch;
            NoteSpeed = 1f;
        }

        public override void Start()
        {
            State = GameSessionState.Going;
            stave = new Stave();

            targetElapsedTime = TimeSpan.FromSeconds(1f / CurrentBeatMap.TPS);
            lastUpdateTime = TimeSpan.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
            if (SessionIgnisGame.IsKeyReleased(Keys.Escape))
            {
                SessionIgnisGame.CurrentSession = new MenuSession(SessionIgnisGame, SessionSpriteBatch);
                SessionIgnisGame.CurrentSession.Start();

                // TODO наверное, нужно что-то вручную почистить или написать Dispose()
            }

            if (gameTime.TotalGameTime - lastUpdateTime >= targetElapsedTime)
            {
                KeyValuePair<byte, HitSound>[] notes = CurrentBeatMap.GetNotes(CurrentTick);
                if (notes != null)
                {
                    foreach (var note in notes)
                    {
                        //note.Draw(SessionSpriteBatch, 0.5f, new Vector2(0.07f, 0.03f));
                        //note.PositionY += NoteSpeed;

                        SpawnedNotes.Add(new Note(note.Value, note.Key, SessionSpriteBatch), CurrentTick);
                    }
                }


                lastUpdateTime = gameTime.TotalGameTime;
                CurrentTick += 1;
            }

            stave.UpdateKeyStates(SessionIgnisGame.CurrentKeyboardState);
        }

        public override void Draw(GameTime gameTime)
        {
            IgnisRender.DrawBackground(IgnisRender.DefaultBackground, SessionSpriteBatch, IgnisRender.ScalingMode.Stretch);

            stave.Draw(SessionSpriteBatch, 0.5f, 0.07f);


            // Debug text
            IgnisRender.DrawText(SessionSpriteBatch,
                $"{SpawnedNotes.Count}\n{gameTime.TotalGameTime}\n{lastUpdateTime}\n{targetElapsedTime}",
                IgnisRender.GetAbsolutePosition(0f, 0.45f, SessionSpriteBatch),
                IgnisRender.DarkBrownColor,
                IgnisRender.GetAbsoluteFontScale(SessionSpriteBatch, IgnisRender.DefaultFont, 0.04f),
                (int)GameDrawOrder.Text / 100f);


            foreach (var pair in SpawnedNotes)
            {
                pair.Key.Draw(SessionSpriteBatch, 0.5f, new Vector2(0.07f, 0.03f));
                pair.Key.PositionY += NoteSpeed;

                if (pair.Key.PositionY > SessionSpriteBatch.GraphicsDevice.Viewport.Height)
                {
                    SpawnedNotes.Remove(pair.Key);
                }
            }
        }


    }
}
