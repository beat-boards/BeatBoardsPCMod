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
        public static string BeatBoardsAPIURL = "http://198.27.67.20:4321";

        public static List<string> RandomTextV1 = new List<string>()
        {
            "Try Minecraft!",
            "Play Vivecraft",
            "\"eef\"\n-Bobbie 2019",
            "william gay\nsqueaksies cute",
            "\"I guess Chocola is superior\nShe actually has her mouth open for curry\"\n-Assistant August 2019",
            "\"b***h won't eat\"\n-squeaksies 2019",
            "\"hey guys i need more quotes for beat boards\"\n-Auros",
            "\"<color=#7289da>@everyone</color>\"-Reaxt",
            "\"besides, im an adult\"-ragesaq 2019",
            "Say the line, <color=purple>Freeek</color>!",
            "\"Quit your family\"\n-Weaboo Jones 2018",
            "\"GASP!\"",
            "This is a random string of text.",
            "Okay, this is epic!",
            "THE BEST THE BEST THE BEST THE BEST THE BEST THE BEST THE BE-",

        }; 
    }

    public class User
    {
        public string Id { get; set; }
        public string SteamId { get; set; }
        public string OculusId { get; set; }
        public bool Banned { get; set; }
        public string Username { get; set; }
        public Role Role { get; set; }
        public string Country { get; set; }
        public float Rp { get; set; }
        public int Fails { get; set; }
        public string[] Following { get; set; }
        public string Image { get; set; }
    }

    public class Map
    {
        public string Id { get; set; }
        public string Hash { get; set; }
        public BeatmapDifficulty Difficulty { get; set; }
        public string SongName { get; set; }
        public string SongSubName { get; set; }
        public string SongAuthorName { get; set; }
        public string LevelAuthorName { get; set; }
        public float DifficultyRating { get; set; }
        public float Length { get; set; }
        public float Bpm { get; set; }
        public float NoteJumpSpeed { get; set; }
        public int NoteCount { get; set; }
        public float Complexity { get; set; }
        public float SaberDistance { get; set; }
        public float MaxRp { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
    }

    public class Score
    {

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
        public string ImageB64 { get; set; }
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
