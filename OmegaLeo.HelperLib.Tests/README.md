# OmegaLeo.HelperLib.Tests

Comprehensive test suite for the OmegaLeo.HelperLib library.

## Overview

This project contains xUnit tests that validate the functionality of the HelperLib library. The tests run automatically in CI/CD pipelines to catch errors before they reach production.

## Test Coverage

### Extension Methods

#### MathExtensions (8 tests)
- `AverageWithNullValidation` for int, double, and float types
- Empty list handling
- Single value handling  
- Negative value handling

#### IListExtensions (14 tests)
- `Swap` - by indices and by items
- `Replace` - existing and non-existing items
- `Random` - single element and multiple elements
- `Shuffle` - list randomization
- Edge cases (empty lists, invalid parameters)

### Helper Utilities

#### BenchmarkUtility (8 tests)
- `Record` - execution time measurement
- `RecordAndSaveToResults` - saved benchmarks
- `Start` / `Stop` - manual timing
- `ClearResults` - cleanup functionality
- `GetResults` / `GetAllResults` - result retrieval

### Models

#### NeoDictionary (12 tests)
- `Add` and `TryGetValue` operations
- `TryGetValueFromIndex` - indexed access
- `ToDictionary` - conversion to standard Dictionary
- LINQ operations (`Any`, `Where`, `FirstOrDefault`, `LastOrDefault`)
- `AddRange` - bulk operations
- `Count` - size queries

## Running Tests

### Run all tests
```bash
dotnet test
```

### Run tests for a specific project
```bash
dotnet test OmegaLeo.HelperLib.Tests/OmegaLeo.HelperLib.Tests.csproj
```

### Run tests with detailed output
```bash
dotnet test --verbosity normal
```

### Run tests with code coverage
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Test Structure

Tests are organized by the area of code they test:

```
OmegaLeo.HelperLib.Tests/
├── Extensions/
│   ├── MathExtensionsTests.cs
│   └── IListExtensionsTests.cs
├── Helpers/
│   └── BenchmarkUtilityTests.cs
└── Models/
    └── NeoDictionaryTests.cs
```

## Writing New Tests

When adding new functionality to the library, follow these guidelines:

1. **Create tests first** (TDD approach recommended)
2. **Use descriptive test names** following the pattern: `MethodName_Scenario_ExpectedResult`
3. **Follow the AAA pattern**:
   - **Arrange**: Set up test data
   - **Act**: Execute the method being tested
   - **Assert**: Verify the results
4. **Test edge cases**: empty collections, null values, boundary conditions
5. **Test error conditions**: ensure exceptions are thrown when expected

### Example Test Structure

```csharp
[Fact]
public void MethodName_Scenario_ExpectedResult()
{
    // Arrange
    var input = new List<int> { 1, 2, 3 };
    
    // Act
    var result = input.MyExtensionMethod();
    
    // Assert
    Assert.Equal(expectedValue, result);
}
```

## Continuous Integration

Tests run automatically on:
- **Pull Requests** to the main branch
- **Pushes** to the main branch

The CI workflow (`.github/workflows/dotnet.yml`) is configured to:
1. Restore dependencies
2. Build the solution
3. Run all tests
4. Report results

Pull requests must pass all tests before they can be merged.

## Test Frameworks and Tools

- **xUnit** - Testing framework
- **coverlet.collector** - Code coverage collection
- **Microsoft.NET.Test.Sdk** - Test SDK

## Current Test Status

✅ **42 tests passing**
- MathExtensions: 8 tests
- IListExtensions: 14 tests  
- BenchmarkUtility: 8 tests
- NeoDictionary: 12 tests

## Contributing

When contributing to this project:

1. Ensure all existing tests pass
2. Add tests for any new functionality
3. Maintain at least the current level of code coverage
4. Follow the existing test naming and structure conventions

## Troubleshooting

### Tests fail locally but pass in CI
- Ensure you're using .NET 8.0 SDK
- Run `dotnet restore` to update dependencies
- Clear bin/obj folders and rebuild

### Tests pass locally but fail in CI
- Check for environment-specific assumptions
- Ensure tests are deterministic (not dependent on timing, randomness, etc.)
- Review test output logs in the CI workflow

## Related Documentation

- [Main Library README](../README.md)
- [Contributing Guidelines](../CONTRIBUTING.md)
- [CI Workflow](.github/workflows/dotnet.yml)
