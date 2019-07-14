﻿$(document).ready(function () {
    $('.date').mask('00/00/0000');
    $('.time').mask('00:00:00');
    $('.date_time').mask('00/00/0000 00:00:00');
    $('.cep').mask('00000-000');
    $('.phone').mask('0000-0000');
    $('.cpf').mask('000.000.000-00', { reverse: true });

    if ($(".js-example-basic-multiple").length) {
        $(".js-example-basic-multiple").select2({
            theme: "classic"
        });
    }
});

function showToastr(type, msg, titulo) {
    switch (type) {
    case "ok" || 2:
        toastr.success(msg, titulo);
        break;

    case "erro" || 3:
        toastr.error(msg, titulo);
        break;

    case "aviso" || 4:
        toastr.warning(msg, titulo);
        break;

    default:
        toastr.info(msg, titulo);
        break;
    }
}