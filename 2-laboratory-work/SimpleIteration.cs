using System;
using System.Collections.Generic;

namespace SimpleIteration {
    public class SimpleIteration {
        private readonly List<double[]> _results;
        private readonly double[][] _odds;
        private readonly double _eps;
        private readonly IReadOnlyList<double[]> _data;
        
        public SimpleIteration(IReadOnlyList<double[]> data) {
            _results = new List<double[]>();
            _odds = new double[data.Count][];
            _data = data;
            _eps = 0.01;
            
            AddOdds(data);
        }

        public void Process() {
            if (IsOptimized()) {
                Resolve("\nThe system is optimized");
            } else {
                Console.WriteLine("\nThe system is not optimized\nStart the rearranging equations process");
                PerformOptimization(_data);
                AddOdds(_data);
                if (IsOptimized()) {
                    Resolve("\nThe system is now optimized\nStart the rearranging equations process");
                } else {
                    throw new Exception("The system is not solvable");
                }
            }
        }

        private void Resolve(string message) {
            Console.WriteLine(message);
            Iterate();
            PrintSolution(_data);
        }
        
        private bool IsOptimized() {
            for (var i = 0; i < _odds[0].Length; i++) {
                double sum = 0;
                for (var j = 0; j < _odds[0].Length; j++) {
                    if (i != j) {
                        sum += _odds[i][j];
                    }
                }
                if (sum >= 2) {
                    return false;
                }
            }
            return true;
        }
        
        private void PrintSolution(IReadOnlyList<double[]> data) {
            for (var i = 0; i < _results[0].Length; i++) {
                Console.Write($"x{ i + 1} = {_results[^1][i]}; ");
            }
            
            Console.WriteLine("\n\nChecking the current solution: ");
            
            for (var i = 0; i < _results[0].Length; i++) {
                Console.WriteLine("\tResult: " + CheckEquation(data[i]));
            }
        }
        
        private void Iterate() {
            var counter = 0;

            while (counter <= 100 && !CheckEpsilon()) {
                PerformIteration(_results[^1]);
                counter++;
            }
        }
       
        private void AddOdds(IReadOnlyList<double[]> data) {
            var current = new double[data[0].Length - 1];

            for (var i = 0; i < data[0].Length - 1; i++) {
                _odds[i] = CalculateOdds(data[i], i);
                current[i] = _odds[i][i];
            }
            _results.Add(current);
        }

        private bool CheckEpsilon() {
            if (_results.Count < 2) {
                return false;
            }
            var result = true;
            var iteration = new double[_results[0].Length];
            for (var i = 0; i < _results[0].Length; i++) {
                var current = _results[^1][i];
                var previous = _results[^2][i];
                iteration[i] = Math.Abs(current - previous);
                if (iteration[i] > _eps) {
                    result = false;
                }
                iteration[i] = Math.Round(iteration[i], 3);
            }
            return result;
        }
        
        private double CheckEquation(IReadOnlyList<double> data) {
            double sum = 0; 
            for (var i = 0; i < _results[0].Length; i++) {
                Console.Write(data[i] + "*" + _results[^1][i]);
                if(i != _results[0].Length - 1) {
                    Console.Write(" + ");
                }
                sum += data[i] * _results[^1][i];
            }
            Console.Write(" = " + data[^1]);
            return sum;
        }
        
        private void PerformIteration(IReadOnlyList<double> data) {
            var iteration = new double[data.Count];

            for (var i = 0; i < iteration.Length; i++) {
                iteration[i] = 0;
                for (var j = 0; j < iteration.Length; j++) {
                    iteration[i] = (i == j) ? iteration[i] + _odds[i][j] : iteration[i] - data[j] * _odds[i][j];
                }
                iteration[i] = Math.Round(iteration[i], 3);
            }
            _results.Add(iteration);
        }
        
        private static void PerformOptimization(IReadOnlyList<double[]> data) {
            for (var i = 0; i < data[0].Length - 1; i++) {
                var max = data[i][i];
                var maxIndex = i;
                for (var j = i + 1; j < data[0].Length - 1; j++) {
                    if (!(data[i][j] > max)) continue;
                    max = data[i][j];
                    maxIndex = j;
                }

                if (maxIndex == i) continue;
                foreach (var item in data) {
                    var temp = item[i];
                    item[i] = item[maxIndex];
                    item[maxIndex] = temp;
                }
            }
        }
        
        private static double[] CalculateOdds(IReadOnlyList<double> data, int index) {
            var odds = new double[data.Count - 1];

            for (var j = 0; j < data.Count - 1; j++) {
                odds[j] = (j == index) ? Math.Round(data[^1] / data[j], 3) : Math.Round(data[j] / data[index], 3);
            }
            return odds;
        }
    }
}