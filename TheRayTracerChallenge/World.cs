using System;
using System.Collections.Generic;
using TheRayTracerChallenge.Lights;
using TheRayTracerChallenge.Math;
using TheRayTracerChallenge.Shapes;
using Tuple = TheRayTracerChallenge.Math.Tuple;

namespace TheRayTracerChallenge
{
    public class World : List<Shape>
    {
        public PointLight LightSource { get; set; } = null;

        public World() : base()
        {
            
        }
        public World(PointLight lightSource, IEnumerable<Shape> collection) : base(collection)
        {
            LightSource = lightSource;
        }
        
        public World(PointLight lightSource, params Shape[] collection) : base(collection)
        {
            LightSource = lightSource;
        }


        public static World Default => new World(new PointLight(Tuple.Point(-10, 10, -10), Tuple.Color(1, 1, 1)), 
            new Sphere()
            {
                Material = new Material() { Color = Tuple.Color(0.8f, 1.0f, 0.6f), Diffuse = 0.7f, Specular = 0.2f }
            }, 
            new Sphere()
        {
            Transform = Matrix4x4.Scaling(0.5f, 0.5f, 0.5f)
        });

        public new bool Contains(Shape item)
        {
            foreach (var sphere in this)
            {
                if (item == sphere)
                    return true;
            }

            return false;
        }

        public Intersection[] Intersect(Ray r)
        {
            var intersections = new List<Intersection>();
            foreach (var sphere in this)
            {
                intersections.AddRange(sphere.Intersect(r));
            }
            intersections.Sort(Extensions.Comparison);
            return intersections.ToArray();
        }


        public Tuple ShadeHit(Computations comps)
        {
            return comps.Object.Material.Lighting(LightSource, comps.OverPoint, comps.EyeV, comps.NormalV, IsShadowed(comps.OverPoint), comps.Object);
        }

        public Tuple ColorAt(Ray ray)
        {
            var xs = Intersect(ray);
            var hit = xs.Hit();
            if (hit == null)
                return Tuple.Color(0, 0, 0);
            var comps = hit.PrepareComputations(ray);
            return ShadeHit(comps);
        }

        public bool IsShadowed(Tuple point)
        {
            var v = LightSource.Position - point;
            
            var distance = v.Magnitude;
            var direction = v.Normalised();
            
            var r = new Ray(point, direction);
            var intersections = Intersect(r);
            
            var h = intersections.Hit();
            return h != null && h.t < distance;
        }
    }
}