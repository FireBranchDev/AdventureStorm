using System;
using System.Collections.Generic;

namespace AdventureStorm.Data
{
    [Serializable]
    public class LevelsData
    {
        public List<LevelData> Levels;

        public LevelData CurrentLevel;
    }
}
