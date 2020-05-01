using System;
using System.Collections.Generic;

namespace FunctionsInterpolation
{
    public class Polinom
    {
        private const int NumberOfPoints = 10;
        private readonly Point[] _points;
        private double _minValue = double.PositiveInfinity;
        private double _maxValue = double.NegativeInfinity;
        private double _minY = double.PositiveInfinity;
        private double _maxY = double.NegativeInfinity;

        public Polinom()
        {
            _points = GetPoints();
        }

        public void Process()
        {
            foreach (var point in _points)
            {
                if (point.X < _minValue)
                {
                    _minValue = point.X;
                }

                if (point.X > _maxValue)
                {
                    _maxValue = point.X;
                }
            }

            var step = (_maxValue - _minValue) / NumberOfPoints;

            Console.WriteLine("\nLagrange interpolation polynomials:");
            for (var i = _minValue; i <= _maxValue; i = Math.Round(step + i, 3))
            {
                var currentY = Math.Round(Solve(_points, i), 2);
                if (currentY >= _maxY)
                {
                    _maxY = currentY;
                }

                if (currentY < _minY)
                {
                    _minY = currentY;
                }

                Console.WriteLine($"x: {i}\ty: {currentY}");
            }

            Console.WriteLine($"\nMinimum Y: {_minY} \nMaximum Y: {_maxY}");
            Console.WriteLine("\n--------------------\n");
        }

        private static double Solve(IReadOnlyList<Point> points, double x)
        {
            var n = points.Count;
            double r = 0, ra, rb;
            for (var i = 0; i < n; i++)
            {
                ra = rb = 1;
                for (var j = 0; j < n; j++)
                    if (i != j)
                    {
                        ra *= x - points[j].X;
                        rb *= points[i].X - points[j].X;
                    }

                r += ra * points[i].Y / rb;
            }

            return r;
        }

        private static Point[] GetPoints()
        {
            return new[]
            {
                new Point(0, 5),
                new Point(3, 2),
                new Point(6, 0),
                new Point(8, 4),
                new Point(10, 6)
            };
        }
    }
}