using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeleteAuditDataDynamicsCrm
{
    class Program
    {
        static string _crmConnectionString;
        static IOrganizationService _orgService;
        static DateTime _auditEndDate;

        static void Main(string[] args)
        {
            LoadConfig();
            ConnectToDynamics();
            Console.WriteLine("==========================================");
            Console.WriteLine("Delete Audit Data - Dynamics CRM");
            Console.WriteLine("==========================================");
            Console.WriteLine();
            Console.WriteLine($"Retrieving audit data until {_auditEndDate.ToString("G")}");
            Console.WriteLine("Do you want to continue? (y/n)");
            Console.Write("R: ");
            var answer = Console.ReadLine();

            switch (answer)
            {
                case "y": DeleteAuditData(); break;
                default: ExitConsole(); break;
            }
        }

        static void LoadConfig()
        {
            var cs = ConfigurationManager.ConnectionStrings["DynamicsCRM"];
            _crmConnectionString = cs.ConnectionString;

            var dt = ConfigurationManager.AppSettings["AuditEndData"];
            _auditEndDate = DateTime.Parse(dt);
        }

        static void DeleteAuditData()
        {
            Console.WriteLine("==========================================");
            Console.WriteLine($"Deleting audit data deletion until {_auditEndDate.ToString("G")}");

            try
            {
                var deleteRequest = new DeleteAuditDataRequest();
                deleteRequest.EndDate = _auditEndDate;

                _orgService.Execute(deleteRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void ExitConsole()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void ConnectToDynamics()
        {
            CrmConnection connection = CrmConnection.Parse(_crmConnectionString);

            _orgService = new OrganizationService(connection);
        }
    }
}
