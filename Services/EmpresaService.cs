using Lanches.Context;
using Microsoft.EntityFrameworkCore;
using Pedidos.Models;

namespace Pedidos.Services { 

    
        public class EmpresaService
        {
            private readonly AppDbContext _context;
            private readonly IConfiguration _configuration;

            public EmpresaService(AppDbContext context, IConfiguration configuration)
            {
                _context = context;
                _configuration = configuration;
            }

            public async Task<Empresa> GetDadosEmpresaAsync()
            {
                var empresa = await _context.Empresas.FirstOrDefaultAsync();

                if (empresa == null)
                {
                    empresa = new Empresa
                    {
                        Nome = _configuration["Empresa:Nome"],
                        LogoUrl = _configuration["Empresa:LogoUrl"],
                        Endereco = _configuration["Empresa:Endereco"],
                        Telefone = _configuration["Empresa:Telefone"],
                        Descricao = _configuration["Empresa:Descricao"],
                        Email = _configuration["Empresa:Email"]
                    };

                    _context.Empresas.Add(empresa);
                    await _context.SaveChangesAsync();
                }

                return empresa;
            }
        }
    }

