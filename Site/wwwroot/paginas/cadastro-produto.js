$(document).ready(function () {
    if ($("#hddAtivo").val() === "0") {
        $("#cbHabilitado").val("0");
        $("#cbHabilitado").prop("checked", false);
    }
});

$("#cbHabilitado").change(function () {
    if ($(this).is(":checked")) {
        $(this).val("1");
    } else {
        $(this).val("0");
    }
});

function alterarFoto() {
    $("#imgAlterarFoto").show();
    $("#btnAlterarFoto").hide();
}