let emissaoNotafiscal = [];

$(document).ready(function () {
   
    FormatarCampoData("data");
    MakeChosen("unidade", null, "100%");
    MakeChosen("departamento", null, "100%");


    $('#empresaOrigem').change(function () {

        var IdEmpresa = $(this).find(':selected').val();
        showLoading();
        $.post("/emissaonotafiscal/BuscarUnidades", { IdEmpresa })

            .done((response) => {
                if (typeof (response) === "object") {
                    CarregaUnidades(response);
                } else {
                    alert("NAOIBJETO");
                }
            })
            .fail((erro) => {

                alert(erro.responseText);
            })
            .always(() => { hideLoading(); });

    });
});

function CarregaUnidades(empresas) {
    //debugger;
    var equipeSelect = document.getElementById("unidadeOrigem");
    equipeSelect.innerHTML = "";

    var option = document.createElement("option");
    option.text = "Selecione uma unidade";
    option.value = 0;
    equipeSelect.options.add(option);
    $.each(empresas, function (i, item) {
        option = document.createElement("option");
        option.text = item.Nome;
        option.value = item.Id;
        equipeSelect.options.add(option);
    });
    MakeChosen("unidadeOrigem");
    hideLoading();
}

function BuscaPagamentos() {

    $.ajax({
        url: '/emissaonotafiscal/BuscaPagamentos',
        type: 'POST',
        dataType: 'json',
        success: function (response) {
            CarregaGridPagamentosReembolso(response);
        }
    });
}


function AdicionarRetiradaSelecionada(componente) {
    var PagamentoViewModel = {
        Id: componente.getAttribute("data-lancamentoreembolsoid")
    };

    if (componente.checked) {
        emissaoNotafiscal.push(PagamentoViewModel);
    } else {
        emissaoNotafiscal.splice(emissaoNotafiscal.findIndex(v => v.Id === PagamentoViewModel.Id), 1);
        //let index = emissaoNotafiscal.indexOf(PagamentoViewModel);
        //emissaoNotafiscal.splice(index, 1);
    }
}

function ExecutarPagamentoModal() {

    $.ajax({
        url: "/emissaonotafiscal/ExecutarPagamentoModal",
        type: "POST",
        dataType: "json",
        success: function (result) {
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
                $("#modalBodyPagamento").empty();
                $("#modalBodyPagamento").append(result);
                $("#modalPagamento").modal({ backdrop: 'static', keyboard: false });
            }
        },
        error: function (error) {
            $("#modalBodyPagamento").empty();
            $("#modalBodyPagamento").append(error.responseText);
            $("#modalPagamento").modal({ backdrop: 'static', keyboard: false });
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });
}

function Pesquisar() {
    emissaoNotafiscal.length = 0;

    if (!ValidarCampos()) return;

    var unidade = {
        Id: $("#unidadeOrigem").val()
    };

    var empresaUnidade =
        {
            Id: $("#empresaOrigem").val()
        };

    filtro = {
        Mes: $('#despesaMes :selected').text(),
        Ano: $('#despesaAno :selected').text(),
        Unidade: unidade,
        Empresa: empresaUnidade
    };

        
    BuscarPartial("/emissaonotafiscal/buscardadospagamentos", "#lista-pagamentosemissao", filtro)
        .done(function () {
            MetodoUtil();
        });
}

function LiberarNotas() {
    
    if (emissaoNotafiscal.length <= 0) {
        toastr.error(error.statusText, "Selecione ao menos 1 registro para Confirmar Liberação");
        return;
    }

    showLoading();

    $.ajax({
        url: "/emissaonotafiscal/LiberarNotas",
        type: "POST",
        dataType: "json",
        data: { emissaoNotafiscal },
        success: function (result) {
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

            $("#modalPagamento").modal('hide');
            $("#lista-pagamentosemissao").empty();
            $("#lista-pagamentosemissao").append(result);
            BuscarPagamentosRePesquisa();
        },
        error: function (error) {
            //$("#modalBodyPagamento").empty();
            //$("#modalPagamento").modal('hide');
            
            $("#modalPagamento").modal('hide');
            $("#lista-pagamentosemissao").empty();
            $("#lista-pagamentosemissao").append(error.responseText);
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            emissaoNotafiscal.length = 0;
            hideLoading();
        }
    });
}

function VerificaRecibo() {

    if (emissaoNotafiscal.length <= 0) {
        toastr.error(error.statusText, "Selecione ao menos 1 registro para verificar recibos.");
        return;
    }

    BuscarPartial("/emissaonotafiscal/VerificaRecibo", "#area-modal-verificarecibo", { emissaoNotafiscal })
        .done(function () {
            $("#modalInformacoes").modal('show');
        });
}

function ValidarCampos() {

    if ($("#empresaOrigem").val() === "") {

        toastr.error("Selecione \"uma empresa\"");
        return false;
    }

    if ($("#despesaMes").val() === "") {

        toastr.error("Selecione \"um mês\"");
        return false;
    }

    if ($("#despesaAno").val() === "") {

        toastr.error("Selecione \"um ano\"");
        return false;
    }

    return true;

}

function BuscarPagamentosRePesquisa() {

    emissaoNotafiscal.length = 0;

    var unidade = {
        Id: $("#unidadeOrigem").val()
    };

    var empresaUnidade =
        {
            Id: $("#empresaOrigem").val()
        };

    filtro = {
        Mes: $('#despesaMes :selected').text(),
        Ano: $('#despesaAno :selected').text(),
        Unidade: unidade,
        Empresa: empresaUnidade
    };

    BuscarPartial("/emissaonotafiscal/buscardadospagamentos", "#lista-pagamentosemissao", filtro)
        .done(function () {
            MetodoUtil();
        });
    
}