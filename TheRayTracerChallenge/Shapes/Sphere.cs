using System;
using TheRayTracerChallenge.Math;
using Tuple = TheRayTracerChallenge.Math.Tuple;

namespace TheRayTracerChallenge.Shapes
{
    public class Sphere : Shape
    {
        public Sphere(string name = "Sphere") : base(name)
        {
        }

        public Sphere(Matrix4x4 transform) : base("Sphere")
        {
            Transform = transform;
        }

        public Sphere(Sphere sphere) : base(sphere.Name)
        {
            Transform = Matrix4x4.Identity * sphere.Transform;
            Material = sphere.Material.Copy;
        }
        
        

        protected override Intersection[] LocalIntersect(Ray ray)
        {
            var sphereToRay = ray.Origin - Tuple.Point(0, 0, 0);

            var a = Tuple.Dot(ray.Direction, ray.Direction);
            var b = 2 * Tuple.Dot(ray.Direction, sphereToRay);
            var c = Tuple.Dot(sphereToRay, sphereToRay) - 1;
            var discriminant = b * b - 4 * a * c;

            if (discriminant < 0)
                return Array.Empty<Intersection>();

            var t1 = (-b - MathF.Sqrt(discriminant)) / (2 * a);
            var t2 = (-b + MathF.Sqrt(discriminant)) / (2 * a);

            //WARNING: Not as in the book to remove acne
            // if (t1 < 0.01f && MathF.Abs(t2) >= 0.01f)
            //     return Intersection.Aggregate(new Intersection(t2, this));
            // if (t2 < 0.01f && MathF.Abs(t1) >= 0.01f)
            //     return Intersection.Aggregate(new Intersection(t1, this));
            
            return Intersection.Aggregate(new Intersection(t1, this), new Intersection(t2, this));

        }

        protected override Tuple LocalNormalAt(Tuple point)
        {
            return point - Tuple.Point(0, 0, 0);
        }

        private bool Equals(Sphere other)
        {
            return Equals(Transform, other.Transform) && Equals(Material, other.Material);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Sphere)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Transform != null ? Transform.GetHashCode() : 0) * 397) ^ (Material != null ? Material.GetHashCode() : 0);
            }
        }

        public static bool operator ==(Sphere left, Sphere right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Sphere left, Sphere right)
        {
            return !Equals(left, right);
        }
    }
}