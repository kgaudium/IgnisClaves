using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.XPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IgnisClaves
{
    public class GameSession : Session
    {
        public int CurrentTick = 0;
        public int TickShift;
        private DateTime startTime;
        private TimeSpan totalGameTime;
        private TimeSpan lastUpdateTime;
        private TimeSpan targetElapsedTime;

        public GameSessionState State = GameSessionState.Stop;
        public int PlayerScore = 0;
        public float NoteSpeed;
        
        public BeatMap CurrentBeatMap;
        private Stave stave;

        public Dictionary<Note, int> SpawnedNotes = new();
        public float StartDelay = 1.5f;
        public Vector2 NoteRelativeSize = new Vector2(0.07f, 0.03f);

        private string debugString = "";

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

        public enum AccuracyEnum
        {
            Ultimate = 16,
            Perfect = 49,
            Great = 82,
            Good = 112,
            Bad = 136,
            EarlyMiss = 173,
            Miss
        }


        public GameSession(IgnisGame ignisGame, SpriteBatch spriteBatch, BeatMap beatMap)
        {
            CurrentBeatMap = beatMap;

            SessionIgnisGame = ignisGame;
            SessionSpriteBatch = spriteBatch;
            NoteSpeed = 15f;
        }

        public override void Start()
        {
            State = GameSessionState.Going;
            startTime = DateTime.Now;
            stave = new Stave();

            targetElapsedTime = TimeSpan.FromSeconds(1f / CurrentBeatMap.TPS);
            lastUpdateTime = TimeSpan.Zero;

            TickShift = (int)(IgnisRender.GetAbsoluteY(Stave.StaveRelativeHeight - NoteRelativeSize.Y, SessionSpriteBatch) / NoteSpeed) + 1 ;
            CurrentTick = -TickShift - (int)(StartDelay * CurrentBeatMap.TPS);

            SessionIgnisGame.SetTargetFps(480f);
        }

        public override void Update(GameTime gameTime)
        {
            // Если нажали ESC во время игры - пауза, если в паузе - выход в меню
            if (SessionIgnisGame.IsKeyReleased(Keys.Escape))
            {
                switch (State)
                {
                    case GameSessionState.Going:
                        State = GameSessionState.Paused;
                        break;
                    case GameSessionState.Paused:
                        QuitGame();
                        break;
                }

                // TODO наверное, нужно что-то вручную почистить или написать Dispose()
            }

            // Выход из паузы
            if (SessionIgnisGame.IsKeyReleased(Keys.Enter) && State == GameSessionState.Paused)
            {
                State = GameSessionState.Going;
            }

            // Когда карта заканчивается, статус - Ended // TODO задержку
            if (CurrentTick >= CurrentBeatMap.Duration)
            {
                State = GameSessionState.Ended;
            }

            PlayNote();

            switch (State)
            {
                // Если игра идёт
                case GameSessionState.Going:
                    totalGameTime = DateTime.Now - startTime;
                    stave.UpdateKeyStates();
                    break;

                // Если игры закончилась - выходит в меню
                case GameSessionState.Ended:
                    QuitGame();
                    break;
            }

            // Когда пора совершать тик
            if (totalGameTime - lastUpdateTime >= targetElapsedTime && State == GameSessionState.Going)
            {
                KeyValuePair<Stave.KeysEnum, HitSound>[] notes = CurrentBeatMap.GetNotes(CurrentTick + TickShift); // Получаем ноты с этого тика
                if (notes != null)
                {
                    foreach (var note in notes)
                    {
                        SpawnedNotes.Add(
                            //new Note(note.Value, note.Key, SessionSpriteBatch, -StartDelay * CurrentBeatMap.TPS * NoteSpeed),
                            new Note(note.Value, note.Key, SessionSpriteBatch, 0f),
                            CurrentTick + TickShift);
                        //SpawnedNotes.Add(new Note(note.Value, note.Key, SessionSpriteBatch, 0f), CurrentTick);
                    }
                }

                foreach (var pair in SpawnedNotes)
                {
                    pair.Key.PositionY += NoteSpeed;
                }

                lastUpdateTime = totalGameTime;
                CurrentTick += 1;
            }

        }

        public override void Draw(GameTime gameTime)
        {
            IgnisRender.DrawBackground(IgnisRender.DefaultBackground, SessionSpriteBatch, IgnisRender.ScalingMode.Stretch);

            stave.Draw(SessionSpriteBatch, 0.5f, 0.07f);


            // Debug text
            IgnisRender.DrawText(SessionSpriteBatch,
                $"{SpawnedNotes.Count}\n{totalGameTime}\n{lastUpdateTime}\n{targetElapsedTime}\n{CurrentTick}\n{PlayerScore}",
                //$"{SpawnedNotes.Count}\n{totalGameTime}\n{lastUpdateTime}\n{targetElapsedTime}\n{CurrentTick}\n{TickShift}",
                IgnisRender.GetAbsolutePosition(0f, 0.25f, SessionSpriteBatch),
                IgnisGame.TextColor,
                IgnisRender.GetAbsoluteFontScale(SessionSpriteBatch, IgnisRender.DefaultFont, 0.04f),
                (int)GameDrawOrder.Text / 100f);


            // Надпись paused
            if (State == GameSessionState.Paused)
            {
                IgnisRender.DrawText(SessionSpriteBatch,
                    "Paused...",
                    IgnisRender.GetAbsolutePosition(0.5f, 0.2f, SessionSpriteBatch),
                    IgnisRender.DarkGrayColor, 
                    IgnisRender.GetAbsoluteFontScale(SessionSpriteBatch, IgnisRender.DefaultFont, 0.04f),
                    (int)GameDrawOrder.Text / 100f);
            }
            

            // Отображение и движение нот
            foreach (var pair in SpawnedNotes)
            {
                pair.Key.Draw(SessionSpriteBatch, 0.5f, new Vector2(0.07f, 0.03f));


                //// Если игра идёт
                //if (totalGameTime - lastUpdateTime >= targetElapsedTime && State == GameSessionState.Going)
                //{
                    
                //}

                // Когда нота выходит за экран, то она удаляется
                if (pair.Key.PositionY > SessionSpriteBatch.GraphicsDevice.Viewport.Height)
                {
                    SpawnedNotes.Remove(pair.Key);
                }
            }
            
        }

        private void QuitGame()
        {
            SessionIgnisGame.CurrentSession = new MenuSession(SessionIgnisGame, SessionSpriteBatch);
            SessionIgnisGame.CurrentSession.Start();
        }

        private KeyValuePair<Note, AccuracyEnum>? CheckKeys()
        {
            //if (Keyboard.GetState().IsKeyDown(IgnisGame.Keybinds[Stave.KeysEnum.OuterLeft]))
            foreach (var pair in IgnisGame.Keybinds.Where(pair => SessionIgnisGame.IsKeyPressed(pair.Value)))
            {
                KeyValuePair<Note, int> note;

                float tickBottomLine = (int)AccuracyEnum.EarlyMiss / 1000f * CurrentBeatMap.TPS;
                float tickUpperLine = (int)AccuracyEnum.Bad / 1000f * CurrentBeatMap.TPS;

                try
                {
                    note = SpawnedNotes.First(x =>
                        x.Key.Line == pair.Key
                        && x.Value > CurrentTick - tickBottomLine
                        && x.Value < CurrentTick + tickUpperLine);
                }
                catch (InvalidOperationException)
                {
                    return null;
                }

                TimeSpan difference = TimeSpan.FromSeconds((float)(note.Value - CurrentTick) / CurrentBeatMap.TPS);

                //if (difference.Milliseconds is > (int)AccuracyEnum.Bad or < -(int)AccuracyEnum.EarlyMiss)
                //{
                //    return;
                //}

                foreach (var accuracy in Enum.GetValues(typeof(AccuracyEnum)))
                {
                    if ((AccuracyEnum)accuracy == AccuracyEnum.EarlyMiss &&
                        difference.Milliseconds > -(int)AccuracyEnum.EarlyMiss)
                    {
                        return new KeyValuePair<Note, AccuracyEnum>(note.Key, AccuracyEnum.EarlyMiss);
                    }

                    if (difference.Milliseconds < (int)accuracy && difference.Milliseconds > -(int)accuracy)
                    {
                        return new KeyValuePair<Note, AccuracyEnum>(note.Key, (AccuracyEnum)accuracy);
                    }
                }

                return new KeyValuePair<Note, AccuracyEnum>(note.Key, AccuracyEnum.Miss);
            }

            return null;
        }

        private void PlayNote()
        {
            KeyValuePair<Note, AccuracyEnum>? pair = CheckKeys();

            if (pair == null) return;

            AccuracyEnum accuracy = pair.Value.Value;
            Note note = pair.Value.Key;

            note.HitSound?.Play();

            SpawnedNotes.Remove(note);

            switch (accuracy)
            {
                case AccuracyEnum.Ultimate:
                    PlayerScore += 400;
                    break;

                case AccuracyEnum.Perfect:
                    PlayerScore += 300;
                    break;

                case AccuracyEnum.Great:
                    PlayerScore += 200;
                    break;

                case AccuracyEnum.Good:
                    PlayerScore += 100;
                    break;

                case AccuracyEnum.Bad:
                    PlayerScore += 50;
                    break;

                case AccuracyEnum.EarlyMiss or AccuracyEnum.Miss:
                    break;
            }
        }
    }
}
