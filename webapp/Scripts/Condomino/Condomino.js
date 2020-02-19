var totalVagas = 0;

$(document).ready(function () {

    $("textarea#textAreaVagas").prop('disabled', true);

    MakeChosen("type");
    MakeChosen("tipoContato");
    MudarTipoPessoa($("#tipo-pessoa").val());

    $("#cnpj").on("keyup", function (e) {
        $(this).val(
            $(this).val()
                .replace(/\D/g, '')
                .replace(/^(\d{2})(\d{3})?(\d{3})?(\d{4})?(\d{2})?/, "$1.$2.$3/$4-$5"));
    });

    $("#tipo-pessoa").on("change", function () {
        MudarTipoPessoa(this.value);
    })

    FormatarCampoData("dtNascimento");
    FormataCampoCpf("cpf");
    FormataCampoRg("Pessoa_Rg");

    $("#Pessoa_Nome").keyup(function () {
        if (!somenteLetra(this.value)) {
            if (!somenteLetra(this.value.substring(this.value.length - 1, this.value.length)))
                this.value = this.value.substring(0, this.value.length - 1);
        }
    });

    $('#cpf').on('blur', function () {
        VerificarCPF();
    });

    $('#cnpj').on('blur', function () {
        VerificarCNPJ();
    });

    $('#txtVagasAdquiridas').on('blur', function () {
        ValidaTotalVagas();
    });

    $('#cbContratoMensalista_Unidade').change(function () {
        CarregarVagasCondominos();
    });

    $("#CondominoForm").submit(function (e) {
        var dadosValidos = true;

        if ($("#cbContratoMensalista_Unidade").val() === "") {
            toastr.error('Preencha a Unidade', 'A Unidade é Obrigatória');
            dadosValidos = false;
        }

        // Pessoa Fisica
        if ($("#tipo-pessoa").val() == 1) {
            if (!$("#Pessoa_Nome").val()) {
                toastr.error('Preencha o Nome', 'Nome é Obrigatório');
                dadosValidos = false;
            }

            else if (!somenteLetra($("#Pessoa_Nome").val())) {
                toastr.error("O campo nome não permite caracteres númericos!", "Nome inválido!");
                dadosValidos = false;
            }

            else if (!VerificarCPF()) {
                dadosValidos = false;
            }

            else if (!verificaData($("#dtNascimento").val())) {
                toastr.error('Preencha uma Data de Nascimento válida!', 'Data de Nascimento Inválida!');
                dadosValidos = false;
            }
        }
        // Pessoa Juridica
        else if ($("#tipo-pessoa").val() == 2) {
            if (!$("#nome_fantasia").val()) {
                toastr.error('Informe o Nome Fantasia', 'Campo obrigatório');
                dadosValidos = false;
            }
            else if (!$("#cnpj").val()) {
                toastr.error('Informe o CNPJ', 'Campo obrigatório');
                dadosValidos = false;
            }
        } else {
            toastr.warning('Selecione um tipo de pessoa', 'Alerta');
            dadosValidos = false;
        }

        if ($("#tipo-pessoa").val() == 1 || $("#tipo-pessoa").val() == 2){
            if (!enderecos.length) {
                toastr.error('Informe pelo menos um endereço', 'Campo obrigatório');
                dadosValidos = false;
            }
            else if (!contatos.length) {
                toastr.error('Informe pelo menos um contato', 'Campo obrigatório');
                dadosValidos = false;
            }
        }

        if (!dadosValidos) {
            e.preventDefault();
            return false;
        }
    });

    $("#marca").change(function () {
        $('#IdMarca').val($('#marca').val());
    });

    $.ajax({
        type: 'POST',
        url: '/condomino/CarregarModelos',
        dataType: 'json',
        success: function (result) {
            $.each(result,
                function (i, result) {
                    $("#modelo").append('<option value="' + result.Value + '">' + result.Text + '</option>');
                });
            hideLoading();
            var identify = $('#IdModelos').val();
            if (identify !== '') {
                $('#marca').val($('#IdModelos').val());
            }
        },
        error: function (ex) {
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        }
    });

    $("#modelo").change(function () {
        $('#IdModelos').val($('#modelo').val());
    });

    let idCondomino = $("#hdnCondomino").val();
    if (idCondomino) {

        let cpf = $("#cpf");
        if (cpf.val()) {
            cpf.attr("readonly", 'readonly');
        }
        
        let cnpj = $("#cnpj");
        if (cnpj.val()) {
            cnpj.attr("readonly", 'readonly');
        }
    }

    buscarCondominos();

});

function buscarCondominos() {
    BuscarPartialSemFiltro("/Condomino/BuscarCondominos", "#lista-tabela-condomino")
        .done(function () {
            ConfigTabelaGridSemCampoFiltroPrincipal();
        });
}

function ValidaTotalVagas()
{
    var vagas = $("#txtVagasAdquiridas").val();
    if (vagas > totalVagas)
        toastr.warning("Não é possível definir uma quantidade de vagas maior que o total disponivel na unidade", "Atenção");
}

function CarregarVagasCondominos() {
    var idUnidade = $("#cbContratoMensalista_Unidade").find(':selected').val();
    console.log(idUnidade);
    if (idUnidade == null) {
        toastr.info('É necessário selecionar uma unidade para atualizar a lista de vagas!');
    }
    else {
        showLoading();

        $.ajax({
            url: '/Condomino/CarregarVagasUnidade',
            dataType: 'json',
            type: 'POST',
            data: { idUnidade: idUnidade },
            success: function (data) {
                $("textarea#textAreaVagas").val(data.VagaDetalhe);
                totalVagas = data.totalVagas;
                hideLoading();
            },
            error: function (ex) {
                hideLoading();
                console.log(JSON.stringify(ex));
                alert("ERRO ao buscar o detalhe das vagas: " + ex);
            }
        });
    }
}

function VerificarCPF() {
    var cpf = $("#cpf").val();

    if (!CPFValido(cpf)) {
        toastr.error("Preencha um CPF válido!", "CPF Inválido!");
        return false;
    }

    return true;
}

function VerificarCNPJ() {
    var cnpj = $("#cnpj").val();

    if (!CnpjValido(cnpj)) {
        toastr.error("Preencha um CNPJ válido!", "CNPJ Inválido!");
        return false;
    }

    return true;
}

function MudarTipoPessoa(tipoPessoa) {
    switch (tipoPessoa) {
        // Fisica
        case "1":
            $(".container-pessoa-fisica").show();
            $(".container-pessoa-juridica").hide();
            break;
        // Juridica
        case "2":
            $(".container-pessoa-fisica").hide();
            $(".container-pessoa-juridica").show();
            break;
        default:
            $(".container-pessoa-fisica").hide();
            $(".container-pessoa-juridica").hide();
            break;
    }
}