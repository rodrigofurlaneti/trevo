$(document).ready(function () {
    ConfigTabelaGridSemCampoFiltroPrincipal();
    FormatarCampoData("DataCompra");
    ValidaNumero("QuantidadeCpf");
    ValidaNumero("QuantidadeContrato");

    $("#ValorCompra").maskMoney({
        prefix: "R$",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });

    $("#PercLucroEsperado").maskMoney({
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });

    $("#ValorLucroEsperado").maskMoney({
        prefix: "R$",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });

    var CredorSelecionado = $('#IdCredor').val();
    BuscaCredores(CredorSelecionado);

    $("#Credor").change(function () {
        BuscaProdutos($("#Credor").val(), '');
    });

    var ProdutoSelecionado = $('#IdProduto').val();
    if (ProdutoSelecionado !== "") {
        BuscaProdutos(CredorSelecionado, ProdutoSelecionado);
    }

    $("#Credor").change(function () {
        $('#IdCredor').val($('#Credor').val());
    });

    $("#Produto").change(function () {
        $('#IdProduto').val($('#Produto').val());
    });

    $("#frmControleProduto").submit(function (e) {
        var dadosValidos = true;

        if ($("#QuantidadeContrato").val() === "") {
            toastr.error('Informe a Quantidade de Contratos!', 'Quantidade de Contratos Obrigatória!');
            dadosValidos = false;
        }
        else {
            if (parseInt($("#QuantidadeContrato").val()) === 0) {
                toastr.error('Quantidade de Contratos não deve ser 0!', 'Quantidade de Contratos Inválido!');
                dadosValidos = false;
            }
        }

        if ($("#QuantidadeCpf").val() === "") {
            toastr.error('Informe a Quantidade de CPFs!', 'Quantidade de CPFs Obrigatória!');
            dadosValidos = false;
        }
        else {
            if (parseInt($("#QuantidadeCpf").val()) === 0) {
                toastr.error('Quantidade de CPFs não deve ser 0!', 'Quantidade de CPFs Inválido!');
                dadosValidos = false;
            }
        }

        if ($("#ValorCompra").val() === "") {
            toastr.error('Informe o Valor da Compra!', 'Valor da Compra Obrigatória!');
            dadosValidos = false;
        }

        if (($("#DataCompra").val()) == '') {
            toastr.error('Preencha uma Data da Compra!', 'Data da Compra Obrigatória!');
            dadosValidos = false;
        }

        if (($("#DataCompra").val()) != '') {
            if (!verificaData($("#DataCompra").val())) {
                toastr.error('Preencha uma Data da Compra Válida!', 'Data da Compra Inválida!');
                dadosValidos = false;
            }
        }

        if ($("#Descricao").val() === "") {
            toastr.error('Informe a Descrição!', 'Descrição Obrigatória!');
            dadosValidos = false;
        }

        if ($('#IdProduto').val() === "") {
            toastr.error('Informe o Produto!', 'Produto Obrigatório!');
            dadosValidos = false;
        }

        if ($('#IdCredor').val() === "") {
            toastr.error('Informe o Credor!', 'Credor Obrigatório!');
            dadosValidos = false;
        }

        if (!dadosValidos) {
            e.preventDefault();
            return false;
        }

        var form = [
            { name: "Id", value: $("#Id").val() },
            { name: "Descricao", value: $("#Descricao").val() },
            { name: "DataCompra", value: $("#DataCompra").val() },
            { name: "ValorCompra", value: $("#ValorCompra").val($("#ValorCompra").val().replace(/\./g, '').replace('R$', ''))},
            { name: "QuantidadeCpf", value: $("#QuantidadeCpf").val() },
            { name: "QuantidadeContrato", value: $("#QuantidadeContrato").val() },
            { name: "PercLucroEsperado", value: $("#PercLucroEsperado").maskMoney("unmasked")[0] },
            { name: "ValorLucroEsperado", value: $("#ValorLucroEsperado").val($("#ValorLucroEsperado").val().replace(/\./g, '').replace('R$', '')) },
            { name: "IdCredor", value: $("#IdCredor").val() },
            { name: "IdProduto", value: $("#IdProduto").val() }

        ];

        if (dadosValidos) {
            submit($(this).prop("action"), "POST", form);
        }
    });
});


function BuscaCredores(credorId) {
    $("#Credor").empty();
    $.ajax({
        type: 'POST',
        url: '/ControleProduto/CarregarCredores',
        dataType: 'json',
        success: function (result) {
            $.each(result,
                function (i, result) {
                    $("#Credor").append('<option value="' + result.Value + '">' + result.Text + '</option>');
                });

            if (credorId !== '') {
                $('#Credor').val(credorId);
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

function BuscaProdutos(credorId, produtoId) {
    $("#Produto").empty();
    $.ajax({
        type: 'POST',
        url: '/ControleProduto/CarregarProdutos',
        dataType: 'json',
        data: { CredorId: credorId },
        success: function (result) {
            $.each(result,
                function (i, result) {
                    $("#Produto").append('<option value="' + result.Value + '">' + result.Text + '</option>');
                });

            if (produtoId !== '') {
                $('#Produto').val(produtoId);
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