using System;
using ReflectionAndDynamicTests.Samples;

namespace ReflectionAndDynamicTests
{
    class Program
    {
        static void Main(string[] args)
        {
            // selected samples to be run
            // comment out the samples not to be called
            SamplesToRun samplesToRun = 
                SamplesToRun.PerformanceComparison | 
                SamplesToRun.DynamicTypeSamples | 
                SamplesToRun.DynamicMethodOverrides |
                SamplesToRun.DynamicMethodBinding | 
                SamplesToRun.ExpandoObject;

            if (TestIsFlagSet(samplesToRun, SamplesToRun.PerformanceComparison))
            {
                Console.Clear();
                ReflectionSimplePerformanceTestSamples.RunTest();
            }

            if (TestIsFlagSet(samplesToRun, SamplesToRun.DynamicTypeSamples))
            {
                Console.Clear();
                DynamicTypeSamples.RunDynamicSamples();
            }
            if (TestIsFlagSet(samplesToRun, SamplesToRun.DynamicMethodOverrides))
            {
                Console.Clear();
                DynamicInheritanceSamples.RunDynamicTests();
            }

            if (TestIsFlagSet(samplesToRun, SamplesToRun.DynamicMethodBinding))
            {
                Console.Clear();
                DynamicMethodBinderSamples.RunMethodBindingTest();
            }

            if (TestIsFlagSet(samplesToRun, SamplesToRun.ExpandoObject))
            {
                Console.Clear();
                ExpandoObjectSamples.RunExpandoObjectTest();
            }

            Console.WriteLine("The program has ended successfully.");
            UtilityMethods.WaitWithMessage();
        }

        private static bool TestIsFlagSet(SamplesToRun objectToBeTested, SamplesToRun flagValueToCheck)
        {
            return (objectToBeTested & flagValueToCheck) == flagValueToCheck;
        }

    }
}
