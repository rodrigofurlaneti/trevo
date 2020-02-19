
let listmodel = [];

$(document).ready(function () {

    $("#statuscheque").children('option[value="2"]').css('display', 'none');
    $("#statuscheque").children('option[value="3"]').css('display', 'none');

    FormatarCampoData("data");
    FormatarCampoData("dataprotesto");

    MakeChosen("contaFinanceira");
    FormataCampoCpf("cpf");

    $('#cpf').on('blur', function (ele) {
        VerificarCPF();
    });

    $("#valor").maskMoney({
        prefix: "R$",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });

    $("#Numero").change(function () {
        var valor = $(this).val();

        if (valor < 0) {
            $(this).val(0);
            toastr.error("O campo \"Numero Cheque\" não pode ser negativo");
        }
    });

    $("#Emitente").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/chequedevolvido/pesquisaemitente',
                data: "{ 'prefix': '" + request.term + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data, function (item) {
                        return item;
                    }))
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        },
        select: function (e, i) {
            $("#Emitente").val(i.item.val);
        },
        minLength: 1
    });
});


function AdicionarRetiradaSelecionada(componente) {
    var retirada = {
        Id: componente.getAttribute("data-retiradaCofreId")
    };

    if (componente.checked) {
        listmodel.push(retirada);
    } else {
        let index = listmodel.indexOf(retirada);
        listmodel.splice(index, 1);
    }
}

function BuscarPartial(url, divId, obj) {
    showLoading();
    return $.post(url, obj)
        .done(function (result) {
            if (typeof (result) === "object") {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () { });
            }
            else {
                $(divId).empty();
                $(divId).append(result);
            }
        })
        .fail(function (error) {
            $(divId).empty();
            $(divId).append(error.responseText);
        }).always(function () {
            hideLoading();
        });
}

function BuscarChequeProtestos() {
    listmodel.length = 0;

    var ChequeViewModel = {
        Cliente: {
            Id: $("#cliente").val()
        },
        Agencia: $("#agencia").val(),
        Conta: $("#conta").val(),
        Numero: $("#Numero").val(),
        Emitente: $("#Emitente").val(),
        CPFCNPJ: $("#cpf").val(),
        Valor: $("#valor").val(),
        StatusCheque: $("#statuscheque").val(),
        DataProtesto: $("#data").val(),
        CartorioProtestado: $("#cartorioprotestadofiltro").val()
    };

    if (!ChequeViewModel.Cliente.Id) {
        toastr.error("Selecione o CLiente", "Filtro Vazio");
    } else {
        BuscarPartial("/chequeprotesto/BuscarChequeProtestos", "#cheque-protesto", ChequeViewModel)
            .done(function () {
                ConfigTabelaGridSemCampoFiltroPrincipal();
            });
    }
}

function AlterarStatusCheque() {

    if (listmodel.length <= 0) {
        toastr.error(error.statusText, "Selecione ao menos 1 registro para Confirmar as Alterações ");
        return;
    }

    var cartorioprotestado = $("#cartorioprotestado").val();
    var dataprotesto = $("#dataprotesto").val();

    showLoading();

    $.post("/chequeprotesto/AlterarStatusCheque", { listmodel, cartorioprotestado, dataprotesto }, function (result) {
        console.log(result);
    })
        .done(function (result) {
            toastr.success(result.status.StatusDescription, "Sucesso");
            hideLoading();
        })
        .fail(function (error) {
            toastr.error(error.statusText, "Erro ao Aprovar");
            hideLoading();
        }).always(function () {
            listmodel.length = 0
            BuscarChequeProtestos();
            hideLoading();
        });
}

function Informacoes() {

    if (listmodel.length <= 0) {
        toastr.error(error.statusText, "Selecione ao menos 1 registro para Confirmar as Alterações");
        return;
    }

    BuscarPartial("/chequeprotesto/Informacoes", "#area-modal-informacoes")
        .done(function () {
            $("#modalInformacoes").modal('show');

                $("#dataprotesto").mask("99/99/9999");
                $("#dataprotesto").datepicker({
                    dateFormat: "dd/mm/yy",
                    dayNames: ["Domingo", "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado"],
                    dayNamesMin: ["D", "S", "T", "Q", "Q", "S", "S", "D"],
                    dayNamesShort: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb", "Dom"],
                    monthNames: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"],
                    monthNamesShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"],
                    nextText: ">",
                    prevText: "<"
                });
        });
}
