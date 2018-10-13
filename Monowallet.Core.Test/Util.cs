// C# program to print all  
// combination of size m  
// in an array of size n 
using System;
using System.Linq;

namespace Monowallet.Core.Test
{
    public class Util
    {

        /* 
        inputData[] ---> Input Array 
        result[] ---> Temporary array to  
                    store current combination 
        start & end ---> Staring and Ending  
                         indexes in inputData[] 
        index ---> Current index in result[] 
        m ---> Size of a combination 
               to be printed 
        */
        /// <summary>
        /// Done with element Include/Exclude
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputData"></param>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <param name="index"></param>
        /// <param name="result"></param>
        /// <param name="i"></param>
        public static void CombinationUtil2<T>(T[] inputData, int m, int n, int index, T[] result, int i = 0)
        {
            // Current combination is ready 
            // to be printed, print it 
            if (index == m)
            {
                for (int j = 0; j < m; j++)
                    Console.Write(result[j] + " ");
                Console.WriteLine("");
                return;
            }

            // When no more elements are  
            // there to put in result[] 
            if (i >= n)
                return;

            // current is included, put 
            // next at next location 
            result[index] = inputData[i];
            CombinationUtil2(inputData, m, n,
                index + 1, result, i + 1);

            // current is excluded, replace 
            // it with next (Note that 
            // i+1 is passed, but index  
            // is not changed) 
            CombinationUtil2(inputData, m, n, index, result, i + 1);
        }


        /* 
        inputData[] ---> Input Array 
        result[] ---> Temporary array to store current combination 
        start & end ---> Staring and Ending indexes in inputData[] 
        index ---> Current index in result[] 
        m ---> Size of a combination to be printed 
                */
        private static void CombinationUtil<T>(T[] inputData, T[] result, int start, int end, int index, int m, Action<T[]> singleResultAction)
        {
            // Current combination is  
            // ready to be printed,  
            // print it 
            if (index == m)
            {
                singleResultAction?.Invoke(result);
                return;
            }

            // replace index with all 
            // possible elements. The  
            // condition "end-i+1 >=  
            // m-index" makes sure that  
            // including one element 
            // at index will make a  
            // combination with remaining  
            // elements at remaining positions 
            for (int i = start; i <= end && end - i + 1 >= m - index; i++)
            {
                result[index] = inputData[i];
                CombinationUtil(inputData, result, i + 1, end, index + 1, m, singleResultAction);
            }
        }

        /// <summary>
        /// The main function that prints all combinations of size m in inputData[] of size n. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputData"></param>
        /// <param name="n"></param>
        /// <param name="m"></param>
        public static void Combination<T>(T[] inputData, int n, int m, Action<T[]> singleResultAction)
        {
            inputData = inputData.Distinct().OrderBy(t => t).ToArray();

            // A temporary array to store all combination one by one 
            T[] result = new T[m];

            // Compute all combination using temprary array 'result[]' 
            CombinationUtil(inputData, result, 0, n - 1, 0, m, singleResultAction);
        }

    }

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