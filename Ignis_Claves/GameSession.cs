using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace IgnisClaves
{
    public class GameSession : Session
    {
        public uint CurrentTick = 0;
        public GameSessionState State = GameSessionState.Stop;
        public int PlayerScore = 0;
        public BeatMap BeatMap;

        public enum GameSessionState
        {
            Stop,
            Going,
            Paused,
            Ended
        }

        public GameSession(BeatMap beatMap)
        {
            BeatMap = beatMap;
        }

        public override void Start()
        {
            State = GameSessionState.Going;

        }

        public override void Draw()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
