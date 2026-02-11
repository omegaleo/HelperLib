using System.Collections.Generic;
using System.Linq;
using OmegaLeo.HelperLib.Extensions;

namespace OmegaLeo.HelperLib.Tests.Extensions;

public class MathExtensionsTests
{
    [Fact]
    public void AverageWithNullValidation_Int_WithValues_ReturnsCorrectAverage()
    {
        // Arrange
        var numbers = new List<int> { 1, 2, 3, 4 };
        
        // Act
        var result = numbers.AverageWithNullValidation();
        
        // Assert
        Assert.Equal(2, result);
    }
    
    [Fact]
    public void AverageWithNullValidation_Int_EmptyList_ReturnsZero()
    {
        // Arrange
        var emptyList = new List<int>();
        
        // Act
        var result = emptyList.AverageWithNullValidation();
        
        // Assert
        Assert.Equal(0, result);
    }
    
    [Fact]
    public void AverageWithNullValidation_Double_WithValues_ReturnsCorrectAverage()
    {
        // Arrange
        var numbers = new List<double> { 1.5, 2.5, 3.5 };
        
        // Act
        var result = numbers.AverageWithNullValidation();
        
        // Assert
        Assert.Equal(2.5, result);
    }
    
    [Fact]
    public void AverageWithNullValidation_Double_EmptyList_ReturnsZero()
    {
        // Arrange
        var emptyList = new List<double>();
        
        // Act
        var result = emptyList.AverageWithNullValidation();
        
        // Assert
        Assert.Equal(0.0, result);
    }
    
    [Fact]
    public void AverageWithNullValidation_Float_WithValues_ReturnsCorrectAverage()
    {
        // Arrange
        var numbers = new List<float> { 1.5f, 2.5f, 3.5f };
        
        // Act
        var result = numbers.AverageWithNullValidation();
        
        // Assert
        Assert.Equal(2.5f, result);
    }
    
    [Fact]
    public void AverageWithNullValidation_Float_EmptyList_ReturnsZero()
    {
        // Arrange
        var emptyList = new List<float>();
        
        // Act
        var result = emptyList.AverageWithNullValidation();
        
        // Assert
        Assert.Equal(0.0f, result);
    }
    
    [Fact]
    public void AverageWithNullValidation_Int_SingleValue_ReturnsThatValue()
    {
        // Arrange
        var singleValue = new List<int> { 5 };
        
        // Act
        var result = singleValue.AverageWithNullValidation();
        
        // Assert
        Assert.Equal(5, result);
    }
    
    [Fact]
    public void AverageWithNullValidation_Int_NegativeValues_ReturnsCorrectAverage()
    {
        // Arrange
        var negativeNumbers = new List<int> { -10, -20, -30 };
        
        // Act
        var result = negativeNumbers.AverageWithNullValidation();
        
        // Assert
        Assert.Equal(-20, result);
    }
}
