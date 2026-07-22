using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace PasswordWallet.Models
{
    public class PasswordGenarator
    {

        public bool IsNumeric { get; set; } = false;
        public bool IsUpperCase { get; set; } = false;
        public bool IsLowerCase { get; set; } = false;

        public bool IsCharacter { get; set; } = false;


        public int Length { get; set; } = 0;


        public string GeneratePassword()
        {

            Random _random = new Random();
            string text = "";


            if (IsNumeric)
            {
                text += "0123456789";
            }
            if (IsLowerCase)
            {
                text += "abcdefghijklmnopqrstuvwxyz";
            }
            if (IsUpperCase)
            {
                text += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            }
            if (IsCharacter)
            {
                text += "!@#$%^&*()_+-=[]{}<>?";
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return "";
            }


            return new string(
           Enumerable.Repeat(text, Length)
                     .Select(s => s[_random.Next(s.Length)])
                     .ToArray()
       );


        }


    }
}