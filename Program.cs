using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                string sUserAccount;

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("Entrer le compte à tester : ");
                Console.ForegroundColor = ConsoleColor.White;
                sUserAccount = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                //Console.WriteLine("Recherche infos sur le compte donné");
                ADMethodsAccountManagement ADMethods = new ADMethodsAccountManagement();
                UserPrincipal myUser = ADMethods.GetUser(sUserAccount);
               

              
                Console.Write("LastLogon : ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(((DateTime)myUser.LastLogon).ToLocalTime());
                
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("LastBadPasswordAttempt : ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(((DateTime)myUser.LastBadPasswordAttempt).ToLocalTime());

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("AccountLockoutTime : ");
                Console.ForegroundColor = ConsoleColor.White;
                if (myUser.AccountLockoutTime.HasValue)
                {
                    Console.WriteLine(((DateTime)myUser.AccountLockoutTime).ToLocalTime());
                }
                else
                { 
                    Console.WriteLine(""); 
                }
                //Console.WriteLine(((DateTime)myUser.AccountLockoutTime).ToLocalTime());
                //Console.WriteLine(myUser.AccountLockoutTime);

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("Enabled : ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(myUser.Enabled);

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                if (myUser.AccountExpirationDate.HasValue)
                {
                    Console.Write("AccountExpirationDate : ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(((DateTime)myUser.AccountExpirationDate).ToLocalTime());

                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                }

                if (myUser.LastPasswordSet.HasValue)
                {
                    DateTime lastChanged = ((DateTime)myUser.LastPasswordSet).ToLocalTime();
                    Console.Write("LastPasswordSet : ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(lastChanged);
                    long maxDays = ADMethods.GetMaxPasswordAge();
                    long daysLeft = 0;
                    DateTime deadlineChtPassword = lastChanged.AddDays(maxDays);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Mot de passe à changer avant le : " + deadlineChtPassword);
                    daysLeft = deadlineChtPassword.Subtract(System.DateTime.Today).Days;

                    Console.Write("Jours restant avant changement de mot de passe : ");
                    if (daysLeft < 20)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    Console.WriteLine(daysLeft);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("User must change password");
                }
                

            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("--- Appuyer sur une touche pour quitter ---");
            Console.ReadLine();
        }
    }
}
