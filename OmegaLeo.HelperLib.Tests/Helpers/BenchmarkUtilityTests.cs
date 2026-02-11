using System;
using System.Threading;
using OmegaLeo.HelperLib.Helpers;

namespace OmegaLeo.HelperLib.Tests.Helpers;

public class BenchmarkUtilityTests
{
    [Fact]
    public void Record_ExecutesActionAndReturnsElapsedTime()
    {
        // Arrange
        var executed = false;
        
        // Act
        var elapsed = BenchmarkUtility.Record(() =>
        {
            executed = true;
            Thread.Sleep(10); // Small delay to ensure measurable time
        });
        
        // Assert
        Assert.True(executed);
        Assert.True(elapsed >= 0); // Should have some elapsed time
    }
    
    [Fact]
    public void RecordAndSaveToResults_SavesResultsUnderKey()
    {
        // Arrange
        var key = "test-benchmark-" + Guid.NewGuid();
        
        // Act
        var elapsed = BenchmarkUtility.RecordAndSaveToResults(key, () =>
        {
            Thread.Sleep(5);
        });
        
        var results = BenchmarkUtility.GetResults(key);
        
        // Assert
        Assert.True(elapsed >= 0);
        Assert.NotNull(results);
        Assert.Single(results);
        Assert.Equal(elapsed, results[0]);
    }
    
    [Fact]
    public void StartAndStop_RecordsElapsedTime()
    {
        // Arrange
        var key = "start-stop-test-" + Guid.NewGuid();
        
        // Act
        BenchmarkUtility.Start(key);
        Thread.Sleep(10);
        BenchmarkUtility.Stop(key);
        
        var results = BenchmarkUtility.GetResults(key);
        
        // Assert
        Assert.NotNull(results);
        Assert.Single(results);
        Assert.True(results[0] >= 0);
    }
    
    [Fact]
    public void Start_MultipleTimesWithSameKey_RecordsMultipleResults()
    {
        // Arrange
        var key = "multiple-runs-" + Guid.NewGuid();
        
        // Act
        BenchmarkUtility.Start(key);
        Thread.Sleep(5);
        BenchmarkUtility.Stop(key);
        
        BenchmarkUtility.Start(key);
        Thread.Sleep(5);
        BenchmarkUtility.Stop(key);
        
        var results = BenchmarkUtility.GetResults(key);
        
        // Assert
        Assert.NotNull(results);
        Assert.Equal(2, results.Count);
    }
    
    [Fact]
    public void ClearResults_RemovesAllBenchmarks()
    {
        // Arrange
        var key1 = "clear-test-1-" + Guid.NewGuid();
        var key2 = "clear-test-2-" + Guid.NewGuid();
        
        BenchmarkUtility.RecordAndSaveToResults(key1, () => Thread.Sleep(1));
        BenchmarkUtility.RecordAndSaveToResults(key2, () => Thread.Sleep(1));
        
        // Act
        BenchmarkUtility.ClearResults();
        
        var results1 = BenchmarkUtility.GetResults(key1);
        var results2 = BenchmarkUtility.GetResults(key2);
        
        // Assert
        Assert.NotNull(results1);
        Assert.Empty(results1);
        Assert.NotNull(results2);
        Assert.Empty(results2);
    }
    
    [Fact]
    public void GetAllResults_ReturnsAllBenchmarkData()
    {
        // Arrange
        BenchmarkUtility.ClearResults(); // Clean slate
        
        var key1 = "all-results-1-" + Guid.NewGuid();
        var key2 = "all-results-2-" + Guid.NewGuid();
        
        BenchmarkUtility.RecordAndSaveToResults(key1, () => Thread.Sleep(1));
        BenchmarkUtility.RecordAndSaveToResults(key2, () => Thread.Sleep(1));
        
        // Act
        var allResults = BenchmarkUtility.GetAllResults();
        
        // Assert
        Assert.NotNull(allResults);
        Assert.True(allResults.ContainsKey(key1));
        Assert.True(allResults.ContainsKey(key2));
    }
    
    [Fact]
    public void GetResults_NonExistentKey_ReturnsEmptyList()
    {
        // Arrange
        var nonExistentKey = "non-existent-" + Guid.NewGuid();
        
        // Act
        var results = BenchmarkUtility.GetResults(nonExistentKey);
        
        // Assert
        Assert.NotNull(results);
        Assert.Empty(results);
    }
}
