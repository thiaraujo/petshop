function alterarEstoque(id) {
    $.get("/estoque/getDetalhes", { id: id })
        .done(function (response) {
            $("#hddIdEdicao").val(id);
            $("#mdlTitle").html(response.descricao);
            $("#lblEstoque").html(response.estoque);

            $("#mdlEstoque").modal("show");
        });

}

function confirmarEstoque() {

    var data = {
        id: $("#hddIdEdicao").val(),
        estoque: $("#txtEstoque").val()
    };
    $.post("/estoque/postConfirmarEstoque", data)
        .done(function (response) {
            if (response === true) {
                showToastr("ok", "Estoque atualizado com sucesso", "Operação Confirmado");
            } else {
                showToastr("warning", "Não foi possível atualizar este registro", "Operação Cancelada");
            }

            setTimeout(function () {
                location.href = location.href;
            },
                200);
        });

}