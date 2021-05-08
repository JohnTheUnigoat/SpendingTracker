﻿using System;
using System.Collections.Generic;

namespace BL.Model.Wallet
{
    public class AddWalletDto
    {
        public string Name { get; set; }

        public IEnumerable<string> Categories { get; set; }

        public int UserId { get; set; }
    }
}
