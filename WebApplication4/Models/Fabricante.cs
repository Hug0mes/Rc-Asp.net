using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebApplication4.Models
{


    [Table("Fabricantes")]
    public class Fabricante
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
        [NotMapped]
        public object Artigos { get; internal set; }
    }
}