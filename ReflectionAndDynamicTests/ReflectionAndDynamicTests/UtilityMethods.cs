using System;
using System.IO;

namespace ReflectionAndDynamicTests
{
    /// <summary>
    /// Various helper methods for the code samples.
    /// </summary>
    public static class UtilityMethods
    {
        const string ContinuePromptString = "Press [Enter] to continue...";

        public static void WaitWithMessage(string message = ContinuePromptString)
        {
            Console.WriteLine(message);
            Console.ReadLine();
        }

        public static string GetParentFolderPath(string path, int ancestorLevelNumber = 1)
        {
            if (ancestorLevelNumber < 1)
            {
                throw new ArgumentException("Invalid ancestor level specified", "ancestorLevelNumber");
            }

            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            int dirSeparatorSearchStartIndex = path.Length - (path.EndsWith("/") ? 2 : 1);
            int currentAncestorLevel = 0;
            while (currentAncestorLevel < ancestorLevelNumber && dirSeparatorSearchStartIndex > 0)
            {
                dirSeparatorSearchStartIndex = path.LastIndexOf(Path.DirectorySeparatorChar, dirSeparatorSearchStartIndex - 1);

                if (dirSeparatorSearchStartIndex == -1)
                {
                    return path.Substring(0, dirSeparatorSearchStartIndex);
                }

                currentAncestorLevel++;
            }

            return path.Substring(0, dirSeparatorSearchStartIndex);
        }
    }
}
