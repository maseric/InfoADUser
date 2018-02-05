using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;

namespace ConsoleApplication1
{
    class ADMethodsAccountManagement
    {
        private string sDomain = "siae.intradef.gouv.fr";
        
        private string sDefaultOU = "DC=your,DC=ldap,DC=ou";
        private string sDefaultRootOU = "DC=your,DC=ldap,DC=root,DC=ou";
        private string sLDAPUrl = "LDAP://your.ldap.com";
        private string sServiceUser = @"youruser";
        private string sServicePassword = "yourpassword";
        
        public bool IsUserExpired(string sUserName)
        {
            UserPrincipal oUserPrincipal = GetUser(sUserName);
            if (oUserPrincipal.AccountExpirationDate != null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }


        public UserPrincipal GetUser(string sUserName)
        {
            PrincipalContext oPrincipalContext = GetPrincipalContext();
            UserPrincipal oUserPrincipal = UserPrincipal.FindByIdentity(oPrincipalContext, sUserName);
            return oUserPrincipal;
        }

        // Gets the principal context
        public PrincipalContext GetPrincipalContext()
        {
            PrincipalContext oPrincipalContext = new PrincipalContext(ContextType.Domain, sDomain, sDefaultRootOU, ContextOptions.Negotiate, sServiceUser, sServicePassword);

            return oPrincipalContext;
        }

        //Get the password expiration max age
        public long GetMaxPasswordAge()
        {
           
                DirectoryEntry de = new DirectoryEntry(sLDAPUrl, sServiceUser, sServicePassword);
                DirectorySearcher mySearcher = new DirectorySearcher(de);
                SearchResultCollection results;
                string filter = "maxPwdAge=*";
                mySearcher.Filter = filter;

                results = mySearcher.FindAll();
                long maxDays = 0;
                if(results.Count>=1)
                {
                    Int64 maxPwdAge = (Int64)results[0].Properties["maxPwdAge"][0];
                    maxDays = maxPwdAge / -864000000000;
                   // Console.WriteLine(maxDays);
                }

            return maxDays;

        }

    }

}
