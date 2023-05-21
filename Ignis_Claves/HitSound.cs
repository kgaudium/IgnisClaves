using Microsoft.Xna.Framework.Audio;

namespace IgnisClaves;

public class HitSound
{
    public static readonly HitSound Clap;
    public static readonly HitSound Kick;
    public static readonly HitSound Snare;
    public static readonly HitSound HiHat;
    private readonly SoundEffect sound;

    static HitSound()
    {
        Clap = new HitSound(AppController.Ignis.Content.Load<SoundEffect>("default\\clap"));
        Kick = new HitSound(AppController.Ignis.Content.Load<SoundEffect>("default\\kick"));
        Snare = new HitSound(AppController.Ignis.Content.Load<SoundEffect>("default\\snare"));
        HiHat = new HitSound(AppController.Ignis.Content.Load<SoundEffect>("default\\hihat"));
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