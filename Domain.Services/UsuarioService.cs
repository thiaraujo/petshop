using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities.Models;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Middleware.Converters;

namespace Domain.Services
{
    public class UsuarioService : BaseRepository<Usuario>, IUsuario
    {
        public UsuarioService(PetshopContext db) : base(db)
        {
        }

        // Verifica se existe um registro para o ID, juntamente com a senha
        // Se não for encontrado, retornara null e a aplicação retorna uma mensagem de erro
        public async Task<Usuario> UsuarioPodeAcessar(int id, string senha)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.Id == id && x.SenhaAcesso == senha.EncriptText());
        }

        // Se a variável tipoPet for > 0, é provavel que seja um veterinário
        public async Task<Usuario> CadastraOuAtualiza(Usuario usuario, int[] tipoPet)
        {
            if (string.IsNullOrEmpty(usuario.Nome) || string.IsNullOrEmpty(usuario.Cpf) ||
                string.IsNullOrEmpty(usuario.Rg))
                return null;

            if (usuario.EhVet == 1)
            {
                if (string.IsNullOrEmpty(usuario.Crv))
                    return null;
            }

            //Verifica se precisa atualizar a senha, se a mesma for inferior a 20 caracteres, então encripta
            if (usuario.SenhaAcesso.Length < 20)
                usuario.SenhaAcesso = usuario.SenhaAcesso.EncriptText();

            if (usuario.Id > 0)
                Db.Update(usuario);
            else
                await DbSet.AddAsync(usuario);

            await Db.SaveChangesAsync();

            return usuario;
        }

        // Função padrão para consulta de registros
        public async Task<IEnumerable<Usuario>> ConsultaRegistros(string usuario)
        {
            var usuarios = await DbSet.ToListAsync();

            if (!string.IsNullOrEmpty(usuario))
                usuarios = usuarios.Where(x => x.Nome.ToLower().Contains(usuario.ToLower())).ToList();

            return usuarios;
        }

        public async Task DesabilitarRegistro(int usuarioId)
        {
            var registro = await DbSet.FindAsync(usuarioId);
            if (registro != null)
            {
                registro.Ativo = 0;

                Db.Update(registro);
                await Db.SaveChangesAsync();
            }
        }
    }
}
