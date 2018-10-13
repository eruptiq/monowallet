using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monowallet.Core.Test
{
    [TestFixture]
    public class MultisigComposerTest
    {
        [TestCase(2, 3)]
        [TestCase(3, 4)]
        [TestCase(2, 4)]
        [TestCase(4, 5)]
        [TestCase(2, 5)]
        [TestCase(3, 5)]
        [TestCase(3, 10)]
        [TestCase(4, 15)]
        [TestCase(4, 20)]
        public async Task MultisigKeyCombination(int m, int n)
        {
            //Arrange
            var nFactorial = n.Factorial();
            var divider = (m.Factorial() * (n - m).Factorial());
            int expectedEncryptedBoxesCount = (int)(nFactorial / divider);
            var accountInfos = new List<IAccountInfo> { };

            for (int i = 0; i < n; i++)
            {
                accountInfos.Add(new EthereumAccountInfo(address: ((char)('A' + i)).ToString()));
            }
            var multisigComposer = new MultisigComposer(accountInfos: accountInfos, m: m, n: n);


            //Act
            var boxes = await multisigComposer.GetKeyBoxesAsync(t =>
            {
                for (int j = 0; j < m; j++)
                    Console.Write(t[j] + " ");
                Console.WriteLine("");
            });

            var boxesValues = boxes.Select(b => b.ToString());
            var boxesAsString = string.Join("; ", boxesValues);


            //Assert
            Assert.AreEqual(expectedEncryptedBoxesCount, boxes.Count());
            Assert.IsTrue(boxesValues.All(v => v.Length == m), "some values have wrong lenght");
            Assert.DoesNotThrow(() => new HashSet<string>(boxesValues));
            Assert.Pass("passed: {0}", boxesAsString);
        }
    }
}
