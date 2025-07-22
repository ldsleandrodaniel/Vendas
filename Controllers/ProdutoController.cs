using Lanches.Models;
using Lanches.Repositories.Interfaces;
using Lanches.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lanches.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public IActionResult List(string categoria)
        {

            
            IEnumerable<Produto> produtos = Enumerable.Empty<Produto>(); ;
            string categoriaAtual = string.Empty;

            if (string.IsNullOrEmpty(categoria))
            {
                produtos = _produtoRepository.Produtos.OrderBy(l => l.ProdutoId);
                categoriaAtual = "Todos os Produtos";
            }
            else
            {
               
                    produtos = _produtoRepository.Produtos
                        .Where(l=> l.Categoria.CategoriaNome.Equals(categoria))
                        .OrderBy(l => l.Nome);
                    categoriaAtual = categoria;
              
            }

            var produtosListViewModel = new ProdutoListViewModel
            {
                Produtos = produtos,
                CategoriaAtual = categoriaAtual
            };


            return View(produtosListViewModel);

        }

        public IActionResult Details(int produtoId)
        {
            var produto = _produtoRepository.Produtos.FirstOrDefault(l => l.ProdutoId == produtoId);
            if (produto == null)
            {
                return NotFound(); // Ou redirecione para uma página de erro
            }
            return View(produto);
            

        }

        public ViewResult Search(string searchString)
        {
            IEnumerable<Produto> produtos;
            string categoriaAtual = string.Empty;
             if (string.IsNullOrEmpty(searchString))
            {
                produtos = _produtoRepository.Produtos.OrderBy(p=> p.ProdutoId);
                categoriaAtual = "Todos os Produtos";
            }
             else
            {
                produtos = _produtoRepository.Produtos.Where(p => p.Nome.ToLower()
                        .Contains(searchString.ToLower()));
                if (produtos.Any())
                {
                    categoriaAtual = "Produtos";
                }
                else
                {
                    categoriaAtual = "Nenhum Produto foi encontrado";
                    
                }
            }

            return View("~/Views/Produto/List.cshtml", new ProdutoListViewModel
            {
                Produtos = produtos,
                CategoriaAtual = categoriaAtual
            });
        }

    }
}
