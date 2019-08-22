
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

$("#ddlAnimal").change(function (e) {
    $("#ddlServico").attr("disabled", false);
});

$("#ddlServico").change(function (e) {
    $("#txtData").attr("disabled", false);
});

$("#txtData").change(function (e) {
    $("#txtHora").attr("disabled", false);
});

$("#ddlUsuario").change(function (e) {
    getEspecialidadeAutoriza();
    getDisponibilidadeAgenda();
});

$("#txtHora").change(function (e) {
    $("#ddlUsuario").attr("disabled", false);
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
            option = "<option>-- selecione --</option>";
            $.each(response,
                function (i, item) {
                    option += "<option value='" + item.id + "'>" + item.nome + "</option>";
                });

            $("#ddlAnimal").append(option);
            $("#ddlAnimal").attr("disabled", false);
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

function getEspecialidadeAutoriza() {

    var data = {
        animalId: $("#ddlAnimal").val(),
        usuarioId: $("#ddlUsuario").val()
    };

    $.get("/caixa/GetUsuariosEspecialidade", data)
        .done(function (status) {
            if (status === true) {
                $("#divEspecialidade").append("p").prop("id", "especialidade").text("Este profissional é capacitado para o atendimento.").addClass("alert alert-success");
            } else {
                $("#divEspecialidade").append("p").prop("id", "especialidade").text("Este profissional não é capacitado para o atendimento.").addClass("alert alert-danger");
            }
        });

}

function getDisponibilidadeAgenda() {

    var data = {
        usuarioId: $("#ddlUsuario").val(),
        data: $("#txtData").val(),
        hora: $("#txtHora").val(),
        servicoId: $("#ddlServico").val()
    };

    $.get("/caixa/GetDisponivelAgendamento", data)
        .done(function (status) {
            if (status !== "00:00:00") {
                $("#divAgendamento").append("p").prop("id", "disponivel").text("Este profissional estará disponível para o atendimento e o tempo estimado será de: " + status + ".").addClass("alert alert-info");
                $("#btnConfirmar").attr("disabled", false);
                $("#txtObs").attr("disabled", true);
            } else {
                $("#divAgendamento").append("p").prop("id", "disponivel").text("Este profissional não estará disponível para o atendimento.").addClass("alert alert-warning");
            }
        });

}

$("#form-agendamento").submit(function (event) {
    event.preventDefault();

    $.post("/caixa/PostAddAgendamento", $("#form-agendamento").serialize())
        .done(function (status) {
            if (status === true) {
                showToastr("ok", "Agendamento realizado com sucesso!", "Operação Confirmada");
            } else {
                showToastr("erro", "O agendamento não pôde ser confirmado!", "Operação Cancelada");
            }
        })
        .fail(function (error) {
            console.log(error);
        });
});