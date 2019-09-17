using Middleware.Converters.Interface;
using Middleware.Converters.Models;

namespace Middleware.Converters.Service
{
    public class ToastrMensagem : IToastrMensagem
    {
        public Payload SemRegistro()
        {
            var mensagem = new Payload
            {
                Mensagem = "Ainda não há registros para os termos consultados",
                Titulo = "Consulta sem Resultado",
                Tipo = "info"
            };

            return mensagem;
        }

        public Payload SemRegistroRelatorio()
        {
            var mensagem = new Payload
            {
                Mensagem = "A consulta realizada não retornou nenhum resultado",
                Titulo = "Consulta sem Resultado",
                Tipo = "info"
            };

            return mensagem;
        }

        public Payload ConsultaIncorreta()
        {
            var mensagem = new Payload
            {
                Mensagem = "Este registro não foi carregado corretamente, em alguns instantes, tente novamente",
                Titulo = "Resultado Inesperado",
                Tipo = "aviso"
            };

            return mensagem;
        }

        public Payload SemRegistroConsulta()
        {
            var mensagem = new Payload
            {
                Mensagem = "Não houve registros encontrados para os termos utilizados",
                Titulo = "Tente alterar os termos",
                Tipo = "info"
            };

            return mensagem;
        }

        public Payload Aviso(string msg)
        {
            var mensagem = new Payload
            {
                Mensagem = msg,
                Tipo = "info"
            };

            return mensagem;
        }

        public Payload CamposEmBranco()
        {
            var mensagem = new Payload
            {
                Mensagem = "Você deixou alguns campos em branco, preencha-os e tente novamente",
                Titulo = "Informaçoes em Branco",
                Tipo = "aviso"
            };

            return mensagem;
        }

        public Payload DadosInformadosInvalidos(string msg)
        {
            var mensagem = new Payload
            {
                Mensagem = msg,
                Titulo = "Informações Inválidas",
                Tipo = "aviso"
            };

            return mensagem;
        }

        public Payload RegistroAtualizado()
        {
            var mensagem = new Payload
            {
                Mensagem = "Registro atualizado com sucesso",
                Titulo = "Informações Atualizadas",
                Tipo = "ok"
            };

            return mensagem;
        }

        public Payload RegistroConfirmado()
        {
            var mensagem = new Payload
            {
                Mensagem = "Registro cadastrado com sucesso",
                Titulo = "Informações Registradas",
                Tipo = "ok"
            };

            return mensagem;
        }

        public Payload UploadConfirmado()
        {
            var mensagem = new Payload
            {
                Mensagem = "Arquivo salvo com sucesso",
                Titulo = "Informações Registradas",
                Tipo = "ok"
            };

            return mensagem;
        }

        public Payload Confirmado(string msg)
        {
            var mensagem = new Payload
            {
                Mensagem = msg,
                Tipo = "ok"
            };

            return mensagem;
        }
    }
}
