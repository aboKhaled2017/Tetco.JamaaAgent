using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Common.Settings
{
    public sealed class GeneralSetting
    {
        [Required(ErrorMessage = "connectionString is required")]
        public string ConnectionString { get; set; }
        [Required(ErrorMessage = "data provider type is required")]
        public string DataProviderTypeStr { get; set; }
        public SupportedProviders DataProviderType => SupportedProviders.GetByKey(DataProviderTypeStr);
    }
}
