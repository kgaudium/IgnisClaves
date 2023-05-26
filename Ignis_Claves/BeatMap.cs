using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Media;
using ProtoBuf;

namespace IgnisClaves;

[ProtoContract]
public class BeatMap
{
    public string Artist;

    //public Song Music;

    public string Name;

    public Dictionary<int, KeyValuePair<Stave.KeysEnum, HitSound>[]> Notes;

    public ushort TPS; // ticks per second

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

    public int Duration { get; private init; }

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

    public static BeatMap FromFile(string absolutePath)
    {
        return IgnisUtils.Deserialize<BeatMapWrapper>(absolutePath).ToBeatMap();
    }

    public static List<BeatMap> LoadBeatMapsFromAppData()
    {
        var mapList = new List<BeatMap>();
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "IgnisClaves\\Beatmaps");

        IgnisUtils.CreateFolderIfNotExists(path);

        foreach (var file in Directory.GetFiles(path))
        {
            if (Path.GetExtension(file) != ".icbm")
                continue;

            try
            {
                mapList.Add(FromFile(file));
            }
            catch (Exception e)
            {
                throw new Exception($"Invalid .icbm file! {e.Message}");
            }
        }

        return mapList;
    }

    // TODO сохранение в файл
    /// <summary>
    ///     Сохраняет карту в указанную папку
    /// </summary>
    public void SaveBeatMap()
    {
        string fileName = IgnisUtils.ConvertFileName($"{Artist} - {Name}");
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "IgnisClaves\\Beatmaps");
        IgnisUtils.CreateFolderIfNotExists(path);
        IgnisUtils.Serialize(new BeatMapWrapper(this), Path.Combine(path,$"{fileName}.icbm"));
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

    [Serializable]
    internal class BeatMapWrapper
    {
        private string Artist { get; }

        private string Name { get; }

        private Dictionary<int, Stave.KeysEnum[]> Notes { get; }

        private ushort TPS { get; }

        private int Duration { get; }

        public BeatMapWrapper(BeatMap map)
        {
            Artist = map.Artist;
            Name = map.Name;
            TPS = map.TPS;
            Duration = map.Duration;

            Notes = new Dictionary<int, Stave.KeysEnum[]>();
            foreach (var pair in map.Notes)
            {
                Notes.Add(pair.Key, pair.Value.Select(x => x.Key).ToArray());
            }
        }

        public BeatMap ToBeatMap()
        {
            BeatMap map = new BeatMap(Duration)
            {
                Artist = Artist,
                Name = Name,
                TPS = TPS,
                Duration = Duration,
                Notes = new Dictionary<int, KeyValuePair<Stave.KeysEnum, HitSound>[]>(),
            };

            foreach (var pair in Notes)
            {
                map.Notes.Add(pair.Key, pair.Value.Select(x => new KeyValuePair<Stave.KeysEnum, HitSound>(x, HitSound.Kick)).ToArray());
            }

            return map;
        }
    }
}