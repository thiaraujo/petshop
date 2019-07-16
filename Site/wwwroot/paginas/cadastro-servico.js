$(document).ready(function () {
    if ($("#hddAtivo").val() === "0") {
        $("#cbHabilitado").val("0");
        $("#cbHabilitado").prop("checked", false);
    }

    if ($("#hddVeterinario").val() === "1") {
        $("#cbVeterinario").val("1");
        $("#cbVeterinario").prop("checked", true);
    }

    carregaProdutos();
});

$("#cbHabilitado").change(function () {
    if ($(this).is(":checked")) {
        $(this).val("1");
    } else {
        $(this).val("0");
    }
});

$("#cbVeterinario").change(function () {
    if ($(this).is(":checked")) {
        $(this).val("1");
    } else {
        $(this).val("0");
    }
});

function carregaProdutos() {

    var option;
    $("#ddlProdutos > option").remove();
    $.get("/Servico/GetProdutos", { id: $("#hddRegistroId").val() })
        .done(function (response) {
            $.each(response,
                function (i, item) {
                    if (item.possui === false)
                        option += "<option value=" + item.id + ">" + item.nome + "</option>";
                    else
                        option += "<option value=" + item.id + " selected='selected'>" + item.nome + "</option>";
                });

            $("#ddlProdutos").append(option);
        });
}