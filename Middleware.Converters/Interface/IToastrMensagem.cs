using Middleware.Converters.Models;

namespace Middleware.Converters.Interface
{
    public interface IToastrMensagem
    {
        #region Tipo: Info

        PayloadMensagem SemRegistro();
        PayloadMensagem SemRegistroRelatorio();
        PayloadMensagem ConsultaIncorreta();
        PayloadMensagem SemRegistroConsulta();
        PayloadMensagem Aviso(string msg);

        #endregion

        #region Tipo: Aviso

        PayloadMensagem CamposEmBranco();
        PayloadMensagem DadosInformadosInvalidos(string msg);

        #endregion

        #region Tipo: Sucesso

        PayloadMensagem RegistroAtualizado();
        PayloadMensagem RegistroConfirmado();
        PayloadMensagem UploadConfirmado();
        PayloadMensagem Confirmado(string msg);

        #endregion
    }
}
