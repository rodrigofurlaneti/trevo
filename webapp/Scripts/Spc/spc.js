$(document).ready(function () {
    $("#hdnTipoLista").val("ListaContratosSpc");
    $("#Cpf").mask("999.999.999-99", { reverse: true });

    $("#CodigoCredor").change(function () {
        $('#CodigoCredorSelecionado').val($('#CodigoCredor').val());
    });

    $("#CodigoProduto").change(function () {
        $('#CodigoProdutoSelecionado').val($('#CodigoProduto').val());
    });

    $("#CodigoCarteira").change(function () {
        $('#CodigoCarteiraSelecionado').val($('#CodigoCarteira').val());
    });

    $("#ValorParcelaDe").maskMoney({
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });

    $("#ValorParcelaAte").maskMoney({
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });

    $("#DataDe").maskMoney({
        allowNegative: false,
        allowZero: true,
        thousands: "",
        precision: 0,
        affixesStay: true
    });

    $("#DataAte").maskMoney({
        allowNegative: false,
        allowZero: true,
        thousands: "",
        precision: 0,
        affixesStay: true
    });
    
    $("#InclusaoSPCForm").submit(function (e) {
        showLoading();

        var dadosValidos = true;

        if ($("#DataAte").val() === "") {
            toastr.error('Período Fim é obrigatório!', 'Período Atraso!');
            dadosValidos = false;
        }

        if ($("#DataDe").val() === "") {
            toastr.error('Período Inicio é obrigatório!', 'Período Atraso!');
            dadosValidos = false;
        }
        
        if (dadosValidos) {
            if ($("#DataDe").val() > ($("#DataAte").val())) {
                toastr.error('Período Inicial não pode ser maior que Período Final!', 'Período Atraso!');
                dadosValidos = false;
            }
        }

        $("#ValorParcelaDe").val($("#ValorParcelaDe").val().replace(/\./g, '').replace('R$', ''));
        $("#ValorParcelaAte").val($("#ValorParcelaAte").val().replace(/\./g, '').replace('R$', ''));

        if (!dadosValidos) {
            e.preventDefault();
            hideLoading();
            return false;
        }

    });

});

