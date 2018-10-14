using Monowallet.Core.Model;
using Monowallet.Core.Multisig;
using System;
using System.Collections.Generic;

namespace Monowallet.Core.Test.Helper
{
    internal class MultisigHelper
    {
        public static int GetExpectedKeyCombinationsCount(int m, int n)
        {
            var nFactorial = n.Factorial();
            var divider = m.Factorial() * (n - m).Factorial();
            int expectedEncryptedBoxesCount = (int)(nFactorial / divider);
            return expectedEncryptedBoxesCount;
        }

        public static MultisigComposer<string> GetNewMultisigComposer(int m, int n)
        {
            var accountInfos = new List<IAccountInfo<string>> { };

            for (int i = 0; i < n; i++)
            {
                accountInfos.Add(new EthereumAccountInfo(address: ((char)('A' + i)).ToString()));
            }
            var multisigComposer = new MultisigComposer<string>(accountInfos: accountInfos, m: m, n: n);
            return multisigComposer;
        }
    }
}
