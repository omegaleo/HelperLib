using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GameDevLibrary.Helpers
{
    public class BenchmarkUtility
    {
        private static Dictionary<string, List<long>> _benchmarks = new Dictionary<string, List<long>>();
        private static Dictionary<string, Stopwatch> _stopwatches = new Dictionary<string, Stopwatch>();

        private static Stopwatch GetStopwatch(string key)
        {
            Stopwatch stopwatch = new Stopwatch();
            
            if (_stopwatches.TryGetValue(key, out stopwatch))
            {
            }
            else
            {
                stopwatch = new Stopwatch();
                _stopwatches.Add(key, stopwatch);
            }
            
            return stopwatch;
        }

        public static long Record(Action actionToRecord)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            actionToRecord.Invoke();
            
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
        
        public static long RecordAndSaveToResults(string key, Action actionToRecord)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            actionToRecord.Invoke();
            
            stopwatch.Stop();
            
            if (!_benchmarks.ContainsKey(key))
            {
                _benchmarks[key] = new List<long>();
            }
            
            _benchmarks[key].Add(stopwatch.ElapsedMilliseconds);
            
            return stopwatch.ElapsedMilliseconds;
        }
        
        public static void Start(string key)
        {
            GetStopwatch(key).Restart();
        }

        public static void Stop(string key)
        {
            if (!_benchmarks.ContainsKey(key))
            {
                _benchmarks[key] = new List<long>();
            }

            GetStopwatch(key).Stop();
            _benchmarks[key].Add(GetStopwatch(key).ElapsedMilliseconds);
        }

        public static List<long> GetResults(string key)
        {
            return _benchmarks.ContainsKey(key) ? _benchmarks[key] : new List<long>();
        }

        public static void ClearResults()
        {
            _benchmarks.Clear();
        }

        public static Dictionary<string, List<long>> GetAllResults()
        {
            return new Dictionary<string, List<long>>(_benchmarks);
        }
    }
}