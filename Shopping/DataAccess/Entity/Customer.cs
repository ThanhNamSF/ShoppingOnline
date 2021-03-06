﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(32)]
        [Index(IsUnique = true)]
        public string UserName { get; set; }

        [Required]
        [StringLength(16)]
        public string Password { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public string Address { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(12)]
        public string Phone { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}
