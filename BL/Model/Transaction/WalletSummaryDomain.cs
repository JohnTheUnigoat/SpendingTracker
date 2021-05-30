using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Model.Transaction
{
    public class WalletSummaryDomain
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }
    }
}
