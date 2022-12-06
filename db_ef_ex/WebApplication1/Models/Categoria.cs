using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ef_2.Models
{ 

    /*
     
    CREATE TABLE [dbo].[Categoria] (
      [Id]       INT           IDENTITY (1, 1) NOT NULL,
      [Nome]     NVARCHAR (50) NOT NULL,
      [Imagem]
      PRIMARY KEY CLUSTERED ([Id] ASC)
    );

    */


    [Table("Categorias")]
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Nome { get; set; }

        //http://www.macoratti.net/18/11/aspn_upload1.htm
        [StringLength(255)]
        public string Imagem { get; set; }

        //https://docs.microsoft.com/en-us/ef/core/modeling/relationships
        public List<Artigo> Artigos { get; set; }


    }
}
