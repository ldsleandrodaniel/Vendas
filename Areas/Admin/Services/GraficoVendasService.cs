using Lanches.Context;
using Lanches.Models;
using Microsoft.EntityFrameworkCore;

namespace Lanches.Areas.Admin.Services
{
    public class GraficoVendasService
    {
        private readonly AppDbContext context;

        public GraficoVendasService(AppDbContext context)
        {
            this.context = context;
        }

        public List<ProdutoGrafico> GetVendasProdutos(int dias = 360)
        {
            var data = DateTime.Now.AddDays(-dias).ToUniversalTime();

            var produtos = context.PedidoDetalhes
                .Where(pd => pd.Pedido.PedidoEnviado >= data)
                .Join(context.Produtos,
                    pd => pd.ProdutoId,
                    l => l.ProdutoId,
                    (pd, l) => new { pd, l })
                .GroupBy(x => new { x.l.ProdutoId, x.l.Nome })
                .Select(g => new ProdutoGrafico
                {
                    ProdutoNome = g.Key.Nome,
                    ProdutosQuantidade = g.Sum(x => x.pd.Quantidade),
                    ProdutosValorTotal = g.Sum(x => x.pd.Preco * x.pd.Quantidade)
                })
                .ToList();

            return produtos;
        }
    }
}
