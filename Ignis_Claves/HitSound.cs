using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;

namespace IgnisClaves
{
    internal class HitSound
    {
        internal SoundEffect Sound;

        public static HitSound Clap;
        public static HitSound Kick;
        public static HitSound Snare;
        public static HitSound HiHat;

        static HitSound()
        {
            // TODO clap = ...
        }

        public HitSound(SoundEffect sound)
        {
            Sound = sound;
        }

        internal void Play()
        {
            //TODO
        }
    }
}
