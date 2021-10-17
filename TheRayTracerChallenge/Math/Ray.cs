namespace TheRayTracerChallenge.Math
{
    public struct Ray
    {
        public Tuple Origin { get; set; }
        public Tuple Direction { get; set; }

        public Ray(Tuple origin, Tuple direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public Tuple Position(float t)
        {
            return Origin + Direction.Normalised() * t;
        }

        public Ray Transform(Matrix4x4 transformMatrix)
        {
            return new Ray(transformMatrix * Origin, transformMatrix * Direction.Normalised());
        }
    }
}