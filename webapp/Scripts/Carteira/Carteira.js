$(document).ready(function () {

    $("#ValMinParcela").maskMoney({
        prefix: "R$",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });

    $("#PerValorParcelaEnt").maskMoney({
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });

    ValidaNumero("Sequencial");
    ValidaNumero("NumMaxParcela");
    ValidaNumero("DiasQuebraAcordo");
    ValidaNumero("DiasComissao");
    ValidaNumero("CodigoAssociado");

    var credorIdSelecionado = $("#CodigoCredorSelecionado").val();
    var produtoIdSelecionado = $("#CodigoProdutoSelecionado").val();

    CarregarCredor(credorIdSelecionado);

    if (credorIdSelecionado !== undefined && credorIdSelecionado !== "") {
        CarregarProduto(credorIdSelecionado, produtoIdSelecionado);
    }

    $("#Credor").change(function () {
        $('#CodigoCredorSelecionado').val($('#Credor').val());
        CarregarProduto($('#CodigoCredorSelecionado').val(),0);
    });

    $("#Produto").change(function () {
        $('#CodigoProdutoSelecionado').val($('#Produto').val());
    });

    $("#CadastroCarteiraForm").submit(function (e) {
        showLoading();

        var dadosValidos = true;

        if ($("#CodigoAssociado").val() === '') {
            toastr.error('Código do Associado é Obrigatório!', 'Código do Associado!');
            dadosValidos = false;
        }

        if ($("#Descricao").val() === '') {
            toastr.error('Descrição da carteira é Obrigatório!', 'Decsrição!');
            dadosValidos = false;
        }

        if ($("#Sequencial").val() === "0" || $("#Sequencial").val() === "") {
            toastr.error('Sequencial da carteira é Obrigatório!', 'Sequencial!');
            dadosValidos = false;
        }

        if ($("#Produto").val() === "0") {
            toastr.error('Produto é obrigatório!', 'Produto!');
            dadosValidos = false;
        }

        if ($("#Credor").val() === "0") {
            toastr.error('Credor é obrigatório!', 'Credor!');
            dadosValidos = false;
        }
        
        $("#ValMinParcela").val($("#ValMinParcela").val().replace(/\./g, '').replace('R$', ''));

        if (!dadosValidos) {
            e.preventDefault();
            hideLoading();
            return false;
        }
    });

});


function CheckValidador(ele) {
    var id = $(ele).attr("id").split("_")[1];
    var check = $(ele).prop("checked");

    $.ajax({
        type: 'POST',
        url: '/carteira/AtualizaValidador',
        dataType: 'json',
        data: { id: id, ativo: check},
        success: function (parametro) {
            hideLoading();
        },
        error: function (ex) {
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        }
    });

}


function CarregarCredor(credorIdSelecionado) {
    $("#Produto").empty();
    $.ajax({
        type: 'POST',
        url: '/carteira/CarregarCredores',
        dataType: 'json',
        success: function (result) {
            $.each(result,
                function (i, result) {
                    $("#Credor").append('<option value="' + result.Value + '">' + result.Text + '</option>');
                });

            if (credorIdSelecionado !== '') {
                $('#Credor').val(credorIdSelecionado);
            }
            hideLoading();
        },
        error: function (ex) {
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        }
    });
}

function CarregarProduto(credorIdSelecionado, produtoIdSelecionado) {
     $("#Produto").empty();
     $.ajax({
            type: 'POST',
            url: '/carteira/CarregarProdutos',
            dataType: 'json',
            data: { CredorId: credorIdSelecionado },
            success: function (parametro) {
                $.each(parametro,
                    function (i, parametro) {
                        $("#Produto").append('<option value="' + parametro.Value + '">' + parametro.Text + '</option>');
                });

            if (produtoIdSelecionado !== '') {
                $('#Produto').val(produtoIdSelecionado);
            }

            hideLoading();
        },
        error: function (ex) {
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        }
    });
    return false;
}