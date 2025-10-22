# Unity C# CodeRabbit Test Project

## Purpose
This project reproduces the false positive issue where CodeRabbit (and standard C# analyzers) incorrectly flag Unity-specific syntax as critical compile errors.

## The Problem
In Unity, types deriving from `UnityEngine.Object` overload:
- `operator true/false` - allows `if (unityObject)` syntax
- `operator ==` and `operator !=` - provides Unity-specific null checking

Standard C# analyzers don't understand these overloads and flag `if (unityObject)` as invalid.

## Testing

### Test Case 1: Without Unity Analyzers (Shows the Problem)
```bash
# Build with standard project file
dotnet build UnityTestProject.csproj
# Expected: Analyzer warnings/errors about if(playerObject)
```

### Test Case 2: With Unity Analyzers (Shows the Fix)
```bash
# Build with Unity-aware project file
dotnet build UnityTestProject.WithAnalyzers.csproj
# Expected: No false positives
```

## Solutions Demonstrated

1. **Microsoft.Unity.Analyzers NuGet package** - Adds Unity-aware analysis
2. **Define constants** - `UNITY_EDITOR`, etc. help analyzers understand context
3. **.editorconfig** - Suppress or downgrade specific rules
4. **CodeRabbit config** - Path-specific instructions for Unity files

## Files
- `SampleUnityScript.cs` - Contains all problematic patterns
- `UnityTestProject.csproj` - Standard (problematic) configuration
- `UnityTestProject.WithAnalyzers.csproj` - Fixed configuration
- `.editorconfig` - Rule severity configuration
- `.coderabbit.yaml` - CodeRabbit-specific configurationS