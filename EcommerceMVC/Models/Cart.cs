using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EcommerceMVC.Models
{
    public class Cart
    {
        public Produtos Produto { get; set; }
        public int ProdutoQuantidade { get; set; }
        [NotMapped]
        public decimal PrecoTotal { get; set; }
    }
}