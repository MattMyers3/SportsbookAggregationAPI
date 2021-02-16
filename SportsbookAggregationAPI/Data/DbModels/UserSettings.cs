using System.ComponentModel.DataAnnotations;

namespace SportsbookAggregationAPI.Data.DbModels
{
    public class UserSettings
    {
        [Key]
        public string UserId { get; set; }
        public string[] DefaultBooks{ get; set; }
    }
}
