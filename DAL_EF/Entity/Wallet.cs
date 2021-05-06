using System.Collections.Generic;

namespace DAL_EF.Entity
{
    public class Wallet
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int UserId { get; set; }

        public ICollection<Category> Categories { get; set; }
    }
}
