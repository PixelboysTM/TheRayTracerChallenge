using System;
using System.Linq;
using NUnit.Framework;
using SixLabors.ImageSharp.PixelFormats;
using TheRayTracerChallenge;
using TheRayTracerChallenge.Lights;
using TheRayTracerChallenge.Math;
using TheRayTracerChallenge.Shapes;
using static TheRayTracerChallenge.Math.Tuple;
using static TheRayTracerChallenge.Math.Matrix4x4;
using Tuple = TheRayTracerChallenge.Math.Tuple;

namespace UnitTests
{
    public class UnitTests
    {
        [SetUp]
        public void Setup()
        {

        }

        /// <summary>
        /// Concatenating two arrays should create a new array.
        /// </summary>
        [Test]
        public void ConcatenatingTwoArrays()
        {
            var a = new[] { 1, 2, 3 }; // Given
            var b = new[] { 3, 4, 5 }; // And
            var c = a.Concat(b).ToArray(); // When
            Assert.AreEqual(new[] { 1, 2, 3, 3, 4, 5 }, c); // Then
        }

        /// <summary>
        /// A tuple with w=1.0 is a point
        /// </summary>
        [Test]
        public void TupleIsPoint()
        {
            var a = new Tuple(4.3f, -4.2f, 3.1f, 1.0f);
            Assert.IsTrue(a.X == 4.3f);
            Assert.IsTrue(a.Y == -4.2f);
            Assert.IsTrue(a.Z == 3.1f);
            Assert.IsTrue(a.W == 1.0f);
            Assert.IsTrue(a.IsPoint);
            Assert.IsFalse(a.IsVector);
        }

        /// <summary>
        /// A tuple with w=0 is a vector
        /// </summary>
        [Test]
        public void TupleIsVector()
        {
            var a = new Tuple(4.3f, -4.2f, 3.1f, 0.0f);
            Assert.IsTrue(a.X == 4.3f);
            Assert.IsTrue(a.Y == -4.2f);
            Assert.IsTrue(a.Z == 3.1f);
            Assert.IsTrue(a.W == 0.0f);
            Assert.IsFalse(a.IsPoint);
            Assert.IsTrue(a.IsVector);
        }

        /// <summary>
        /// Tuple.Point() creates tuples with w=1
        /// </summary>
        [Test]
        public void CreatePoint()
        {
            var p = Tuple.Point(4, -4, 3);
            Assert.AreEqual(p, new Tuple(4, -4, 3, 1));
        }

        /// <summary>
        /// Tuple.Vector() creates tuples with w=0
        /// </summary>
        [Test]
        public void CreateVector()
        {
            var v = Tuple.Vector(4, -4, 3);
            Assert.AreEqual(v, new Tuple(4, -4, 3, 0));
        }

        /// <summary>
        /// Adding two tuples
        /// </summary>
        [Test]
        public void AddTuples()
        {
            var a = new Tuple(3, -2, 5, 1);
            var b = new Tuple(-2, 3, 1, 0);
            Assert.AreEqual(a + b, new Tuple(1, 1, 6, 1));
        }

        /// <summary>
        /// Subtracting two points
        /// </summary>
        [Test]
        public void SubtractTwoPoints()
        {
            var p1 = Point(3, 2, 1);
            var p2 = Point(5, 6, 7);
            Assert.AreEqual(p1 - p2, Vector(-2, -4, -6));
        }

        /// <summary>
        /// Subtracting a vector from a point
        /// </summary>
        [Test]
        public void SubtractVectorFromPoint()
        {
            var p = Point(3, 2, 1);
            var v = Vector(5, 6, 7);
            Assert.AreEqual(p - v, Point(-2, -4, -6));
        }

        /// <summary>
        /// Subtracting two vectors
        /// </summary>
        [Test]
        public void SubtractingTwoVectors()
        {
            var v1 = Vector(3, 2, 1);
            var v2 = Vector(5, 6, 7);
            Assert.AreEqual(v1 - v2, Vector(-2, -4, -6));
        }

        /// <summary>
        /// Subtracting a vector from the zero vector
        /// </summary>
        [Test]
        public void SubtractingFromZeroVector()
        {
            var zero = Vector(0, 0, 0);
            var v = Vector(1, -2, 3);
            Assert.AreEqual(zero - v, Vector(-1, 2, -3));
        }

        /// <summary>
        /// Negating a tuple
        /// </summary>
        [Test]
        public void NegateTuple()
        {
            var a = new Tuple(1, -2, 3, -4);
            Assert.AreEqual(-a, new Tuple(-1, 2, -3, 4));
        }

        /// <summary>
        /// Multiplying a tuple by a scalar
        /// </summary>
        [Test]
        public void MultiplyTupleByScalar()
        {
            var a = new Tuple(1, -2, 3, -4);
            Assert.AreEqual(a * 3.5f, new Tuple(3.5f, -7, 10.5f, -14));
        }

        /// <summary>
        /// Multiplying a tuple by a fraction
        /// </summary>
        [Test]
        public void MultiplyTupleByFraction()
        {
            var a = new Tuple(1, -2, 3, -4);
            Assert.AreEqual(a * 0.5f, new Tuple(0.5f, -1f, 1.5f, -2f));
        }

        /// <summary>
        /// Dividing a tuple by a scalar
        /// </summary>
        [Test]
        public void DividingTupleByScalar()
        {
            var a = new Tuple(1, -2, 3, -4);
            Assert.AreEqual(a / 2, new Tuple(0.5f, -1f, 1.5f, -2f));
        }

        /// <summary>
        /// Computing the magnitude of five vectors (1,0,0) (0,1,0) (0,0,1) (1,2,3) (-1,-2,-3)
        /// </summary>
        [Test]
        public void ComputingMagnitude()
        {
            var v1 = Vector(1, 0, 0);
            Assert.AreEqual(v1.Magnitude, 1);
            var v2 = Vector(0, 1, 0);
            Assert.AreEqual(v2.Magnitude, 1);
            var v3 = Vector(0, 0, 1);
            Assert.AreEqual(v3.Magnitude, 1);
            var v4 = Vector(1, 2, 3);
            Assert.AreEqual(v4.Magnitude, MathF.Sqrt(14));
            var v5 = Vector(-1, -2, -3);
            Assert.AreEqual(v5.Magnitude, MathF.Sqrt(14));
        }

        /// <summary>
        /// Normalizing vectors (4,0,0) (1,2,3)
        /// </summary>
        [Test]
        public void Normalizing()
        {
            var v1 = Vector(4, 0, 0);
            Assert.AreEqual(v1.Normalised(), Vector(1, 0, 0));
            var v2 = Vector(1, 2, 3);
            Assert.AreEqual(v2.Normalised(), Vector(1 / MathF.Sqrt(14), 2 / MathF.Sqrt(14), 3 / MathF.Sqrt(14)));
        }

        /// <summary>
        /// The dot product of two tuples
        /// </summary>
        [Test]
        public void DotProduct()
        {
            var a = Vector(1, 2, 3);
            var b = Vector(2, 3, 4);
            Assert.AreEqual(Dot(a, b), 20);
        }

        /// <summary>
        /// The cross product of two vectors
        /// </summary>
        [Test]
        public void CrossProduct()
        {
            var a = Vector(1, 2, 3);
            var b = Vector(2, 3, 4);
            Assert.AreEqual(Cross(a, b), Vector(-1, 2, -1));
            Assert.AreEqual(Cross(b, a), Vector(1, -2, 1));
        }

        /// <summary>
        /// Colors are (red, green, blue) tuples
        /// </summary>
        [Test]
        public void Colors()
        {
            var c = Color(-0.5f, 0.4f, 1.7f);
            Assert.AreEqual(c.Red, -0.5f);
            Assert.AreEqual(c.Green, 0.4f);
            Assert.AreEqual(c.Blue, 1.7f);
        }

        /// <summary>
        /// Adding Colors
        /// </summary>
        [Test]
        public void AddingColors()
        {
            var c1 = Color(0.9f, 0.6f, 0.75f);
            var c2 = Color(0.7f, 0.1f, 0.25f);
            Assert.AreEqual(c1 + c2, Color(1.6f, 0.7f, 1.0f));
        }

        /// <summary>
        /// Subtracting Colors
        /// </summary>
        [Test]
        public void SubtractingColors()
        {
            var c1 = Color(0.9f, 0.6f, 0.75f);
            var c2 = Color(0.7f, 0.1f, 0.25f);
            Assert.AreEqual(c1 - c2, Color(0.2f, 0.5f, 0.5f));
        }

        /// <summary>
        /// Multiplying a color by a Scalar
        /// </summary>
        [Test]
        public void MultiplyingColorByScalar()
        {
            var c = Color(0.2f, 0.3f, 0.4f);
            Assert.AreEqual(c * 2, Color(0.4f, 0.6f, 0.8f));
        }

        /// <summary>
        /// Multiplying Colors
        /// </summary>
        [Test]
        public void MultiplyingColors()
        {
            var c1 = Color(1f, 0.2f, 0.4f);
            var c2 = Color(0.9f, 1f, 0.1f);
            Assert.AreEqual(c1 * c2, Color(0.9f, 0.2f, 0.04f));
        }

        /// <summary>
        /// Creating a canvas
        /// </summary>
        [Test]
        public void CreatingCanvas()
        {
            var c = new Canvas(10, 20);
            Assert.AreEqual(c.Width, 10);
            Assert.AreEqual(c.Height, 20);
            Assert.IsTrue(c.IsOfColor(Color(0, 0, 0)));
        }

        /// <summary>
        /// Writing pixels to canvas
        /// </summary>
        [Test]
        public void WritingPixelToCanvas()
        {
            var c = new Canvas(10, 20);
            var red = Color(1, 0, 0);
            c.WritePixel(2, 3, red);
            Assert.AreEqual(c.GetPixel(2, 3), red);
        }

        /// <summary>
        /// Saving a canvas to a file.
        /// </summary>
        [Test]
        public void SavingCanvas()
        {
            var c = new Canvas(10, 10);
            c.SaveToFile("img/SavingCanvas.png");
            c.WritePixel(0, 0, Color(1, 0, 0));
            c.WritePixel(1, 0, Color(1, 1, 0));
            c.SaveToFile("img/SavingCanvas2.png");
        }

        /// <summary>
        /// Constructing and inspecting a 4x4 matrix
        /// </summary>
        [Test]
        public void CreateMatrix()
        {
            var M = new Matrix4x4()
            {
                {
                    1, 2, 3, 4,
                    5.5f, 6.5f, 7.5f, 8.5f,
                    9, 10, 11, 12,
                    13.5f, 14.5f, 15.5f, 16.5f
                }
            };
            Assert.AreEqual(M[0, 0], 1);
            Assert.AreEqual(M[0, 3], 4);
            Assert.AreEqual(M[1, 0], 5.5f);
            Assert.AreEqual(M[1, 2], 7.5f);
            Assert.AreEqual(M[2, 2], 11);
            Assert.AreEqual(M[3, 0], 13.5f);
            Assert.AreEqual(M[3, 2], 15.5f);
        }

        /// <summary>
        /// A 2x2 matrix ought to be representable
        /// </summary>
        [Test]
        public void CreateMatrix2x2()
        {
            var M = new Matrix2x2()
            {
                {
                    -3, 5,
                    1, -2
                }

            };
            Assert.AreEqual(M[0, 0], -3);
            Assert.AreEqual(M[0, 1], 5);
            Assert.AreEqual(M[1, 0], 1);
            Assert.AreEqual(M[1, 1], -2);
        }

        /// <summary>
        /// A 3x3 matrix ought to be representable
        /// </summary>
        [Test]
        public void CreateMatrix3x3()
        {
            var M = new Matrix3x3()
            {
                {
                    -3, 5, 0,
                    1, -2, -7,
                    0, 1, 1
                }
            };
            Assert.AreEqual(M[0, 0], -3);
            Assert.AreEqual(M[1, 1], -2);
            Assert.AreEqual(M[2, 2], 1);
        }

        /// <summary>
        /// Matrix equality with identical matrices
        /// </summary>
        [Test]
        public void MatricesEquality()
        {
            var A = new Matrix4x4()
            {
                {
                    1, 2, 3, 4,
                    5, 6, 7, 8,
                    9, 8, 7, 6,
                    5, 4, 3, 2
                }
            };
            var B = new Matrix4x4()
            {
                {
                    1, 2, 3, 4,
                    5, 6, 7, 8,
                    9, 8, 7, 6,
                    5, 4, 3, 2
                }
            };
            Assert.IsTrue(A == B);
        }

        /// <summary>
        /// Matrix equality with different matrices
        /// </summary>
        [Test]
        public void MatricesDifferent()
        {
            var A = new Matrix4x4()
            {
                {
                    1, 2, 3, 4,
                    5, 6, 7, 8,
                    9, 8, 7, 6,
                    5, 4, 3, 2
                }
            };
            var B = new Matrix4x4()
            {
                {
                    2, 3, 4, 5,
                    6, 7, 8, 9,
                    8, 7, 6, 5,
                    4, 3, 2, 1
                }
            };
            Assert.IsTrue(A != B);
        }

        /// <summary>
        /// Multiplying two matrices
        /// </summary>
        [Test]
        public void MultiplyingMatrices()
        {
            var A = new Matrix4x4()
            {
                {
                    1, 2, 3, 4,
                    5, 6, 7, 8,
                    9, 8, 7, 6,
                    5, 4, 3, 2
                }
            };
            var B = new Matrix4x4()
            {
                {
                    -2, 1, 2, 3,
                    3, 2, 1, -1,
                    4, 3, 6, 5,
                    1, 2, 7, 8
                }
            };

            Assert.AreEqual(A * B, new Matrix4x4()
            {
                {
                    20, 22, 50, 48,
                    44, 54, 114, 108,
                    40, 58, 110, 102,
                    16, 26, 46, 42
                }
            });
        }

        /// <summary>
        /// A matrix multiplied by a tuple
        /// </summary>
        [Test]
        public void MultiplyingMatrixByTuple()
        {
            var A = new Matrix4x4()
            {
                {
                    1, 2, 3, 4,
                    2, 4, 4, 2,
                    8, 6, 4, 1,
                    0, 0, 0, 1
                }
            };
            var b = new Tuple(1, 2, 3, 1);
            Assert.AreEqual(A * b, new Tuple(18, 24, 33, 1));
        }

        /// <summary>
        /// Multiplying a matrix by the identity matrix
        /// </summary>
        [Test]
        public void MultiplyingByIdentityMatrix()
        {
            var A = new Matrix4x4()
            {
                {
                    0, 1, 2, 4,
                    1, 2, 4, 8,
                    2, 4, 8, 16,
                    4, 8, 16, 32
                }
            };
            Assert.IsTrue(A * Matrix4x4.Identity == A);
        }

        /// <summary>
        /// Multiplying the identity matrix by a tuple
        /// </summary>
        [Test]
        public void MultiplyingTupleByIdentityMatrix()
        {
            var a = new Tuple(1, 2, 3, 4);
            Assert.AreEqual(Matrix4x4.Identity * a, a);
        }

        /// <summary>
        /// Transposing a matrix
        /// </summary>
        [Test]
        public void TransposingMatrix()
        {
            var A = new Matrix4x4()
            {
                {
                    0, 9, 3, 0,
                    9, 8, 0, 8,
                    1, 8, 5, 3,
                    0, 0, 5, 8
                }
            };
            Assert.AreEqual(A.Transpose, new Matrix4x4()
            {
                {
                    0, 9, 1, 0,
                    9, 8, 8, 0,
                    3, 0, 5, 5,
                    0, 8, 3, 8
                }
            });
        }

        /// <summary>
        /// Transposing the identity matrix
        /// </summary>
        [Test]
        public void TransposeIdentityMatrix()
        {
            var A = Matrix4x4.Identity.Transpose;
            Assert.AreEqual(A, Matrix4x4.Identity);
        }

        /// <summary>
        /// Calculating the determinant of a 2x2 matrix
        /// </summary>
        [Test]
        public void DeterminantOf2X2Matrix()
        {
            var A = new Matrix2x2()
            {
                {
                    1, 5,
                    -3, 2
                }
            };
            Assert.AreEqual(A.Determinant, 17);
        }

        /// <summary>
        /// A submatrix of a 3x3 matrix is a 2x2 matrix
        /// </summary>
        [Test]
        public void SubmatrixOfMatrix3X3()
        {
            var A = new Matrix3x3()
            {
                {
                    1, 5, 0,
                    -3, 2, 7,
                    0, 6, -3
                }
            };
            Assert.AreEqual(A.SubMatrix(0, 2), new Matrix2x2()
            {
                {
                    -3, 2,
                    0, 6
                }
            });
        }

        /// <summary>
        /// A submatrix of a 4x4 matrix is a 3x3 matrix
        /// </summary>
        [Test]
        public void SubmatrixOfMatrix4X4()
        {
            var A = new Matrix4x4()
            {
                {
                    -6, 1, 1, 6,
                    -8, 5, 8, 6,
                    -1, 0, 8, 2,
                    -7, 1, -1, 1
                }
            };
            Assert.AreEqual(A.SubMatrix(2, 1), new Matrix3x3()
            {
                {
                    -6, 1, 6,
                    -8, 8, 6,
                    -7, -1, 1
                }
            });
        }

        /// <summary>
        /// Calculating a minor of a 3x3 matrix
        /// </summary>
        [Test]
        public void MinorOfMatrix3X3()
        {
            var A = new Matrix3x3()
            {
                {
                    3, 5, 0,
                    2, -1, -7,
                    6, -1, 5
                }
            };
            var B = A.SubMatrix(1, 0);
            Assert.AreEqual(B.Determinant, A.Minor(1, 0));
        }

        /// <summary>
        /// Calculating a cofactor of a 3x3 matrix
        /// </summary>
        [Test]
        public void CofactorOfMatrix3X3()
        {
            var A = new Matrix3x3()
            {
                {
                    3, 5, 0,
                    2, -1, -7,
                    6, -1, 5
                }
            };
            Assert.AreEqual(A.Minor(0, 0), -12);
            Assert.AreEqual(A.Cofactor(0, 0), -12);
            Assert.AreEqual(A.Minor(1, 0), 25);
            Assert.AreEqual(A.Cofactor(1, 0), -25);
        }

        /// <summary>
        /// Calculating the determinant of a 3x3 matrix
        /// </summary>
        [Test]
        public void DeterminantOfMatrix3X3()
        {
            var A = new Matrix3x3()
            {
                {
                    1, 2, 6,
                    -5, 8, -4,
                    2, 6, 4
                }
            };
            Assert.AreEqual(A.Cofactor(0, 0), 56);
            Assert.AreEqual(A.Cofactor(0, 1), 12);
            Assert.AreEqual(A.Cofactor(0, 2), -46);
            Assert.AreEqual(A.Determinant, -196);
        }

        /// <summary>
        /// Calculating the determinant of a 4x4 matrix
        /// </summary>
        [Test]
        public void DeterminantOfMatrix4X4()
        {
            var A = new Matrix4x4()
            {
                {
                    -2, -8, 3, 5,
                    -3, 1, 7, 3,
                    1, 2, -9, 6,
                    -6, 7, 7, -9
                }
            };
            Assert.AreEqual(A.Cofactor(0, 0), 690);
            Assert.AreEqual(A.Cofactor(0, 1), 447);
            Assert.AreEqual(A.Cofactor(0, 2), 210);
            Assert.AreEqual(A.Cofactor(0, 3), 51);
            Assert.AreEqual(A.Determinant, -4071);
        }

        /// <summary>
        /// Testing an invertible matrix for invertibility
        /// </summary>
        [Test]
        public void InvertibleMatrix()
        {
            var A = new Matrix4x4()
            {
                {
                    6, 4, 4, 4,
                    5, 5, 7, 6,
                    4, -9, 3, -7,
                    9, 1, 7, -6
                }
            };
            Assert.AreEqual(A.Determinant, -2120);
            Assert.IsTrue(A.Invertible);
        }
        
        /// <summary>
        /// Testing an invertible matrix for invertibility
        /// </summary>
        [Test]
        public void NonInvertibleMatrix()
        {
            var A = new Matrix4x4()
            {
                {
                    -4,2,-2,-3,
                    9,6,2,6,
                    0,-5,1,-5,
                    0,0,0,0
                }
            };
            Assert.AreEqual(A.Determinant, 0);
            Assert.IsFalse(A.Invertible);
        }

        /// <summary>
        /// Calculating the inverse of a matrix
        /// </summary>
        [Test]
        public void InvertingMatrices()
        {
            var A = new Matrix4x4
            {
                {
                    -5, 2, 6, -8,
                    1, -5, 1, 8,
                    7, 7, -6, -7,
                    1, -3, 7, 4
                }
            };
            var B = A.Inverse;
            Assert.AreEqual(A.Determinant, 532);
            Assert.AreEqual(A.Cofactor(2,3), -160);
            Assert.AreEqual(B[3,2], -160/532.0f);
            Assert.AreEqual(A.Cofactor(3,2), 105);
            Assert.AreEqual(B[2,3], 105/532.0f);
            Assert.IsTrue(B == new Matrix4x4
            {
                {
                    0.21805f, 0.45113f, 0.24060f, -0.04511f,
                    -0.80827f, -1.45677f, -0.44361f, 0.52068f,
                    -0.07895f, -0.22368f, -0.05263f, 0.19737f,
                    -0.52256f, -0.81391f, -0.30075f, 0.30639f
                }
            });


            A = new Matrix4x4()
            {
                {
                    8, -5, 9, 2,
                    7, 5, 6, 1,
                    -6, 0, 9, 6,
                    -3, 0, -9, -4
                }
            };
            Assert.IsTrue(A.Inverse == new Matrix4x4()
            {
                {
                    -0.15385f, -0.15385f, -0.28205f, -0.53846f,
                    -0.07692f, 0.12308f, 0.02564f, 0.03077f,
                    0.35897f, 0.35897f, 0.43590f, 0.92308f,
                    -0.69231f, -0.69231f, -0.76923f, -1.92308f
                }
            });

            A = new Matrix4x4()
            {
                {
                    9, 3, 0, 9,
                    -5, -2, -6, -3,
                    -4, 9, 6, 4,
                    -7, 6, 6, 2
                }
            };
            Assert.IsTrue(A.Inverse == new Matrix4x4
            {
                {
                    -0.04074f, 
                    -0.07778f, 
                     0.14444f, 
                    -0.22222f,
                    -0.07778f, 
                     0.03333f, 
                     0.36667f, 
                    -0.33333f,
                    -0.02901f, 
                    -0.14630f, 
                    -0.10926f, 
                     0.12963f,
                     0.17778f, 
                     0.06667f, 
                    -0.26667f, 
                     0.33333f
                }

            });
        }

        /// <summary>
        /// Multiplying a product by its inverse
        /// </summary>
        [Test]
        public void MultiplyingByInverse()
        {
            var A = new Matrix4x4()
            {
                {
                    3, -9, 7, 3,
                    3, -8, 2, -9,
                    -4, 4, 4, 1,
                    -6, 5, -1, 1
                }
            };
            var B = new Matrix4x4()
            {
                {
                    8, 2, 2, 2,
                    3, -1, 7, 0,
                    7, 0, 5, 4,
                    6, -2, 0, 5
                }
            };
            var C = A * B;
            Assert.IsTrue(C * B.Inverse == A);
        }

        /// <summary>
        /// Multiplying by a translation Matrix
        /// </summary>
        [Test]
        public void MultiplyingByTranslationMatrix()
        {
            var transform = Translation(5, -3, 2);
            var p = Point(-3, 4, 5);
            Assert.AreEqual(transform * p, Point(2,1,7));
        }

        /// <summary>
        /// Multiplying by the inverse of a translation matrix
        /// </summary>
        [Test]
        public void MultiplyingByTheInverseTranslationMatrix()
        {
            var transform = Translation(5, -3, 2);
            var inv = transform.Inverse;
            var p = Point(-3, 4, 5);
            Assert.AreEqual(inv * p, Point(-8, 7, 3));
        }

        /// <summary>
        /// Translation does not affect vectors
        /// </summary>
        [Test]
        public void TransformingVectors()
        {
            var transform = Translation(5, -3, 2);
            var v = Vector(-3, 4, 5);
            Assert.AreEqual(transform * v, v);
        }

        /// <summary>
        /// A scaling matrix applied to a point
        /// </summary>
        [Test]
        public void MultiplyingByScalerMatrix()
        {
            var transform = Scaling(2, 3, 4);
            var p = Point(-4, 6, 8);
            Assert.AreEqual(transform * p, Point(-8,18, 32));
        }

        /// <summary>
        /// A scaling matrix applied to a vector
        /// </summary>
        [Test]
        public void ScalingVectors()
        {
            var transform = Scaling(2, 3, 4);
            var v = Vector(-4, 6, 8);
            Assert.AreEqual(transform * v, Vector(-8, 18, 32));
        }

        /// <summary>
        /// Multiplying by the inverse of a scaling matrix
        /// </summary>
        [Test]
        public void ScalingByTheInverse()
        {
            var transform = Scaling(2, 3, 4);
            var inv = transform.Inverse;
            var v = Vector(-4, 6, 8);
            Assert.AreEqual(inv * v, Vector(-2,2,2));
        }

        /// <summary>
        /// Reflecting is scaling by a negative value
        /// </summary>
        [Test]
        public void ReflectingAPoint()
        {
            var transform = Scaling(-1, 1, 1);
            var p = Point(2, 3, 4);
            Assert.AreEqual(transform * p, Point(-2, 3, 4));
        }

        /// <summary>
        /// Rotating a point around the x axis
        /// </summary>
        [Test]
        public void RotatingAroundX()
        {
            var p = Point(0, 1, 0);
            var halfQuarter = RotationX(MathF.PI / 4);
            var fullQuarter = RotationX(MathF.PI / 2);
            Assert.AreEqual(halfQuarter * p, Point(0, MathF.Sqrt(2) / 2.0f, MathF.Sqrt(2) / 2.0f));
            Assert.AreEqual(fullQuarter * p, Point(0,0,1));
        }

        /// <summary>
        /// The inverse of an x-rotation rotates in the opposite direction
        /// </summary>
        [Test]
        public void RotatingAroundCounterX()
        {
            var p = Point(0, 1, 0);
            var halfQuarter = RotationX(MathF.PI / 4);
            var inv = halfQuarter.Inverse;
            Assert.AreEqual(inv * p, Point(0, MathF.Sqrt(2) / 2.0f, -MathF.Sqrt(2) / 2.0f));
        }
        
        /// <summary>
        /// Rotating a point around the y axis
        /// </summary>
        [Test]
        public void RotatingAroundY()
        {
            var p = Point(0, 0, 1);
            var halfQuarter = RotationY(MathF.PI / 4);
            var fullQuarter = RotationY(MathF.PI / 2);
            Assert.AreEqual(halfQuarter * p, Point (MathF.Sqrt(2) / 2.0f, 0, MathF.Sqrt(2) / 2.0f));
            Assert.AreEqual(fullQuarter * p, Point(1,0,0));
        }
        
        /// <summary>
        /// Rotating a point around the z axis
        /// </summary>
        [Test]
        public void RotatingAroundZ()
        {
            var p = Point(0, 1, 0);
            var halfQuarter = RotationZ(MathF.PI / 4);
            var fullQuarter = RotationZ(MathF.PI / 2);
            Assert.AreEqual(halfQuarter * p, Point (-MathF.Sqrt(2) / 2.0f, MathF.Sqrt(2) / 2.0f, 0));
            Assert.AreEqual(fullQuarter * p, Point(-1,0,0));
        }

        /// <summary>
        /// A shearing transformation moves x in proportion to y
        /// </summary>
        [Test]
        public void ShearingAPointXY()
        {
            var transform = Shearing(1, 0, 0, 0, 0, 0);
            var p = Point(2, 3, 4);
            Assert.AreEqual(transform * p, Point(5,3,4));
        }
        
        /// <summary>
        /// A shearing transformation moves x in proportion to z
        /// </summary>
        [Test]
        public void ShearingAPointXZ()
        {
            var transform = Shearing(0, 1, 0, 0, 0, 0);
            var p = Point(2, 3, 4);
            Assert.AreEqual(transform * p, Point(6,3,4));
        }
        
        /// <summary>
        /// A shearing transformation moves y in proportion to x
        /// </summary>
        [Test]
        public void ShearingAPointYX()
        {
            var transform = Shearing(0, 0, 1, 0, 0, 0);
            var p = Point(2, 3, 4);
            Assert.AreEqual(transform * p, Point(2,5,4));
        }
        
        /// <summary>
        /// A shearing transformation moves y in proportion to z
        /// </summary>
        [Test]
        public void ShearingAPointYZ()
        {
            var transform = Shearing(0, 0, 0, 1, 0, 0);
            var p = Point(2, 3, 4);
            Assert.AreEqual(transform * p, Point(2,7,4));
        }
        
        /// <summary>
        /// A shearing transformation moves z in proportion to x
        /// </summary>
        [Test]
        public void ShearingAPointZX()
        {
            var transform = Shearing(0, 0, 0, 0, 1, 0);
            var p = Point(2, 3, 4);
            Assert.AreEqual(transform * p, Point(2,3,6));
        }
        
        /// <summary>
        /// A shearing transformation moves z in proportion to y
        /// </summary>
        [Test]
        public void ShearingAPointZY()
        {
            var transform = Shearing(0, 0, 0, 0, 0, 1);
            var p = Point(2, 3, 4);
            Assert.AreEqual(transform * p, Point(2,3,7));
        }

        /// <summary>
        /// Individual transformations are applied in sequence.
        /// </summary>
        [Test]
        public void TransformsAppliedInSequence()
        {
            var p = Point(1, 0, 1);
            var A = RotationX(MathF.PI / 2);
            var B = Scaling(5, 5, 5);
            var C = Translation(10, 5, 7);
            
            var p2 = A * p;
            Assert.AreEqual(p2, Point(1,-1, 0));

            var p3 = B * p2;
            Assert.AreEqual(p3, Point(5,-5, 0));

            var p4 = C * p3;
            Assert.AreEqual(p4, Point(15,0,7));
        }

        /// <summary>
        /// Chained transformations must be applied in reverse order
        /// </summary>
        [Test]
        public void TransformationChained()
        {
            var p = Point(1,0,1);
            var A = RotationX(MathF.PI / 2.0f);
            var B = Scaling(5, 5, 5);
            var C = Translation(10, 5, 7);
            var T = C * B * A;
            Assert.AreEqual(T * p, Point(15,0, 7));
        }

        /// <summary>
        /// Chained Transform by chaining Methods
        /// </summary>
        [Test]
        public void TransformationChainedOnAnother()
        {
            var p = Point(1,0,1);
            var transform = Identity.RotateX(MathF.PI / 2).Scale(5, 5, 5).Translate(10, 5, 7);
            Assert.AreEqual(transform * p, Point(15,0,7));
        }

        /// <summary>
        /// Draws a Clock on a Canvas
        /// </summary>
        [Test]
        public void DrawClock()
        {
            Canvas canvas = new Canvas(100, 100);
            for (int i = 0; i < 12; i++)
            {
                float r = ((MathF.PI * 2) / 12.0f)* i;
                var T = Identity.RotateZ(r).Scale(25, 25, 1).Translate(50,50,0);
                var p = T * Point(1, 1, 0);
                canvas.WritePixel((int)p.X, (int)p.Y, Color(1,1,1));
            }
            canvas.SaveToFile("img/DrawClock.png");
        }

        /// <summary>
        /// Creating and querying a ray
        /// </summary>
        [Test]
        public void CreatingRays()
        {
            var origin = Point(1, 2, 3);
            var direction = Vector(4, 5, 6);
            var r = new Ray(origin, direction);
            Assert.AreEqual(r.Origin, origin);
            Assert.AreEqual(r.Direction, direction);
        }

        /// <summary>
        /// Computing a point from a distance
        /// </summary>
        [Test]
        public void PointAtDistanceFromRay()
        {
            var r = new Ray(Point(2, 3, 4), Vector(1, 0, 0));
            Assert.AreEqual(r.Position(0), Point(2,3,4));
            Assert.AreEqual(r.Position(1), Point(3,3,4));
            Assert.AreEqual(r.Position(-1), Point(1,3,4));
            Assert.AreEqual(r.Position(2.5f), Point(4.5f,3,4));
        }

        /// <summary>
        /// A ray intersects a sphere at two points
        /// </summary>
        [Test]
        public void RaySphereIntersection()
        {
            var r = new Ray(Point(0, 0, -5), Vector(0, 0, 1));
            var s = new Sphere();
            var xs = s.Intersect(r);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0].t, 4.0f);
            Assert.AreEqual(xs[1].t, 6.0f);
        }
        
        /// <summary>
        /// A ray intersects a sphere at a tangent
        /// </summary>
        [Test]
        public void RaySphereIntersectionTangent()
        {
            var r = new Ray(Point(0, 1, -5), Vector(0, 0, 1));
            var s = new Sphere();
            var xs = s.Intersect(r);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0].t, 5.0f);
            Assert.AreEqual(xs[1].t, 5.0f);
        }
        
        /// <summary>
        /// A ray misses a sphere
        /// </summary>
        [Test]
        public void RaySphereMiss()
        {
            var r = new Ray(Point(0, 2, -5), Vector(0, 0, 1));
            var s = new Sphere();
            var xs = s.Intersect(r);
            Assert.AreEqual(xs.Length, 0);
        }
        
        /// <summary>
        /// A ray originates inside a sphere
        /// </summary>
        [Test]
        public void RaySphereIntersectionInside()
        {
            var r = new Ray(Point(0, 0, 0), Vector(0, 0, 1));
            var s = new Sphere();
            var xs = s.Intersect(r);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0].t, -1.0f);
            Assert.AreEqual(xs[1].t, 1.0f);
        }
        
        /// <summary>
        /// A sphere is behind a ray
        /// </summary>
        [Test]
        public void RaySphereIntersectionBehind()
        {
            var r = new Ray(Point(0, 0, 5), Vector(0, 0, 1));
            var s = new Sphere();
            var xs = s.Intersect(r);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0].t, -6.0f);
            Assert.AreEqual(xs[1].t, -4.0f);
        }

        /// <summary>
        /// An intersection encapsulates t and object
        /// </summary>
        [Test]
        public void CreatingAnIntersection()
        {
            var s = new Sphere();
            var i = new Intersection(3.5f, s);
            Assert.AreEqual(i.t, 3.5f);
            Assert.AreEqual(i.Object, s);
        }

        /// <summary>
        /// Aggregating intersections
        /// </summary>
        [Test]
        public void AggregatingIntersections()
        {
            var s = new Sphere();
            var i1 = new Intersection(1, s);
            var i2 = new Intersection(2, s);
            var xs = Intersection.Aggregate(i1, i2);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0].t, 1);
            Assert.AreEqual(xs[1].t, 2);
        }

        /// <summary>
        /// Intersect sets the object on the intersection
        /// </summary>
        [Test]
        public void IntersectOnSet()
        {
            var r = new Ray(Point(0, 0, -5), Vector(0, 0, 1));
            var s = new Sphere();
            var xs = s.Intersect(r);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0].Object, s);
            Assert.AreEqual(xs[1].Object, s);
        }

        /// <summary>
        /// The hit, when all intersections have positive t
        /// </summary>
        [Test]
        public void HitForAllPositiveT()
        {
            var s = new Sphere();
            var i1 = new Intersection(1, s);
            var i2 = new Intersection(2, s);
            var xs = Intersection.Aggregate(i2, i1);
            var i = xs.Hit();
            Assert.AreEqual(i, i1);
        }
        
        /// <summary>
        /// The hit, when some intersections have negative t
        /// </summary>
        [Test]
        public void HitForPositiveAndNegativeT()
        {
            var s = new Sphere();
            var i1 = new Intersection(-1, s);
            var i2 = new Intersection(1, s);
            var xs = Intersection.Aggregate(i2, i1);
            var i = xs.Hit();
            Assert.AreEqual(i, i2);
        }
        
        /// <summary>
        /// The hit, when all intersections have negative t
        /// </summary>
        [Test]
        public void HitForAllNegativeT()
        {
            var s = new Sphere();
            var i1 = new Intersection(-2, s);
            var i2 = new Intersection(-1, s);
            var xs = Intersection.Aggregate(i2, i1);
            var i = xs.Hit();
            Assert.AreEqual(i, null);
        }
        
        /// <summary>
        /// The hit is always the lowest nonnegative intesection
        /// </summary>
        [Test]
        public void HitIsLowestNonnegativeT()
        {
            var s = new Sphere();
            var i1 = new Intersection(5, s);
            var i2 = new Intersection(7, s);
            var i3 = new Intersection(-3, s);
            var i4 = new Intersection(2, s);
            var xs = Intersection.Aggregate(i1, i2, i3, i4);
            var i = xs.Hit();
            Assert.AreEqual(i, i4);
        }

        /// <summary>
        /// Translating a ray
        /// </summary>
        [Test]
        public void TranslatingARay()
        {
            var r = new Ray(Point(1, 2, 3), Vector(0, 1, 0));
            var m = Translation(3, 4, 5);
            var r2 = r.Transform(m);
            Assert.IsTrue(r2.Origin == Point(4,6,8));
            Assert.IsTrue(r2.Direction == Vector(0,1,0));
        }
        
        /// <summary>
        /// Scaling a ray
        /// </summary>
        [Test]
        public void ScalingARay()
        {
            var r = new Ray(Point(1, 2, 3), Vector(0, 1, 0));
            var m = Scaling(2, 3, 4);
            var r2 = r.Transform(m);
            Assert.IsTrue(r2.Origin == Point(2,6,12));
            Assert.IsTrue(r2.Direction == Vector(0,3,0));
        }

        /// <summary>
        /// A sphere's default transformation
        /// </summary>
        [Test]
        public void SphereDefaultTransform()
        {
            var s = new Sphere();
            Assert.AreEqual(s.Transform, Matrix4x4.Identity);
        }

        /// <summary>
        /// CHanging a sphere's transformation
        /// </summary>
        [Test]
        public void SphereTransformation()
        {
            var s = new Sphere();
            var t = Translation(2, 3, 4);
            s.Transform = t;
            Assert.IsTrue(s.Transform == t);
        }

        /// <summary>
        /// Intersecting a scaled sphere with a ray
        /// </summary>
        [Test]
        public void IntersectingARayWithAScaledSphere()
        {
            var r = new Ray(Point(0, 0, -5), Vector(0, 0, 1));
            var s = new Sphere();
            s.Transform = Scaling(2, 2, 2);
            var xs = s.Intersect(r);
            Assert.AreEqual(xs.Length, 2);
            Assert.AreEqual(xs[0].t, 3);
            Assert.AreEqual(xs[1].t, 7);
        }
        
        /// <summary>
        /// Intersecting a translated sphere with a ray
        /// </summary>
        [Test]
        public void IntersectingARayWithATranslatedSphere()
        {
            var r = new Ray(Point(0, 0, -5), Vector(0, 0, 1));
            var s = new Sphere();
            s.Transform = Translation(5, 0, 0);
            var xs = s.Intersect(r);
            Assert.AreEqual(xs.Length, 0);
        }

        /// <summary>
        /// Putting it together Chapter 5 
        /// </summary>
        [Test]
        public void DrawingSilhouette()
        {

            var rayOrigin = Point(0, 0, -5);
            var wallZ = 10.0f;
            var wallSize = 7.0f;
            var canvasPixel = 250;
            var pixelSize = wallSize / canvasPixel;
            var half = wallSize / 2.0f;
            
            var canvas = new Canvas(canvasPixel, canvasPixel);
            var s = new Sphere();
            s.Transform = s.Transform.Scale(1.5f, 1, 1);

            for (int y = 0; y < canvasPixel; y++)
            {
                var worldY = half - pixelSize * y;
                for (int x = 0; x < canvasPixel; x++)
                {
                    var worldX = -half + pixelSize * x;

                    var position = Point(worldX, worldY, wallZ);
                    var r = new Ray(rayOrigin, (position - rayOrigin).Normalised());
                    var xs = s.Intersect(r);
                    if (xs.Hit() != null)
                    {
                        canvas[x, y] = Color(1, 0, 0);
                    }
                }
            }
            
            canvas.SaveToFile("img/Chapter5.png");
            canvas.PrintToConsole();
        }

        /// <summary>
        /// The normal on a sphere at a point on the x axis
        /// </summary>
        [Test]
        public void NormalOnSphereX()
        {
            var s = new Sphere();
            var n = s.NormalAt(Point(1, 0, 0));
            Assert.AreEqual(n, Vector(1,0,0));
        }
        
        /// <summary>
        /// The normal on a sphere at a point on the y axis
        /// </summary>
        [Test]
        public void NormalOnSphereY()
        {
            var s = new Sphere();
            var n = s.NormalAt(Point(0, 1, 0));
            Assert.AreEqual(n, Vector(0,1,0));
        }
        
        /// <summary>
        /// The normal on a sphere at a point on the z axis
        /// </summary>
        [Test]
        public void NormalOnSphereZ()
        {
            var s = new Sphere();
            var n = s.NormalAt(Point(0, 0, 1));
            Assert.AreEqual(n, Vector(0,0,1));
        }
        
        /// <summary>
        /// The normal on a sphere at a nonaxial point
        /// </summary>
        [Test]
        public void NormalOnSphere()
        {
            var sq = MathF.Sqrt(3) / 3.0f;
            
            var s = new Sphere();
            var n = s.NormalAt(Point(sq, sq, sq));
            Assert.AreEqual(n, Vector(sq,sq,sq));
        }

        /// <summary>
        /// The normal is a normalized vector
        /// </summary>
        [Test]
        public void NormalVectorsAreNormalised()
        {
            var sq = MathF.Sqrt(3) / 3.0f;
            
            var s = new Sphere();
            var n = s.NormalAt(Point(sq, sq, sq));
            Assert.AreEqual(n, n.Normalised());
        }

        /// <summary>
        /// Computing the normal on a translated sphere
        /// </summary>
        [Test]
        public void NormalOnTranslatedSphere()
        {
            var s = new Sphere();
            s.Transform = s.Transform.Translate(0, 1, 0);
            var n = s.NormalAt(Point(0, 1.70711f, -0.70711f));
            Assert.IsTrue(n == Vector(0, 0.70711f, -0.70711f));
        }
        
        /// <summary>
        /// Computing the normal on a transformed sphere
        /// </summary>
        [Test]
        public void NormalOnTransformedSphere()
        {
            var s = new Sphere();
            s.Transform = Scaling(1,0.5f,1) * RotationZ(MathF.PI / 5.0f);
            var n = s.NormalAt(Point(0, MathF.Sqrt(2)/2.0f, -MathF.Sqrt(2)/2.0f));
            Assert.IsTrue(n == Vector(0, 0.97014f, -0.24254f));
        }

        /// <summary>
        /// Reflecting a vector approaching at 45°
        /// Reflecting a vector off a slanted surface
        /// </summary>
        [Test]
        public void Reflecting()
        {
            var v = Vector(1, -1, 0);
            var n = Vector(0, 1, 0);
            var r = v.Reflect(n);
            Assert.AreEqual(r, Vector(1,1,0));

            v = Vector(0, -1, 0);
            n = Vector(MathF.Sqrt(2) / 2.0f, MathF.Sqrt(2) / 2.0f, 0);
            r = v.Reflect(n);
            Assert.AreEqual(r, Vector(1,0,0));
        }

        /// <summary>
        /// A point light has a position and intensity
        /// </summary>
        [Test]
        public void PointLight()
        {
            var intensity = Color(1, 1, 1);
            var position = Point(0, 0, 0);
            var light = new PointLight(position, intensity);
            Assert.AreEqual(light.Position, position);
            Assert.AreEqual(light.Intensity, intensity);
        }

        /// <summary>
        /// The default material
        /// </summary>
        [Test]
        public void DefaultMaterial()
        {
            var m = new Material();
            Assert.AreEqual(m.Color, Color(1,1,1));
            Assert.AreEqual(m.Ambient, 0.1f);
            Assert.AreEqual(m.Diffuse, 0.9f);
            Assert.AreEqual(m.Specular, 0.9f);
            Assert.AreEqual(m.Shininess, 200f);
        }

        /// <summary>
        /// A sphere has a default material
        /// </summary>
        [Test]
        public void SphereDefaultMaterial()
        {
            var s = new Sphere();
            var m = s.Material;
            Assert.IsTrue(m == new Material());
        }

        /// <summary>
        /// A sphere may be assigned a material
        /// </summary>
        [Test]
        public void SphereCustomMaterial()
        {
            var s = new Sphere();
            var m = new Material();
            m.Ambient = 1;
            s.Material = m;
            Assert.IsTrue(s.Material == m);
        }

        /// <summary>
        /// Several tests regarding lightning
        /// </summary>
        [Test]
        public void Lightning()
        {
            var m = new Material();
            var position = Point(0, 0, 0);


            {
                //Lighting with eye between the light anf the surface
                var eyev = Vector(0, 0, -1);
                var normalv = Vector(0, 0, -1);
                var light = new PointLight(Point(0, 0, -10), Color(1, 1, 1));
                var result = m.Lighting(light, position, eyev, normalv, false);
                Assert.IsTrue(result == Color(1.9f, 1.9f, 1.9f));
            }

            {
                //Lighting with eye between light and surface, eye offset 45°
                var eyev = Vector(0, MathF.Sqrt(2) / 2.0f, MathF.Sqrt(2) / 2.0f);
                var normalv = Vector(0, 0, -1);
                var light = new PointLight(Point(0, 0, -10), Color(1, 1, 1));
                var result = m.Lighting(light, position, eyev, normalv, false);
                Assert.IsTrue(result == Color(1.0f, 1.0f, 1.0f));
            }

            {
                //Lighting with eye opposite surface, light offset 45°
                var eyev = Vector(0, 0, -1);
                var normalv = Vector(0, 0, -1);
                var light = new PointLight(Point(0, 10, -10), Color(1, 1, 1));
                var result = m.Lighting(light, position, eyev, normalv, false);
                Assert.IsTrue(result == Color(0.7364f,0.7364f,0.7364f));
            }

            {
                //Lighting with eye in the path of the reflection vector
                var eyev = Vector(0, -MathF.Sqrt(2) / 2.0f, -MathF.Sqrt(2) / 2.0f);
                var normalv = Vector(0, 0, -1);
                var light = new PointLight(Point(0, 10, -10), Color(1,1,1));
                var result = m.Lighting(light, position, eyev, normalv, false);
                Assert.IsTrue(result == Color(1.6363853f,1.6363853f,1.6363853f));
            }

            {
                //Lighting with the light behind the surface
                var eyev = Vector(0, 0, 1);
                var normalv = Vector(0, 0, -1);
                var light = new PointLight(Point(0, 0, 10), Color(1, 1, 1));
                var result = m.Lighting(light, position, eyev, normalv, false);
                Assert.IsTrue(result == Color(0.1f,0.1f,0.1f));
            }
        }
        
        /// <summary>
        /// Putting it together Chapter 6 
        /// </summary>
        [Test]
        public void DrawingSphere()
        {

            var rayOrigin = Point(0, 0, -5);
            var wallZ = 10.0f;
            var wallSize = 7.0f;
            var canvasPixel = 250;
            var pixelSize = wallSize / canvasPixel;
            var half = wallSize / 2.0f;
            
            var canvas = new Canvas(canvasPixel, canvasPixel);
            var s = new Sphere();
            s.Material.Color = Color(244/255.0f, 97/255.0f, 0);
            s.Transform = s.Transform.Scale(1f, 1, 1);

            var light = new PointLight(Point(22, 8, -12), Color(1,1,1));

            for (int y = 0; y < canvasPixel; y++)
            {
                var worldY = half - pixelSize * y;
                for (int x = 0; x < canvasPixel; x++)
                {
                    var worldX = -half + pixelSize * x;

                    var position = Point(worldX, worldY, wallZ);
                    var r = new Ray(rayOrigin, (position - rayOrigin).Normalised());
                    var xs = s.Intersect(r);
                    Intersection hit;
                    if ((hit = xs.Hit()) != null)
                    {
                        r.Direction = r.Direction.Normalised();
                        var point = r.Position(hit.t);
                        var normal = hit.Object.NormalAt(point);
                        var eye = -r.Direction;
                        var color = hit.Object.Material.Lighting(light, point, eye, normal, false);
                        canvas[x, y] = color;
                    }
                }
            }
            
            canvas.SaveToFile("img/Chapter6.png");
            canvas.PrintToConsole();
        }

        /// <summary>
        /// Creating a world
        /// </summary>
        [Test]
        public void WorldCreation()
        {
            var w = new World();
            Assert.IsTrue(w.Count == 0);
            Assert.IsTrue(w.LightSource == null);
        }

        /// <summary>
        /// The default world
        /// </summary>
        [Test]
        public void DefaultWorld()
        {
            var light = new PointLight(Point(-10, 10, -10), Color(1, 1, 1));
            var s1 = new Sphere()
            {
                Material = new Material()
                {
                    Color = Color(0.8f, 1.0f, 0.6f),
                    Diffuse = 0.7f,
                    Specular = 0.2f
                }
            };
            var s2 = new Sphere()
            {
                Transform = Scaling(0.5f, 0.5f, 0.5f)
            };

            var w = World.Default;
            Assert.IsTrue(w.LightSource == light);
            Assert.IsTrue(w.Contains(s1));
            Assert.IsTrue(w.Contains(s2));
        }

        /// <summary>
        /// Intersecting a world with a ray
        /// </summary>
        [Test]
        public void IntersectWorldRay()
        {
            var w = World.Default;
            var r = new Ray(Point(0, 0, -5), Vector(0, 0, 1));
            var xs = w.Intersect(r);
            Assert.AreEqual(xs.Length, 4);
            Assert.IsTrue(Math.Abs(xs[0].t - 4) < EPSILON);
            Assert.IsTrue(Math.Abs(xs[1].t - 4.5f) < EPSILON);
            Assert.IsTrue(Math.Abs(xs[2].t - 5.5f) < EPSILON);
            Assert.IsTrue(Math.Abs(xs[3].t - 6) < EPSILON);
        }

        /// <summary>
        /// Precomputing the state of an intersection
        /// </summary>
        [Test]
        public void PrecomputingStateOfIntersection()
        {
            var r = new Ray(Point(0, 0, -5), Vector(0, 0, 1));
            var shape = new Sphere();
            var i = new Intersection(4, shape);
            var comps = i.PrepareComputations(r);
            Assert.IsTrue(Math.Abs(comps.t - i.t) < EPSILON);
            Assert.IsTrue(comps.Object == i.Object);
            Assert.IsTrue(comps.Point == Point(0,0, -1));
            Assert.IsTrue(comps.EyeV == Vector(0,0,-1));
            Assert.IsTrue(comps.NormalV == Vector(0,0,-1));
        }

        /// <summary>
        /// Th hit, when an intersection occurs on the outside
        /// The hit, when an intersection occurs on the inside
        /// </summary>
        [Test]
        public void PrecomputingInsideSphere()
        {
            {
                var r = new Ray(Point(0, 0, -5), Vector(0, 0, 1));
                var shape = new Sphere();
                var i = new Intersection(4, shape);
                var comps = i.PrepareComputations(r);
                Assert.IsTrue(!comps.Inside);
            }
            
            {
                var r = new Ray(Point(0, 0, 0), Vector(0, 0, 1));
                var shape = new Sphere();
                var i = new Intersection(1, shape);
                var comps = i.PrepareComputations(r);
                Assert.IsTrue(comps.Point == Point(0,0,1));
                Assert.IsTrue(comps.EyeV == Vector(0,0,-1));
                Assert.IsTrue(comps.Inside);
                Assert.IsTrue(comps.NormalV == Vector(0,0,-1));
            }
        }

        /// <summary>
        /// Shading an intersection
        /// </summary>
        [Test]
        public void ShadingIntersection()
        {
            var w = World.Default;
            var r = new Ray(Point(0, 0, -5), Vector(0, 0, 1));
            var shape = w[0];
            var i = new Intersection(4, shape);
            var comps = i.PrepareComputations(r);
            var c = w.ShadeHit(comps);
            Assert.IsTrue(c == Color(0.38066f, 0.47583f, 0.2855f));
        }

        /// <summary>
        /// Shading an intersection from the inside
        /// </summary>
        [Test]
        public void ShadingIntersectionInside()
        {
            var w = World.Default;
            w.LightSource = new PointLight(Point(0, 0.25f, 0), Color(1, 1, 1));
            var r = new Ray(Point(0, 0, 0), Vector(0, 0, 1));
            var shape = w[1];
            var i = new Intersection(0.5f, shape);
            var comps = i.PrepareComputations(r);
            var c = w.ShadeHit(comps);
            Assert.IsTrue(c == Color(0.90498f, 0.90498f, 0.90498f));
        }

        /// <summary>
        /// Different tests for the ColorAt function
        /// </summary>
        [Test]
        public void ColorAtFunction()
        {
            {
                // The color when the ray misses
                var w = World.Default;
                var r = new Ray(Point(0, 0, -5), Vector(0, 1, 0));
                var c = w.ColorAt(r);
                Assert.IsTrue(c == Color(0,0,0));
            }

            {
                // The color when a ray hits
                var w = World.Default;
                var r = new Ray(Point(0, 0, -5), Vector(0, 0, 1));
                var c = w.ColorAt(r);
                Assert.IsTrue(c == Color(0.38066f,0.47583f,0.2855f));
            }
            
            {
                // The color with an intersection behind the ray
                var w = World.Default;
                var outer = w[0];
                outer.Material.Ambient = 1;
                var inner = w[1];
                inner.Material.Ambient = 1;

                var r = new Ray(Point(0, 0, 0.75f), Vector(0, 0, -1));
                var c = w.ColorAt(r);
                Assert.IsTrue(c == inner.Material.Color);
            }
        }

        /// <summary>
        /// The transformation matrix for the default orientation
        /// </summary>
        [Test]
        public void DefaultViewMatrix()
        {
            var from = Point(0, 0, 0);
            var to = Point(0, 0, -1);
            var up = Vector(0, 1, 0);
            var t = ViewTransformation(from, to, up);
            Assert.IsTrue(t == Identity);
        }

        /// <summary>
        /// A view transformation matrix looking in positive z direction
        /// </summary>
        [Test]
        public void ViewingInPositiveZ()
        {
            var from = Point(0, 0, 0);
            var to = Point(0, 0, 1);
            var up = Vector(0, 1, 0);
            var t = ViewTransformation(from, to, up);
            Assert.IsTrue(t == Scaling(-1,1,-1));
        }

        /// <summary>
        /// The view transformation moves the world
        /// </summary>
        [Test]
        public void ViewMovesWorld()
        {
            var from = Point(0, 0, 8);
            var to = Point(0, 0, 0);
            var up = Vector(0, 1, 0);
            var t = ViewTransformation(from, to, up);
            Assert.IsTrue(t == Translation(0,0,-8));
        }

        /// <summary>
        /// A arbitrary view transformation
        /// </summary>
        [Test]
        public void ViewMatrixLooking()
        {
            var from = Point(1, 3, 2);
            var to = Point(4, -2, 8);
            var up = Vector(1, 1, 0);
            var t = ViewTransformation(from, to, up);
            Assert.IsTrue(t == new Matrix4x4()
            {
                {
                    -0.50709f, +0.50709f, +0.67612f, -2.36643f,
                    +0.76772f, +0.60609f, +0.12122f, -2.82843f,
                    -0.35857f, +0.59761f, -0.71714f, +0.00000f,
                    +0.00000f, +0.00000f, +0.00000f, +1.00000f
                }
            });
        }

        /// <summary>
        /// Constructing a camera
        /// </summary>
        [Test]
        public void ConstructingCamera()
        {
            var hSize = 160;
            var vSize = 120;
            var fieldOfView = PI / 2.0f;
            var c = new Camera(hSize, vSize, fieldOfView);
            Assert.IsTrue(c.HSize == 160);
            Assert.IsTrue(c.VSize == 120);
            Assert.IsTrue(Math.Abs(c.FieldOfView - PI/2.0f) < EPSILON);
            Assert.IsTrue(c.Transform == Identity);
        }

        /// <summary>
        /// The pixel size for a horizontal and for a vertical canvas
        /// </summary>
        [Test]
        public void PixelSizes()
        {
            {
                // Horizontal
                var c = new Camera(200, 125, PI / 2.0f);
                Assert.IsTrue(Math.Abs(c.PixelSize - 0.01f) < EPSILON);
            }
            
            {
                // Vertical
                var c = new Camera(125, 200, PI / 2.0f);
                Assert.IsTrue(Math.Abs(c.PixelSize - 0.01f) < EPSILON);
            }
        }

        /// <summary>
        /// Constructing a ray through the center of the canvas
        /// </summary>
        [Test]
        public void RayThroughCenter()
        {
            var c = new Camera(201, 101, PI / 2.0f);
            var r = c.RayForPixel(100, 50);
            Assert.IsTrue(r.Origin == Point(0,0,0));
            Assert.IsTrue(r.Direction == Vector(0,0,-1));
        }
        
        /// <summary>
        /// Constructing a ray through the corner of the canvas
        /// </summary>
        [Test]
        public void RayThroughCorner()
        {
            var c = new Camera(201, 101, PI / 2.0f);
            var r = c.RayForPixel(0, 0);
            Assert.IsTrue(r.Origin == Point(0,0,0));
            Assert.IsTrue(r.Direction == Vector(0.66519f,0.33259f,-0.66851f));
        }
        
        /// <summary>
        /// Constructing a ray when the camera is transformed
        /// </summary>
        [Test]
        public void RayThroughTransformedCamera()
        {
            var c = new Camera(201, 101, PI / 2.0f);
            c.Transform = RotationY(PI / 4.0f) * Translation(0, -2, 5);
            var r = c.RayForPixel(100, 50);
            Assert.IsTrue(r.Origin == Point(0,2,-5));
            Assert.IsTrue(r.Direction == Vector(MathF.Sqrt(2)/2.0f,0,-MathF.Sqrt(2)/2.0f));
        }

        /// <summary>
        /// Rendering a world with a camera
        /// </summary>
        [Test]
        public void RenderWithCamera()
        {
            var w = World.Default;
            var c = new Camera(11, 11, PI / 2.0f);
            var from = Point(0, 0, -5);
            var to = Point(0, 0, 0);
            var up = Vector(0, 1, 0);
            c.Transform = ViewTransformation(from, to, up);
            Canvas image = c.Render(w);
            Assert.IsTrue(image[5,5] == Color(0.38066f, 0.47583f, 0.2855f));
        }

        /// <summary>
        /// Lighting with eye between the light anf the surface
        /// </summary>
        [Test]
        public void LightingShadows()
        {
            var m = new Material();
            var position = Point(0, 0, 0);
            
            var eyev = Vector(0, 0, -1);
            var normalv = Vector(0, 0, -1);
            var light = new PointLight(Point(0, 0, -10), Color(1, 1, 1));
            var inShadow = true;
            var result = m.Lighting(light, position, eyev, normalv, inShadow);
            Assert.IsTrue(result == Color(0.1f, 0.1f, 0.1f));
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestingForShadows()
        {
            var w = World.Default;

            {
                // There is no shadow when nothing is collinear with point and light
                var p = Point(0, 10, 0);
                Assert.IsTrue(!w.IsShadowed(p));
            }
            
            {
                // The shadow when an object is between the point and the light
                var p = Point(10, -10, 10);
                Assert.IsTrue(w.IsShadowed(p));
            }
            
            {
                // There is no shadow when an object is behind the light
                var p = Point(-20, 20, -20);
                Assert.IsTrue(!w.IsShadowed(p));
            }
            
            {
                // There is no shadow when an object is behind the point
                var p = Point(-1, 2, -2);
                Assert.IsTrue(!w.IsShadowed(p));
            }
        }

        /// <summary>
        /// ShadeHit() is given an intersection in shadow
        /// </summary>
        [Test]
        public void ShadeHit()
        {
            var w = new World();
            w.LightSource = new PointLight(Point(0, 0, -10), Color(1, 1, 1));
            var s1 = new Sphere();
            w.Add(s1);
            var s2 = new Sphere()
            {
                Transform = Translation(0, 0, 10)
            };
            w.Add(s2);
            var r = new Ray(Point(0, 0, 5), Vector(0, 0, 1));
            var i = new Intersection(4, s2);
            var comps = i.PrepareComputations(r);
            var c = w.ShadeHit(comps);
            Assert.IsTrue(c == Color(0.1f,0.1f,0.1f));
        }

        /// <summary>
        /// The hit should offset the point
        /// </summary>
        [Test]
        public void HitOffsetPoint()
        {
            var r = new Ray(Point(0, 0, -5), Vector(0, 0, 1));
            var shape = new Sphere()
            {
                Transform = Translation(0, 0, 1)
            };
            var i = new Intersection(5, shape);
            var comps = i.PrepareComputations(r);
            Assert.IsTrue(comps.OverPoint.Z < -EPSILON/2.0f);
            Assert.IsTrue(comps.Point.Z > comps.OverPoint.Z);
        }
    }
}