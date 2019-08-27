using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities.Models;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services
{
    public class AgendamentoService : BaseRepository<Agendamento>, IAgendamento
    {
        public AgendamentoService(PetshopContext db) : base(db)
        {
        }

        // Antes de confirmar o agendamento, é verificado se o agendamento pode ser concluído
        // Verifica se o profissional tem agenda disponível, baseado na data e na quantidade de tempo que o serviço exige
        public async Task<TimeSpan> HorarioAgendamentoDisponivel(int profissionalId, DateTime dataAgendado, TimeSpan horaAgendado, int servicoAgendadoId)
        {
            var servico = await Db.Servico.FindAsync(servicoAgendadoId);
            var agendamentos = await DbSet.Where(x => x.DiaMarcado == dataAgendado && x.UsuarioId == profissionalId).ToListAsync();

            // Se não existir, retorna true
            if (!agendamentos.Any())
                return servico.TempoEstimado ?? TimeSpan.Zero;

            var tempoServico = servico.TempoEstimado?.Minutes ?? 10; //Se não tiver tempo estimado, sera o tempo de 10 minutos
            var minHora = horaAgendado;
            var maxHora = new TimeSpan(0, (minHora.Minutes + tempoServico), 0);

            // Verifica se existe horário marcado no período da data
            var existeConflitoAgenda = agendamentos.FirstOrDefault(x => x.HoraMarcado >= minHora && x.HoraMarcado <= maxHora);
            if (existeConflitoAgenda != null) return TimeSpan.Zero;

            return servico.TempoEstimado ?? TimeSpan.Zero;
        }

        // Baseado no serviço selecionado, consulta as areas que o profissional atual, sendo vet ou não
        public async Task<bool> ConsultaProfissionalBaseadoNoAnimal(int animalId, int usuarioId)
        {
            //Pega o tipo do animal
            var tipoAnimal = await Db.Animal.FirstOrDefaultAsync(x => x.Id == animalId);

            var especialidadePermitida = await Db.UsuarioEspecialidade.FirstOrDefaultAsync(x => x.UsuarioId == usuarioId && x.TipoAnimalId == tipoAnimal.TipoAnimalId);

            return especialidadePermitida != null; //se não diferente de null (true) então ele tem esta especialidade
        }

        // Faz pequenas validações no registro, se estiver tudo ok, registra e retorna o objeto salvo
        public async Task<Agendamento> CadastraOuAtualiza(Agendamento agendamento)
        {
            if (agendamento.ClienteId < 1)
            {
                return null;
            }

            //Se já existir um ID então é atualização
            if (agendamento.Id > 0)
                DbSet.Update(agendamento);
            else
                await DbSet.AddAsync(agendamento);

            await Db.SaveChangesAsync();

            return agendamento;
        }

        // Caso o cliente falte, é informado um cancelamento
        public async Task AgendamentoCancelado(int agendamentoId)
        {
            var registro = await DbSet.FindAsync(agendamentoId);

            if (registro != null)
            {
                registro.Ausente = 1;

                Db.Update(registro);
                await Db.SaveChangesAsync();
            }
        }

        // Consulta os agendamentos registrados
        public async Task<IEnumerable<Agendamento>> ConsultaRegistros(int? profissionalId, DateTime? dataAgendamento)
        {
            var agendamentos = await DbSet
                .Include(x => x.Servico)
                .Include(x => x.Animal)
                .Include(x => x.Cliente)
                .Include(x => x.Usuario)
                .ToListAsync();

            if (profissionalId.HasValue)
                agendamentos = agendamentos.Where(x => x.UsuarioId == profissionalId).ToList();

            if (dataAgendamento.HasValue)
                agendamentos = agendamentos.Where(x => x.DiaMarcado.Date == dataAgendamento.Value.Date).ToList();

            return agendamentos;
        }
    }
}
