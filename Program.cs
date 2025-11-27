using System.Drawing;
using System.Drawing.Imaging;
using netDxf;
using netDxf.Entities;
using Raylib_cs;
using Svg;
using Color = Raylib_cs.Color;
using Image = Raylib_cs.Image;
using Vector2 = System.Numerics.Vector2;

namespace AdAstra;

internal static class Program
{
    private static void Main()
    {
        Raylib.SetConfigFlags(ConfigFlags.VSyncHint | ConfigFlags.Msaa4xHint | ConfigFlags.UndecoratedWindow);
        Raylib.InitWindow(0, 0, "AdAstra");
        Raylib.SetWindowSize(Raylib.GetScreenWidth() + 1, Raylib.GetScreenHeight() + 1);
        Raylib.SetExitKey(KeyboardKey.Null);

        /**/

        SvgDocument svgDoc = SvgDocument.Open("C:\\Users\\jqntn\\adastra\\Assets\\circle.svg");
        Bitmap bmp = svgDoc.Draw();

        byte[] pngBytes;
        using (MemoryStream ms = new())
        {
            bmp.Save(ms, ImageFormat.Png);
            pngBytes = ms.ToArray();
        }

        /**/

        Image image = Raylib.LoadImageFromMemory(".png", pngBytes);
        Texture2D texture = Raylib.LoadTextureFromImage(image);
        Raylib.UnloadImage(image);

        /**/

        Camera2D cam2D = new(
            offset: new Vector2(Raylib.GetScreenWidth() * 0.5f, Raylib.GetScreenHeight() * 0.5f),
            target: Vector2.Zero,
            rotation: 0.0f,
            zoom: 100.0f);

        /**/

        DxfDocument dxfDoc = DxfDocument.Load("C:\\Users\\jqntn\\adastra\\Assets\\square-meter-a.dxf");

        List<DxfPoly> dxfPolys = [];
        foreach (Polyline2D poly2D in dxfDoc.Entities.Polylines2D)
        {
            if (poly2D.IsClosed)
            {
                dxfPolys.Add(new DxfPoly(poly2D));
            }
        }

        /**/

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Raylib.BeginMode2D(cam2D);

            //Raylib.DrawTextureV(texture, new Vector2(-texture.Width * 0.5f, -texture.Height * 0.5f), Color.White);
            //Raylib.DrawCircleV(Vector2.Zero, 45.0f, Color.Red);
            //Raylib.DrawPoly(Vector2.Zero, 64, 45.0f, 0.0f, Color.Red);

            foreach (DxfPoly poly in dxfPolys)
            {
                Raylib.DrawPolyLinesEx(poly.Center, poly.Sides, poly.Radius, poly.Rotation, poly.Thickness, Color.Green);
            }

            Raylib.EndMode2D();

            Raylib.EndDrawing();
        }

        Raylib.UnloadTexture(texture);
        Raylib.CloseWindow();
    }

    private record DxfPoly
    {
        public List<Vector2> Vertices { get; init; }
        public Vector2 Center { get; init; }
        public int Sides { get; init; }
        public float Radius { get; init; }
        public float Rotation { get; init; }
        public float Thickness { get; init; }

        public DxfPoly(Polyline2D poly2D)
        {
            Vertices = [.. poly2D.Vertexes.Select(v => new Vector2((float)v.Position.X, (float)-v.Position.Y))];
            Center = new Vector2(Vertices.Average(v => v.X), Vertices.Average(v => v.Y));
            Sides = poly2D.Vertexes.Count;
            Radius = Vertices.Max(v => Vector2.Distance(Center, v));
            Rotation = 180.0f / Sides;
            Thickness = (float)poly2D.Vertexes.Average(v => (v.StartWidth + v.EndWidth) / 2.0);
        }
    }
}