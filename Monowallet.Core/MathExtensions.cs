// C# program to print all  
// combination of size m  
// in an array of size n 
using System;
using System.Linq;

namespace Monowallet.Core.Multisig
{
    public static class MathExtensions
    {
        public static ulong Factorial(this int number)
        {
            var fact = (ulong)number;
            for (ulong i = (ulong)number - 1; i >= 1; i--)
            {
                fact = fact * i;
            }
            return fact;
        }
    }
}

// This code is contributed by m_kit
// This code is contributed by ajit