using Lanches.Context;
using Lanches.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedidos.ViewModels;

namespace Pedidos.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminSenhaController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AdminSenhaController(AppDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
            {
                _context = context;
                _userManager = userManager;
                _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new AlterarSenhaViewModel());
        }


        [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> AlteraSenha(
                [Bind("SenhaAtual,NovaSenha,ConfirmacaoSenha")] AlterarSenhaViewModel model)
            {
                if (!ModelState.IsValid)
                    return RedirectToAction(nameof(Index));

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return RedirectToAction(nameof(Index));

                // Verifica senha atual
                if (!await _userManager.CheckPasswordAsync(user, model.SenhaAtual))
                {
                    TempData["Erro"] = "Senha atual incorreta";
                    return RedirectToAction(nameof(Index));
                }

                // Altera a senha
                var result = await _userManager.ChangePasswordAsync(
                    user,
                    model.SenhaAtual,
                    model.NovaSenha);

                if (result.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(user);
                    TempData["Sucesso"] = "Senha alterada com sucesso!";
                }
                else
                {
                    TempData["Erro"] = "Erro ao alterar senha";
                }

                return RedirectToAction(nameof(Index));
            }
        }

    }


