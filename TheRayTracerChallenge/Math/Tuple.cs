using System;

namespace TheRayTracerChallenge.Math
{
    public struct Tuple
    {
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                hashCode = (hashCode * 397) ^ W.GetHashCode();
                return hashCode;
            }
        }

        public const float EPSILON = 0.00001f;
        public const float PI = MathF.PI;
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }
        
        public float Red
        {
            get => X;
            set => X = value;
        }

        public float Green
        {
            get => Y;
            set => Y = value;
        }

        public float Blue
        {
            get => Z;
            set => Z = value;
        }

        public float Alpha
        {
            get => W;
            set => W = value;
        }

        public static Tuple Point(float x, float y, float z)
        {
            return new Tuple()
            {
                X = x,
                Y = y,
                Z = z,
                W = 1
            };
        }
        public static Tuple Vector(float x, float y, float z)
        {
            return new Tuple()
            {
                X = x,
                Y = y,
                Z = z,
                W = 0
            };
        }

        public static Tuple Color(float r, float g, float b)
        {
            return Vector(r, g, b);
        }

        public Tuple(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public bool IsPoint => System.Math.Abs(W - 1.0f) < EPSILON;
        public bool IsVector => W == 0.0f;

        public Tuple Copy => new(X, Y,Z,W);

        public bool Equals(Tuple other)
        {
            return System.Math.Abs(X - other.X) < EPSILON && System.Math.Abs(Y - other.Y) < EPSILON && System.Math.Abs(Z - other.Z) < EPSILON && System.Math.Abs(W - other.W) < EPSILON;
        }

        public override bool Equals(object obj)
        {
            return obj is Tuple other && Equals(other);
        }

        public float Magnitude => MathF.Sqrt((X * X) + (Y * Y) + (Z * Z) + (W * W));

        public Tuple Clamp =>
            new(System.Math.Clamp(X, 0.0f, 1.0f), System.Math.Clamp(Y, 0.0f, 1.0f), System.Math.Clamp(Z, 0.0f, 1.0f), System.Math.Clamp(W, 0.0f, 1.0f));

        public Tuple Normalised()
        {
            float magnitude = Magnitude;
            return new Tuple(X / magnitude, Y / Magnitude, Z / Magnitude, W / Magnitude);
        }

        public static Tuple operator +(Tuple a, Tuple b)
        {
            return new Tuple(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
        }
        public static Tuple operator -(Tuple a, Tuple b)
        {
            return new Tuple(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
        }
        public static Tuple operator -(Tuple a)
        {
            return new Tuple(-a.X, -a.Y, -a.Z, -a.W);
        }
        public static Tuple operator *(Tuple a, float scalar)
        {
            return new Tuple(a.X * scalar, a.Y * scalar, a.Z * scalar, a.W * scalar);
        }
        public static Tuple operator /(Tuple a, float scalar)
        {
            return new Tuple(a.X / scalar, a.Y / scalar, a.Z / scalar, a.W / scalar);
        }
        public static Tuple operator*(Tuple a, Tuple b)
        {
            return new Tuple(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * b.W);
        }
        public static bool operator ==(Tuple a, Tuple b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Tuple a, Tuple b)
        {
            return !(a == b);
        }

        public static float Dot(Tuple a, Tuple b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;
        }

        public static Tuple Cross(Tuple a, Tuple b)
        {
            return Vector(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
        }

        public Tuple Reflect(Tuple normal)
        {
            return this - normal * 2 * Dot(this, normal);
        }
    }
}