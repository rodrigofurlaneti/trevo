var arrayIdsDespesasAIgnorar = [];

$(document).ready(function () {
    MakeChosen("cbEmpresaUnidade");
    MakeChosen("unidadeOrigem");
    MakeChosen("despesaAno");
    MakeChosen("despesaMes");

    $("#SelecaoDespesaForm").submit(function(e) {
        //AtualizarDespesas();
        return true;
    });

    $('#cbEmpresaUnidade').change(function () {
        var id = $(this).val();
        BuscarUnidades(id);
    });
});

function CheckDespesa(elem, id) {
    
    $(elem).closest('td').toggleClass('red', this.checked);
    
    if ($(elem).prop('checked') === true) {
        adicionarDespesa(id);
    }
    else if ($(elem).prop('checked') === false) {
        removerDespesa(id);
    }
}

function BuscarUnidades(id) {
    showLoading();

    $.ajax({
        url: '/SelecaoDespesa/BuscarUnidades',
        type: 'get',
        dataType: 'json',
        data: { idEmpresa: id },
        success: function (data) {
            hideLoading();
            //if (data == false) { toastr.error('Unidades não encontradas com a empresa selecionada!'); }
        },
        error: function (xhr) {
            toastr.error('Erro ao BuscarUnidades:' + xhr.responseText);
        }
    });
}

function Informacoes(info) {
    $("#myModalInfo").modal("show");
    $("#txtInfo").val(info);
}

function adicionarDespesa(id) {
    debugger;
    arrayIdsDespesasAIgnorar.push(id);
}

function removerDespesa(id) {

    arrayIdsDespesasAIgnorar = arrayIdsDespesasAIgnorar.filter(function (e) {
        return e.IdContasAPagar !== id;
    });
}

function isEmpty(value) {
    return (value === null || value.length === 0);
}

function Pesquisar() {
    var idEmpresa = $("#cbEmpresaUnidade").find(':selected').val();
    var idUnidade = $("#unidadeOrigem").find(':selected').val();
    var mes = $("#despesaMes").find(':selected').val();
    var ano = $("#despesaAno").find(':selected').val();

    if (idUnidade === "" && idEmpresa === "") {
        toastr.info('É necessário selecionar uma empresa ou uma unidade!');
        return false;
    };
    if (mes === "") {
        toastr.info('É necessário selecionar um mês!');
        return false;
    };
    if (ano === "") {
        toastr.info('É necessário selecionar um ano!');
        return false;
    };

    post(`BuscarDespesas`, { idEmpresa, idUnidade, ano, mes })
        .done((response) => {
            $('#dvListaDespesas').empty().append(response);
        })
        .fail((error) => {
            toastr.error('BuscarDespesas:' + error.responseText);
        });
}



function AtualizarDespesas() {
    debugger;
    showLoading();
    $.ajax({
        url: '/SelecaoDespesa/AtualizarDespesas',
        dataType: 'json',
        type: 'post',
        traditional: true,
        data: { DespesasAIgnorar: arrayIdsDespesasAIgnorar },
        success: function () {
            hideLoading();
        },
        error: function (xhr) {
            toastr.error('AtualizarDespesas:' + xhr.responseText);
        }
    });
}


