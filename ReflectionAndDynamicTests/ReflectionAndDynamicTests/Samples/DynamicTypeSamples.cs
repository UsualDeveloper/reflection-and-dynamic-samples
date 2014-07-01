using System;

namespace ReflectionAndDynamicTests.Samples
{
    /// <summary>
    /// Basic dynamic type usage sample code.
    /// </summary>
    public static class DynamicTypeSamples
    {
        public static void RunDynamicSamples()
        {
            Console.WriteLine("Running dynamic type basic capabilities sample...");

            Console.WriteLine("String assigned to dynamic");
            // assign string
            dynamic dynamicVariable = "my dynamic text";
            //call string method
            Console.WriteLine("Value: {0}", dynamicVariable.Substring(3));
            UtilityMethods.WaitWithMessage();

            // assign different type to the same variable
            Console.WriteLine("Type changed to bool");
            dynamicVariable = true;
            Console.WriteLine("Value: {0}", dynamicVariable);
            UtilityMethods.WaitWithMessage();

            Console.WriteLine("Method assigned");
            // this way you can assign Action to call variable like a method
            dynamicVariable = new Action<string>((s) => WriteUpperCase(s));
            Console.WriteLine("Dynamic object action assignment test:");
            dynamicVariable("This text should be uppercase.");
            UtilityMethods.WaitWithMessage();
        }

        private static void WriteUpperCase(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            Console.WriteLine(message.ToUpper());
        }
    }
}
