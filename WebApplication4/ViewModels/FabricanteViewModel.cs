using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace WebApplication4.Models
{
    public partial class FabricanteViewModel
    {
        public Fabricante? Fabricante { get; set; }
        public IFormFile? FicheiroImagem { get; set; }
        public bool RemoverImagem { get; set; }

    }
}
