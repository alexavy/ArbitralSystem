using System;
using JetBrains.Annotations;

namespace ArbitralSystem.Common.Validation
{
    [UsedImplicitly]
    public class Preconditions
    {
        public static T CheckNotNull<T>(T argument)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(nameof(argument));
            }

            return argument;
        }
        
        public static void CheckNotNull(params object[] arguments)
        {
            foreach (var argument in arguments)
            {
                if (argument == null)
                {
                    throw new ArgumentNullException(nameof(argument));
                }
            }
        }
        
        public static string CheckNotNullOrEmpty(string argument, string errorMessage)
        {
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentException(errorMessage);
            }

            return argument;
        }
        
        public static string CheckNotNullOrWhiteSpace(string argument, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new ArgumentException(errorMessage);
            }

            return argument;
        }
    }
}