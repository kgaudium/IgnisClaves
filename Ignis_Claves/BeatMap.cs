using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Media;
using ProtoBuf;

namespace IgnisClaves
{
    [ProtoContract]
    public class BeatMap
    {
        [ProtoMember(1)]
        public string Artist;

        [ProtoMember(2)]
        public string Name;

        [ProtoMember(3)]
        public ushort TPS; // ticks per second

        [ProtoMember(4)]
        public uint Duration { get; private set; }

        [ProtoMember(5)]
        public Dictionary<uint, KeyValuePair<byte, HitSound>[]> Notes;

        [ProtoMember(6)]
        public Song Music;

        public BeatMap(uint duration)
        {
            Duration = duration;
            // TODO конструктор сделать (наверное, когда буду делать редактор)
        }

        public BeatMap(string name, uint duration, ushort tps)
        {
            Duration = duration;
            Name = name;
            TPS = tps;
            Notes = new Dictionary<uint, KeyValuePair<byte, HitSound>[]>()
            {
                {
                    0, new KeyValuePair<byte , HitSound>[]
                    {new (0, HitSound.HiHat), new (2, HitSound.HiHat)}

                },

                {
                    10, new KeyValuePair<byte , HitSound>[]
                    {new (0, HitSound.HiHat), new (2, HitSound.HiHat), new (3, HitSound.HiHat)}
                }
            };

            // Music = 
            // Artist = 
        }

        //public IEnumerable<KeyValuePair<byte, HitSound>[]> GetNotes()
        //{
        //    for (uint i = 0; i < Duration; i++)
        //    {
        //        yield return Notes.ContainsKey(i) ? Notes[i] : null;
        //    }
        //}

        public KeyValuePair<byte, HitSound>[] GetNotes(uint tick)
        {
            return Notes.ContainsKey(tick) ? Notes[tick] : null;
        }

        public static BeatMap FromFile(string path)
        {
            return IgnisUtils.LoadFromFile<BeatMap>(path);
        }


        // TODO сохранение в файл
        /// <summary>
        /// Сохраняет карту в указанную папку
        /// </summary>
        /// <param name="folderPath"></param>
        public void SaveBeatMap(string folderPath)
        {
            string fileName = IgnisUtils.ConvertFileName($"{Artist} - {Name}");
            IgnisUtils.CreateFolderIfNotExists(folderPath);
            IgnisUtils.SaveToFile(this, folderPath.Replace('/', '\\') + $"\\{fileName}.icbm");
        }
    }
}
