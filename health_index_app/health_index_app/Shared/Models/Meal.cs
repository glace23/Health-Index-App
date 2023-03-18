﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace health_index_app.Shared.Models
{
    public class Meal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(3, 1)")]
        [Range(0, 10, ErrorMessage = "Score must be between 0 and 10")]
        public double HealthIndex { get; set; } = -1;

    }
}
