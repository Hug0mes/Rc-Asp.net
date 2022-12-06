using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ef_2.Models
{
   

        [Table("Fabricantes")]
        public class Fabricante1
        {
            [Key]
            public int Id { get; set; }

            [Required]
            [StringLength(50, MinimumLength = 3)]
            public string Nome { get; set; }

            [Required]
            [Display(Name = "site_web")]
            public string site_web { get; set; }

            [StringLength(255)]
            public string Imagem { get; set; }



        }
    
}
