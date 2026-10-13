//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace VolleyStats.Models
//{
//    public class Match
//    {
//        public string HomeTeam { get; set; } = "";
//        public string AwayTeam { get; set; } = "";
//        public DateTime Date { get; set; } = DateTime.Today;

//        public string Location { get; set; } = "";

//        public string Status { get; set; } = "Tervezett";

//        public int HomeScore { get; set; }

//        public int AwayScore { get; set; }
//    }
//}
using System;
using System.Collections.Generic;

namespace VolleyStats.Models
{
    // 1. Egy adott szett adatai
    public class SetDetail
    {
        public int SetNumber { get; set; } // 1-5
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }

        // Időkérések (mikor kérték ki - pl. 15:14-nél) - Max 2/csapat
        public List<string> HomeTimeouts { get; set; } = new();
        public List<string> AwayTimeouts { get; set; } = new();

        // Cserék (ki jött be, ki ment le - pl. "12 in, 4 out")
        public List<string> HomeSubstitutions { get; set; } = new();
        public List<string> AwaySubstitutions { get; set; } = new();
    }

    // 2. A kibővített Mérkőzés modell
    public class Match
    {
        public string HomeTeam { get; set; } = "";
        public string AwayTeam { get; set; } = "";
        public DateTime Date { get; set; } = DateTime.Today;
        public string Location { get; set; } = "";
        public string Status { get; set; } = "Tervezett";

        // VÉGEREDMÉNY (Szettek aránya, pl. 3-1)
        public int HomeSetsWon { get; set; }
        public int AwaySetsWon { get; set; }

        // KIK JÁTSZOTTAK (A meccsre nevezett keret)
        public List<Player> HomeRoster { get; set; } = new();
        public List<Player> AwayRoster { get; set; } = new();

        // SZETTEK PONTJAI ÉS RÉSZLETEI
        public List<SetDetail> Sets { get; set; } = new();

        // Segédtulajdonság a felületnek, hogy egy stringként kiírhassuk a szettek pontjait (pl. "25:20, 25:23, 15:25, 25:18")
        public string SetScoresDisplay => string.Join(", ", Sets.Select(s => $"{s.HomeScore}:{s.AwayScore}"));
    }
}
