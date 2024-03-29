﻿using System.Collections.Generic;

namespace DAL_EF.Entity
{
    public class User
    {
        public int Id { get; set; }

        public string GoogleId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PictureUrl { get; set; }

        public UserSettings Settings { get; set; }

        public List<Category> Categories { get; set; }

        public List<Wallet> Wallets { get; set; }
    }
}
