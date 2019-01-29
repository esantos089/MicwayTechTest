using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MicwayTechTest.Models
{
    /// <summary>
    /// Driver Class
    /// </summary>
    public class Driver
    {
        [Key]
        public int Id { get; set; }
        [Required] [StringLength(50)]
        public string FirstName { get; set; }
        [Required] [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required] [StringLength(100)]
        public string Email { get; set; }
    }
}