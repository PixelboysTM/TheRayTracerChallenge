using TheRayTracerChallenge.Math;
using TheRayTracerChallenge.Shapes;

namespace TheRayTracerChallenge.Pattern
{
    public abstract class Pattern
    {
        public Matrix4x4 Transform { get; set; } = Matrix4x4.Identity;

        public Tuple PatternAtObject(Shape shape, Tuple worldPoint)
        {
            var objectPoint = shape.Transform.Inverse * worldPoint;
            var patternPoint = Transform.Inverse * objectPoint;
            return PatternAt(patternPoint);
        }

        public abstract Tuple PatternAt(Tuple localPoint);
    }
}