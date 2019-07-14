using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities.Models;

namespace Domain.Interfaces
{
    public interface IAgendamento : IBaseRepository<Agendamento>
    {
        Task<bool> HorarioAgendamentoDisponivel(int profissionalId, DateTime dataAgendado, TimeSpan horaAgendado, int servicoAgendadoId);
        Task<IEnumerable<Usuario>> ConsultaProfissionalBaseadoNoServico(int servicoId);
        Task<Agendamento> CadastraOuAtualiza(Agendamento agendamento);
        Task AgendamentoCancelado(int agendamentoId);
        Task<IEnumerable<Agendamento>> ConsultaRegistros(int? profissionalId, DateTime? dataAgendamento);
    }
}
