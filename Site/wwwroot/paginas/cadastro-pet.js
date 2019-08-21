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
