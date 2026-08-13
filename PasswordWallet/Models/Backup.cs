using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordWallet.Models
{
    public abstract class Backup
    {
        public string BackupName {get; set;}="";
        public DateTime BackupDate {get; set;}=DateTime.Now;

        public  string BackupType {get; set;}="";

        public Backup(string backupName,DateTime backupDate)
        {
            BackupName=backupName;
            BackupDate=backupDate;
            BackupType="Default";
        }

        public Backup()
        {
            Console.WriteLine("Backup Created");
        }   
    }
}