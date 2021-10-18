using System;
using TheRayTracerChallenge.Math;
using Tuple = TheRayTracerChallenge.Math.Tuple;

namespace TheRayTracerChallenge.Shapes
{
    public class Plane : Shape
    {
        protected override Intersection[] LocalIntersect(Ray ray)
        {
            if (System.Math.Abs(ray.Direction.Y) < Tuple.EPSILON)
            {
                return Array.Empty<Intersection>();
            }

            float t = -ray.Origin.Y / ray.Direction.Y;
            return new[] { new Intersection(t, this) };

        }

        protected override Tuple LocalNormalAt(Tuple point)
        {
            return Tuple.Vector(0, 1, 0);
        }

        public Plane(string name = "Plane") : base(name)
        {
        }
    }
}