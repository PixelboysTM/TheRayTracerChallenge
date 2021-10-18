using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;
using TheRayTracerChallenge.Math;
using Canvas = TheRayTracerChallenge.Util.Canvas;
using Tuple = TheRayTracerChallenge.Math.Tuple;

namespace TheRayTracerChallenge
{
    public class Camera
    {
        public int HSize { get; set; }
        public int VSize { get; set; }
        public float FieldOfView { get; set; }
        public Matrix4x4 Transform { get; set; }
        private float HalfWidth { get; set; }
        private float HalfHeight { get; set; }
        public float PixelSize {
            get
            {
                var halfView = MathF.Tan(FieldOfView / 2.0f);
                var aspect = HSize * 1.0f / VSize;
                if (aspect >= 1)
                {
                    HalfWidth = halfView;
                    HalfHeight = halfView / aspect;
                }
                else
                {
                    HalfWidth = halfView * aspect;
                    HalfHeight = halfView;
                }

                return (HalfWidth * 2) / HSize;
            }
        }

        public Camera(int hSize, int vSize, float fieldOfView)
        {
            HSize = hSize;
            VSize = vSize;
            FieldOfView = fieldOfView;
            Transform = Matrix4x4.Identity;
        }

        public Ray RayForPixel(int px, int py)
        {
            var xOffset = (px + 0.5f) * PixelSize;
            var yOffset = (py + 0.5f) * PixelSize;

            var worldX = HalfWidth - xOffset;
            var worldY = HalfHeight - yOffset;

            var pixel = Transform.Inverse * Tuple.Point(worldX, worldY, -1);
            var origin = Transform.Inverse * Tuple.Point(0, 0, 0);
            var direction = (pixel - origin).Normalised();
            return new Ray(origin, direction);
        }

        public Canvas Render(World w)
        {
            var image = new Canvas(HSize, VSize);

            for (int y = 0; y < VSize; y++)
            {
                for (int x = 0; x < HSize; x++)
                {
                    var ray = RayForPixel(x, y);
                    var color = w.ColorAt(ray);
                    image[x, y] = color;
                }
            }

            return image;
        }

        public IEnumerator<System.ValueTuple<float, Canvas>> RenderCo(World w)
        {
            float step = 100.0f / (HSize * VSize);
            
            var image = new Canvas(HSize, VSize);

            for (int y = 0; y < VSize; y++)
            {
                for (int x = 0; x < HSize; x++)
                {
                    var ray = RayForPixel(x, y);
                    var color = w.ColorAt(ray);
                    image[x, y] = color;
                    yield return (step, null);
                }
            }

            yield return (step, image);
        }

        public Canvas RenderAsync(World w, int taskAmount, int chunkSize)
        {
            Tuple[] BuildChunks(int sizeX, int sizeY, int chunkSize)
            {
                List<Tuple> parts = new List<Tuple>();
                int x = 0, y = 0;
                while (y < sizeY)
                {
                    while (x < sizeX)
                    {
                        parts.Add(new Tuple(x,y,System.Math.Min(x + chunkSize, sizeX), System.Math.Min(y + chunkSize, sizeY)));
                        x += chunkSize;
                    }

                    y += chunkSize;
                    x = 0;
                }

                return parts.ToArray();
            }

            ValueTuple<Canvas, Tuple> RenderChunk(Tuple chunk)
            {
                var image = new Canvas((int)chunk.Z, (int)chunk.W);
                int xi = 0;
                for (int x = (int)chunk.X; x < chunk.Z; x++)
                {
                    var yi = 0;
                    for (int y = (int)chunk.Y; y < chunk.W; y++)
                    {
                        var ray = RayForPixel(x, y);
                        var color = w.ColorAt(ray);
                        image[xi, yi] = color;
                        yi++;
                    }
                    xi++;
                }

                return (image, chunk);
            }

            var finalImage = new Canvas(HSize, VSize);
            AnsiConsole.Progress()
                .HideCompleted(false)
                .Columns(
                    new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(),
                    new ElapsedTimeColumn(), new RemainingTimeColumn(), new SpinnerColumn(Spinner.Known.Clock))
                .Start(ctx =>
                {
                    // Bars
                    var preprareTask = ctx.AddTask("Preparing...", true, 3);
                    var renderTask = ctx.AddTask("Rendering...", false, 10);
                    var overallTask = ctx.AddTask("Progress...", true, 3);
                    
                    //Prepare
                    var tokenSource = new CancellationTokenSource();
                    var token = tokenSource.Token;
                    preprareTask.Increment(1);
                    var chunks = BuildChunks(HSize, VSize, chunkSize).ToList();
                    var chunksCount = chunks.Count;
                    renderTask.MaxValue(chunks.Count);
                    var initialChunksCount = System.Math.Min(taskAmount, chunks.Count);
                    preprareTask.Increment(1);
                    Task<ValueTuple<Canvas, Tuple>>[] tasks = new Task<ValueTuple<Canvas, Tuple>>[initialChunksCount];
                    for (int i = 0; i < initialChunksCount; i++)
                    {
                        var chunk = chunks[0];
                        chunks.RemoveAt(0);
                        var task = new Task<ValueTuple<Canvas, Tuple>>(o => RenderChunk((Tuple)o),chunk, token);
                        tasks[i] = task;
                        task.Start();
                    }
                    preprareTask.Increment(1);
                    preprareTask.StopTask();
                    overallTask.Increment(1);
                    
                    renderTask.StartTask();
                    while (!tasks.All(task => task is null))
                    {
                        var id = tasks.WaitAny(token);
                        var finishedTask = tasks[id];
                        var result = finishedTask.Result;
                        for (int x = (int)result.Item2.X; x < result.Item2.Z; x++)
                        {
                            for (int y = (int)result.Item2.Y; y < result.Item2.W; y++)
                            {
                                finalImage[x, y] = result.Item1[x - (int)(result.Item2.X), y - (int)(result.Item2.Y)].Copy;
                            }
                        }

                        if (chunks.Count > 0)
                        {
                            var chunk = chunks[0];
                            chunks.RemoveAt(0);
                            tasks[id] = new Task<ValueTuple<Canvas, Tuple>>(o => RenderChunk((Tuple)o),chunk, token);
                            tasks[id].Start();
                        }
                        else
                        {
                            tasks[id] = null;
                        }
                        
                        renderTask.Increment(1);
                        overallTask.Increment(1.0f/chunksCount);
                    }
                    overallTask.Increment(1);
                    overallTask.Description("Finishing up");
                    Task.Delay(1000, token).Wait(token);
                    overallTask.Increment(1);
                    Task.Delay(1000, token).Wait(token);
                });
            
            return finalImage;
        }
    }
}