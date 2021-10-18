using System;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;
using Canvas = TheRayTracerChallenge.Util.Canvas;
using Tuple = TheRayTracerChallenge.Math.Tuple;

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

        public static int WaitAny(this Task<ValueTuple<Canvas, Tuple>>[] tasks, CancellationToken token = new())
        {
            
            while (!token.IsCancellationRequested)
            {
                for (int i = 0; i < tasks.Length; i++)
                {
                    if (tasks[i] is not null && tasks[i].IsCompleted)
                        return i;
                }
                Thread.Sleep(100);
            }

            return -1;
        }

        public static float ToRad(this int degree)
        {
            return degree * Tuple.PI / 180.0f;
        }
    }
}