﻿using System;
using System.Collections.Generic;
using System.Text;
namespace Monowallet.Core.Model
{
    public class AccountInfo
    {
        public AccountInfo()
        {
            this.Eth = new EthAccountToken();
            this.AccountTokens = new List<AccountToken>();
        }
        public string Address { get; set;}
        public AccountToken Eth { get; set; }
        public List<AccountToken> AccountTokens { get; set; }

    }
}
