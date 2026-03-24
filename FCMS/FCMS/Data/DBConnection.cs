using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WiseX.Data
{
    public class DBConnection
    {
        //public static string CommonConnectionString = "Server=MEDI-SQL02.medicountdom.com;Database=MC_FCMS_Transaction;User ID=WebUser;Password=$t3v3$uck$@$$";
        public static string ConnectionString = "Server=MEDI-SQL02.medicountdom.com;Database=MC_FCMS_Transaction;User ID=WebUser;Password=$t3v3$uck$@$$";

        //Dev
        //public static string ConnectionString = "Server=WTSRND01\\WTSRND01;Database=MC_CCMS_Transaction_Live;Trusted_Connection=True;MultipleActiveResultSets=true";

        //Testing
        //public static string ConnectionString = "Server=WSSPRD001\\WSSDEV01;Database=MC_CCMS_Transaction_Testing;user id =dbuser;password=S#Ab5pW$5d3u$Er#30)8!vQ1j;MultipleActiveResultSets=true";

        //Staging
        //public static string ConnectionString = "Server=WSSPRD001\\WSSDEV01;Database=MC_CCMS_Transaction_Staging;user id =dbuser;password=S#Ab5pW$5d3u$Er#30)8!vQ1j;MultipleActiveResultSets=true";

        //Live
        //public static string ConnectionString = "Server=MEDI-APP01\\SQLEXPRESS;Database=MC_CCMS_Transaction;user id =dbuser;password=Me6!C0unT#619;MultipleActiveResultSets=true";
    }
}
