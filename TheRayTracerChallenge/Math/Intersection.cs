using System.Collections.Generic;
using TheRayTracerChallenge.Math;
using TheRayTracerChallenge.Shapes;

namespace TheRayTracerChallenge
{
    public class Intersection
    {
        public float t { get; set; }
        public Sphere Object { get; set; }

        public Intersection(float t, Sphere obj)
        {
            this.t = t;
            Object = obj;
        }

        public static Intersection[] Aggregate(params Intersection[] intersections)
        {
            var list = new List<Intersection>(intersections);
            list.Sort(Extensions.Comparison);
            return list.ToArray();
        }


        public Computations PrepareComputations(Ray ray)
        {
            var comps = new Computations();

            comps.t = t;
            comps.Object = new Sphere(Object);

            comps.Point = ray.Position(comps.t);
            comps.EyeV = -ray.Direction;
            comps.NormalV = comps.Object.NormalAt(comps.Point);

            if (Tuple.Dot(comps.NormalV, comps.EyeV) < 0)
            {
                comps.Inside = true;
                comps.NormalV = -comps.NormalV;
            }
            else
                comps.Inside = false;

            comps.OverPoint = comps.Point + (comps.NormalV * Tuple.EPSILON);
            
            return comps;
        }
    }
}