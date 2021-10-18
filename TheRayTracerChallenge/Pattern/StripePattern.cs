using System;
using TheRayTracerChallenge.Math;
using TheRayTracerChallenge.Shapes;
using Tuple = TheRayTracerChallenge.Math.Tuple;

namespace TheRayTracerChallenge.Pattern
{
    public class StripePattern : Pattern
    {
        public Tuple A { get; set; }
        public Tuple B { get; set; }

        public StripePattern(Tuple a, Tuple b)
        {
            A = a;
            B = b;
        }
        
        public override Tuple PatternAt(Tuple point) => MathF.Floor(point.X) % 2 == 0 ? A.Copy : B.Copy;
        
    }
}