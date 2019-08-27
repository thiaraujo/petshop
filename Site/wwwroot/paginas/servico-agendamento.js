$(document).ready(function () {
    getAgendamentos();
});

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

function getClientes(selected) {

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
            if (selected !== "") {
                $("#ddlCliente").val(selected);
                $("#ddlCliente").trigger('change.select2');
            }
        });

}

function getPets(selected) {

    var id = selected !== "" ? selected : $("#ddlCliente").val();

    $("#ddlAnimal > option").remove();
    var option;
    $.get("/caixa/GetPets", { clienteId: id })
        .done(function (response) {
            option = "<option>-- selecione --</option>";
            $.each(response,
                function (i, item) {
                    option += "<option value='" + item.id + "'>" + item.nome + "</option>";
                });

            $("#ddlAnimal").append(option);
            if (selected !== "") {
                $("#ddlAnimal").val(selected);
                $("#ddlAnimal").trigger('change.select2');
                $("#ddlAnimal").attr("disabled", false);
            } else
                $("#ddlAnimal").attr("disabled", false);
        });

}

function getServicos(selected) {

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
            if (selected !== "") {
                $("#ddlServico").val(selected);
                $("#ddlServico").trigger('change.select2');
                $("#ddlServico").attr("disabled", false);
            }
        });

}

function getUsuarios(selected) {

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
            if (selected !== "") {
                $("#ddlUsuario").val(selected);
                $("#ddlUsuario").trigger('change.select2');
                $("#ddlUsuario").attr("disabled", false);
            }
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

function getRegistroEdicao(id) {

    $.get("/caixa/GetAgendamento", { id: id })
        .done(function (response) {
            //seta as variaveis
            getClientes(response.clienteId);
            getPets(response.animalId);
            getServicos(response.servicoId);
            getUsuarios(response.usuarioId);
            $("#txtData").val(response.dia);
            $("#txtData").attr("disabled", false);

            $("#txtHora").val(response.hora);
            $("#txtHora").attr("disabled", false);

            $("#txtObs").val(response.obs);

            $("#mdlTitle").text("Edição do agendamento: " + response.id);

            $("#hddEdicaoId").val(response.id);
            $("#btnConfirmar").attr("disabled", false);

            $("#mdlAgendamento").modal("show");
        });
}

function getAgendamentos() {

    $("#tblAgendamentos > tbody > tr").remove();
    var tr;

    $.get("/caixa/GetAgendamentos")
        .done(function (response) {
            if (response.length < 1) {
                $("#divSemAgendamento").show();
            } else {
                $.each(response,
                    function (i, item) {
                        tr += "<tr>" +
                            "<td>" + item.pet + "</td>" +
                            "<td>" + item.cliente + "</td>" +
                            "<td class='text-center'>" + item.hora + "</td>" +
                            "<td class='text-center'><button class='btn btn-xs btn-info' onclick='getRegistroEdicao(" + item.id + ")'>ver</button></td>" +
                            "</tr>";
                    });

                $("#tblAgendamentos").append(tr);
            }
        });

}

$("#form-agendamento").submit(function (event) {
    event.preventDefault();

    $.post("/caixa/PostAddAgendamento", $("#form-agendamento").serialize())
        .done(function (status) {
            if (status === true) {
                showToastr("ok", "Agendamento realizado com sucesso!", "Operação Confirmada");
                getAgendamentos();
            } else {
                showToastr("erro", "O agendamento não pôde ser confirmado!", "Operação Cancelada");
            }
            $("#mdlAgendamento").modal("hide");
        })
        .fail(function (error) {
            console.log(error);
        });
});
