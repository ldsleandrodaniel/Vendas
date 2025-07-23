using Lanches.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Pedidos.Models;
using Pedidos.Services;

namespace Pedidos.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminEmpresaController : Controller
    {

        

            private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public AdminEmpresaController(AppDbContext context, IWebHostEnvironment hostingEnvironment)
            {
                _context = context;
            _hostingEnvironment = hostingEnvironment;
               
            }

        [HttpGet]
        public IActionResult GetImagens()
        {
            try
            {
                var imagesPath = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                var files = Directory.GetFiles(imagesPath)
                                   .Where(f => f.EndsWith(".jpg") || f.EndsWith(".jpeg") ||
                                               f.EndsWith(".png") || f.EndsWith(".gif"))
                                   .Select(Path.GetFileName)
                                   .ToList();

                return Json(files);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }





        [HttpGet]
            public async Task<IActionResult> AlteraEmpresa()
            {
                var empresa = await _context.Empresas.FirstOrDefaultAsync(); // Pega a primeira (e única) empresa
           
            return View(empresa);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> AlteraEmpresa([Bind("EmpresaId,Nome,Endereco,Telefone,Email,LogoUrl,Descricao")] Empresa empresa)
            {
                if (ModelState.IsValid)
                {
                if (!string.IsNullOrEmpty(empresa.LogoUrl))
                {
                   
                    if (!empresa.LogoUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                        !empresa.LogoUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    {
                        
                        empresa.LogoUrl = "/images/" + empresa.LogoUrl.TrimStart('/');
                    }
                    
                }


                _context.Update(empresa);
                    await _context.SaveChangesAsync();
                    TempData["Sucesso"] = "Dados atualizados com sucesso!";
                    return View(empresa); // Retorna para a mesma view com os dados atualizados
                }
                return View(empresa);
            }
        }
    }

