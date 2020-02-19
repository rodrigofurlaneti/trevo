
$(document).ready(function () {

    FormatarCampoData("datavencimento");
    FormatarCampoData("iniciocontrato");
    FormatarCampoData("finalcontrato");

    formatarDropDowLists();

    $("#InformarVencimentoDias").change(function () {
        var valor = $(this).val();

        if (valor < 0) {
            $(this).val(0);
            toastr.error("O campo \"Informar Vencimento Antes dos:\" não pode ser negativo");
        }

        //if (valor.indexOf('-') === -1) {
        //    $(this).val(0);
        //    toastr.error("O campo \"Informar Vencimento Antes dos:\" é inválido");
        //}
    });

    $('#NumeroContrato').keyup(function () {
        $(this).val(this.value.replace(/\D/g, ''));
    });
    EscondeCampos();

    ConfiguraCampos();

    $("#tipocontrato").change(function () {
        ConfiguraCampos();
    });

    //$("#valor").maskMoney({
    //    prefix: "R$",
    //    allowNegative: false,
    //    allowZero: true,
    //    thousands: ".",
    //    decimal: ",",
    //    affixesStay: false
    //});
    $("input[class*=valmoney]").maskMoney({
        prefix: "",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false,
        numeralMaxLength: true
    });

    MetodoUtil();

	$("#ContratoForm").submit(function (e) {
		var dadosValidos = true;
        var tipoContratoSelecionado = $("#tipocontrato").val();


        if ($("#iniciocontrato").val() > $("#finalcontrato").val()) {
            toastr.error('Data Inicial Maior que Data Final!', 'Datas Invalidas!');
            dadosValidos = false;
        }

		if ($("#unidade").val() === "") {
			toastr.error('Selecione uma Unidade', 'Unidade Inválida!');
			dadosValidos = false;
		}
		else {
            if (parseInt($("#unidade").val()) === 0) {
                toastr.error('Selecione uma Unidade', 'Unidade Inválida!');
                e.preventDefault();
                return false;
			}
		}

        if ($("#tipocontrato").val() === "") {
			toastr.error('Selecione um Tipo de Contrato!', 'Tipo de Contrato Inválido!');
            e.preventDefault();
            return false;
		}
		else {
            if (parseInt($("#tipocontrato").val()) === 0) {
                toastr.error('Selecione um Tipo de Contrato!', 'Tipo de Contrato Inválido!');
                e.preventDefault();
                return false;
			}
        }


        if (tipoContratoSelecionado === "2" || tipoContratoSelecionado === "3") {



            if (($("#iniciocontrato").val()) === "") {
                toastr.error('Preencha uma Data de Inicio Contrato!', 'Data de Inicio Contrato Obrigatória!');
                dadosValidos = false;
            }

            if (($("#finalcontrato").val()) === "") {
                toastr.error('Preencha uma Data de Final Contrato!', 'Data de Final Contrato Obrigatória!');
                dadosValidos = false;
            }

            if (($("#iniciocontrato").val()) !== '') {
                if (!verificaData($("#iniciocontrato").val())) {
                    toastr.error('Preencha uma Data de Inicio Contrato!!', 'Data de Inicio Contrato Inválida!');
                    dadosValidos = false;
                }
            }

            if (($("#finalcontrato").val()) !== '') {
                if (!verificaData($("#iniciocontrato").val())) {
                    toastr.error('Preencha uma Data de Final Contrato!!', 'Data de Final Contrato Inválida!');
                    dadosValidos = false;
                }
            }

            if ($("#tipovalor").val() === "") {
                toastr.error('Informe um Tipo de Valor válido!', 'Tipo de Valor Inválido!');
                dadosValidos = false;
            }

        }

        if ($("#DiaVencimento").val() === "") {
            toastr.error('Informe um dia vencimento válido!', 'Dia de Vencimento Inválido!');
            dadosValidos = false;
        }

        if ($("#DiaVencimento").val() > 30) {
            toastr.error('Informe um dia vencimento válido!', 'Dia de Vencimento Inválido!');
            dadosValidos = false;
        }        
        
        if ($("#informarvencimentodias").val() === "") {
            toastr.error('Informe campo \'Informar vencimento antes dos: válido!\'', 'Informar vencimento antes dos: Inválido!');
            dadosValidos = false;
        }
        
        if (($("#valor").val()) === "") {
            toastr.error('Preencha um Valor!', 'Valor Obrigatória!');
            dadosValidos = false;
        }

        var num = parseFloat($("#valor").val()).toFixed(2);
        if (num <= 0) {
            toastr.error('O valor não pode ser igual a 0!', 'Valor');
            dadosValidos = false;
        }

		if (!dadosValidos) {
			e.preventDefault();
			return false;
		}
	});


});

function formatarDropDowLists() {

    MakeChosen("unidade");
    MakeChosen("tipocontrato");
    MakeChosen("tipovalor");
    MakeChosen("indicereajuste");
    //MakeChosen("horarioTrabalho");
    //MakeChosen("turnos");
    //MakeChosen("ColaboradorID");


}

function ConfiguraCampos() {
    //debugger;
    EscondeCampos();

    var selecionado = $("#tipocontrato").val();

    if (selecionado === "1") {
        $("#diavencimentofiltro").show();
        $("#informarvencimentodiasfiltro").show();
        $("#valorfiltro").show();
    }
    else if (selecionado === "2") {
        MostraCampos();
    }
    else if (selecionado === "3") {
        MostraCampos();
    }
}

function EscondeCampos() {

    //var element = document.getElementById("camposfiltro");
    //element.style.display = 'none';   

    $("#diavencimentofiltro").hide();
    $("#informarvencimentodiasfiltro").hide();
    $("#valorfiltro").hide();
    $("#tipovalorfiltro").hide();
        
    $("#iniciocontratofiltro").hide();
    $("#finalcontratofiltro").hide();
    $("#existirareajustefiltro").hide();
    $("#indicereajustefiltro").hide();
    $("#existirareajustefiltro").hide();
}

function MostraCampos() {

    //var element = document.getElementById("camposfiltro");
    //element.style.display = 'none';   

    $("#diavencimentofiltro").show();
    $("#informarvencimentodiasfiltro").show();
    $("#valorfiltro").show();
    $("#tipovalorfiltro").show();

    $("#iniciocontratofiltro").show();
    $("#finalcontratofiltro").show();
    $("#existirareajustefiltro").show();
    $("#indicereajustefiltro").show();
    $("#existirareajustefiltro").show();
    
}
