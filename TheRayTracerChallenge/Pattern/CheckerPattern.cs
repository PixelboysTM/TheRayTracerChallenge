using System;
using Spectre.Console;
using Tuple = TheRayTracerChallenge.Math.Tuple;

namespace TheRayTracerChallenge.Pattern
{
    public class CheckerPattern : Pattern
    {
        public Tuple A { get; set; }
        public Tuple B { get; set; }

        public CheckerPattern(Tuple a, Tuple b)
        {
            A = a;
            B = b;
        }

        public override Tuple PatternAt(Tuple localPoint) =>
            (MathF.Floor(localPoint.X) + MathF.Floor(localPoint.Y) + MathF.Floor(localPoint.Z)) % 2 == 0
                ? A.Copy
                : B.Copy;
    }
}