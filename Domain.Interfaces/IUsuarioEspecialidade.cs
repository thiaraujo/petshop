using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities.Models;

namespace Domain.Interfaces
{
    public interface IUsuarioEspecialidade : IBaseRepository<UsuarioEspecialidade>
    {
        Task CadastraOuAtualizaEspecialidade(int usuarioId, int[] tipos);

        // Utilizado somente quando o usuário for do tipo Veterinário
        Task<IEnumerable<UsuarioEspecialidade>> ConsultaEspecialidadesUsuario(int usuarioId);
    }
}
