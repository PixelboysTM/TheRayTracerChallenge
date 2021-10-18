using System;
using TheRayTracerChallenge.Lights;
using TheRayTracerChallenge.Pattern;
using TheRayTracerChallenge.Shapes;
using Tuple = TheRayTracerChallenge.Math.Tuple;

namespace TheRayTracerChallenge
{
    public class Material
    {
        public Tuple Color { get; set; } = Tuple.Color(1,1,1);
        public float Ambient { get; set; } = 0.1f;
        public float Diffuse { get; set; } = 0.9f;
        public float Specular { get; set; } = 0.9f;
        public float Shininess { get; set; } = 200f;
        public StripePattern Pattern { get; set; } = null;

        public Material Copy => new()
            { Ambient = Ambient, Color = Color.Copy, Diffuse = Diffuse, Shininess = Shininess, Specular = Specular };

        protected bool Equals(Material other)
        {
            return Color.Equals(other.Color) && System.Math.Abs(Ambient - other.Ambient) < Tuple.EPSILON && System.Math.Abs(Diffuse - other.Diffuse) < Tuple.EPSILON && System.Math.Abs(Specular - other.Specular) < Tuple.EPSILON && System.Math.Abs(Shininess - other.Shininess) < Tuple.EPSILON;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Material)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Color.GetHashCode();
                hashCode = (hashCode * 397) ^ Ambient.GetHashCode();
                hashCode = (hashCode * 397) ^ Diffuse.GetHashCode();
                hashCode = (hashCode * 397) ^ Specular.GetHashCode();
                hashCode = (hashCode * 397) ^ Shininess.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Material left, Material right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Material left, Material right)
        {
            return !(left == right);
        }

        public Tuple Lighting(PointLight light, Tuple point, Tuple eyeV, Tuple normalV, bool inShadow, Shape obj)
        {
            Tuple color;
            if (Pattern is not null)
                color = Pattern.StripeAtObject(obj,point);
            else
                color = Color;
            
            var effectiveColor = color * light.Intensity;

            var lightv = (light.Position - point).Normalised();

            var ambient = effectiveColor * Ambient;

            var diffuse = Tuple.Color(0,0,0);
            var specular = Tuple.Color(0, 0, 0);
            
            var lightDotNormal = Tuple.Dot(lightv, normalV);
            if (!(lightDotNormal <0))
            {
                diffuse = effectiveColor * Diffuse * lightDotNormal;

                var reflectv = (-lightv).Reflect(normalV);
                var reflectDotEye = Tuple.Dot(reflectv, eyeV);

                if (reflectDotEye <= 0)
                {
                    specular = Tuple.Color(0, 0, 0);
                }
                else
                {
                    var factor = MathF.Pow(reflectDotEye, Shininess);
                    specular = light.Intensity * Specular * factor;
                }
            }

            return inShadow ? ambient : ambient + diffuse + specular;
        }
    }
}