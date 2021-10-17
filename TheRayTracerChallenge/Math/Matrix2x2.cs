using System;
using System.Collections;
using System.Diagnostics.Contracts;

namespace TheRayTracerChallenge.Math
{
    public class Matrix2x2 : IEnumerable
    {
        private float[,] m;

        public Matrix2x2(float initialValue = 0)
        {
            m = new float[2, 2];
            m.Fill(initialValue);
        }

        public float this[int x, int y]
        {
            get => m[x, y];
            set => m[x, y] = value;
        }

        public void Add(params float[] values)
        {
            Contract.Requires(values.Length == 4);
            if (values.Length != 4)
                throw new ArgumentException("Wrong number of values");
            m[0,0] = values[0];
            m[0,1] = values[1];
            m[1,0] = values[2];
            m[1,1] = values[3];
           
        }

        public IEnumerator GetEnumerator()
        {
            return m.GetEnumerator();
        }
        
        protected bool Equals(Matrix2x2 other)
        {
            for (var x = 0; x < m.GetLength(0); x++)
            {
                for (int y = 0; y < m.GetLength(1); y++)
                {
                    if (System.Math.Abs(m[x, y] - other[x, y]) > Tuple.EPSILON)
                        return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Matrix2x2)obj);
        }

        public override int GetHashCode()
        {
            return (m != null ? m.GetHashCode() : 0);
        }

        public static bool operator ==(Matrix2x2 a, Matrix2x2 b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Matrix2x2 a, Matrix2x2 b)
        {
            return !(a == b);
        }

        public float Determinant => this[0,0]*this[1,1]-this[0,1]*this[1,0];
        
    }
}