using System;
using System.Collections.Generic;
using System.Diagnostics;
using OmegaLeo.HelperLib.Shared.Attributes;

namespace OmegaLeo.HelperLib.Helpers
{
    [Documentation(nameof(BenchmarkUtility), "Utility class for benchmarking code execution time.", null, @"```csharp
BenchmarkUtility.Start(""MyBenchmark"");
// Code to benchmark
BenchmarkUtility.Stop(""MyBenchmark"");
var results = BenchmarkUtility.GetResults(""MyBenchmark"");
```")]
    [Changelog("1.2.0", "Fixed root namespace to OmegaLeo.HelperLib.Helpers.", "January 28, 2026")]
    /// <summary>
    /// Utility class for benchmarking code execution time.
    /// </summary>
    public class BenchmarkUtility
    {
        private static Dictionary<string, List<long>> _benchmarks = new Dictionary<string, List<long>>();
        private static Dictionary<string, Stopwatch> _stopwatches = new Dictionary<string, Stopwatch>();

        [Documentation("GetStopwatch", "Retrieves or creates a Stopwatch instance for the given key.", null, null)]
        private static Stopwatch GetStopwatch(string key)
        {
            Stopwatch stopwatch;
            
            if (!_stopwatches.TryGetValue(key, out stopwatch))
            {
                stopwatch = new Stopwatch();
                _stopwatches.Add(key, stopwatch);
            }
            
            return stopwatch;
        }

        [Documentation("Record", "Records the execution time of the provided action and returns the elapsed time in milliseconds.", null, @"```csharp
var time = BenchmarkUtility.Record(() =>
{
    // Code to benchmark
});

Console.WriteLine($""Elapsed time: {time} ms"");
```")]
        public static long Record(Action actionToRecord)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            actionToRecord.Invoke();
            
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
        
        [Documentation("RecordAndSaveToResults", "Records the execution time of the provided action, saves it under the given key, and returns the elapsed time in milliseconds.", null, @"```csharp
var time = BenchmarkUtility.RecordAndSaveToResults(""MyBenchmark"", () =>
{
    // Code to benchmark
});

Console.WriteLine($""Elapsed time: {time} ms"");

var results = BenchmarkUtility.GetResults(""MyBenchmark"");

var averageTime = results.Average();
Console.WriteLine($""Average time: {averageTime} ms"");
```")]
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
        
        [Documentation("Start", "Starts or restarts the stopwatch for the given key.", null, null)]
        /// <summary>
        /// Starts or restarts the stopwatch for the given key.
        /// </summary>
        /// <param name="key">The benchmark identifier</param>
        public static void Start(string key)
        {
            GetStopwatch(key).Restart();
        }

        [Documentation("Stop", "Stops the stopwatch for the given key and records the elapsed time.", null, null)]
        /// <summary>
        /// Stops the stopwatch for the given key and records the elapsed time.
        /// </summary>
        /// <param name="key">The benchmark identifier</param>
        public static void Stop(string key)
        {
            if (!_benchmarks.ContainsKey(key))
            {
                _benchmarks[key] = new List<long>();
            }

            GetStopwatch(key).Stop();
            _benchmarks[key].Add(GetStopwatch(key).ElapsedMilliseconds);
        }

        [Documentation("GetResults", "Retrieves the list of recorded times for the given key.", null, null)]
        /// <summary>
        /// Retrieves the list of recorded times for the given key.
        /// </summary>
        /// <param name="key">The benchmark identifier</param>
        /// <returns>List of recorded times in milliseconds</returns>
        public static List<long> GetResults(string key)
        {
            return _benchmarks.ContainsKey(key) ? _benchmarks[key] : new List<long>();
        }

        [Documentation("ClearResults", "Clears all recorded benchmark results.", null, null)]
        public static void ClearResults()
        {
            _benchmarks.Clear();
        }

        [Documentation("GetAllResults", "Retrieves all recorded benchmark results.", null, null)]
        public static Dictionary<string, List<long>> GetAllResults()
        {
            return new Dictionary<string, List<long>>(_benchmarks);
        }
    }
}