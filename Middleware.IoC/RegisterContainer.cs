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

            services.AddScoped<IUsuario, UsuarioService>();
            services.AddScoped<IUsuarioEspecialidade, UsuarioEspecialidadeService>();
            services.AddScoped<IAnimal, AnimalService>();
            services.AddScoped<IAgendamento, AgendamentoService>();
            services.AddScoped<ICliente, ClienteService>();
            services.AddScoped<IPorteAnimal, PorteAnimalService>();
            services.AddScoped<IProduto, ProdutoService>();
            services.AddScoped<IServicoProduto, ServicoProdutoService>();
            services.AddScoped<IPromocao, PromocaoService>();
            services.AddScoped<IPromocaoProdServ, PromocaoProdServService>();
            services.AddScoped<IRacaAnimal, RacaAnimalService>();
            services.AddScoped<IServico, ServicoService>();
            services.AddScoped<ITipoAnimal, TipoAnimalService>();
            services.AddScoped<ITipoPagamento, TipoPagamentoService>();
            services.AddScoped<IVenda, VendaService>();
            services.AddScoped<IVendaProduto, VendaProdutoService>();
            services.AddScoped<IClientePontuacao, ClientePontuacaoService>();
        }
    }
}
