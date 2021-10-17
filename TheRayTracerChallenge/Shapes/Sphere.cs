using System;
using TheRayTracerChallenge.Math;
using Tuple = TheRayTracerChallenge.Math.Tuple;

namespace TheRayTracerChallenge.Shapes
{
    public class Sphere
    {
        public Matrix4x4 Transform { get; set; } = Matrix4x4.Identity;
        public Material Material { get; set; } = new();
        
        public Sphere()
        {
        }

        public Sphere(Matrix4x4 transform)
        {
            Transform = transform;
        }

        public Sphere(Sphere sphere)
        {
            Transform = Matrix4x4.Identity * sphere.Transform;
            Material = sphere.Material.Copy;
        }

        public Intersection[] Intersect(Ray ray)
        {
            var ray2 = ray.Transform(Transform.Inverse);
            
            var sphereToRay = ray2.Origin - Tuple.Point(0, 0, 0);

            var a = Tuple.Dot(ray2.Direction, ray2.Direction);
            var b = 2 * Tuple.Dot(ray2.Direction, sphereToRay);
            var c = Tuple.Dot(sphereToRay, sphereToRay) - 1;
            var discriminant = b * b - 4 * a * c;

            if (discriminant < 0)
                return Array.Empty<Intersection>();

            var t1 = (-b - MathF.Sqrt(discriminant)) / (2 * a);
            var t2 = (-b + MathF.Sqrt(discriminant)) / (2 * a);

            return Intersection.Aggregate(new Intersection(t1, this), new Intersection(t2, this));
        }

        public Tuple NormalAt(Tuple point)
        {
            var objectPoint = Transform.Inverse * point;
            var objectNormal = objectPoint - Tuple.Point(0, 0, 0);
            var worldNormal = Transform.Inverse.Transpose * objectNormal;
            worldNormal.W = 0;
            return worldNormal.Normalised();
        }

        protected bool Equals(Sphere other)
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