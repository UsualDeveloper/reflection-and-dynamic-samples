using System;

namespace ReflectionAndDynamicTests
{
    /// <summary>
    /// Enumeration of code samples to be run.
    /// </summary>
    [Flags]
    public enum SamplesToRun
    {
        None = 0, 
        PerformanceComparison = 1, 
        DynamicTypeSamples = 2, 
        DynamicMethodOverrides = 4, 
        DynamicMethodBinding = 8, 
        ExpandoObject = 16 
    }
}
