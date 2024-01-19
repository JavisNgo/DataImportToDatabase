using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImportToDatabase.Models
{
    [Table("Score")]
    public class Score : BaseEntity
    {
        public int StudentID { get; set; }
        public int SubjectID { get; set; }
        public double Point { get; set; }
        public Student Student { get; set; }
        public Subject Subject { get; set; }

        public Score( int studentID, int subjectID, double point)
        {
            StudentID = studentID;
            SubjectID = subjectID;
            Point = point;
        }
    }
}
