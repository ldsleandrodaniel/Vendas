﻿using Lanches.Context;
using Lanches.Models;
using Lanches.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace Lanches.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminPedidosController : Controller
    {
        private readonly AppDbContext _context;

        public AdminPedidosController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult PedidoProdutos(int? id)
        {
            var pedido = _context.Pedidos
                        .Include(pd=> pd.PedidoItens)
                        .ThenInclude(l=> l.Produto)
                        .FirstOrDefault(p=> p.PedidoId == id);
            if (pedido == null)
            {
                Response.StatusCode = 404;
                return View("PedidoNotFound", id.Value);
            }
            PedidoProdutoViewModel pedidoProdutos = new PedidoProdutoViewModel()
            {
                Pedido = pedido,
                PedidoDetalhes = pedido.PedidoItens
            };
            return View(pedidoProdutos);


        }





        
        public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "Nome")
        {
            var resultado = _context.Pedidos.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(filter))
            {
                resultado = resultado.Where(p => p.Nome.Contains(filter.ToLower()));
            }
            var model = await PagingList.CreateAsync(resultado, 5, pageindex, sort, "Nome");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };
            return View(model);


        }

       
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .FirstOrDefaultAsync(m => m.PedidoId == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

       
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PedidoId,Nome,Sobrenome,Endereco1,Endereco2,Cep,Estado,Cidade,Telefone,Email,PedidoEnviado,PedidoEntregueEm")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pedido);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            return View(pedido);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PedidoId,Nome,Sobrenome,Endereco1,Endereco2,Cep,Estado,Cidade,Telefone,Email,PedidoEnviado,PedidoEntregueEm")] Pedido pedido)
        {
            if (id != pedido.PedidoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.PedidoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pedido);
        }

      
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .FirstOrDefaultAsync(m => m.PedidoId == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Admin/AdminPedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.PedidoId == id);
        }

        public async Task<IActionResult> PedidosNaoEntregues()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.PedidoItens)
                .Where(p => p.PedidoEntregueEm == null) // Filtra apenas pedidos não entregues
                .OrderBy(p => p.PedidoId)
                .ToListAsync();

            return View(pedidos);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarcarComoEntregue(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }

            // Só atualiza se ainda não foi marcado como entregue
            if (pedido.PedidoEntregueEm == null)
            {
                pedido.PedidoEntregueEm = DateTime.Now;
                _context.Update(pedido);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(PedidosNaoEntregues));
        }


    }
}
