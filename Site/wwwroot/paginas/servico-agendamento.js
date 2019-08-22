
function abrirModal() {
    $("#mdlTitle").text("Novo Agendamento");

    getClientes();
    getServicos();
    getUsuarios();

    $("#mdlAgendamento").modal("show");
}

$("#ddlCliente").change(function (e) {
    getPets();
});

function getClientes() {

    $("#ddlCliente > option").remove();
    var option;
    $.get("/caixa/GetClientes")
        .done(function (response) {
            option = "<option>-- selecione --</option>";
            $.each(response,
                function (i, item) {
                    option += "<option value='" + item.id + "'>" + item.nome + "</option>";
                });

            $("#ddlCliente").append(option);
        });

}

function getPets() {

    $("#ddlAnimal > option").remove();
    var option;
    $.get("/caixa/GetPets", { clienteId: $("#ddlCliente").val() })
        .done(function (response) {
            $.each(response,
                function (i, item) {
                    option += "<option value='" + item.id + "'>" + item.nome + "</option>";
                });

            $("#ddlAnimal").append(option);
        });

}

function getServicos() {

    $("#ddlServico > option").remove();
    var option;
    $.get("/caixa/GetServicos")
        .done(function (response) {
            option = "<option>-- selecione --</option>";
            $.each(response,
                function (i, item) {
                    option += "<option value='" + item.id + "'>" + item.nome + "</option>";
                });

            $("#ddlServico").append(option);
        });

}

function getUsuarios() {

    $("#ddlUsuario > option").remove();
    var option;
    $.get("/caixa/GetUsuarios")
        .done(function (response) {
            option = "<option>-- selecione --</option>";
            $.each(response,
                function (i, item) {
                    option += "<option value='" + item.id + "'>" + item.nome + "</option>";
                });

            $("#ddlUsuario").append(option);
        });

}