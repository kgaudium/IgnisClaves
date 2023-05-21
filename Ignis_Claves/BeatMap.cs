using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using ProtoBuf;

namespace IgnisClaves;

[ProtoContract]
public class BeatMap
{
    [ProtoMember(1)] public string Artist;

    [ProtoMember(6)] public Song Music;

    [ProtoMember(2)] public string Name;

    [ProtoMember(5)] public Dictionary<int, KeyValuePair<Stave.KeysEnum, HitSound>[]> Notes;

    [ProtoMember(3)] public ushort TPS; // ticks per second

    public BeatMap(int duration)
    {
        Duration = duration;
        // TODO конструктор сделать (наверное, когда буду делать редактор)
    }

    public BeatMap(string name, int duration, ushort tps)
    {
        Duration = duration;
        Name = name;
        TPS = tps;
        Notes = new Dictionary<int, KeyValuePair<Stave.KeysEnum, HitSound>[]>
        {
            {
                0, new KeyValuePair<Stave.KeysEnum, HitSound>[]
                    { new(0, HitSound.HiHat), new(Stave.KeysEnum.InnerLeft, HitSound.HiHat) }
            },

            {
                10, new KeyValuePair<Stave.KeysEnum, HitSound>[]
                {
                    new(0, HitSound.HiHat), new(Stave.KeysEnum.InnerRight, HitSound.HiHat),
                    new(Stave.KeysEnum.InnerLeft, HitSound.HiHat)
                }
            }
        };

        // Music = 
        // Artist = 
    }

    [ProtoMember(4)] public int Duration { get; private set; }

    //public IEnumerable<KeyValuePair<byte, HitSound>[]> GetNotes()
    //{
    //    for (uint i = 0; i < Duration; i++)
    //    {
    //        yield return Notes.ContainsKey(i) ? Notes[i] : null;
    //    }
    //}

    public KeyValuePair<Stave.KeysEnum, HitSound>[] GetNotes(int tick)
    {
        return Notes.ContainsKey(tick) ? Notes[tick] : null;
    }

    public static BeatMap FromFile(string path)
    {
        return IgnisUtils.LoadFromFile<BeatMap>(path);
    }


    // TODO сохранение в файл
    /// <summary>
    ///     Сохраняет карту в указанную папку
    /// </summary>
    /// <param name="folderPath"></param>
    public void SaveBeatMap(string folderPath)
    {
        string fileName = IgnisUtils.ConvertFileName($"{Artist} - {Name}");
        IgnisUtils.CreateFolderIfNotExists(folderPath);
        IgnisUtils.SaveToFile(this, folderPath.Replace('/', '\\') + $"\\{fileName}.icbm");
    }

    public static BeatMap GenerateStairsBeatMap(string name, int duration, ushort tps, int step)
    {
        BeatMap result = new(duration)
        {
            Name = name,
            TPS = tps
        };

        Random rand = new();

        result.Notes = new Dictionary<int, KeyValuePair<Stave.KeysEnum, HitSound>[]>();

        int key = 0;
        int flag = 1;

        for (int i = 0; i < duration; i+=step)
        {
            KeyValuePair<Stave.KeysEnum, HitSound>[] pairArray = new KeyValuePair<Stave.KeysEnum, HitSound>[1];

            pairArray[0] = new KeyValuePair<Stave.KeysEnum, HitSound>((Stave.KeysEnum)key, HitSound.Kick);
            
            result.Notes.Add(i, pairArray);

            if (key >= 3)
                flag = -1;
            else if (key <= 0)
                flag = 1;

            key += flag;
        }

        return result;
    }

    public static BeatMap GenerateRandomBeatMap(string name, int duration, ushort tps)
    {
        BeatMap result = new(duration)
        {
            Name = name,
            TPS = tps
        };

        Random rand = new();

        result.Notes = new Dictionary<int, KeyValuePair<Stave.KeysEnum, HitSound>[]>();

        for (int i = 0; i < duration; i+=3)
        {
            int randInt = rand.Next(20);

            if (randInt == 9)
            {
                var pairArray = new KeyValuePair<Stave.KeysEnum, HitSound>[4];

                for (byte j = 0; j < 4; j++)
                    if (rand.Next(4) == 0)
                        pairArray[j] = new KeyValuePair<Stave.KeysEnum, HitSound>((Stave.KeysEnum)j, HitSound.Kick);


                result.Notes.Add(i, pairArray);
            }
        }

        return result;
    }
}