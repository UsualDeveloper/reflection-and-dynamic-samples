using System;
using ReflectionAndDynamicTests.Models;

namespace ReflectionAndDynamicTests.Samples
{
    /// <summary>
    /// Class showing behaviour of virtual method when using dynamic type.
    /// </summary>
    public static class DynamicInheritanceSamples
    {
        public static void RunDynamicTests()
        {
            Console.WriteLine("Running sample of dynamic type used in context of method overriding...");

            // call virtual method in the base class
            UtilityMethods.WaitWithMessage("Computer");
            Computer computer = new Computer();
            Console.WriteLine(computer.VerifyHardware());
            UtilityMethods.WaitWithMessage();

            // run the same virtual method and expect call of overriding implementations in inherited classes
            UtilityMethods.WaitWithMessage("Laptop");
            computer = new Laptop();
            Console.WriteLine(computer.VerifyHardware());
            UtilityMethods.WaitWithMessage();

            UtilityMethods.WaitWithMessage("DesktopPC");
            computer = new DesktopPC();
            Console.WriteLine(computer.VerifyHardware());
            UtilityMethods.WaitWithMessage();

            // CustomPC class defines a new implementation of the virtual method and breaks the overrides chain, 
            // the variable is declares as Computer type and the CustomPC class inherits from DesktopPC, so expect that implementation
            UtilityMethods.WaitWithMessage("CustomHomemadePC (as Computer)");
            computer = new CustomHomemadePC();
            Console.WriteLine(computer.VerifyHardware());
            UtilityMethods.WaitWithMessage();

            // assign the variable to dynamic variable and call the same virtual method, 
            // in this case, method from CustomPC will be called despite the overriding chain, 
            // because type is being checked on run time
            UtilityMethods.WaitWithMessage("CustomHomemadePC (as dynamic)");
            dynamic dynamicComputer = computer;
            Console.WriteLine(dynamicComputer.VerifyHardware());
            UtilityMethods.WaitWithMessage();
        }
    }
}
