using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.MobileServices;

namespace Audity.Models
{
    [JsonObject("receipt")]
    class Receipt
    {
        [JsonProperty("titulo")]
        public string titulo { get; set; }
        [JsonProperty("costo")]
        public double costo { get; set; }

        [UpdatedAt]
        public DateTimeOffset UpdatedAt { get; set; }

        
        public string Id { get; set; }

        [Version]
        public string version { get; set; }
    }
}
