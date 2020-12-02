using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportsbookAggregationAPI.Data.Models
{
    public class UserSettings
    {
        [Key]
        public string UserId { get; set; }
        public string[] DefaultBooks{ get; set; }
    }
}
