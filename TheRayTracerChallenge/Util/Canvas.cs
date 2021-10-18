using System;
using System.Drawing;
using System.IO;
using Spectre.Console;
using Tuple = TheRayTracerChallenge.Math.Tuple;

namespace TheRayTracerChallenge.Util
{
    public class Canvas
    {
        private Tuple[,] _data;
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Canvas(int width, int height)
        {
            Width = width;
            Height = height;
            _data = new Tuple[width, height];
            _data.Initialize();
        }

        public void WritePixel(int x, int y, Tuple color)
        {
            _data[x, y] = color.Copy.Clamp;
        }

        public Tuple GetPixel(int x, int y)
        {
            return _data[x, y];
        }

        /// <summary>
        /// Whether the Canvas is filled with one single color
        /// </summary>
        /// <param name="color">The Color to test against</param>
        /// <returns>True or False</returns>
        public bool IsOfColor(Tuple color)
        {
            foreach (var tuple in _data)
            {
                if (tuple != color)
                    return false;
            }

            return true;
        }

        public void SaveToFile(string name)
        {
            if (name.Contains('/'))
                Directory.CreateDirectory(name.Substring(0, name.LastIndexOf('/')));
            Bitmap b = new Bitmap(Width, Height);
            for (var x = 0; x < _data.GetLength(0); x++)
            {
                for (int y = 0; y < _data.GetLength(1); y++)
                {
                    var c = _data[x, y];
                    b.SetPixel(x,y, System.Drawing.Color.FromArgb(255, (int)(c.Red*255), (int)(c.Green*255), (int)(c.Blue*255)));
                }
            }

            var path = Path.Combine(AppContext.BaseDirectory, name);
            b.Save(path);
        }

        public Tuple this[int x, int y]
        {
            get => GetPixel(x, y);
            set => WritePixel(x, y, value);
        }

        public void Show()
        {
            SaveToFile( Path.Combine(Path.GetTempPath(), "raytracerout.jpg"));
           var P =  System.Diagnostics.Process.Start("CMD.exe", Path.Combine(Path.GetTempPath(), "raytracerout.png"));
           P.Start();
        }

        public void PrintToConsole()
        {
            var p = Path.Combine(Path.GetTempPath(), "raytracerout.png");
            SaveToFile(p);
            CanvasImage image = new CanvasImage(p);
            AnsiConsole.Write(image);
        }
    }
}