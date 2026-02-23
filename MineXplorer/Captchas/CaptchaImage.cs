using Eyedrop.Helpers;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Eyedrop.MineXplorer.Captchas;

public static class CaptchaImage
{
    private static readonly FontFamily Regular;
    private static readonly FontFamily Bold;
    
    static CaptchaImage()
    {   
        FontCollection collection = new();
        Bold = collection.Add("Assets/Fonts/AtkinsonHyperlegible-Bold.ttf");
        Regular = collection.Add("Assets/Fonts/AtkinsonHyperlegible-Regular.ttf");
    }

    private static Font CreateFont(FontFamily family, float size, FontStyle style) => family.CreateFont(size, style);

    public static async Task<byte[]> CreatePNG(string captcha)
    {
        if (captcha.Length != 8)
        {
            throw new InvalidOperationException("MineXplorer captchas can only be 8 characters in length!");
        }
        // Create image
        using Image<Rgba32> image = new(200, 100);
        
        // Background
        {
            var flip = RandomHelper.Bool();
            var point1 = new PointF(RandomHelper.Single(-300, 0), RandomHelper.Single(-150, 0));
            var point2 = new PointF(RandomHelper.Single(200, 400), RandomHelper.Single(100, 200));
            
            var backgroundGradient = new LinearGradientBrush(
                point1,
                point2,
                GradientRepetitionMode.None,
                new ColorStop(flip ? 1 : 0, Rgba32.ParseHex("#612caf")),
                new ColorStop(flip ? 0 : 1, Rgba32.ParseHex("#3d1c6e"))
            );
            
            image.Mutate(x => x.Fill(backgroundGradient));
        }
        
        // Random circles
        {
            for (int i = 0; i < 10; i++)
            {
                var polygon = new EllipsePolygon(new PointF(RandomHelper.Single(0, 200), RandomHelper.Single(0, 100)), RandomHelper.Single(4, 16));
                image.Mutate(x => x.Fill(new Rgba32(1, 1, 1, RandomHelper.Single(0.05f, 0.35f)), polygon));
            }
        }
        
        // Random characters
        {
            var font = CreateFont(Bold, 20f, FontStyle.Bold);
            var characters = RandomHelper.String(64);
            for (int i = 0; i < 64; i++)
            {
                var point = new PointF(RandomHelper.Single(0, 200), RandomHelper.Single(0, 100));
                image.Mutate(x => x.DrawText(characters[i].ToString(), font, new Rgba32(1, 1, 1, 0.05f), point));
            }
        }
        
        // Random lines
        {
            for (int i = 0; i < 5; i++)
            {
                var point1 = new PointF(RandomHelper.Single(-40, 240), RandomHelper.Single(-40, 0));
                var point2 = new PointF(RandomHelper.Single(-40, 240), RandomHelper.Single(100, 140));
            
                image.Mutate(x => x.DrawLine(new DrawingOptions(), Color.White, 0.5f, point1, point2));
            }
        }
        
        // Captcha solution
        {
            var shuffled = RandomHelper.Shuffle(captcha);
            var indexFont = CreateFont(Regular, 10f, FontStyle.Regular);
            
            for (int i = 0; i < 8; i++)
            {
                var fontSize = RandomHelper.Single(18f, 22f);
                var font = CreateFont(Bold, fontSize, FontStyle.Bold);
                
                var px = (200f / 4f) * (i % 4) + (fontSize - 5) + RandomHelper.Single(-5, 5);
                var py = (100f / 2f) * MathF.Floor(i / 4) + fontSize + RandomHelper.Single(-5, 5);
                
                var restColor = RandomHelper.Single(130f, 255f) / 255f;
                var alpha = RandomHelper.Single(0.55f, 0.8f);
                image.Mutate(x => x.DrawText(shuffled[i].ch.ToString(), font, new Rgba32(restColor, restColor, restColor, alpha), new PointF(px, py)));
                
                image.Mutate(x => x.DrawText((shuffled[i].idx + 1).ToString(), indexFont, new Rgba32(1, 1, 1, 0.35f), new PointF(px + (fontSize - 5), py - (fontSize - 5))));
            }
        }
        
        // Get bytes
        using var ms = new MemoryStream();
        await image.SaveAsPngAsync(ms);
        return ms.ToArray();
    }
}