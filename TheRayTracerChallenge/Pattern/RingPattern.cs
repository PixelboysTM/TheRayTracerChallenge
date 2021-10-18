using System;
using Tuple = TheRayTracerChallenge.Math.Tuple;

namespace TheRayTracerChallenge.Pattern
{
    public class RingPattern : Pattern
    {
        
        public Tuple A { get; set; }
        public Tuple B { get; set; }

        public RingPattern(Tuple a, Tuple b)
        {
            A = a;
            B = b;
        }

        public override Tuple PatternAt(Tuple localPoint) =>
            MathF.Floor(MathF.Sqrt(localPoint.X * localPoint.X + localPoint.Z * localPoint.Z)) % 2 == 0 ? A.Copy : B.Copy;
    }
}