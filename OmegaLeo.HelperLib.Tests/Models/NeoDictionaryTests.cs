using System;
using System.Collections.Generic;
using System.Linq;
using OmegaLeo.HelperLib.Models;

namespace OmegaLeo.HelperLib.Tests.Models;

public class NeoDictionaryTests
{
    [Fact]
    public void Add_AddsItemSuccessfully()
    {
        // Arrange
        var dict = new NeoDictionary<string, int>();
        
        // Act
        dict.Add("one", 1);
        dict.Add("two", 2);
        
        // Assert
        Assert.Equal(2, dict.Items.Count);
        Assert.Equal("one", dict.Items[0].Key);
        Assert.Equal(1, dict.Items[0].Value);
    }
    
    [Fact]
    public void TryGetValue_ExistingKey_ReturnsTrue()
    {
        // Arrange
        var dict = new NeoDictionary<string, int>();
        dict.Add("test", 42);
        
        // Act
        var result = dict.TryGetValue("test", out var value);
        
        // Assert
        Assert.True(result);
        Assert.Equal(42, value);
    }
    
    [Fact]
    public void TryGetValue_NonExistingKey_ReturnsFalse()
    {
        // Arrange
        var dict = new NeoDictionary<string, int>();
        dict.Add("test", 42);
        
        // Act
        var result = dict.TryGetValue("nonexistent", out var value);
        
        // Assert
        Assert.False(result);
        Assert.Equal(default(int), value);
    }
    
    [Fact]
    public void TryGetValueFromIndex_ValidIndex_ReturnsTrue()
    {
        // Arrange
        var dict = new NeoDictionary<string, int>();
        dict.Add("first", 1);
        dict.Add("second", 2);
        
        // Act
        var result = dict.TryGetValueFromIndex(1, out var value);
        
        // Assert
        Assert.True(result);
        Assert.Equal(2, value);
    }
    
    [Fact]
    public void TryGetValueFromIndex_InvalidIndex_ReturnsFalse()
    {
        // Arrange
        var dict = new NeoDictionary<string, int>();
        dict.Add("first", 1);
        
        // Act
        var result = dict.TryGetValueFromIndex(5, out var value);
        
        // Assert
        Assert.False(result);
        Assert.Equal(default(int), value);
    }
    
    [Fact]
    public void ToDictionary_ConvertsToStandardDictionary()
    {
        // Arrange
        var neoDict = new NeoDictionary<string, int>();
        neoDict.Add("a", 1);
        neoDict.Add("b", 2);
        
        // Act
        var standardDict = neoDict.ToDictionary();
        
        // Assert
        Assert.Equal(2, standardDict.Count);
        Assert.Equal(1, standardDict["a"]);
        Assert.Equal(2, standardDict["b"]);
    }
    
    [Fact]
    public void ImplicitConversion_ConvertsToStandardDictionary()
    {
        // Arrange
        var neoDict = new NeoDictionary<string, int>();
        neoDict.Add("x", 10);
        neoDict.Add("y", 20);
        
        // Act
        Dictionary<string, int> standardDict = neoDict;
        
        // Assert
        Assert.Equal(2, standardDict.Count);
        Assert.Equal(10, standardDict["x"]);
        Assert.Equal(20, standardDict["y"]);
    }
    
    [Fact]
    public void Any_EmptyDictionary_ReturnsFalse()
    {
        // Arrange
        var dict = new NeoDictionary<string, int>();
        
        // Act
        var result = dict.Any();
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void Any_WithItems_ReturnsTrue()
    {
        // Arrange
        var dict = new NeoDictionary<string, int>();
        dict.Add("item", 1);
        
        // Act
        var result = dict.Any();
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void Any_WithPredicate_FiltersCorrectly()
    {
        // Arrange
        var dict = new NeoDictionary<string, int>();
        dict.Add("one", 1);
        dict.Add("five", 5);
        dict.Add("ten", 10);
        
        // Act
        var result = dict.Any(item => item.Value > 5);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void Where_FiltersItems()
    {
        // Arrange
        var dict = new NeoDictionary<string, int>();
        dict.Add("one", 1);
        dict.Add("two", 2);
        dict.Add("three", 3);
        dict.Add("four", 4);
        
        // Act
        var filtered = dict.Where(item => item.Value > 2);
        
        // Assert
        Assert.Equal(2, filtered.Count());
        Assert.All(filtered, item => Assert.True(item.Value > 2));
    }
    
    [Fact]
    public void FirstOrDefault_EmptyDictionary_ReturnsNull()
    {
        // Arrange
        var dict = new NeoDictionary<string, int>();
        
        // Act
        var result = dict.FirstOrDefault();
        
        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public void FirstOrDefault_WithItems_ReturnsFirstItem()
    {
        // Arrange
        var dict = new NeoDictionary<string, int>();
        dict.Add("first", 1);
        dict.Add("second", 2);
        
        // Act
        var result = dict.FirstOrDefault();
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("first", result.Key);
        Assert.Equal(1, result.Value);
    }
    
    [Fact]
    public void LastOrDefault_WithItems_ReturnsLastItem()
    {
        // Arrange
        var dict = new NeoDictionary<string, int>();
        dict.Add("first", 1);
        dict.Add("second", 2);
        dict.Add("third", 3);
        
        // Act
        var result = dict.LastOrDefault();
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("third", result.Key);
        Assert.Equal(3, result.Value);
    }
    
    [Fact]
    public void AddRange_FromNeoDictionary_AddsAllItems()
    {
        // Arrange
        var dict1 = new NeoDictionary<string, int>();
        dict1.Add("a", 1);
        dict1.Add("b", 2);
        
        var dict2 = new NeoDictionary<string, int>();
        dict2.Add("c", 3);
        
        // Act
        dict2.AddRange(dict1);
        
        // Assert
        Assert.Equal(3, dict2.Items.Count);
    }
    
    [Fact]
    public void Count_ReturnsCorrectCount()
    {
        // Arrange
        var dict = new NeoDictionary<string, int>();
        dict.Add("one", 1);
        dict.Add("two", 2);
        dict.Add("three", 3);
        
        // Act
        var count = dict.Count();
        
        // Assert
        Assert.Equal(3, count);
    }
}
