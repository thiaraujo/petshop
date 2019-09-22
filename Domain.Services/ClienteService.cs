using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities.Models;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services
{
    public class ClienteService : BaseRepository<Cliente>, ICliente
    {
        public ClienteService(PetshopContext db) : base(db)
        {

        }

        // Função padrão para atualizar ou cadastrar um registro
        public async Task<Cliente> CadastraOuAtualiza(Cliente cliente)
        {
            if (string.IsNullOrEmpty(cliente.Nome) || string.IsNullOrEmpty(cliente.Cpf) ||
                string.IsNullOrEmpty(cliente.Email) || string.IsNullOrEmpty(cliente.Endereco) ||
                string.IsNullOrEmpty(cliente.Rg))
                return null;

            if (cliente.Id > 0)
                Db.Update(cliente);
            else
            {
                cliente.DataCadastro = DateTime.Now;
                await DbSet.AddAsync(cliente);
            }

            await Db.SaveChangesAsync();
            return cliente;
        }

        // Função padrão para consulta de registros
        public async Task<IEnumerable<Cliente>> ConsultaRegistros(string cliente)
        {
            var clientes = await DbSet.ToListAsync();

            if (!string.IsNullOrEmpty(cliente))
                clientes = clientes.Where(x => x.Nome.ToLower().Contains(cliente.ToLower())).ToList();

            return clientes;
        }
    }
}
