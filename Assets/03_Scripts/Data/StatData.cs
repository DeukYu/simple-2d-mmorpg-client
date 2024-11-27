using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class Stat
    {
        public int level;
        public int maxHp;
        public int maxMp;
        public int attack;
        public int totalExp;

    }

    public class StatData : ILoader<int, Stat>
    {
        public List<Stat> stats = new List<Stat>();

        public Dictionary<int, Stat> MakeDict()
        {
            Dictionary<int, Stat> dict = new Dictionary<int, Stat>();
            foreach (var stat in stats)
            {
                dict.Add(stat.level, stat);
            }
            return dict;
        }
    }
}