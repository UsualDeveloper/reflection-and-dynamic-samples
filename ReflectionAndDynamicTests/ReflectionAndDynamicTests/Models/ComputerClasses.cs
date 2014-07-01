namespace ReflectionAndDynamicTests.Models
{
    #region Test classes hierarchy to use with dynamic method binding and inheritance samples.

    public class Computer
    {
        public string Name { get; set; }

        public string Manufacturer { get; set; }

        public virtual string VerifyHardware()
        {
            return "The computer instance is OK.";
        }
    }

    public class Laptop : Computer
    {
        public bool IsBatteryOk { get; set; }

        public override string VerifyHardware()
        {
            return "The laptop instance is OK.";
        }
    }

    public class DesktopPC : Computer
    {
        public bool IsExternalMonitorOk { get; set; }

        public override string VerifyHardware()
        {
            return "The desktop PC is OK, but no battery found (it's not required anyway...).";
        }
    }

    public class CustomHomemadePC : DesktopPC
    {
        public bool IsCustomExternalDeviceOk { get; set; }

        public new string VerifyHardware()
        {
            return "Custom hardware check completed.";
        }
    }

    #endregion
}
