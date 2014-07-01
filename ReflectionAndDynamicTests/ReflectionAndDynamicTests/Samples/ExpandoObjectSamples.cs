using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Microsoft.CSharp.RuntimeBinder;
using ReflectionAndDynamicTests.Models;

namespace ReflectionAndDynamicTests.Samples
{
    /// <summary>
    /// Class showing some basic capabilities of ExpandoObject class.
    /// </summary>
    public static class ExpandoObjectSamples
    {
        public static void RunExpandoObjectTest()
        {
            Console.WriteLine("Running simple ExpandObject capabilities sample...");

            TestNonExistantMemberCallOnUserDefinedClassInstance();

            RunExpandoObjectSampleCode();
        }

        private static void TestNonExistantMemberCallOnUserDefinedClassInstance()
        {
            Computer computer = new Computer();
            computer.Name = "My computer";

            dynamic dynamicComputer = computer;
            dynamicComputer.Name = "My dynamic computer";
            Console.WriteLine("Accessed Name property in dynamic variable: (Name = {0})", dynamicComputer.Name);
            UtilityMethods.WaitWithMessage();

            try
            {
                // try caling a property that is not defined in Computer class
                dynamicComputer.Model = "1st model";
            }
            catch (RuntimeBinderException err)
            {
                Console.WriteLine("Error while calling non-existant property: {0}", err.Message);
            }
            UtilityMethods.WaitWithMessage();
        }

        private static void RunExpandoObjectSampleCode()
        {
            // ExpandoObject can be instantiated like other classes, 
            // but its expanding features are available when assigned to variable declared with dynamic type
            ExpandoObject declaredExpandoObject = new ExpandoObject();
            Console.WriteLine("Dynamically adding members to ExpandoObject instance...");
            dynamic expandoObject = declaredExpandoObject;
            expandoObject.Name = "My dynamic object";

            // add new members dynamically
            expandoObject.OrderNumber = 1234567890;
            expandoObject.ProductionDate = DateTime.Parse("2014-05-26", System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat);
            UtilityMethods.WaitWithMessage();

            // add new "method" to the object instance
            expandoObject.ColorPrintTest = new Action<string, ConsoleColor>((string a, ConsoleColor color) =>
            {
                var originalColor = Console.ForegroundColor;
                Console.ForegroundColor = color;

                Console.WriteLine("Text printed: {0}", a);

                Console.ForegroundColor = originalColor;
            });

            // call dynamically added "method"
            expandoObject.ColorPrintTest("test (this should be written in yellow)", ConsoleColor.Yellow);
            UtilityMethods.WaitWithMessage();

            PrintExpandoObjectMembers(expandoObject);
        }

        private static void PrintExpandoObjectMembers(ExpandoObject expandoObject)
        {
            Console.WriteLine("Printing members of the ExpandoObject...");
            var expandoDictionary = (IDictionary<string, object>)expandoObject;

            Console.WriteLine("First element ({0})", expandoDictionary.FirstOrDefault());

            foreach (var objectMember in expandoDictionary)
            {
                Console.WriteLine("Key: {0} | Value: {1}", objectMember.Key, objectMember.Value);
            }
            UtilityMethods.WaitWithMessage();
        }
    }
}
