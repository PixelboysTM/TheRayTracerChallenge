using System;
using TheRayTracerChallenge.Math;
using Tuple = TheRayTracerChallenge.Math.Tuple;

namespace TheRayTracerChallenge.Shapes
{
    public class TestShape : Shape
    {
        protected override Intersection[] LocalIntersect(Ray ray)
        {
            return Array.Empty<Intersection>();
        }

        protected override Tuple LocalNormalAt(Tuple point)
        {
            return Tuple.Vector(0,0,0);
        }

        public TestShape() : base("Test Shape")
        {
        }
    }
}