using TheRayTracerChallenge.Math;

namespace TheRayTracerChallenge.Lights
{
    public class PointLight
    {
        public Tuple Position { get; set; }
        public Tuple Intensity { get; set; }

        public PointLight(Tuple position, Tuple intensity)
        {
            Position = position;
            Intensity = intensity;
        }

        protected bool Equals(PointLight other)
        {
            return Position.Equals(other.Position) && Intensity.Equals(other.Intensity);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PointLight)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Position.GetHashCode() * 397) ^ Intensity.GetHashCode();
            }
        }

        public static bool operator ==(PointLight left, PointLight right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PointLight left, PointLight right)
        {
            return !Equals(left, right);
        }
    }
}