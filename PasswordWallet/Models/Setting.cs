using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarfBuzzSharp;

namespace PasswordWallet.Models
{
    public class Security
    {
        
        
        public bool IsCompleteTime{get; set;}=false;
        public DateTime SessionStartTime{get; set;}=DateTime.Now;

        

        public Security()
        {
            
        }

        public void StartSession()
        {
            int seconds=60*10;
            while(seconds>0)
            {
                
                System.Threading.Thread.Sleep(1000);
                seconds--;
            }
            this.IsCompleteTime=true;

        }

    }
}