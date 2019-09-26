$(document).ready(function () {
    getAgendamentos();
    getConcluidos();
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
    if ($(this).val() === "-- selecione --") {
        $("#divAgendamento").hide();
        $("#divEspecialidade").hide();
    } else {
        getEspecialidadeAutoriza();
        getDisponibilidadeAgenda();
    }
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

    var id = selected !== undefined ? selected : $("#ddlCliente").val();

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
            $("#divEspecialidade").show();
            if (status === true) {
                $("#divEspecialidadeText").removeClass("alert alert-danger").text("Este profissional é capacitado para o atendimento.").addClass("alert alert-success");
            } else {
                $("#divEspecialidadeText").removeClass("alert alert-success").text("Este profissional não é capacitado para o atendimento.").addClass("alert alert-danger");
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
            $("#divEspecialidade").show();
            if (status !== "00:00:00") {
                $("#divAgendamento").removeClass("alert alert-warning").text("Este profissional estará disponível para o atendimento e o tempo estimado será de: " + status + ".").addClass("alert alert-info");
                $("#btnConfirmar").attr("disabled", false);
                $("#txtObs").attr("disabled", false);
            } else {
                $("#divAgendamento").removeClass("alert alert-info").text("Este profissional não estará disponível para o atendimento.").addClass("alert alert-warning");
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
                            "<td class='text-center'>" +
                            "<button class='btn btn-xs btn-info " + (item.ausente ? 'ausente' : '') + "' onclick='getRegistroEdicao(" + item.id + ")' title='Visualizar detalhes'><i class='fa fa-edit'></i></button>" +
                            "<button class='btn btn-xs btn-success " + (item.ausente ? 'ausente' : '') + "' onclick='abrirPagamento(" + item.id + ")' title='Realizar o pagamento'><i class='fa fa-credit-card'></i></button>" +
                            "<span class='badge badge-danger " + (!item.ausente ? 'ausente' : '') + "'>ausente</span>" +
                            "</td>" +
                            "</tr>";
                    });

                $("#tblAgendamentos").append(tr);
                $("#divSemAgendamento").hide();
            }
        });

}

function abrirPagamento(id) {

    $.get("/caixa/GetPagamentoDetalhes", { id: id })
        .done(function (response) {
            $("#mdlTitlePagamento").text("Pagamento do agendamento " + id);

            $("#hddEdicaoIdPag").val(id);
            $("#txtCliente").val(response.cliente);
            $("#txtPet").val(response.pet);
            $("#txtServico").val(response.servico);
            $("#txtValorServico").val(response.valor);
            $("#txtPz").val(response.pataz);
            $("#spPzFinal").text(response.pzFinal);
            $("#txtPzNecessario").val(response.pzNecessario);

            getProdutos();
            carregaProdutosVenda(id);
        });

    $("#mdlPagamento").modal("show");
}

function getProdutos() {
    $("#ddlProdutos > option").remove();

    var op;
    $.get("caixa/GetProdutos")
        .done(function (response) {
            op = "<option>-- selecione --</option>";
            $.each(response, function (i, item) {
                op += "<option value='" + item.id + "'>" + item.nome + "</option>";
            });
            $("#ddlProdutos").append(op);
        });
}

function getTipoPagamento() {
    $("#ddlTipoPagamento > option").remove();

    //tem pataz suficiente para habilitar a opção
    var necessario = $("#txtPzNecessario").val();
    var possui = $("#txtPz").val();
    var temPz = false;
    if (parseInt(necessario === "" ? 0 : necessario) <= parseInt(possui === "" ? 0 : possui))
        temPz = true;

    var op;
    $.get("caixa/GetTipoPagamento")
        .done(function (response) {
            op = "<option>-- selecione --</option>";
            $.each(response, function (i, item) {
                if (item.nome.indexOf("Pataz") > -1) {
                    if (temPz === false) {
                        op += "<option value='" + item.id + "' disabled=''>" + item.nome + "</option>";
                    }
                } else
                    op += "<option value='" + item.id + "'>" + item.nome + "</option>";
            });
            $("#ddlTipoPagamento").append(op);
        });
}

function confirmarProduto() {
    var produtoId = $("#ddlProdutos").val();
    var qtd = $("#txtQuantidade").val();
    var agendamento = $("#hddEdicaoIdPag").val();

    var venda = {
        AgendamentoId: agendamento,
        ProdutoId: produtoId,
        Quantidade: qtd
    };

    $.post("/caixa/PostAddProduto", venda)
        .done(function (response) {
            if (response === true) {
                showToastr("ok", "Produto adicionado com sucesso!", "Operação Confirmada");
                carregaProdutosVenda(agendamento);
            } else {
                showToastr("erro", "Não foi possível adicioanr este produto!", "Operação Cancelada");
            }
        });

}

function carregaProdutosVenda(id) {

    $("#tblProdutos > tbody > tr").remove();
    var tr;
    $.get("/caixa/GetProdutosVenda", { id: id })
        .done(function (response) {

            $.each(response, function (i, item) {
                tr += "<tr>" +
                    "<td>" + item.produto + "</td>" +
                    "<td class='text-center'>" + item.valor + "</td>" +
                    "<td class='text-center'>" + item.desconto + "</td>" +
                    "<td class='text-center'>" + item.quantidade + "</td>" +
                    "<td class='text-center'>" + item.total + "</td>" +
                    "</tr>";
            });

            getValorServico(id);
            $("#tblProdutos").append(tr);

            getTipoPagamento();
        });

}

function getValorServico(id) {
    $.get("caixa/GetValorTotal", { id: id })
        .done(function (response) {
            $("#txtValorTotal").val(response.total);
            $("#txtValorDescontos").val(response.desconto);
        });
}

function getConcluidos() {

    $("#tblConcluidos > tbody > tr").remove();
    var tr;

    $.get("/caixa/GetConcluidos")
        .done(function (response) {
            if (response.length < 1) {
                $("#divSemConclusao").show();
            } else {
                $.each(response,
                    function (i, item) {
                        tr += "<tr>" +
                            "<td>" + item.pet + "</td>" +
                            "<td>" + item.cliente + "</td>" +
                            "<td class='text-center'>" + item.valor + "</td>" +
                            "</tr>";
                    });

                $("#tblConcluidos").append(tr);
                $("#divSemConclusao").hide();
            }
        });

}

function clienteAusente() {

    var id = $("#hddEdicaoIdPag").val();

    $.post("/caixa/PostClienteAusente", { id: id })
        .done(function (response) {
            if (response === true) {
                showToastr("ok", "Cliente esteve ausente neste serviço.", "Operação Confirmada");
                $("#mdlPagamento").modal("hide");
                getAgendamentos();
            } else {
                showToastr("aviso", "Não foi possível informar ausência do cliente.", "Operação Cancelada");
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
