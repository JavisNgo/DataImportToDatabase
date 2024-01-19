using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImportToDatabase.Models
{
    [Table("SchoolYear")]
    public class SchoolYear : BaseEntity
    {

        [Range(2017,2021, ErrorMessage = "Value must be 2017-2021")]
        public int ExamYear { get; set; }
        public bool Status { get; set; }
        public List<Student> Students { get; set; }
        public SchoolYear( int examYear, bool status) 
        {
            ExamYear = examYear;
            Status = status;
        }
    }
}
