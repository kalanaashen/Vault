using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordWallet.Models
{
    public class PasswordEntry
    {
        public int Id { get; set; }

        public string Website { get; set; } = "";

        public string Username { get; set; } = "";

        public string Password { get; set; } = "";

        public bool IsFavorite { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsPasswordVisible { get; set; } = false;
        public String DisplayPassword
        {

            get
            {
                if (IsPasswordVisible)
                {
                    return Password;
                }
                else
                {
                    return "XXXXXX";
                }
            }
        }





    }
}