namespace Eyedrop.MineXplorer.Types;

internal static class MineXplorerInfo
{
    public const string DataPath = "mexp";
    public const int Version = 37;
    public const bool PlayingAllowed = true;
    
    public static readonly List<string> AllTokens = [
        "cave", "ballpit", "station", "station_disco", "employee",
        "vip", "theater", "museum", "museumexit", "underground_part1",
        "underground_part2", "corporation", "basement"
    ];
    
    public static readonly List<string> AllMaps = [
        "map_welcome", "map_welcome_beach", "map_house", "map_house_jukeroom",
        "map_hell", "map_void", "map_void_white", "map_parkour", "map_herobrine",
        "map_maze", "map_cave_pond", "map_ballpit", "map_ballpit_cave",
        "map_ballpit_alt", "map_vip", "map_corporation_part1", "map_corporation_part2",
        "map_corporation_part3", "map_corporation_part4", "map_corporation_part5",
        "map_corporation_part6", "map_corporation_part7", "map_corporation_part8",
        "map_corporation_part9", "map_corporation_part10", "map_corporation_part11",
        "map_corporation_part12", "map_corporation_part13", "map_corporation_part14",
        "map_corporation_part15", "map_corporation_part16", "map_corporation_part17",
        "map_corporation_part18", "map_corporation_part19", "map_corporation_part20",
        "map_underground_part1", "map_underground_part2", "map_theater",
        "map_theater_employee", "map_station_disco", "map_station_break",
        "map_station_check", "map_station_four", "map_station_gallery",
        "map_station_hallway", "map_station_low", "map_station_prison",
        "map_station_levers", "map_station_storage", "map_museum_part0",
        "map_museum_part1", "map_museum_part2", "map_museum_part3",
        "map_museum_part4", "map_museum_part5", "map_museum_part6",
        "map_museum_part7",
    ];
    
    public static readonly Dictionary<string, string> TokenMap = new()
    {
        {"cave", "map_welcome"},
        {"ballpit", "map_void"},
        {"station", "map_ballpit"},
        {"station_disco", "map_ballpit"},
        {"employee", "map_maze"},
        {"vip", "map_maze"},
        {"basement", "map_maze"},
        {"theater", "map_vip"},
        {"museum", "map_theater"},
        {"museumexit", "map_museum_part3"},
        {"underground_part1", "map_museum_part4"},
        {"underground_part2", "map_parkour"},
        {"corporation", "map_ballpit_alt"},
    };
    
    public static readonly Dictionary<string, string> TokenRequirements = new()
    {
        {"map_ballpit", "ballpit"},
        {"map_ballpit_cave", "cave"},
        {"map_cave", "cave"},
        {"map_vip", "vip"},
        
        {"map_corporation_part1", "corporation"},
        {"map_corporation_part2", "corporation"},
        {"map_corporation_part3", "corporation"},
        {"map_corporation_part4", "corporation"},
        {"map_corporation_part5", "corporation"},
        {"map_corporation_part6", "corporation"},
        {"map_corporation_part7", "corporation"},
        {"map_corporation_part8", "corporation"},
        {"map_corporation_part9", "corporation"},
        {"map_corporation_part10", "corporation"},
        {"map_corporation_part11", "corporation"},
        {"map_corporation_part12", "corporation"},
        {"map_corporation_part13", "corporation"},
        {"map_corporation_part14", "corporation"},
        {"map_corporation_part15", "corporation"},
        {"map_corporation_part16", "corporation"},
        {"map_corporation_part17", "corporation"},
        {"map_corporation_part18", "corporation"},
        
        {"map_corporation_part19", "basement"},
        {"map_corporation_part20", "basement"},
        
        {"map_underground_part1", "underground_part1"},
        {"map_underground_part2", "underground_part2"},
        
        {"map_theater", "theater"},
        {"map_theater_employee", "employee"},
        
        {"map_station_disco", "station"},
        {"map_station_break", "station"},
        {"map_station_check", "station"},
        {"map_station_four", "station"},
        {"map_station_gallery", "station"},
        {"map_station_hallway", "station"},
        {"map_station_low", "station"},
        {"map_station_prison", "station"},
        {"map_station_levers", "employee"},
        {"map_station_storage", "employee"},
        
        {"map_museum_part0", "youdontknowhowtoopendoors"},
        {"map_museum_part1", "museum"},
        {"map_museum_part2", "museum"},
        {"map_museum_part3", "museum"},
        {"map_museum_part4", "museum"},
        {"map_museum_part5", "museum"},
        {"map_museum_part6", "museum"},
        {"map_museum_part7", "museum"},
    };
}