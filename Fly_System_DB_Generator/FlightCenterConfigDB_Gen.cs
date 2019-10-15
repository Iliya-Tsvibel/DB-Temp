using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly_System_DB_Generator
{
    class FlightCenterConfigDB_Gen
    {
        public const string AIRLINE_NAME = "AEROFLOT";
        public const string AIRLINE_USER_NAME = "AIRLINE";
        public const string AIRLINE_PASSWORD = "1234";
        public const string CUSTOMER_USER_NAME = "CUSTOMER";
        public const string CUSTOMER_PASSWORD = "1234";
        public static string connectionString = /*("Server=tcp:iliyadb.database.windows.net,1433;Initial Catalog=Flight_System;Persist Security Info=False;User ID=iliyadb;Password=!_Tsvibel;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30");*/
        @"Data Source=.; Initial Catalog=Flights Management System; Integrated Security=True";
    }
}
