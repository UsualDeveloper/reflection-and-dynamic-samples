using System.Text.RegularExpressions;

namespace SampleLibraryToTest
{
    /// <summary>
    /// Sample class to be used in the other project as dynamically called library.
    /// </summary>
    public class ExternalTestClass
    {
        public string AlgorithmName { get; set; }

        private string InternalName { get; set; }

        private int currentValue;

        public ExternalTestClass()
        {
            this.AlgorithmName = "Sample algorithm";

            this.InternalName = "SampleAlgorithm0";

            this.currentValue = 0;
        }

        public int AddNumbers(int a, int b)
        {
            return a + b;
        }

        public int AddToValue(int v)
        {
            return this.currentValue += v;
        }

        public bool IsRegexMatch(string stringToProcess, string pattern)
        {
            return Regex.IsMatch(stringToProcess, pattern);
        }

        private string LogInstanceActivity(string message)
        {
            return string.Format("Logged message: {0}", message);
        }
    }
}
