using System;
using System.Collections;
using System.Diagnostics.Contracts;

namespace TheRayTracerChallenge.Math
{
    public class Matrix3x3 : IEnumerable
    {
        private float[,] m;

        public Matrix3x3(float initialValue = 0)
        {
            m = new float[3, 3];
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
            if (values.Length != 9)
                throw new ArgumentException("Wrong number of values");
            m[0,0] = values[0];
            m[0,1] = values[1];
            m[0,2] = values[2];
            m[1,0] = values[3];
            m[1,1] = values[4];
            m[1,2] = values[5];
            m[2,0] = values[6];
            m[2,1] = values[7];
            m[2,2] = values[8];
           
        }

        public IEnumerator GetEnumerator()
        {
            return m.GetEnumerator();
        }
        protected bool Equals(Matrix3x3 other)
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
            return Equals((Matrix3x3)obj);
        }

        public override int GetHashCode()
        {
            return (m != null ? m.GetHashCode() : 0);
        }

        public static bool operator ==(Matrix3x3 a, Matrix3x3 b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Matrix3x3 a, Matrix3x3 b)
        {
            return !(a == b);
        }

        public Matrix2x2 SubMatrix(int row, int column)
        {
            int x = 0, y = 0;
            var newMatrix = new Matrix2x2();
            for (int i = 0; i < 3; i++)
            {
                if (row == i)
                    continue;
                for (int j = 0; j < 3; j++)
                {
                    if (column == j)
                        continue;
                    newMatrix[x, y] = this[i, j];
                    y++;
                }

                y = 0;
                x++;
            }

            return newMatrix;
        }

        public float Minor(int row, int column)
        {
            return SubMatrix(row, column).Determinant;
        }

        public float Cofactor(int row, int column)
        {
            return (row + column) % 2 == 0 ? Minor(row, column) : -Minor(row, column);
        }

        public float Determinant => m[0,0]*Cofactor(0,0) + m[0,1] * Cofactor(0,1) + m[0,2] * Cofactor(0,2);
    }
}