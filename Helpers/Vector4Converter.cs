using System.Globalization;
using System.Numerics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Eyedrop.Helpers;

internal class Vector4Converter : ValueConverter<Vector4, string>
{
    public static string VecToString(Vector4 vec)
    {
        var culture = CultureInfo.GetCultureInfo("en-US");
        return $"{vec.X.ToString(culture)} {vec.Y.ToString(culture)} {vec.Z.ToString(culture)} {vec.W.ToString(culture)}";
    }
    
    public static Vector4 StringToVec(string vec)
    {
        var culture = CultureInfo.GetCultureInfo("en-US");
        string[] split = vec.Split(" ");
        float[] floats = split.Select(x => Convert.ToSingle(x, culture)).ToArray();
        return new Vector4(floats[0], floats[1], floats[2], floats[3]);
    }
    
    public Vector4Converter()
        : base(
            v => VecToString(v),
            v => StringToVec(v))
    {
    }
}