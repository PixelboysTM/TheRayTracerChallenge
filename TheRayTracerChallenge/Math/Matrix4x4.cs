using System;
using System.Collections;
using System.Diagnostics.Contracts;

namespace TheRayTracerChallenge.Math
{
    public class Matrix4x4 : IEnumerable
    {
        private float[,] m;

        public Matrix4x4(float initialValue = 0)
        {
            m = new float[4, 4];
            m.Fill(initialValue);
        }

        public float this[int x, int y]
        {
            get => m[x, y];
            set => m[x, y] = value;
        }

        public void Add(params float[] values)
        {
            Contract.Requires(values.Length == 16);
            if (values.Length != 16)
                throw new ArgumentException("Wrong number of values");
            m[0,0] = values[0];
            m[0,1] = values[1];
            m[0,2] = values[2];
            m[0,3] = values[3];
            m[1,0] = values[4];
            m[1,1] = values[5];
            m[1,2] = values[6];
            m[1,3] = values[7];
            m[2,0] = values[8];
            m[2,1] = values[9];
            m[2,2] = values[10];
            m[2,3] = values[11];
            m[3,0] = values[12];
            m[3,1] = values[13];
            m[3,2] = values[14];
            m[3,3] = values[15];
        }

        public IEnumerator GetEnumerator()
        {
            return m.GetEnumerator();
        }

        protected bool Equals(Matrix4x4 other)
        {
            for (var x = 0; x < m.GetLength(1); x++)
            {
                for (int y = 0; y < m.GetLength(0); y++)
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
            return Equals((Matrix4x4)obj);
        }

        public override int GetHashCode()
        {
            return (m != null ? m.GetHashCode() : 0);
        }

        public static bool operator ==(Matrix4x4 a, Matrix4x4 b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Matrix4x4 a, Matrix4x4 b)
        {
            return !(a == b);
        }

        public static Matrix4x4 operator *(Matrix4x4 a, Matrix4x4 b)
        {
            return new Matrix4x4()
            {
                {
                    Mul(a,b,0,0),Mul(a,b,1,0),Mul(a,b,2,0),Mul(a,b,3,0),
                    Mul(a,b,0,1),Mul(a,b,1,1),Mul(a,b,2,1),Mul(a,b,3,1),
                    Mul(a,b,0,2),Mul(a,b,1,2),Mul(a,b,2,2),Mul(a,b,3,2),
                    Mul(a,b,0,3),Mul(a,b,1,3),Mul(a,b,2,3),Mul(a,b,3,3)
                }
            };
        }

        public static Tuple operator *(Matrix4x4 matrix, Tuple tuple)
        {
            return new Tuple(
                matrix[0,0] * tuple.X + matrix[0,1] * tuple.Y + matrix[0,2] * tuple.Z + matrix[0,3] * tuple.W,
                matrix[1,0] * tuple.X + matrix[1,1] * tuple.Y + matrix[1,2] * tuple.Z + matrix[1,3] * tuple.W,
                matrix[2,0] * tuple.X + matrix[2,1] * tuple.Y + matrix[2,2] * tuple.Z + matrix[2,3] * tuple.W,
                matrix[3,0] * tuple.X + matrix[3,1] * tuple.Y + matrix[3,2] * tuple.Z + matrix[3,3] * tuple.W
            );
        }

        public static Matrix4x4 operator /(Matrix4x4 matrix, float value)
        {
            return new Matrix4x4()
            {
                {
                    matrix[0,0] / value, matrix[0,1] / value, matrix[0,2] / value, matrix[0,3] / value,
                    matrix[1,0] / value, matrix[1,1] / value, matrix[1,2] / value, matrix[1,3] / value,
                    matrix[2,0] / value, matrix[2,1] / value, matrix[2,2] / value, matrix[2,3] / value,
                    matrix[3,0] / value, matrix[3,1] / value, matrix[3,2] / value, matrix[3,3] / value
                }
            };
        }

        private static float Mul(Matrix4x4 a, Matrix4x4 b, int dstX, int dstY)
        {
            return a[dstY,0] * b[0, dstX] +
                   a[dstY,1] * b[1, dstX] +
                   a[dstY,2] * b[2, dstX] +
                   a[dstY,3] * b[3, dstX];
        }

        public Matrix4x4 Transpose => new Matrix4x4()
            {
                {
                    this[0,0], this[1,0], this[2,0], this[3,0],
                    this[0,1], this[1,1], this[2,1], this[3,1],
                    this[0,2], this[1,2], this[2,2], this[3,2],
                    this[0,3], this[1,3], this[2,3], this[3,3]
                }
            };
        
        public Matrix3x3 SubMatrix(int row, int column)
        {
            int x = 0, y = 0;
            var newMatrix = new Matrix3x3();
            for (int i = 0; i < 4; i++)
            {
                if (row == i)
                    continue;
                for (int j = 0; j < 4; j++)
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
        

        public static Matrix4x4 Identity => new ()
        {
            {
                1,0,0,0,
                0,1,0,0,
                0,0,1,0,
                0,0,0,1
            }
        };

        public float Determinant => m[0,0]*Cofactor(0,0) + m[0,1] * Cofactor(0,1) + m[0,2] * Cofactor(0,2) + m[0,3] * Cofactor(0,3);
        public bool Invertible => System.Math.Abs(Determinant) > Tuple.EPSILON;
        public Matrix4x4 Inverse => _Inverse();

        private Matrix4x4 _Inverse()
        {
            if (!Invertible)
                throw new ArgumentException("Matrix not invertible");

            var m2 = new Matrix4x4();
            float det = Determinant;
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    var c = Cofactor(col, row);

                    m2[row, col] = c / det;
                }
            }
            // {
            //     {
            //         Cofactor(0, 0), Cofactor(0, 1), Cofactor(0, 2), Cofactor(0, 3),
            //         Cofactor(1, 0), Cofactor(1, 1), Cofactor(1, 2), Cofactor(1, 3),
            //         Cofactor(2, 0), Cofactor(2, 1), Cofactor(2, 2), Cofactor(2, 3),
            //         Cofactor(3, 0), Cofactor(3, 1), Cofactor(3, 2), Cofactor(3, 3)
            //     }
            // };
            // var transpose = cofactors.Transpose;
            return m2;
        }
        public float Cofactor(int column, int row)
        {
            return (column + row) % 2 == 0 ? Minor(column, row) : -Minor(column, row);
        }
        
        public float Minor(int row, int column)
        {
            return SubMatrix(row, column).Determinant;
        }

        public override string ToString()
        {
            return $"[\n" +
                   $"[{m[0, 0]}|{m[0, 1]}|{m[0, 2]}|{m[0, 3]}]\n" +
                   $"[{m[1, 0]}|{m[1, 1]}|{m[1, 2]}|{m[1, 3]}]\n" +
                   $"[{m[2, 0]}|{m[2, 1]}|{m[2, 2]}|{m[2, 3]}]\n" +
                   $"[{m[3, 0]}|{m[3, 1]}|{m[3, 2]}|{m[3, 3]}]\n" +
                   $"]";
        }

        public static Matrix4x4 Translation(float x, float y, float z)
        {
            var matrix = Identity;
            matrix[0, 3] = x;
            matrix[1, 3] = y;
            matrix[2, 3] = z;
            return matrix;
        }

        public static Matrix4x4 Scaling(float x, float y, float z)
        {
            var matrix = Identity;
            matrix[0, 0] = x;
            matrix[1, 1] = y;
            matrix[2, 2] = z;
            return matrix;
        }

        public static Matrix4x4 RotationX(float radians)
        {
            var matrix = Identity;
            matrix[1, 1] = MathF.Cos(radians);
            matrix[1, 2] = -MathF.Sin(radians);
            matrix[2, 1] = MathF.Sin(radians);
            matrix[2, 2] = MathF.Cos(radians);
            return matrix;
        }
        public static Matrix4x4 RotationY(float radians)
        {
            var matrix = Identity;
            matrix[0, 0] = MathF.Cos(radians);
            matrix[2, 0] = -MathF.Sin(radians);
            matrix[0, 2] = MathF.Sin(radians);
            matrix[2, 2] = MathF.Cos(radians);
            return matrix;
        }
        
        public static Matrix4x4 RotationZ(float radians)
        {
            var matrix = Identity;
            matrix[0, 0] = MathF.Cos(radians);
            matrix[0, 1] = -MathF.Sin(radians);
            matrix[1, 0] = MathF.Sin(radians);
            matrix[1, 1] = MathF.Cos(radians);
            return matrix;
        }

        public static Matrix4x4 Shearing(float xy, float xz, float yx, float yz, float zx, float zy)
        {
            var matrix = Identity;
            matrix[0, 1] = xy;
            matrix[0, 2] = xz;

            matrix[1, 0] = yx;
            matrix[1, 2] = yz;

            matrix[2, 0] = zx;
            matrix[2, 1] = zy;
            return matrix;
        }

        public Matrix4x4 Translate(float x, float y, float z)
        {
            return Translation(x, y, z) * this;
        }

        public Matrix4x4 Scale(float x, float y, float z)
        {
            return Scaling(x, y, z) * this;
        }

        public Matrix4x4 RotateX(float radians)
        {
            return RotationX(radians) * this;
        }
        public Matrix4x4 RotateY(float radians)
        {
            return RotationY(radians) * this;
        }
        public Matrix4x4 RotateZ(float radians)
        {
            return RotationZ(radians) * this;
        }
        public Matrix4x4 Shear(float xy, float xz, float yx, float yz, float zx, float zy)
        {
            return Shearing(xy, xz, yx, yz, zx, zy) * this;
        }

        public static Matrix4x4 ViewTransformation(Tuple from, Tuple to, Tuple up)
        {
            var forward = (to - from).Normalised();
            var left = Tuple.Cross(forward, up.Normalised());
            var trueUp = Tuple.Cross(left, forward);
            var orientation = new Matrix4x4()
            {
                {
                    left.X, left.Y, left.Z, 0,
                    trueUp.X, trueUp.Y, trueUp.Z, 0,
                    -forward.X, -forward.Y, -forward.Z, 0,
                    0, 0, 0, 1
                }
            };

            return orientation * Translation(-from.X, -from.Y, -from.Z);
        }
    }
}