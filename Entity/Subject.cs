using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImportToDatabase.Models
{
    [Table("Subject")]
    public class Subject : BaseEntity
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public List<Score> Scores { get; set; }

        public Subject( string? code, string? name)
        {
            Code = code;
            Name = name;
        }
    }
}
