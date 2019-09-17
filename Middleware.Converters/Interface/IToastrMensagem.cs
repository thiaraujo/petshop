using Middleware.Converters.Models;

namespace Middleware.Converters.Interface
{
    public interface IToastrMensagem
    {
        #region Tipo: Info

        Payload SemRegistro();
        Payload SemRegistroRelatorio();
        Payload ConsultaIncorreta();
        Payload SemRegistroConsulta();
        Payload Aviso(string msg);

        #endregion

        #region Tipo: Aviso

        Payload CamposEmBranco();
        Payload DadosInformadosInvalidos(string msg);

        #endregion

        #region Tipo: Sucesso

        Payload RegistroAtualizado();
        Payload RegistroConfirmado();
        Payload UploadConfirmado();
        Payload Confirmado(string msg);

        #endregion
    }
}
