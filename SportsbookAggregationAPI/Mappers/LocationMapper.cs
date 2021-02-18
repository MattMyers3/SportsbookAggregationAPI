namespace SportsbookAggregation.SportsBooks
{
    public class LocationMapper
    {
        public static string GetFullTeamName(string shortTeamName, string sport)
        {
            if (sport == "NCAAF" || sport == "NCAAB")
                return MapCollege(shortTeamName);

            var teamNameArray = shortTeamName.Split(' ');
            var location = LocationMapper.MapLocation(teamNameArray[0], sport);
            return $"{location}{shortTeamName.Substring(teamNameArray[0].Length)}";
        }

        private static string MapCollege(string college)
        {
            college = college.Replace('-', '-');
            if (college.StartsWith('#'))//#1 Villanova, need to strip the rank
                college = college.Substring(college.IndexOf(' ') + 1);
            switch (college.ToLower())
            {
                case "abilene christian university":
                    return "Abilene Christian";
                case "american university":
                case "american uni eagles":
                    return "American";
                case "albany ny great danes":
                    return "Albany";
                case "arkansas pine bluff":
                case "arkansas pine bluff golden lions":
                    return "Arkansas-Pine Bluff";
                case "austin peay state":
                    return "Austin Peay";
                case "brigham young cougars":
                case "brigham young":
                    return "BYU";
                case "buffalo u":
                    return "Buffalo";
                case "cal baptist":
                    return "California Baptist";
                case "cal poly slo":
                    return "Cal Poly";
                case "central conn. state":
                case "central connecticut state":
                case "central connecticut state blue devils":
                    return "Central Connecticut";
                case "csu bakersfield":
                case "california state bakersfield roadrunners":
                case "cs bakersfield":
                    return "Cal State Bakersfield";
                case "csu fullerton":
                case "csu fullerton titans":
                case "cs fullerton":
                    return "Cal State Fullerton";
                case "csu northridge":
                case "cs northridge":
                    return "Cal State Northridge";
                case "central florida knights":
                case "central florida":
                    return "UCF";
                case "college of charleston":
                    return "Charleston";
                case "charleston sou":
                    return "Charleston Southern";
                case "charlotte u":
                    return "Charlotte";
                case "tennessee chattanooga mocs":
                    return "Chattanooga";
                case "cincinnati u":
                    return "Cincinnati";
                case "citadel":
                    return "The Citadel";
                case "delaware blue hens":
                    return "Delaware";
                case "depaul":
                    return "DePaul";
                case "detroit":
                    return "Detroit Mercy";
                case "east tenn state":
                case "etsu":
                    return "East Tennessee State";
                case "florida intl":
                case "florida international":
                case "florida international panthers":
                case "florida intl panthers":
                    return "FIU";
                case "gardner webb":
                case "gardner webb runnin bulldogs":
                    return "Gardner-Webb";
                case "gonzaga bulldogs":
                    return "Gonzaga";
                case "grambling st":
                case "grambling":
                    return "Grambling State";
                case "uw green bay":
                case "wisconsin green bay phoenix":
                case "wisc green bay":
                    return "Green Bay";
                case "hawaii warriors":
                    return "Hawaii";
                case "houston u":
                    return "Houston";
                case "indiana u.":
                    return "Indiana";
                case "lafayette college":
                    return "Lafayette";
                case "arkansas little rock":
                case "arkansas little rock trojans":
                    return "Little Rock";
                case "liu":
                case "liu sharks":
                case "long island sharks":
                    return "LIU[d]";
                case "long beach state 49ers":
                    return "Long Beach State";
                case "la lafayette":
                case "la-lafayette":
                case "louisiana-lafayette":
                case "louisiana lafayette ragin cajuns":
                case "ul lafayette ragin cajuns":
                case "ul lafayette":
                case "ul - lafayette":
                case "louisiana lafayette":
                    return "Louisiana";
                case "louisiana monroe warhawks":
                case "ul monroe":
                case "ul - monroe":
                case "louisiana monroe":
                case "louisiana - monroe":
                    return "Louisiana-Monroe";
                case "loyola (md)":
                case "loyola md":
                    return "Loyola Maryland";
                case "lmu":
                    return "Loyola Marymount";
                case "miami florida":
                case "miami florida hurricanes":
                case "miami hurricanes":
                    return "Miami (FL)";
                case "miami ohio":
                case "miami-ohio redhawks":
                    return "Miami (OH)";
                case "miami ohio red hawks":
                    return "Miami (OH)";
                case "middle tennessee state":
                case "middle tenn state blue raiders":
                case "middle tenn st":
                    return "Middle Tennessee";
                case "uw milwaukee":
                case "wisconsin milwaukee":
                case "wisc milwaukee":
                case "wisconsin milwaukee panthers":
                    return "Milwaukee";
                case "miss valley state":
                    return "Mississippi Valley State";
                case "minnesota u":
                    return "Minnesota";
                case "mississippi rebels":
                case "mississippi":
                    return "Ole Miss";
                case "mount st. marys":
                case "mt st mary's":
                case "mt. st. mary's":
                case "mount st. marys mountaineers":
                    return "Mount Saint Mary's";
                case "new jersey tech highlanders":
                    return "NJIT";
                case "nicholls state":
                    return "Nicholls";
                case "nicholls state colonels":
                    return "Nicholls";
                case "uni":
                    return "Northern Iowa";
                case "north alabama":
                case "north alabama lions":
                    return "UNA";
                case "n carolina central":
                case "nc central":
                    return "North Carolina Central";
                case "north carolina st wolfpack":
                case "north carolina state":
                case "north carolina state wolfpack":
                    return "NC State";
                case "no. colorado":
                    return "Northern Colorado";
                case "northwestern state uni demons":
                case "northwestern st":
                    return "Northwestern State";
                case "ohio st.":
                    return "Ohio State";
                case "nebraska-omaha":
                case "nebraska omaha":
                case "nebraska omaha mavericks":
                    return "Omaha";
                case "ipfw":
                    return "Purdue Fort Wayne";
                case "sam houston st":
                    return "Sam Houston State";
                case "saint marys":
                case "saint marys ca":
                case "saint mary´s ca":
                case "saint marys gaels":
                    return "Saint Mary's";
                case "san josé state":
                    return "San Jose State";
                case "siu-edwardsville cougars":
                case "siue":
                    return "SIU Edwardsville";
                case "se louisiana":
                    return "Southeastern Louisiana";
                case "se missouri state":
                case "se missouri state redhawks":
                    return "Southeast Missouri State";
                case "southern university":
                case "southern university jaguars":
                    return "Southern";
                case "southern methodist mustangs":
                case "southern methodist":
                    return "SMU";
                case "so illinois":
                    return "Southern Illinois";
                case "southern mississippi":
                case "southern mississippi golden eagles":
                    return "Southern Miss";
                case "stephen f austin":
                case "sfa":
                    return "Stephen F. Austin";
                case "seattle university":
                case "seattle":
                case "seattle redhawks":
                case "seattle u":
                    return "SU";
                case "st. francis (bkn)":
                case "st francis brooklyn":
                case "st. francis ny":
                case "st. francis (ny)":
                case "saint francis brooklyn":
                    return "St. Francis Brooklyn";
                case "st. francis (pa)":
                case "st francis (pa)":
                case "st. francis pa red flash":
                    return "Saint Francis";
                case "usc upstate":
                case "sc upstate spartans":
                case "usc-upstate":
                    return "South Carolina Upstate";
                case "st bonaventure":
                case "saint bonaventure bonnies":
                    return "St. Bonaventure";
                case "st. peter's":
                case "saint peters peacocks":
                    return "Saint Peter's";
                case "st johns":
                case "st johns red storm":
                case "st. johns":
                    return "St. John's";
                case "st. joseph's":
                case "st josephs":
                case "st. josephs":
                    return "Saint Joseph's";
                case "tarleton st":
                    return "Tarleton State";
                case "texas arlington":
                case "ut arlington":
                case "texas-arlington mavericks":
                case "texas-arlington":
                    return "Texas–Arlington";
                case "t a&m corpus christi":
                case "texas a&m-corpus christi": //took me forever to figure out that's a different dash
                case "texas a&m-cc":
                case "texas a&m cc islanders":
                    return "Texas A&M–Corpus Christi";
                case "connecticut":
                case "connecticut huskies":
                    return "UConn";
                case "california irvine":
                case "california irvine anteaters":
                case "cal irvine":
                    return "UC Irvine";
                case "california riverside highlanders":
                case "cal riverside":
                    return "UC Riverside";
                case "san diego tritons":
                    return "UC San Diego";
                case "ucsb":
                case "cal santa barbara":
                case "cal santa barbara gauchos":
                    return "UC Santa Barbara";
                case "illinois chicago":
                case "illinois chicago flames":
                    return "UIC";
                case "massachusetts":
                case "massachusetts minutemen":
                    return "UMass";
                case "md baltimore county retrievers":
                case "md baltimore co":
                case "md baltimore":
                    return "UMBC";
                case "missouri kansas city kangaroos":
                    return "UMKC";
                case "north carolina asheville":
                case "unc-asheville bulldogs":
                    return "UNC Asheville";
                case "north carolina greensboro":
                case "nc greensboro":
                    return "UNC Greensboro";
                case "uncw":
                case "nc wilmington":
                    return "UNC Wilmington";
                case "unlv runnin rebels":
                    return "UNLV";
                case "southern california trojans":
                    return "USC";
                case "tennessee martin":
                case "tennessee-martin":
                case "tennessee martin skyhawks":
                case "tenn martin":
                    return "UT Martin";
                case "utah valley state":
                    return "Utah Valley";
                case "texas el paso miners":
                case "texas el paso":
                    return "UTEP";
                case "ut rio grande valley":
                case "texas rio grande":
                case "ut-rio grande valley":
                case "texas-rio grande valley vaqueros":
                    return "UTRGV";
                case "ut san antonio":
                case "texas san antonio roadrunners":
                case "texas san antonio":
                    return "UTSA";
                case "virginia commonwealth rams":
                case "va commonwealth":
                    return "VCU";
                case "virginia military keydets":
                    return "VMI";
                case "washington u":
                    return "Washington";
                case "william and mary":
                    return "William & Mary";
                default:
                    return college;
            }
        }
        private static string MapLocation(string abbreviation, string sport)
        {
            switch (abbreviation.ToUpper())
            {
                case "ANA":
                case "ANH":
                    return "Anaheim";
                case "ARI":
                    return "Arizona";
                case "ATL":
                    return "Atlanta";
                case "BAL":
                    return "Baltimore";
                case "BOS":
                    return "Boston";
                case "BKN":
                    return "Brooklyn";
                case "BUF":
                    return "Buffalo";
                case "CAR":
                    return "Carolina";
                case "CGY":
                    return "Calgary";
                case "CHA":
                    return "Charlotte";
                case "CHI":
                    return "Chicago";
                case "CIN":
                    return "Cincinnati";
                case "CLE":
                    return "Cleveland";
                case "CBJ":
                case "CLS":
                    return "Columbus";
                case "DAL":
                    return "Dallas";
                case "DEN":
                    return "Denver";
                case "DET":
                    return "Detroit";
                case "EDM":
                    return "Edmonton";
                case "FLA":
                    return "Florida";
                case "GB":
                    return "Green Bay";
                case "GS":
                    return "Golden State";
                case "HOU":
                    return "Houston";
                case "IND":
                    return sport == "NBA" ? "Indiana" : "Indianapolis";
                case "JAX":
                    return "Jacksonville";
                case "KC":
                    return "Kansas City";
                case "LA":
                    return "Los Angeles";
                case "LV":
                    return "Las Vegas";
                case "MEM":
                    return "Memphis";
                case "MIA":
                    return "Miami";
                case "MIL":
                    return "Milwaukee";
                case "MIN":
                    return "Minnesota";
                case "MTL":
                case "MON":
                    return "Montreal";
                case "NE":
                    return "New England";
                case "NJ":
                    return "New Jersey";
                case "NO":
                    return "New Orleans";
                case "NY":
                    return "New York";
                case "NSH":
                    return "Nashville";
                case "OKC":
                    return "Oklahoma City";
                case "ORL":
                    return "Orlando";
                case "OTT":
                case "Ottowa":
                    return "Ottawa";
                case "PHI":
                    return "Philadelphia";
                case "PIT":
                    return "Pittsburgh";
                case "PHO":
                    return "Phoenix";
                case "PHX":
                    return "Phoenix";
                case "POR":
                    return "Portland";
                case "SAC":
                    return "Sacramento";
                case "SA":
                    return "San Antonio";
                case "SEA":
                    return "Seattle";
                case "SF":
                    return "San Francisco";
                case "SJ":
                    return "San Jose";
                case "TB":
                    return "Tampa Bay";
                case "TEN":
                    return "Tennessee";
                case "TOR":
                    return "Toronto";
                case "UTA":
                    return "Utah";
                case "VAN":
                    return "Vancouver";
                case "VGS":
                    return "Vegas";
                case "WAS":
                case "WSH":
                    return "Washington";
                case "WPG":
                    return "Winnipeg";
                case "SD":
                    return "San Diego";
                case "OAK":
                    return "Oakland";
                case "TEX":
                    return "Texas";
                case "STL":
                    return "St. Louis";
                case "COL":
                    return "Colorado";
                default:
                    return abbreviation;
            }
        }
    }
}