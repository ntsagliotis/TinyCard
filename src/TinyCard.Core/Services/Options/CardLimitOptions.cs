using System;
using TinyCard.Core.Model;

namespace TinyCard.Core.Services.Options
{
    public class CardLimitOptions
    {
        //public int ID { get; set; }
        public string CardNumber { get; set; }
        public decimal AvailableBalance { get; set; }
        public CardTransactionType TrsansactionType { get; set; }
        public DateTime ReferenceDate { get; set; }
    }
}
