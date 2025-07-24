using Lanches.Models;
using Lanches.Repositories.Interfaces;
using Lanches.ViewModels;
using Lanches.Models;
using Lanches.Repositories.Interfaces;
using Lanches.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Lanches.Controllers
{
    public class CarrinhoCompraController : Controller
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly CarrinhoCompra _carrinhoCompra;

        public CarrinhoCompraController(IProdutoRepository produtoRepository,
            CarrinhoCompra carrinhoCompra)
        {
            _produtoRepository = produtoRepository;
            _carrinhoCompra = carrinhoCompra;
        }

        public IActionResult Index()
        {
            var itens = _carrinhoCompra.GetCarrinhoCompraItens();
            _carrinhoCompra.CarrinhoCompraItems = itens;

            var carrinhoCompraVM = new CarrinhoCompraViewModel
            {
                CarrinhoCompra = _carrinhoCompra,
                CarrinhoCompraTotal = _carrinhoCompra.GetCarrinhoCompraTotal()
            };

            return View(carrinhoCompraVM);
        }

        [Authorize]
        public IActionResult AdicionarItemNoCarrinhoCompra(int produtoId)
        {
            var produtoSelecionado = _produtoRepository.Produtos
                                    .FirstOrDefault(p => p.ProdutoId == produtoId);

            if (produtoSelecionado != null)
            {
                _carrinhoCompra.AdicionarAoCarrinho(produtoSelecionado);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult RemoverItemDoCarrinhoCompra(int produtoId)
        {
            var produtoSelecionado = _produtoRepository.Produtos
                                    .FirstOrDefault(p => p.ProdutoId == produtoId);

            if (produtoSelecionado != null)
            {
                _carrinhoCompra.RemoverDoCarrinho(produtoSelecionado);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public IActionResult AtualizarQuantidade(int produtoId, int alteracao) // +1 ou -1
        {
            var produto = _produtoRepository.Produtos.FirstOrDefault(p => p.ProdutoId == produtoId);
            if (produto == null) return NotFound();

            // Atualiza o carrinho
            if (alteracao > 0)
            {
                _carrinhoCompra.AdicionarAoCarrinho(produto);
            }
            else
            {
                _carrinhoCompra.RemoverDoCarrinho(produto);
            }

            // Obtém os valores atualizados
            var item = _carrinhoCompra.GetCarrinhoCompraItens()
                .FirstOrDefault(i => i.Produto.ProdutoId == produtoId);

            // Correção: Verifica se item existe antes de formatar
            return Json(new
            {
                novaQuantidade = item?.Quantidade ?? 0,
                novoSubtotal = item != null ? (item.Quantidade * item.Produto.Preco).ToString("C") : "R$ 0,00",
                novoTotal = _carrinhoCompra.GetCarrinhoCompraTotal().ToString("C")
            });
        }



    }
}