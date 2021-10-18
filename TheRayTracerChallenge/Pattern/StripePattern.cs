using System;
using TheRayTracerChallenge.Math;
using TheRayTracerChallenge.Shapes;
using Tuple = TheRayTracerChallenge.Math.Tuple;

namespace TheRayTracerChallenge.Pattern
{
    public class StripePattern
    {
        public Tuple A { get; set; }
        public Tuple B { get; set; }
        public Matrix4x4 Transform { get; set; } = Matrix4x4.Identity;

        public StripePattern(Tuple a, Tuple b)
        {
            A = a;
            B = b;
        }


        public Tuple StripeAt(Tuple point) => MathF.Floor(point.X) % 2 == 0 ? A : B;

        public Tuple StripeAtObject(Shape shape, Tuple point)
        {
            var objectPoint = shape.Transform.Inverse * point;
            var patterPoint = Transform.Inverse * objectPoint;

            return StripeAt(patterPoint);
        }
    }
}