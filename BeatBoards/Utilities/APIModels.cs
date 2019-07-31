using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatBoards.Utilities
{
    public class APIModels
    {
        public class User
        {
            public UserData UserData { get; set; }
            public string[] Following { get; set; }
            public Platform Platform { get; set; }
            public string PlatformID { get; set; }
            public string BeatBoardsID { get; set; }
            public float RankPoints { get; set; }
            public int Rank { get; set; }
            public string Role { get; set; }
            public bool Banned { get; set; }
            public int Fails { get; set; }
            
        }

        public class UserData
        {
            public string Nickname { get; set; }
            public string Image { get; set; }
            public string CountryCode { get; set; }
        }

        public class LiteMap
        {
            public string MapHash { get; set; }
            public string LevelHash { get; set; }
            public BeatmapDifficulty Difficulty { get; set; }
            public string SongName { get; set; }
            public string SongSubName { get; set; }
            public string SongAuthorName { get; set; }
            public string LevelAuthorName { get; set; }
            public float BPM { get; set; }
            public int NoteCount { get; set; }
            public int ScoreCount { get; set; }
            public float MaxRP { get; set; }
            public int MaxScore { get; set; }
        }
        public class Score
        {
            public string[] ActiveModifiers { get; set; }
            public int RawScore { get; set; }
            public float RawPercent { get; set; }
            public int AdjustedScore { get; set; }
            public float RawRP { get; set; }
            public string Date { get; set; }
            public string UserID { get; set; }
            public string MapHash { get; set; }
        }

        public enum Platform
        {
            Steam,
            Oculus,
            Quest
        }
    }
}
