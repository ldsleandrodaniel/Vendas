﻿using System.ComponentModel.DataAnnotations;

namespace Lanches.Models
{
    public class CarrinhoCompraItem
    {
        public int CarrinhoCompraItemId {  get; set; }

        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public int Quantidade { get; set; }
        [StringLength(200)]
        public string CarrinhoCompraId{ get; set; }

    }
}
