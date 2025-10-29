using System;
using System.ComponentModel.DataAnnotations;

namespace CMCS.Models
{
    public class Lecturer
    {
        [Key]
        public int LecturerID { get; set; }
        public int? UserID { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }

        // Navigation property
        public User? User { get; set; }
    }
}