using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImportToDatabase.Models
{
    [Table("Student")]
    public class Student : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string? StudentCode { get; set; }
        public bool Status { get; set; }
        [Required]
        [StringLength(100)]
        public int? SchoolYearId { get; set; } //// Foreign key
        public SchoolYear SchoolYear { get; set; }
        public List<Score> Scores { get; set; }

        public Student(string? studentCode, int? schoolYearId, bool status)
        {
            StudentCode = studentCode;
            SchoolYearId = schoolYearId;
            Status = status;
        }
    }
}
