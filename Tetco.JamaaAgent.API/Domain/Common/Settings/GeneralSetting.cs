using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Common.Settings
{
    public sealed class GeneralSetting
    {
        //[Required(ErrorMessage = "connectionString is required")]
        //public string ConnectionString { get; set; }
        //[Required(ErrorMessage = "data provider type is required")]
        //public string DataProviderTypeStr { get; set; }
        //public SupportedProviders DataProviderType => SupportedProviders.GetByKey(DataProviderTypeStr);
        public int StudentProviderId { get; set; }
        public string ConnectionStr { get; set; }
        public StudentConnection StudentConnection { get; set; }
        public EmployeeConnection EmployeeConnection { get; set; }
        public int TimeOut { get; set; }
        
        public string InstituteCode { get; set; }
        public string AgentVersion { get; set; }
    }

    public class StudentConnection
    {
        public string SQLConnectionStr { get; set; }
        public string MySQLConnectionStr { get; set; }
        public string ORACLEConnectionStr { get; set; }
    }

    public class EmployeeConnection
    {
        public string SQLConnectionStr { get; set; }
        public string MySQLConnectionStr { get; set; }
        public string ORACLEConnectionStr { get; set; }
    }

}
