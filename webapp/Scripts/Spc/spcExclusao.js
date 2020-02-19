$(document).ready(function () {
    $("#hdnTipoLista").val("ListaExclusaoSpc");
    $("#Cpf").mask("999.999.999-99", { reverse: true });

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

    $("#CodigoCredor").change(function () {
        $('#CodigoCredorSelecionado').val($('#CodigoCredor').val());
    });

    $("#CodigoProduto").change(function () {
        $('#CodigoProdutoSelecionado').val($('#CodigoProduto').val());
    });

    $("#CodigoCarteira").change(function () {
        $('#CodigoCarteiraSelecionado').val($('#CodigoCarteira').val());
    });

    $("#ExclusaoSPCForm").submit(function (e) {
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

        if (!dadosValidos) {
            e.preventDefault();
            hideLoading();
            return false;
        }
    });

});
