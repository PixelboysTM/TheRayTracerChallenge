using System;
using Tuple = TheRayTracerChallenge.Math.Tuple;

namespace TheRayTracerChallenge.Pattern
{
    [Obsolete]
    public class RadialGradient : Pattern
    {
        public Tuple A { get; set; }
        public Tuple B { get; set; }

        public RadialGradient(Tuple a, Tuple b)
        {
            A = a;
            B = b;
        }
        public override Tuple PatternAt(Tuple localPoint) => A + (B - A) * (localPoint.Magnitude - MathF.Floor(localPoint.Magnitude));
    }
}