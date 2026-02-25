using System.Numerics;
using Eyedrop.Helpers;

namespace Eyedrop.MineXplorer.Types;

public class SpawnData
{
    public static readonly Vector4 DefaultPosition = new(0.0f, 0.9f, 0.0f, 0.0f);
    
    public string Scene { get; set; } = "map_welcome";
    public Vector4 Position { get; set; } = DefaultPosition;
    
    public static SpawnData FromString(string str)
    {
        var split = str.Split(' ');
        if (split.Length != 5) throw new InvalidOperationException($"The SpawnData \"{str}\" doesn't follow MineXplorer's SpawnData format (map x y z r)!");
        
        var map = split[0];
        var positionStr = string.Join(' ', split.Skip(1));
        
        return new SpawnData
        {
            Scene = map,
            Position = Vector4Converter.StringToVec(positionStr)
        };
    }
    
    public override string ToString()
    {
        return $"{Scene} {Vector4Converter.VecToString(Position)}";
    }
}