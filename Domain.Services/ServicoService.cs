using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities.Models;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services
{
    public class ServicoService : BaseRepository<Servico>, IServico
    {
        public ServicoService(PetshopContext db) : base(db)
        {
        }

        // Função padrão para atualizar ou cadastrar um registro ->incluindo histórico
        public async Task<Servico> CadastraOuAtualiza(Servico servico, int usuarioAlterando)
        {
            if (!servico.TempoEstimado.HasValue || servico.Preco < 1)
                return null;

            //Add
            if (servico.Id < 1)
            {
                await DbSet.AddAsync(servico);
            }
            else //Update
            {
                var checagemValor = await HouveAlteracaoPreco(servico);
                if (checagemValor > 0)
                {
                    servico.PrecoAntigo = checagemValor;
                    await RegistraHistoricoPreco(servico, usuarioAlterando);
                }

                Db.Update(servico);
            }

            await Db.SaveChangesAsync();
            return servico;
        }

        // Verifica se houve alteração de preço
        private async Task<decimal> HouveAlteracaoPreco(Servico servico)
        {
            var registroAtual = await DbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == servico.Id);
            // Se o preço registrado atualmente for diferente do novo preço, retorna o valor atual para histórico
            if (registroAtual.Preco != servico.Preco)
                return registroAtual.Preco;

            return 0; //Se não houve alteração, retorna 0
        }

        // Registra o histórico dos preços
        private async Task RegistraHistoricoPreco(Servico servico, int profissional)
        {
            var historico = new ServicoLog
            {
                DataAlteracao = DateTime.Now,
                PrecoAntigo = servico.PrecoAntigo ?? 0,
                PrecoNovo = servico.Preco,
                ServicoId = servico.Id,
                UsuarioId = profissional
            };

            await Db.ServicoLog.AddAsync(historico);
            await Db.SaveChangesAsync();
        }

        // Retorna os serviços cadastrados
        public async Task<IEnumerable<Servico>> ConsultaRegistros(string servico)
        {
            var servicos = await DbSet.ToListAsync();

            if (!string.IsNullOrEmpty(servico))
                servicos = servicos.Where(x => x.Nome.ToLower().Contains(servico.ToLower())).ToList();

            return servicos;
        }

        public async Task DesabilitarServico(int servicoId)
        {
            var registro = await DbSet.FindAsync(servicoId);

            if (registro != null)
            {
                registro.Ativo = 0;

                Db.Update(registro);
                await Db.SaveChangesAsync();
            }
        }
    }
}
