using System;
using ReflectionAndDynamicTests.Models;

namespace ReflectionAndDynamicTests.Samples
{
    /// <summary>
    /// Class showing examples of runtime method binding using dynamic type.
    /// </summary>
    public static class DynamicMethodBinderSamples
    {
        /// <summary>
        /// Sample class defining private methods for parameters of different types with and without dynamic type usage.
        /// </summary>
        public class ComputerVerificationTool
        {
            /// <summary>
            /// Deciding which method implementation to invoke based on parameter's type during runtime.
            /// </summary>
            public bool VerifyStatus(Computer computer)
            {
                if (computer == null)
                    throw new ArgumentNullException("computer");

                var laptop = (computer as Laptop);
                if (laptop != null) return VerifyComputerStatus(laptop);
                var desktopPc = (computer as DesktopPC);
                if (desktopPc != null) return VerifyComputerStatus(desktopPc);

                return VerifyComputerStatus(computer);
            }

            /// <summary>
            /// Using dynamic type to automatically select 
            /// appropriate method implementation dynamically based on parameter's type during runtime.
            /// </summary>
            public bool VerifyStatusDynamic(Computer computer)
            {
                return VerifyComputerStatus((dynamic)computer);
            }


            private bool VerifyComputerStatus(Computer computer)
            {
                Console.WriteLine(computer.VerifyHardware());
                return true;
            }

            private bool VerifyComputerStatus(Laptop laptop)
            {
                Console.WriteLine(laptop.VerifyHardware());
                return laptop.IsBatteryOk;
            }

            private bool VerifyComputerStatus(DesktopPC desktopPc)
            {
                Console.WriteLine(desktopPc.VerifyHardware());
                return desktopPc.IsExternalMonitorOk;
            }

            /// <summary>
            /// This is defined to show that it won't be called as long as parameter is instance of a Computer or any inherited type.
            /// In this case public method requires parameter to be instance of Computer, so this method should never be called.
            /// </summary>
            private bool VerifyComputerStatus(dynamic dynamicComputer)
            {
                Console.WriteLine("Not supported object type.");

                throw new NotSupportedException("Object type not supported for computer verification check.");
            }
        }

        public static void RunMethodBindingTest()
        {
            Console.WriteLine("Running sample of dynamic type used to dynamically select method...");

            var verificationTool = new ComputerVerificationTool();

            // without dynamic type
            Console.WriteLine("Without dynamic type");
            verificationTool.VerifyStatus(new Laptop());
            verificationTool.VerifyStatus(new DesktopPC());
            verificationTool.VerifyStatus(new Computer());
            verificationTool.VerifyStatus(new CustomHomemadePC());

            UtilityMethods.WaitWithMessage();

            // with dynamic type
            Console.WriteLine("With dynamic type");
            verificationTool.VerifyStatusDynamic(new Laptop());
            verificationTool.VerifyStatusDynamic(new DesktopPC());
            verificationTool.VerifyStatusDynamic(new Computer());
            verificationTool.VerifyStatusDynamic(new CustomHomemadePC());

            UtilityMethods.WaitWithMessage();
        }
    }
}
