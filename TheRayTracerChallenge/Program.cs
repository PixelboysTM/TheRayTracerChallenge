using System;
using System.Text;
using Spectre.Console;
using TheRayTracerChallenge.Lights;
using TheRayTracerChallenge.Math;
using TheRayTracerChallenge.Shapes;
using Tuple = TheRayTracerChallenge.Math.Tuple;

namespace TheRayTracerChallenge
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            AnsiConsole.Clear();

            var floor = new Sphere
            {
                Transform = Matrix4x4.Scaling(10,0.01f, 10),
                Material = new Material
                {
                    Color = Tuple.Color(1,0.9f,0.9f),
                    Specular = 0
                }
            };

            var leftWall = new Sphere
            {
                Transform = Matrix4x4.Translation(0,0,5) * Matrix4x4.RotationY(-Tuple.PI/4.0f) * Matrix4x4.RotationX(Tuple.PI/2.0f) * Matrix4x4.Scaling(10,0.01f, 10),
                Material = floor.Material
            };

            var rightWall = new Sphere
            {
                Transform = Matrix4x4.Translation(0,0,5) * Matrix4x4.RotationY(Tuple.PI/4.0f) * Matrix4x4.RotationX(Tuple.PI/2.0f) * Matrix4x4.Scaling(10,0.01f, 10),
                Material = floor.Material
            };

            var middle = new Sphere
            {
                Transform = Matrix4x4.Translation(-0.5f, 1, 0.5f) * Matrix4x4.Scaling(1,0.5f,0.01f),
                Material = new Material
                {
                    Color = Tuple.Color(0.1f, 1, 0.5f),
                    Diffuse = 0.7f,
                    Specular = 0.3f
                }
            };

            var right = new Sphere
            {
                Transform = Matrix4x4.Translation(1.5f, 0.5f, -0.5f) * Matrix4x4.Scaling(0.5f, 0.5f, 0.5f),
                Material = new Material
                {
                    Color = Tuple.Color(0.5f, 1, 0.1f),
                    Diffuse = 0.7f,
                    Specular = 0.3f
                }
            };

            var left = new Sphere
            {
                Transform = Matrix4x4.Translation(-1.5f, 0.33f, -0.75f) * Matrix4x4.Scaling(0.33f, 0.33f, 0.33f),
                Material = new Material
                {
                    Color = Tuple.Color(1f, 0.8f, 0.1f),
                    Diffuse = 0.7f,
                    Specular = 0.3f
                }
            };

            var w = new World(
                new PointLight(Tuple.Point(-10,10,-10), Tuple.Color(1,1,1)),
                floor, leftWall, rightWall, left, middle, right);
            var c = new Camera(192*10, 108*10, Tuple.PI / 3)
            {
                Transform = Matrix4x4.ViewTransformation(Tuple.Point(0,1.5f, -5), Tuple.Point(0,1,0), Tuple.Vector(0,1,0))
            };


           
                DateTime start = DateTime.Now;
                
                var s = c.RenderCo(w);
                AnsiConsole.Progress()
                    .HideCompleted(true)
                    .Columns(
                    new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(),
                    new ElapsedTimeColumn(), new RemainingTimeColumn(), new SpinnerColumn(Spinner.Known.Dots)).Start(
                    ctx =>
                    {
                        var task = ctx.AddTask("Rendering Progress");
                        while (s.MoveNext())
                        {
                            task.Increment(s.Current.Item1);
                        }
                    });
                
                
                var image = s.Current.Item2;
                DateTime end = DateTime.Now;
                var span = end.Subtract(start);
                AnsiConsole.WriteLine("Finished Rendering in: " + new DateTime(2004,1,1,span.Hours, span.Minutes, span.Seconds) .ToString("HH:mm:ss"));
                
                AnsiConsole.Status().Spinner(Spinner.Known.Clock).Start("Rendering...", ctx =>
                {
                ctx.Status = "Saving";
                ctx.Spinner = Spinner.Known.Pong;
                image.SaveToFile("img/Chapter8.png");
            });
            AnsiConsole.WriteLine("Finished ✔");
        }
    }
}