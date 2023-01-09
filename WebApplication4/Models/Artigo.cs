using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebApplication4.Models
{

    /*

    CREATE TABLE [dbo].[Artigos] (
      [Id]       INT           IDENTITY (1, 1) NOT NULL,
      [Nome]     NVARCHAR (50) NOT NULL,
      [Preco]    INT           NOT NULL,
      [QtaStock] SMALLINT      NULL,
      PRIMARY KEY CLUSTERED ([Id] ASC)
    );

    */


    [Table("Artigos")]
    public class Artigo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Nome { get; set; }

        [Required]
        [Range(1, 1000)]
        [Display(Name = "Preço")]
        public int Preco { get; set; }

        [Display(Name = "Qta em Stock")]
        public Int16 QtaStock { get; set; }

        [Required]
        [ForeignKey("Fabricantes")]
        public int CategoriaId { get; set; }


        //https://docs.microsoft.com/en-us/ef/core/modeling/relationships
        //https://docs.microsoft.com/en-us/ef/core/querying/related-data
        public Categoria Categoria { get; set; }
        
        [NotMapped]
        public Fabricante Fabricante { get; set; }
        //public List<Categoria> Fabricantes { get; set; }


    }
}