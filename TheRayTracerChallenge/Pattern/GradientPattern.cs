using System;
using Tuple = TheRayTracerChallenge.Math.Tuple;

namespace TheRayTracerChallenge.Pattern
{
    public class GradientPattern : Pattern
    {
        public Tuple A { get; set; }
        public Tuple B { get; set; }

        public GradientPattern(Tuple a, Tuple b)
        {
            A = a;
            B = b;
        }

        public override Tuple PatternAt(Tuple localPoint) => A + (B - A) * (localPoint.X - MathF.Floor(localPoint.X));
    }
}