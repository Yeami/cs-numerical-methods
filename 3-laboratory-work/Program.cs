using System;

namespace NewtonsMethod
{
    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine("-----------------------\n    Newton's method\n-----------------------");
            const double eps = 0.01;
            const double x = 3.0;
            const double y = 2.0;
                
            Console.WriteLine($"Initial approximation:\nx0: {x}\ny0: {y}\neps: {eps}");

            var result = Newton(x, y, eps);

            Console.WriteLine($"\nResult:\nx: {result[0]}\ny: {result[1]}");
        }

        private static double F1(double x, double y)
        {
            return Math.Sin(x) + Math.Sqrt(2 * Math.Pow(y, 3.0)) - 4;
        }

        private static double F2(double x, double y)
        {
            return Math.Tan(x) - Math.Pow(y, 2.0) + 4;
        }

        private static double Df1dx(double x)
        {
            return Math.Cos(x);
        }

        private static double Df1dy(double y)
        {
            return 3 * Math.Pow(y, 2.0) / Math.Sqrt(2) * Math.Sqrt(Math.Pow(y, 3.0));
        }

        private static double Df2dx(double x)
        {
            return 1 / Math.Pow(Math.Cos(x), 2);
        }

        private static double Df2dy(double y)
        {
            return -2 * y;
        }

        private static double GetJacobian(double x, double y)
        {
            return Df1dx(x) * Df2dy(y) - Df1dy(y) * Df2dx(x);
        }

        private static double[] Newton(double x, double y, double eps)
        {
            double xNext, yNext;
            var counter = 0;

            do
            {
                counter++;
                var jacobian = GetJacobian(x, y);

                xNext = x + (-F1(x, y) * Df2dy(y) - -F2(x, y) * Df1dy(y)) / jacobian;
                yNext = y + (-F2(x, y) * Df1dx(x) - -F1(x, y) * Df2dx(x)) / jacobian;

                Console.WriteLine($"\n-----------------------\nIteration №{counter}:\nx: {xNext}\ny: {yNext}");

                if (Math.Abs(xNext - x) < eps && Math.Abs(yNext - y) < eps) break;

                x = xNext;
                y = yNext;
            } while (true);

            return new[] {xNext, yNext};
        }
    }
}