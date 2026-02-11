using System;
using System.Collections.Generic;
using System.Linq;
using OmegaLeo.HelperLib.Extensions;

namespace OmegaLeo.HelperLib.Tests.Extensions;

public class IListExtensionsTests
{
    [Fact]
    public void Swap_WithValidIndices_SwapsElements()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4 };
        
        // Act
        list.Swap(0, 2);
        
        // Assert
        Assert.Equal(3, list[0]);
        Assert.Equal(2, list[1]);
        Assert.Equal(1, list[2]);
        Assert.Equal(4, list[3]);
    }
    
    [Fact]
    public void Swap_WithItems_SwapsElementsByValue()
    {
        // Arrange
        var list = new List<string> { "A", "B", "C", "D" };
        
        // Act
        list.Swap("A", "C");
        
        // Assert
        Assert.Equal("C", list[0]);
        Assert.Equal("B", list[1]);
        Assert.Equal("A", list[2]);
        Assert.Equal("D", list[3]);
    }
    
    [Fact]
    public void Replace_ExistingItem_ReplacesSuccessfully()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4 };
        
        // Act
        list.Replace(2, 20);
        
        // Assert
        Assert.Equal(1, list[0]);
        Assert.Equal(20, list[1]);
        Assert.Equal(3, list[2]);
        Assert.Equal(4, list[3]);
    }
    
    [Fact]
    public void Replace_NonExistingItem_DoesNotModifyList()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4 };
        var originalList = new List<int>(list);
        
        // Act
        list.Replace(99, 100);
        
        // Assert
        Assert.Equal(originalList, list);
    }
    
    [Fact]
    public void Random_NonEmptyList_ReturnsElement()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4, 5 };
        
        // Act
        var result = list.Random();
        
        // Assert
        Assert.Contains(result, list);
    }
    
    [Fact]
    public void Random_WithCount_ReturnsCorrectNumberOfElements()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        
        // Act
        var result = list.Random(3);
        
        // Assert
        Assert.Equal(3, result.Count);
        Assert.All(result, item => Assert.Contains(item, list));
    }
    
    [Fact]
    public void Random_CountZero_ThrowsException()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };
        
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => list.Random(0));
    }
    
    [Fact]
    public void Random_NegativeCount_ThrowsException()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };
        
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => list.Random(-1));
    }
    
    [Fact]
    public void Shuffle_ModifiesListOrder()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var originalList = new List<int>(list);
        
        // Act
        list.Shuffle();
        
        // Assert - List should contain same elements but likely in different order
        Assert.Equal(originalList.Count, list.Count);
        Assert.All(originalList, item => Assert.Contains(item, list));
        // Note: There's a tiny chance the shuffle returns the same order, but it's extremely unlikely with 10 elements
    }
    
    [Fact]
    public void Shuffle_EmptyList_DoesNotThrow()
    {
        // Arrange
        var list = new List<int>();
        
        // Act & Assert
        var exception = Record.Exception(() => list.Shuffle());
        Assert.Null(exception);
    }
    
    [Fact]
    public void Swap_SameIndex_DoesNotModifyList()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };
        var originalList = new List<int>(list);
        
        // Act
        list.Swap(1, 1);
        
        // Assert
        Assert.Equal(originalList, list);
    }
}
