using TheRayTracerChallenge.Math;

namespace TheRayTracerChallenge
{
    public static class Extensions
    {
        public static T[,] Fill<T>(this T[,] array, T value)
        {
            for (var index0 = 0; index0 < array.GetLength(0); index0++)
            for (var index1 = 0; index1 < array.GetLength(1); index1++)
            {
                array[index0, index1] = value;
            }

            return array;
        }
        
        public static Intersection Hit(this Intersection[] intersections)
        {
            for (var i = 0; i < intersections.Length; i++)
            {
                if (intersections[i].t >= Tuple.EPSILON)
                {
                    return intersections[i];
                }
            }

            return null;
        }
        
        public static int Comparison(Intersection i1, Intersection i2)
        {
            if (i1.t < i2.t)
                return -1;

            if (i1.t > i2.t)
                return 1;

            return 0;
        }
    }
}