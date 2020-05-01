using System;
using System.Collections.Generic;

namespace FunctionsInterpolation
{
    public class Spline
    {
        private float[] _a;
        private float[] _b;

        private float[] _originalX;
        private float[] _originalY;

        private int _lastIndex;

        public Spline(float startSlope = float.NaN, float endSlope = float.NaN)
        {
            float[] x = {0, 3, 6, 8, 10};
            float[] y = {5, 2, 0, 4, 6};
            Fit(x, y, startSlope, endSlope);
        }

        public void Process()
        {
            var resultX = new float[11];

            for (float i = 0; i <= 10; i++)
            {
                resultX[(int) i] = (float) Math.Round(i, 2);
            }

            Console.WriteLine("Spline interpolation:");
            var resultY = Evaluate(resultX);
            for (var i = 0; i < resultX.Length; i++)
            {
                Console.WriteLine($"x: {resultX[i]}\ty: {resultY[i]}");
            }
        }

        private void Fit(float[] x, float[] y, float startSlope = float.NaN, float endSlope = float.NaN)
        {
            if (float.IsInfinity(startSlope) || float.IsInfinity(endSlope))
            {
                throw new Exception("startSlope and endSlope cannot be infinity.");
            }

            _originalX = x;
            _originalY = y;

            var length = x.Length;
            var array = new float[length];

            var matrix = new TriDiagonalMatrix(length);
            float dx1;
            float dy1;

            if (float.IsNaN(startSlope))
            {
                dx1 = x[1] - x[0];
                matrix.C[0] = 1.0f / dx1;
                matrix.B[0] = 2.0f * matrix.C[0];
                array[0] = 3 * (y[1] - y[0]) / (dx1 * dx1);
            }
            else
            {
                matrix.B[0] = 1;
                array[0] = startSlope;
            }

            for (var i = 1; i < length - 1; i++)
            {
                dx1 = x[i] - x[i - 1];
                var dx2 = x[i + 1] - x[i];

                matrix.A[i] = 1.0f / dx1;
                matrix.C[i] = 1.0f / dx2;
                matrix.B[i] = 2.0f * (matrix.A[i] + matrix.C[i]);

                dy1 = y[i] - y[i - 1];
                var dy2 = y[i + 1] - y[i];
                array[i] = 3 * (dy1 / (dx1 * dx1) + dy2 / (dx2 * dx2));
            }

            if (float.IsNaN(endSlope))
            {
                dx1 = x[length - 1] - x[length - 2];
                dy1 = y[length - 1] - y[length - 2];
                matrix.A[length - 1] = 1.0f / dx1;
                matrix.B[length - 1] = 2.0f * matrix.A[length - 1];
                array[length - 1] = 3 * (dy1 / (dx1 * dx1));
            }
            else
            {
                matrix.B[length - 1] = 1;
                array[length - 1] = endSlope;
            }

            var k = matrix.Solve(array);

            _a = new float[length - 1];
            _b = new float[length - 1];

            for (var i = 1; i < length; i++)
            {
                dx1 = x[i] - x[i - 1];
                dy1 = y[i] - y[i - 1];
                _a[i - 1] = k[i - 1] * dx1 - dy1;
                _b[i - 1] = -k[i] * dx1 + dy1;
            }
        }

        private float[] Evaluate(IReadOnlyList<float> x)
        {
            CheckAlreadyFitted();
            var y = new float[x.Count];
            _lastIndex = 0;

            for (var i = 0; i < x.Count; i++)
            {
                var index = GetNextIndexForX(x[i]);
                y[i] = EvaluateSpline(x[i], index);
            }

            return y;
        }

        private void CheckAlreadyFitted()
        {
            if (_a == null) throw new Exception("The fit must be called before you can evaluate.");
        }

        private int GetNextIndexForX(float x)
        {
            if (x < _originalX[_lastIndex])
            {
                throw new ArgumentException("The X values to evaluate must be sorted.");
            }

            while (_lastIndex < _originalX.Length - 2 && x > _originalX[_lastIndex + 1]) _lastIndex++;

            return _lastIndex;
        }

        private float EvaluateSpline(float x, int index)
        {
            var dx = _originalX[index + 1] - _originalX[index];
            var t = (x - _originalX[index]) / dx;

            return (1 - t) * _originalY[index] + t * _originalY[index + 1] +
                   t * (1 - t) * (_a[index] * (1 - t) + _b[index] * t);
        }
    }
}