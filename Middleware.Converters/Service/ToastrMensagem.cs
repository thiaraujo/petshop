using Middleware.Converters.Interface;
using Middleware.Converters.Models;

namespace Middleware.Converters.Service
{
    public class ToastrMensagem : IToastrMensagem
    {
        public PayloadMensagem SemRegistro()
        {
            var mensagem = new PayloadMensagem
            {
                Mensagem = "Ainda não há registros para os termos consultados",
                Titulo = "Consulta sem Resultado",
                Tipo = "info"
            };

            return mensagem;
        }

        public PayloadMensagem SemRegistroRelatorio()
        {
            var mensagem = new PayloadMensagem
            {
                Mensagem = "A consulta realizada não retornou nenhum resultado",
                Titulo = "Consulta sem Resultado",
                Tipo = "info"
            };

            return mensagem;
        }

        public PayloadMensagem ConsultaIncorreta()
        {
            var mensagem = new PayloadMensagem
            {
                Mensagem = "Este registro não foi carregado corretamente, em alguns instantes, tente novamente",
                Titulo = "Resultado Inesperado",
                Tipo = "aviso"
            };

            return mensagem;
        }

        public PayloadMensagem SemRegistroConsulta()
        {
            var mensagem = new PayloadMensagem
            {
                Mensagem = "Não houve registros encontrados para os termos utilizados",
                Titulo = "Tente alterar os termos",
                Tipo = "info"
            };

            return mensagem;
        }

        public PayloadMensagem Aviso(string msg)
        {
            var mensagem = new PayloadMensagem
            {
                Mensagem = msg,
                Tipo = "info"
            };

            return mensagem;
        }

        public PayloadMensagem CamposEmBranco()
        {
            var mensagem = new PayloadMensagem
            {
                Mensagem = "Você deixou alguns campos em branco, preencha-os e tente novamente",
                Titulo = "Informaçoes em Branco",
                Tipo = "aviso"
            };

            return mensagem;
        }

        public PayloadMensagem DadosInformadosInvalidos(string msg)
        {
            var mensagem = new PayloadMensagem
            {
                Mensagem = msg,
                Titulo = "Informações Inválidas",
                Tipo = "aviso"
            };

            return mensagem;
        }

        public PayloadMensagem RegistroAtualizado()
        {
            var mensagem = new PayloadMensagem
            {
                Mensagem = "Registro atualizado com sucesso",
                Titulo = "Informações Atualizadas",
                Tipo = "ok"
            };

            return mensagem;
        }

        public PayloadMensagem RegistroConfirmado()
        {
            var mensagem = new PayloadMensagem
            {
                Mensagem = "Registro cadastrado com sucesso",
                Titulo = "Informações Registradas",
                Tipo = "ok"
            };

            return mensagem;
        }

        public PayloadMensagem UploadConfirmado()
        {
            var mensagem = new PayloadMensagem
            {
                Mensagem = "Arquivo salvo com sucesso",
                Titulo = "Informações Registradas",
                Tipo = "ok"
            };

            return mensagem;
        }

        public PayloadMensagem Confirmado(string msg)
        {
            var mensagem = new PayloadMensagem
            {
                Mensagem = msg,
                Tipo = "ok"
            };

            return mensagem;
        }
    }
}
