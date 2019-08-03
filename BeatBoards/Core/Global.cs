using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatBoards.Core
{
    public class Global
    {
        public static string BeatBoardsID = "0";
    }

    public class Following
    {
        public string Uuid { get; set; }
        public string Username { get; set; }
        public Role Role { get; set; }
        public string Country { get; set; }
        
        public float RankingPoints { get; set; }
        public int Rank { get; set; }
        public int Fails { get; set; }

    }


    public enum Role
    {
        Owner,
        Contributor,
        Supporter,
        Ranker,
        Curator,
        ScoreSaber,
        Player,
        Toxic
    }
}
