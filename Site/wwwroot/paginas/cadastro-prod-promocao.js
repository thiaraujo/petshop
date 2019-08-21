$(document).ready(function () {
    carregaProdutos();
    carregaServicos();
});

function carregaProdutos() {

    var option;
    $("#ddlProdutos > option").remove();
    $.get("/Produto/GetProdutos", { id: $("#hddRegistroId").val() })
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

function carregaServicos() {

    var option;
    $("#ddlServicos > option").remove();
    $.get("/Produto/GetServicos", { id: $("#hddRegistroId").val() })
        .done(function (response) {
            $.each(response,
                function (i, item) {
                    if (item.possui === false)
                        option += "<option value=" + item.id + ">" + item.nome + "</option>";
                    else
                        option += "<option value=" + item.id + " selected='selected'>" + item.nome + "</option>";
                });

            $("#ddlServicos").append(option);
        });
}