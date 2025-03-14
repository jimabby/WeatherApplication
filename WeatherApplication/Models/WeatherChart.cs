using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication.Models
{
    class WeatherChart
    {
        [Key]
        public long Id { get; set; }
        public string City { get; set; }
        public DateTime Date { get; set; }

        [Column(TypeName ="BLOB")]
        public byte[] ChartImage { get; set; }
    }
}
