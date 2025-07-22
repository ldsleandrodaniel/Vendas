using Lanches.Context;
using Lanches.Models;
using Lanches.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Pedidos.Services;

namespace Lanches.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;
        private readonly ImagemService _imagemService;

        public ProdutoRepository(AppDbContext context, ImagemService imagemService)
        {
            _context = context;
            _imagemService = imagemService;
        }

        public IEnumerable<Produto> Produtos => _context.Produtos
       .Include(c => c.Categoria)
       .AsEnumerable()
       .Select(p => {
           p.ImagemUrl = _imagemService.ObterCaminhoImagem(p.ImagemUrl);
           return p;
       });

        public IEnumerable<Produto> ProdutosPreferidos => _context.Produtos
            .Where(p => p.IsProdutoPreferido)
            .Include(c => c.Categoria)
            .AsEnumerable()
            .Select(p => {
                p.ImagemUrl = _imagemService.ObterCaminhoImagem(p.ImagemUrl);
                return p;
            });

        public Produto GetProdutoById(int produtoId)
        {
            var produto = _context.Produtos
                .Include(p => p.Categoria)
                .FirstOrDefault(p => p.ProdutoId == produtoId);

            if (produto != null)
            {
                produto.ImagemUrl = _imagemService.ObterCaminhoImagem(produto.ImagemUrl);
            }

            return produto;
        }

    }
}
