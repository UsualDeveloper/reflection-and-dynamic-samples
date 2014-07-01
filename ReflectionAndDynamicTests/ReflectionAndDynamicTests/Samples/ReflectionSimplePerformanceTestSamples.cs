using System;
using System.Diagnostics;
using System.Reflection;

namespace ReflectionAndDynamicTests.Samples
{
    /// <summary>
    /// Helper class running a simple comparison of different ways to invoke a method in context of a specific object instance.
    /// </summary>
    public static class ReflectionSimplePerformanceTestSamples
    {
        /// <summary>
        /// Number of iterations to use in each of the methods tested.
        /// </summary>
        public const int NumberOfIterations = 1000000;

        /// <summary>
        /// Name of the method to be used in the test.
        /// </summary>
        public const string MethodName = "AddNumbers";

        public static void RunTest()
        {
            Console.WriteLine("Running performance comparison of method invocation...");
            Console.WriteLine(string.Format("Method name: {0}, iterations: {1}", MethodName, NumberOfIterations));
            Console.WriteLine("Please note that some preparatory actions for each method performed only once are put outside the time measurement.");
            Console.WriteLine();

            // get path to the assembly containing the class to use in the test
            string path = UtilityMethods.GetParentFolderPath(Assembly.GetEntryAssembly().Location, 1) + @"\SampleLibraryToTest.dll";
            string typeName = "SampleLibraryToTest.ExternalTestClass";

            object instance = Assembly.LoadFrom(path).CreateInstance(typeName);
            Type objType = instance.GetType();

            // first, call method without reflection, using usual c# syntax in order to get reference time for other tests
            MeasureMethodCall((SampleLibraryToTest.ExternalTestClass)instance);
            UtilityMethods.WaitWithMessage();

            // call the same method using reflection with Invoke() method, getting MethodInfo object each time
            MeasureSimpleReflection(instance, objType);
            UtilityMethods.WaitWithMessage();

            // call the method using reflection with Invoke() method, getting MethodInfo object only once and reusing it in all iterations
            MeasureSimpleReflectionMethodInfoCached(instance, objType);
            UtilityMethods.WaitWithMessage();

            // call the method using instance-bound delegate created only once
            MeasureReflectionWithClosedDelegate(instance, objType);
            UtilityMethods.WaitWithMessage();

            // call the method using delegate not bound to a specific instance, created only once
            MeasureReflectionWithOpenDelegate((SampleLibraryToTest.ExternalTestClass)instance, objType);
            UtilityMethods.WaitWithMessage();

            // creating method delegates requires developer to specify number and types of parameters and return value
            // this sample uses generic method to partially overcome those restrictions while still using delegates
            MeasureReflectionWithDelegateGeneralFunction(instance, objType);
            UtilityMethods.WaitWithMessage();

            // call method on a dynamic variable
            MeasureDynamicVariableUsage(instance);
            UtilityMethods.WaitWithMessage();
        }

        /// <summary>
        /// Simple method calls to be used as reference for other methods.
        /// </summary>
        private static void MeasureMethodCall(SampleLibraryToTest.ExternalTestClass instance, int numberOfIterations = NumberOfIterations)
        {
            Stopwatch stopWatch = Stopwatch.StartNew();
            int r = 0;
            for (int i = 0; i < numberOfIterations; ++i)
            {
                r = instance.AddNumbers(1, i);
            }
            stopWatch.Stop();
            Console.WriteLine(string.Format("Method call (no reflection): {0} ms", stopWatch.ElapsedMilliseconds));
        }

        /// <summary>
        /// Calls a method getting MethodInfo object each time before calling Invoke() method.
        /// </summary>
        private static void MeasureSimpleReflection(object instance, Type objType, string methodName = MethodName, int numberOfIterations = NumberOfIterations)
        {
            Stopwatch stopWatch = Stopwatch.StartNew();
            int r = 0;
            for (int i = 0; i < numberOfIterations; ++i)
            {
                var methodInfo = objType.GetMethod(methodName);

                r = (int)methodInfo.Invoke(instance, new object[] { 1, i });
            }
            stopWatch.Stop();
            Console.WriteLine(string.Format("Not optimized reflection (Invoke() method calls): {0} ms", stopWatch.ElapsedMilliseconds));
        }

        /// <summary>
        /// Calls a method using Invoke() method, but gets MethodInfo only once and use it in all iterations.
        /// </summary>
        private static void MeasureSimpleReflectionMethodInfoCached(object instance, Type objType, string methodName = MethodName, int numberOfIterations = NumberOfIterations)
        {
            var methodInfo = objType.GetMethod(methodName);

            Stopwatch stopWatch = Stopwatch.StartNew();
            int r = 0;
            for (int i = 0; i < numberOfIterations; ++i)
            {
                r = (int)methodInfo.Invoke(instance, new object[] { 1, i });
            }
            stopWatch.Stop();
            Console.WriteLine(string.Format("Normal reflection (Invoke() method calls with MethodInfo requested only once): {0} ms", stopWatch.ElapsedMilliseconds));
        }

        /// <summary>
        /// Calls a method creating a closed delegate bound to a specific object instance.
        /// </summary>
        private static void MeasureReflectionWithClosedDelegate(object instance, Type objType, string methodName = MethodName, int numberOfIterations = NumberOfIterations)
        {
            var methodInfo = objType.GetMethod(methodName);

            var stopWatch = Stopwatch.StartNew();
            var func = (Func<int, int, int>)Delegate.CreateDelegate(typeof(Func<int, int, int>), instance, methodInfo);
            int r = 0;
            for (int i = 0; i < numberOfIterations; ++i)
            {
                r = func(1, i);
            }
            stopWatch.Stop();
            Console.WriteLine(string.Format("Closed delegate: {0} ms", stopWatch.ElapsedMilliseconds));
        }

        /// <summary>
        /// Calls a method using an open delegate not bound to any object instance.
        /// </summary>
        private static void MeasureReflectionWithOpenDelegate(SampleLibraryToTest.ExternalTestClass instance, Type objType, string methodName = MethodName, int numberOfIterations = NumberOfIterations)
        {
            var methodInfo = objType.GetMethod(methodName);

            var stopWatch = Stopwatch.StartNew();
            var func = (Func<SampleLibraryToTest.ExternalTestClass, int, int, int>)Delegate.CreateDelegate(typeof(Func<SampleLibraryToTest.ExternalTestClass, int, int, int>), methodInfo);
            int r = 0;
            for (int i = 0; i < numberOfIterations; ++i)
            {
                r = func(instance, 1, i);
            }
            stopWatch.Stop();
            Console.WriteLine(string.Format("Opened delegate: {0} ms", stopWatch.ElapsedMilliseconds));
        }

        /// <summary>
        /// Calls a method using an open delegate indirectly to overcome reqiurement to specify parameters types and number.
        /// </summary>
        private static void MeasureReflectionWithDelegateGeneralFunction(object instance, Type objType, string methodName = MethodName, int numberOfIterations = NumberOfIterations)
        {
            // get method info of delegate creating helper generic method
            var delegateCreatingMethodInfo = typeof(ReflectionSimplePerformanceTestSamples).GetMethod("CreateDelegateFunc", BindingFlags.Static | BindingFlags.NonPublic);
            var methodInfo = objType.GetMethod(methodName);
            var stopWatch = Stopwatch.StartNew();

            // create MethodInfo object for specific number and types of parameters and return value.
            var delegateMethodInfoWithGenericParams = delegateCreatingMethodInfo.MakeGenericMethod(objType, typeof(int), typeof(int), typeof(int));

            // invoke the method to create a delegate for the specific return value and parameters and return a function for its access
            var func = (Func<object, object[], object>)delegateMethodInfoWithGenericParams.Invoke(null, new object[] { methodInfo });
            
            int r = 0;
            object[] argsArray = new object[2];
            for (int i = 0; i < numberOfIterations; ++i)
            {
                argsArray[0] = 1;
                argsArray[1] = i;

                r = (int)func(instance, argsArray);
            }
            stopWatch.Stop();
            Console.WriteLine(string.Format("Opened delegate with weaker parameter type and number restrictions: {0} ms", stopWatch.ElapsedMilliseconds));
        }

        /// <summary>
        /// Call a method treating instance as dynamic.
        /// </summary>
        private static void MeasureDynamicVariableUsage(object instance, int numberOfIterations = NumberOfIterations)
        {
            var stopWatch = Stopwatch.StartNew();
            dynamic dynamicInstance = instance;
            int r = 0;
            for (int i = 0; i < numberOfIterations; ++i)
            {
                r = dynamicInstance.AddNumbers(1, i);
            }
            stopWatch.Stop();
            Console.WriteLine(string.Format("Dynamic variable: {0} ms", stopWatch.ElapsedMilliseconds));
        }

        private static Func<object, object[], object> CreateDelegateFunc<T, P1, P2, TResult>(MethodInfo methodInfo)
        {
            var d = (Func<T, P1, P2, TResult>)Delegate.CreateDelegate(typeof(Func<T, P1, P2, TResult>), methodInfo);

            return (object instance, object[] args) => d((T)instance, (P1)args[0], (P2)args[1]);
        }
    }
}
