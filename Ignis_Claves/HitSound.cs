using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace IgnisClaves
{
    public class HitSound
    {
        private readonly SoundEffect sound;

        public static HitSound Clap;
        public static HitSound Kick;
        public static HitSound Snare;
        public static HitSound HiHat;

        static HitSound()
        {
            Clap = new HitSound(AppController.Ignis.Content.Load<SoundEffect>("clap"));
            Kick = new HitSound(AppController.Ignis.Content.Load<SoundEffect>("kick"));
            Snare = new HitSound(AppController.Ignis.Content.Load<SoundEffect>("snare"));
            HiHat = new HitSound(AppController.Ignis.Content.Load<SoundEffect>("hihat"));
        }

        public HitSound(SoundEffect sound)
        {
            this.sound = sound;
        }

        internal void Play()
        {
            sound.Play();
        }
    }
}
