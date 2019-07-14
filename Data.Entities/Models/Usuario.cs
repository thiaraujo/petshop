using System;
using System.Collections.Generic;

namespace Data.Entities.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Agendamento = new HashSet<Agendamento>();
            ServicoLog = new HashSet<ServicoLog>();
            ServicoUsuario = new HashSet<ServicoUsuario>();
            UsuarioEspecialidade = new HashSet<UsuarioEspecialidade>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public string Crv { get; set; }
        public int? CodigoAcesso { get; set; }
        public string SenhaAcesso { get; set; }
        public int? EhVet { get; set; }
        public int? EhAdministrador { get; set; }
        public DateTime DataCadastro { get; set; }
        public int Ativo { get; set; }

        public virtual ICollection<Agendamento> Agendamento { get; set; }
        public virtual ICollection<ServicoLog> ServicoLog { get; set; }
        public virtual ICollection<ServicoUsuario> ServicoUsuario { get; set; }
        public virtual ICollection<UsuarioEspecialidade> UsuarioEspecialidade { get; set; }
    }
}
