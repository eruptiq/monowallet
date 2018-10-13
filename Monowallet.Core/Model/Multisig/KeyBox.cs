using System.Security;

namespace Monowallet.Core.Model.Multisig
{
    public class KeyBox
    {
        public OrderedToken<char> Key { get; set; }

        public KeyBox()
        {
        }

        public override string ToString()
        {
            return Key.ToString();
        }

        //public SecureString 
    }
}
