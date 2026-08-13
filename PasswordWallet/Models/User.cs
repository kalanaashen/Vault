using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordWallet.Models
{
    public class User
    {
        public int Id {get; set;}=0;
        public string Username{get; set;}="";
        public string Password {get; set;}="";


        public User(string username,string password)
        {

            Username=username;
            Password=password;
        }
        public User()
        {
            
        }
    }
}