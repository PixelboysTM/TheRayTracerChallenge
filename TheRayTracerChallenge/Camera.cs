using System;
using System.Collections.Generic;
using System.Drawing;
using TheRayTracerChallenge.Math;
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
    }
}