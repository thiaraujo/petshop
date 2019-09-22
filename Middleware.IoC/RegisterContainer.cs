using Data.Entities.Models;
using Domain.Interfaces;
using Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Middleware.IoC
{
    public class RegisterContainer
    {
        public static void RegisterDependencies(IServiceCollection services)
        {
            //Base
            services.AddScoped<PetshopContext>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            // Usuario-operador-veterinario
            services.AddScoped<IUsuario, UsuarioService>();
            services.AddScoped<IUsuarioEspecialidade, UsuarioEspecialidadeService>();

            // Cliente-pet
            services.AddScoped<IAnimal, AnimalService>();
            services.AddScoped<ICliente, ClienteService>();
            services.AddScoped<IPorteAnimal, PorteAnimalService>();
            services.AddScoped<IRacaAnimal, RacaAnimalService>();
            services.AddScoped<ITipoAnimal, TipoAnimalService>();
            services.AddScoped<IClientePontuacao, ClientePontuacaoService>();

            // Produtos-promoções
            services.AddScoped<IProduto, ProdutoService>();
            services.AddScoped<IServicoProduto, ServicoProdutoService>();
            services.AddScoped<IPromocao, PromocaoService>();
            services.AddScoped<IPromocaoProdServ, PromocaoProdServService>();

            // Serviços-vendas
            services.AddScoped<IServico, ServicoService>();
            services.AddScoped<IVendaAvaliacao, VendaAvaliacaoService>();
            services.AddScoped<ITipoPagamento, TipoPagamentoService>();
            services.AddScoped<IVenda, VendaService>();
            services.AddScoped<IVendaProduto, VendaProdutoService>();

            // Agendamento
            services.AddScoped<IAgendamento, AgendamentoService>();
        }
    }
}
