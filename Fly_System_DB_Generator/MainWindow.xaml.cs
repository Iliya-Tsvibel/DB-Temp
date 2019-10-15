using Flights_Management_System;
using Flights_Management_System.DAO;
using Flights_Management_System.Facade;
using Fly_System_DB_Generator.StaticData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fly_System_DB_Generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<string> countryList;
        static public LoggedInAdministratorFacade adminFacade = new LoggedInAdministratorFacade();
        static public LoginToken<Administrator> defaultToken = new LoginToken<Administrator> { User = new Administrator { USER_NAME = FlightCenterConfig.ADMIN_USER_NAME, PASSWORD = FlightCenterConfig.ADMIN_PASSWORD } };
        static public LoggedInAirlineFacade airlineFacade = new LoggedInAirlineFacade();
        static public LoginToken<AirlineCompany> defaultAirlioneToken = new LoginToken<AirlineCompany> { User = new AirlineCompany { AIRLINE_NAME = FlightCenterConfigDB_Gen.AIRLINE_NAME, USER_NAME = FlightCenterConfigDB_Gen.AIRLINE_USER_NAME, PASSWORD = FlightCenterConfigDB_Gen.AIRLINE_PASSWORD, COUNTRY_CODE = adminFacade.GetCountryByName("Germany").ID } };
        static public LoginToken<Customer> defaultCustomerToken = new LoginToken<Customer> { User = new Customer { USER_NAME = FlightCenterConfigDB_Gen.CUSTOMER_USER_NAME, PASSWORD = FlightCenterConfigDB_Gen.CUSTOMER_PASSWORD } };
        static public LoggedInCustomerFacade customerFacade = new LoggedInCustomerFacade();
        static public LoginToken<Administrator> adminToken = new LoginToken<Administrator>();
        static public AnonymousUserFacade anonFacade = new AnonymousUserFacade();
        static public TicketDAOMSSQL ticketDAO = new TicketDAOMSSQL();
        static public FlightDAOMSSQL flightDAO = new FlightDAOMSSQL();
        static public CountryDAOMSSQL countryDAO = new CountryDAOMSSQL();
        static public AirlineDAOMSSQL airlineDAO = new AirlineDAOMSSQL();

        Random random = new Random();
       

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Creating new customer.
            Customer newCustomer = new Customer
            {
                FIRST_NAME = "Iliya",
                LAST_NAME = "Tsvibel",
                USER_NAME = "Iliya",
                PASSWORD = "1234",
                ADDRESS = "Rishon",
                PHONE_NO = "0546800559",
                CREDIT_CARD_NUMBER = "12345"
            };

            adminFacade.CreateNewCustomer(defaultToken, newCustomer);

            // Creating new country.
            Country country = null;
            country = new Country();
            for (int i = 0; i < ListOfCountries.CountryNames.Length; i++)
            {

                country = countryDAO.GetByName(ListOfCountries.CountryNames[i]);

                if (country == null)
                {

                    Country newCountry = new Country { COUNTRY_NAME = ListOfCountries.CountryNames[i] };
                    adminFacade.CreateNewCountry(defaultToken, newCountry);
                }

            }

            // Creating new administrator.
            Administrator newAdmin = new Administrator { FIRST_NAME = "Iliya", LAST_NAME = "Tsvibel", USER_NAME = "Admin", PASSWORD = "1234" };
            adminFacade.CreateNewAdmin(defaultToken, newAdmin);

            //Creating new airline.
            //AirlineCompany newAirline = new AirlineCompany
            //{
            //    AIRLINE_NAME = "Aeroflot",
            //    USER_NAME = "Vladimir",
            //    COUNTRY_CODE = adminFacade.GetCountryByName("Germany").ID,
            //    PASSWORD = "12345"
            //};
            //adminFacade.CreateNewAirline(defaultToken, newAirline);

            AirlineCompany airline = null;
            airline = new AirlineCompany();
            long countryStartID = 0;
            countryStartID = countryDAO.GetByName(ListOfCountries.CountryNames[0]).ID;
            for (int i = 0; i < ListOfAirlinesCompanies.AirlineNames.Length; i++)
            {

                airline = airlineDAO.GetAirlineByName(ListOfAirlinesCompanies.AirlineNames[i]);

                if (airline == null)
                {
                    AirlineCompany newAirline = new AirlineCompany { AIRLINE_NAME = ListOfAirlinesCompanies.AirlineNames[i], USER_NAME = "UserName-" + ListOfAirlinesCompanies.AirlineNames[i], PASSWORD = random.Next(1000, 10000).ToString(), COUNTRY_CODE = random.Next(Convert.ToInt32(countryStartID), (Convert.ToInt32(countryStartID) + ListOfCountries.CountryNames.Length)) };
                    adminFacade.CreateNewAirline(defaultToken, newAirline);
                }

            }

            // Creating new flight.
            Flight flight = new Flight { AIRLINECOMPANY_ID = adminFacade.GetAirlineByName(adminToken, "SOLAR CARGO, C.A.").ID, DEPARTURE_TIME = DateTime.Now, LANDING_TIME = DateTime.Now + TimeSpan.FromHours(1), ORIGIN_COUNTRY_CODE = adminFacade.GetCountryByName("Germany").ID, DESTINATION_COUNTRY_CODE = adminFacade.GetCountryByName("Germany").ID, REMAINING_TICKETS = 250 };
            //airlineFacade.CreateFlight(defaultAirlioneToken, flight);
            flightDAO.Add(flight);

            // Creating new ticket.
            Ticket tickets = new Ticket { FLIGHT_ID = anonFacade.GetFlightsByDestinationCountryForTest(adminFacade.GetCountryByName("Germany").ID).ID, CUSTOMER_ID = adminFacade.GetCustomerByUserName(adminToken, "Iliya").ID };
            //customerFacade.PurchaseTicket(defaultCustomerToken, tickets);
            ticketDAO.Add(tickets);
            MessageBox.Show($"All data successfully added");
        }


        private void Edit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Clean_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection sqlConnection = new SqlConnection(FlightCenterConfigDB_Gen.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("CLEAR_DB", sqlConnection))
                {
                    cmd.Connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }

            }
            MessageBox.Show($"All data successfully deleted");
        }
    }
}
