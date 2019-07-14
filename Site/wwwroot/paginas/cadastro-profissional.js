$(document).ready(function () {
    if ($("#hddAdmin").val() === "1") {
        $("#cbAdministrador").val("1");
        $("#cbAdministrador").prop("checked", true);
    }

    if ($("#hddAtivo").val() === "0") {
        $("#cbHabilitado").val("0");
        $("#cbHabilitado").prop("checked", false);
    }

    if ($("#hddVeterinario").val() === "1") {
        $("#cbVeterinario").val("1");
        $("#cbVeterinario").prop("checked", true);
    }

    if ($("#hddVeterinario").val() === "1") {
        carregaEspecialidades();
        especialidade(true);
    } else
        especialidade(false);
});

$("#cbHabilitado").change(function () {
    if ($(this).is(":checked")) {
        $(this).val("1");
    } else {
        $(this).val("0");
    }
});

$("#cbAdministrador").change(function () {
    if ($(this).is(":checked")) {
        $(this).val("1");
    } else {
        $(this).val("0");
    }
});

$("#cbVeterinario").change(function () {
    if ($(this).is(":checked")) {
        $(this).val("1");
        carregaEspecialidades();
    } else {
        $(this).val("0");
        especialidade(false);
    }
});

function carregaEspecialidades() {

    var option;
    $("#ddlEspecialidades > option").remove();
    $.get("/profissional/CarregaTipoDeAnimais", { id: $("#hddRegistroId").val() })
        .done(function (response) {
            $.each(response,
                function (i, item) {
                    if (item.possui === false)
                        option += "<option value=" + item.id + ">" + item.nome + "</option>";
                    else
                        option += "<option value=" + item.id + " selected='selected'>" + item.nome + "</option>";
                });

            $("#ddlEspecialidades").append(option);
            especialidade(true);
        });
}

function especialidade(show) {
    var obj = $(".especialidade");
    $.each(obj, function (i, item) {
        if (show) {
            $(item).show();
        } else {
            $(item).hide();
        }
    });
}