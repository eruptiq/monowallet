// C# program to print all  
// combination of size m  
// in an array of size n 
using System;
using System.Linq;

namespace Monowallet.Core
{
    public class Util
    {
        private static void CombinationRecursive<T>(
            T[] inputData, T[] result, int start, int end, int index, int m, Action<T[]> combinationResultAction)
        {
            // Current combination is  
            // ready to be printed,  
            // print it 
            if (index == m)
            {
                combinationResultAction?.Invoke(result);
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
                CombinationRecursive(inputData, result, i + 1, end, index + 1, m, combinationResultAction);
            }
        }

        /// <summary>
        /// The main function that prints all combinations of size m in inputData[] of size n. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputData"></param>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <param name="combinationResultAction"></param>
        public static void Combination<T>(T[] inputData, int n, int m, Action<T[]> combinationResultAction)
        {
            inputData = inputData.Distinct().OrderBy(t => t).ToArray();

            // A temporary array to store all combination one by one 
            T[] result = new T[m];

            // Compute all combination using temprary array 'result[]' 
            CombinationRecursive(inputData, result, 0, n - 1, 0, m, combinationResultAction);
        }
    }
}

// This code is contributed by m_kit
// This code is contributed by ajit