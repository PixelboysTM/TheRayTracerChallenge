using TheRayTracerChallenge.Math;
using TheRayTracerChallenge.Shapes;

namespace TheRayTracerChallenge
{
    public class Computations
    {
        public float t { get; set; }
        public Sphere Object { get; set; }
        public Tuple Point { get; set; }
        public Tuple EyeV { get; set; }
        public Tuple NormalV { get; set; }
        public bool Inside { get; set; }
        public Tuple OverPoint { get; set; }
    }
}