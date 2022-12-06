using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;


namespace ef_2.Models
{
    public class LojaContext :  DbContext
    {
        public LojaContext(DbContextOptions<LojaContext> options)
                  : base(options)
        { }
        public DbSet<Artigo> Artigos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
    }
}
