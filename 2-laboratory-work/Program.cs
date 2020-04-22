namespace SimpleIteration
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            double[][] data = {
                new double[]{ 4, 7, 4, 17 },
                new double[]{ 1, 2, 6, 12 },
                new double[]{ 9, 5, 3, 14 },
            };
            
            var simpleIteration = new SimpleIteration(data);
            simpleIteration.Process();
        }
    }
}