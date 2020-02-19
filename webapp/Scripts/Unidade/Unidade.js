var listaequipamentos = [];
var parametroPassar;

$(document).ready(function () {
    $("input[name=quantidade]").change(function () {
        var valor = $(this).val();

        if (valor < 0) {
            $(this).val(0);
            toastr.error("O campo \"Quantidade Equipamento\" não pode ser negativo");
        }
    });

    FormataCampoCnpj("cpf");

    $('.horarioPreco').keyup(function () {
        $(this).val(this.value.replace(/\D/g, ''));
    });
    $("#UnidadeForm").submit(function (e) {

        if (ValidaCampos()) {
            return true;
        }

        return false;
    });

    MakeChosen("tipoUnidade", null, "100%");
    MakeChosen("supervisorUnidade", null, "100%");
    MakeChosen("tipoPagamentoUnidade", null, "100%");
    MakeChosen("tabelaPrecoUnidade", null, "100%");
    MakeChosen("empresaUnidade", null, "100%");

    $("input[class*=valmoney]").maskMoney({
        prefix: "",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: "",
        affixesStay: false
    });

    maskTime('time');

    BuscarUnidade();

    $("#cep").on("keyup", function (e) {
        $(this).val(
            $(this).val()
                .replace(/\D/g, "")
                .replace(/^(\d{5})?(\d{3})/, "$1-$2"));
    });

    $("#search-icon").on("click",
        function () {
            BuscarCep();
        });

    $("#cep").blur(function () {
        BuscarCep();
    });
    $("#CNPJ_Numero").mask("99.999.999/9999-99");


    $("#equipamentostab tbody").change(function () {


        SalvarDadosEquipamentos();

    });

    $("#check-list-atividade").change(function () {
        CarregarCheckListAtividade(this.value ? this.value : 0);
    });

    function BuscarCep() {
        var cep = $("#cep").val().replace(/\D/g, "");
        if (cep !== "") {
            var validacep = /^[0-9]{8}$/;
            if (validacep.test(cep)) {
                $("#invalid_zipcode").hide();
                $("#address, #district, #city, #state").val("...");

                $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?", function (dados) {

                    if (!("erro" in dados)) {
                        $("#logradouro").val(dados.logradouro);
                        $("#bairro").val(dados.bairro);
                        $("#cidade").val(dados.localidade);
                        //$("#state").val(dados.uf);
                    }
                    else {
                        clear_form();
                        toastr.error("CEP não encontrado.");
                    }
                });
            }
            else {
                clear_form();
            }
        }
        else {
            $("#invalid_zipcode").hide();
            clear_form();
        }
    }




    $(window).load(function () {

        MakeChosenMult("tipoPagamentoUnidade");

        BuscarCep();

    });

});

function ValidaCampos() {
    if ($("#nomeUnidade").val() === "") {
        toastr.error("O campo \"Nome da Unidade\" é obrigatório");
        return false;
    }

    if (document.getElementById('tipoUnidade').value === '') {
        toastr.error("O campo \"Tipo da Unidade\" é obrigatório");
        return false;
    }

    if (document.getElementById('supervisorUnidade').value === '') {
        toastr.error("O campo \"Supervisor da Unidade\" é obrigatório");
        return false;
    }

    if (!CnpjValido($("#cpf").val())) {
        toastr.error('Preencha um CNPJ válido da unidade!', 'CNPJ Inválido!');
        return false;
    }

    let diaVencimento = $("#diaVencimento").val();
    if (diaVencimento === '' || diaVencimento === undefined || diaVencimento === null || parseInt(diaVencimento) <= 0) {
        toastr.error("O campo \"Dia de Vencimento\" é obrigatório.");
        return false;
    }

    let horarioInicial = $("#horarioInicial").val();
    if (horarioInicial === '') {
        toastr.error("O campo \"Horário Inicial\" é obrigatório");
        return false;
    }

    let horarioInicialInt = parseInt(horarioInicial.replace(':', ''));
    if (horarioInicialInt >= 2400) {
        toastr.error("O campo \"Horário Inicial\" deve estar no intervalo de 00:00 - 23:59");
        return false;
    }

    let horarioFinal = $("#horarioFinal").val();
    if (horarioFinal === '') {
        toastr.error("O campo \"Horário Final\" é obrigatório");
        return false;
    }

    let horarioFinalInt = parseInt(horarioFinal.replace(':', ''));
    if (horarioFinalInt >= 2400) {
        toastr.error("O campo \"Horário Final\" deve estar no intervalo de 00:00 - 23:59");
        return false;
    }

    if ($("#cep").val() === '') {
        toastr.error("O campo \"Cep\" é obrigatório");
        return false;
    }

    if ($("#logradouro").val() === '') {
        toastr.error("O campo \"Endereço\" é obrigatório");
        return false;
    }

    if ($("#numeroVaga").val() === '') {
        toastr.error("O campo \"Número de Vagas da Unidade\" é obrigatório");
        return false;
    }

    if ($("#empresaUnidade").val() === '') {
        toastr.error("O campo \"Empresa\" é obrigatório");
        return false;
    }

    if ($("#codigo").val() === '') {
        toastr.error("O campo \"Código\" é obrigatório");
        return false;
    }

    if ($("#tipoPagamentoUnidade").val() === null || $("#tipoPagamentoUnidade").val() === 'null') {
        toastr.error("O campo \"Tipo de Pagamento da Unidade\" é obrigatório");
        return false;
    }

    var arrayTipoPagamento = $("#tipoPagamentoUnidade");

    if (arrayTipoPagamento.val().includes("1") || $("#tipoPagamentoUnidade").val().includes("2")) {

        if (!CnpjValido($("#MaquinaCartao_CNPJ_Numero").val())) {
            toastr.error('Preencha um CNPJ válido da máquina de cartão!', 'CNPJ Inválido!');
            dadosValidos = false;
            return false;
        }

        if ($("#MaquinaCartao_MarcaMaquina").val() === '') {
            toastr.error("O campo \"Marca máquina de Cartão\" é obrigatório");
            return false;
        }

        if ($("#supervisorResponsavel").val() === '') {
            toastr.error("O campo \"Responsável Máquina Cartão\" é obrigatório");
            return false;
        }

        //verificação para atender a GTE-1785 (o campo na base é not nullable)
        if ($("#MaquinaCartao_Observacao").val() === '') {
            toastr.error("O campo \"Observação máquina de cartão\" é obrigatório");
            return false;
        }
    }

    if ($("#cidade").val() === '') {
        toastr.error("O campo \"Cidade\" é obrigatório");
        return false;
    }

    return true;
}

function BuscarUnidade() {
    var unidade = document.getElementById("nomeUnidadeBusca");

    showLoading();
    $.ajax({
        url: "/Unidade/BuscarUnidade",
        type: "POST",
        data: { unidade: unidade.value },
        success: function (response) {
            if (response.Status === "Success") {
                $('#divUnidade').empty();
                $('#divUnidade').append(response.Html);
                ConfigTabelaGridSemCampoFiltroPrincipal();
            }
        },
        error: function (error) {
            window.alert('Error');
        }
    });

    hideLoading();
}

function HabilitaQuantidade(teste) {
    var id_div = "#".concat(teste.id.replace("Checkado_CheckBox", "Div"));
    var id_button = "#".concat(teste.id.replace("Checkado_CheckBox", "Button"));


    if ($(id_div).is(':visible')) {
        $(id_div).hide();
        $(id_button).show();
    }
    else {
        $(id_div).show();
        $(id_button).hide();
    }
}

function SalvarDadosEquipamentos() {
    listaequipamentos.length = 0;
    var escopoQuantidade;
    var escopoBotao;
    $("#equipamentostab tbody").find('tr').each(function (rowIndex, r) {
        if ($(this).find('input[name="ativo"]:checked').length === 1) {
            $(this).find('input[name="quantidade"]').show();
            $(this).find('button[name="btnAdd"]').hide();
        }
        else {
            $(this).find('input[name="quantidade"]').val("0");
            $(this).find('input[name="quantidade"]').hide();
            $(this).find('button[name="btnAdd"]').show();
        }

        var periodohorario = {
            Id: $(this).find('input').attr("id"),
            EstruturaGaragem: $(this).find('input[name="equipamento"]').val(),
            Quantidade: $(this).find('input[name="quantidade"]').val(),
            Ativo: $(this).find('input[name="ativo"]:checked').length === 1 ? 'true' : 'false'
        }

        listaequipamentos.push(periodohorario);

    });

    showLoading();
    $.post("/unidade/AtualizaEquipamentos", { listquipamentos: listaequipamentos })
        .done((response) => {
            if (typeof (response) === "object") {
                openCustomModal(null,
                    null,
                    response.TipoModal,
                    response.Titulo,
                    response.Mensagem,
                    false,
                    null,
                    function () { });
            } else {
                $("#lista-equipamentos-result")
                    .empty()
                    .append(response);
            }
        })
        .fail(() => { })
        .always(() => { hideLoading(); });
}


function CarregarCheckListAtividade(id) {
    return post("CarregarCheckListAtividade", { id })
        .done((response) => {
            $("#lista-checklist-result")
                .empty()
                .append(response);
        })
}

function AlternarStatusTipoAtividade(id) {
    return post("AlternarStatusTipoAtividade", { id })
}

function VerificarCPF() {
    var cpf = $("#cpf").val();

    if (!CnpjValido(cpf)) {
        toastr.error("Preencha um CNPJ válido!", "CNPJ Inválido!");
        return false;
    }

    return true;
}

function MostrarMensagemCompra() {

    toastr.success("Compra realizada!");
}

function maskTime(className) {
    $("." + className).mask("99:99");
}