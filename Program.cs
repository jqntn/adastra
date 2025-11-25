using System.Drawing;
using System.Drawing.Imaging;
using Raylib_cs;
using Svg;
using Color = Raylib_cs.Color;
using Image = Raylib_cs.Image;

namespace AdAstra;

internal static class Program
{
    private static void Main()
    {
        Raylib.SetConfigFlags(ConfigFlags.VSyncHint | ConfigFlags.Msaa4xHint | ConfigFlags.UndecoratedWindow);
        Raylib.InitWindow(0, 0, "AdAstra");
        Raylib.SetWindowSize(Raylib.GetScreenWidth() + 1, Raylib.GetScreenHeight() + 1);
        Raylib.SetExitKey(KeyboardKey.Null);

        /*-*/

        SvgDocument svgDoc = SvgDocument.Open("C:\\Users\\jqntn\\adastra\\Assets\\circle.svg");
        Bitmap bmp = svgDoc.Draw();

        byte[] pngBytes;
        using (MemoryStream ms = new())
        {
            bmp.Save(ms, ImageFormat.Png);
            pngBytes = ms.ToArray();
        }

        Image image = Raylib.LoadImageFromMemory(".png", pngBytes);
        Texture2D texture = Raylib.LoadTextureFromImage(image);
        Raylib.UnloadImage(image);

        /*-*/

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
            Raylib.DrawTexture(texture, 0, 0, Color.White);
            Raylib.EndDrawing();
        }

        Raylib.UnloadTexture(texture);
        Raylib.CloseWindow();
    }
}