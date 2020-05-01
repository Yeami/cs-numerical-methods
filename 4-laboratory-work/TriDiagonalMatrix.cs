using System;

namespace FunctionsInterpolation
{
    public class TriDiagonalMatrix
    {
        public readonly float[] A;
        public readonly float[] B;
        public readonly float[] C;

        private int Length => A?.Length ?? 0;

        public TriDiagonalMatrix(int length)
        {
            A = new float[length];
            B = new float[length];
            C = new float[length];
        }

        public float[] Solve(float[] array)
        {
            var length = Length;

            if (array.Length != length)
            {
                throw new ArgumentException("The input array is not the same size as this matrix.");
            }

            var firstPrime = new float[length];
            firstPrime[0] = C[0] / B[0];

            for (var i = 1; i < length; i++)
            {
                firstPrime[i] = C[i] / (B[i] - firstPrime[i - 1] * A[i]);
            }

            var secondPrime = new float[length];
            secondPrime[0] = array[0] / B[0];

            for (var i = 1; i < length; i++)
            {
                secondPrime[i] = (array[i] - secondPrime[i - 1] * A[i]) / (B[i] - firstPrime[i - 1] * A[i]);
            }

            var x = new float[length];
            x[length - 1] = secondPrime[length - 1];

            for (var i = length - 2; i >= 0; i--)
            {
                x[i] = secondPrime[i] - firstPrime[i] * x[i + 1];
            }

            return x;
        }
    }
}