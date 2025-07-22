using Lanches.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Pedidos.Models;

namespace Lanches.Context
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Produto> Produtos { get; set; }

        public DbSet<CarrinhoCompraItem> CarrinhoCompraItens { get; set; }

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoDetalhe> PedidoDetalhes  { get; set; }

        public DbSet<Empresa> empresas { get; set; }

      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // APENAS ESTA LINHA RESOLVE O PROBLEMA DA CHAVE PRIMÁRIA
            modelBuilder.Entity<CarrinhoCompraItem>()
                .Property(e => e.CarrinhoCompraItemId)
                .UseIdentityAlwaysColumn();
            modelBuilder.Entity<Categoria>()
                .Property(c => c.CategoriaId)
                .UseIdentityAlwaysColumn();
            modelBuilder.Entity<Produto>()
                 .Property(l => l.ProdutoId)
                 .UseIdentityAlwaysColumn();

            modelBuilder.Entity<Pedido>()
                .Property(p => p.PedidoId)
                .UseIdentityAlwaysColumn();

            modelBuilder.Entity<PedidoDetalhe>()
                .Property(pd => pd.PedidoDetalheId)
                .UseIdentityAlwaysColumn();

            modelBuilder.Entity<Empresa>().HasData(
               new Empresa
               {
                   EmpresaId = 1,
                   Nome = "Empresa",
                   Endereco = "Endereco",
                   Telefone = "55........",
                   Email = "email@email.com",
                   LogoUrl = "https://neigrando.com/wp-content/uploads/2021/12/empresa-da-nova-economia.jpg"
               }
           );



        }
    }
}
