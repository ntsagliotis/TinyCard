using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyCard.Core.Model
{
    public class Card
    {
        public int ID { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string CardHolderSurname { get; set; }
        public DateTime CreatedDate { get; private set; }

        public Card()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
