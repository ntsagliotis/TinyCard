using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyCard.Core.Model
{
    public class Limit
    {
        public int ID { get; set; }
        public string CardNumber { get; set; }
        public decimal AvailableBalance { get; set; }
        public CardTransactionType TrsansactionType { get; set; }
        public DateTime ReferenceDate { get; set; }
        public DateTime CreatedDate { get; private set; }

        public Limit()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
