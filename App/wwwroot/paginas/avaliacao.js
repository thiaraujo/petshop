function avaliar(id) {

    $("#hddId").val(id);
    $("#mdlAvaliar").modal("show");

}

function confirmar() {

    $.post("/agendamento/PostAvaliacao", $("#form-avaliacao").serialize())
        .done(function (status) {
            if (status === false) {
                showToastr("info", "Sua nota não é valida!", "Nota inválida");
            } else {
                showToastr("ok", "Avaliação realizada com sucesso", "Nota valida");
                $("#mdlAvaliar").modal("hide");
                setTimeout(function () {
                    location.href = location.href;
                },
                    300);
            }
        });
}