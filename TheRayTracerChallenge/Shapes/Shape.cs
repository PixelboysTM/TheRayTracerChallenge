using TheRayTracerChallenge.Math;

namespace TheRayTracerChallenge.Shapes
{
    public abstract class Shape
    {
        public readonly string Name;
        public Matrix4x4 Transform { get; set; } = Matrix4x4.Identity;
        public Material Material { get; set; } = new();

        protected Shape(string name)
        {
            Name = name;
        }
        
        public Intersection[] Intersect(Ray ray)
        {
            var localRay = ToObjectSpace(ray);
            return LocalIntersect(localRay);
        }

        public Tuple NormalAt(Tuple point)
        {
            return ToWorldNormal(LocalNormalAt(ToObjectSpace(point)));
        }
        
        protected abstract Intersection[] LocalIntersect(Ray ray);
        protected abstract Tuple LocalNormalAt(Tuple point);

        private Ray ToObjectSpace(Ray ray) => ray.Transform(Transform.Inverse);
        private Tuple ToObjectSpace(Tuple point) => Transform.Inverse * point;
        private Tuple ToWorldNormal(Tuple objectNormal)
        {
            var m =Transform.Inverse.Transpose * objectNormal;
            m.W = 0;
            return m.Normalised();
        }

        public static bool operator ==(Shape s1, Shape s2)
        {
            if (s1.GetType() != s2.GetType())
                return false;
            return s1.Equals(s2);
        }

        public static bool operator !=(Shape s1, Shape s2)
        {
            return !(s1 == s2);
        }

        protected bool Equals(Shape other)
        {
            return Transform.Equals(other.Transform) && Material.Equals(other.Material);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Shape)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Transform.GetHashCode() * 397) ^ Material.GetHashCode();
            }
        }
    }
}