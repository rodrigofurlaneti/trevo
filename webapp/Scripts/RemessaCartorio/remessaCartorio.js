$(document).ready(function () {
    $("#hdnTipoLista").val("ListaContratosCartorio");
    $("#Cpf").mask("999.999.999-99", { reverse: true });

    $("#ValorParcelaInicio").maskMoney({
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });

    $("#ValorParcelaFim").maskMoney({
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });

    $("#PeriodoInicio").maskMoney({
        allowNegative: false,
        allowZero: true,
        thousands: "",
        precision: 0,
        affixesStay: true
    });

    $("#PeriodoFim").maskMoney({
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

    $("#RemessaCartorioForm").submit(function (e) {
        showLoading();

        var dadosValidos = true;

        if ($("#PeriodoInicio").val() === "") {
            toastr.error('Período Inicio é obrigatório!', 'Período Atraso!');
            dadosValidos = false;
        }

        if ($("#PeriodoFim").val() === "") {
            toastr.error('Período Fim é obrigatório!', 'Período Atraso!');
            dadosValidos = false;
        }

        if (dadosValidos) {
            if ($("#PeriodoInicio").val() > ($("#PeriodoFim").val())) {
                toastr.error('Período Inicial não pode ser maior que Período Final!', 'Período Atraso!');
                dadosValidos = false;
            }
        }

        $("#ValorParcelaInicio").val($("#ValorParcelaInicio").val().replace(/\./g, '').replace('R$', ''));
        $("#ValorParcelaFim").val($("#ValorParcelaFim").val().replace(/\./g, '').replace('R$', ''));

        if (!dadosValidos) {
            e.preventDefault();
            hideLoading();
            return false;
        }
    });
 });

function exportar() {
    var dadosValidos = true;

    if ($("#GerarExel").val() === '0') {
        toastr.error('Não existe Parametro de Calculo com a descrição "Negativação" cadastrado para prosseguir!', 'Configuração');
        dadosValidos = false;
    }

    if (!dadosValidos) {
        e.preventDefault();
        return false;
    }

    window.open("/remessacartorio/ExportToExcel");
    hideLoading();
}
