using System;
using System.Collections.Generic;

namespace IgnisClaves
{
    public class BeatMap
    {
        internal ushort TPS;
        internal uint Duration { get; private set; }

        internal Dictionary<uint, Note[]> Stave;

        public BeatMap()
        {
            // TODO конструктор сделать (наверное, когда буду делать редактор)
        }

        public IEnumerable<Note[]> GetNotes()
        {
            for (uint i = 0; i < Duration; i++)
            {
                yield return Stave.ContainsKey(i) ? Stave[i] : null;
            }
        }
    }
}
