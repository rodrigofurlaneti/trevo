var indiceContatosAdicionados = 0;
var indiceEnderecosAdicionadosComercial = 0;
var indiceEnderecosAdicionadosResidencial = 0;
var contratos = [];
var indiceContratosAdicionados = 0;

$(document).ready(function () {
    callbackPaginacao = BuscarClientes;

    MakeChosen("unidades");
    ResetCarteirasChosen();
    MakeChosen("tipoContato");
    MakeChosen("conta-corrente-contratos");

    $("#cnpj").on("keyup", function (e) {
        $(this).val(
            $(this).val()
                .replace(/\D/g, '')
                .replace(/^(\d{2})(\d{3})?(\d{3})?(\d{4})?(\d{2})?/, "$1.$2.$3/$4-$5"));
    });

    FormatarCampoData("dtNascimento");
    FormatarCampoData("dtInicio");
    FormatarCampoData("dtFim");
    FormataCampoCpf("cpf");
    FormataCampoRg("Cliente_Pessoa_Rg");

    $("#vlrPago").maskMoney({
        prefix: "R$",
        allowNegative: false,
        allowZero: false,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });
    $("#vlrPago").change(function () {
        var value = $(this).val();
        if (value !== "" && value !== null && value !== undefined) {
            $(this).val(value.replace(".", ""));
        }
    });

    $("#Cliente_Pessoa_Nome").keyup(function () {
        if (!somenteLetra(this.value)) {
            if (!somenteLetra(this.value.substring(this.value.length - 1, this.value.length)))
                this.value = this.value.substring(0, this.value.length - 1);
        }
    });

    $('#cpf').on('blur', function () {
        VerificarCPF();
        if (BuscarCPF() === false)
            CarregarContratosMensalistas();
    });

    $('#cnpj').on('blur', function (ele) {
        VerificarCNPJ();
        if (BuscarCNPJ() === false)
            CarregarContratosMensalistas();
    });

    $("#ClienteForm").submit(function (e) {
        var dadosValidos = true;
        enderecos = [];
        contatos = [];

        let tipoPessoa = $("input[name='Cliente.TipoPessoa']:checked").val();

        switch (tipoPessoa) {
            // Pessoa Fisica
            case "1":
                if (!$("#Cliente_Pessoa_Nome").val()) {
                    toastr.error('Preencha o Nome', 'Nome é Obrigatório');
                    dadosValidos = false;
                }
                else if (!somenteLetra($("#Cliente_Pessoa_Nome").val())) {
                    toastr.error("O campo nome não permite caracteres númericos!", "Nome inválido!");
                    dadosValidos = false;
                }
                else if (!VerificarCPF()) {
                    dadosValidos = false;
                }
                break;
            // Pessoa Juridica
            case "2":
                if (!$("#razao_social").val()) {
                    toastr.error('Informe a Razão Social', 'Campo obrigatório');
                    dadosValidos = false;
                }
                if (!$("#cnpj").val()) {
                    toastr.error('Informe o CNPJ', 'Campo obrigatório');
                    dadosValidos = false;
                }
                break;
            default:
                break;
        }

        var CepComercial = $("#zipcode-comercial").val();
        var CepResidencial = $("#zipcode").val();

        if (!CepComercial && !CepResidencial) {
            toastr.error('Informe pelo menos um endereço', 'Campo obrigatório');
            dadosValidos = false;
        }
        else {
            if (CepComercial) {
                adicionarEnderecoComercial();
            }

            if (CepResidencial) {
                adicionarEnderecoResidencial();
            }
        }

        adicionarContatocadastroCliente();

        if ($("#unidades_chosen li.search-choice").length === 0) {
            toastr.error('Informe pelo menos uma unidade', 'Campo obrigatório');
            dadosValidos = false;
        }

        if (!dadosValidos) {
            e.preventDefault();
            return false;
        }

        showLoading();
    });

    $("#marca-comercial").change(function () {
        $('#IdMarca-comercial').val($('#marca-comercial').val());
    });

    $.ajax({
        type: 'POST',
        url: '/cliente/CarregarModelos',
        dataType: 'json',
        success: function (result) {
            $.each(result,
                function (i, result) {
                    $("#modelo-comercial").append('<option value="' + result.Value + '">' + result.Text + '</option>');
                });
            hideLoading();
            var identify = $('#IdModelos-comercial').val();
            if (identify !== '') {
                $('#marca-comercial').val($('#IdModelos-comercial').val());
            }
        },
        error: function (ex) {
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        }
    });

    $("#modelo-comercial").change(function () {
        $('#IdModelos-comercial').val($('#modelo-comercial').val());
    });

    let idCliente = $("#hdnCliente-comercial").val();
    if (idCliente) {

        let cpf = $("#cpf");
        if (cpf.val()) {
            cpf.attr("readonly", 'readonly');
            cpf.attr("disabled", 'disabled');
        }

        let cnpj = $("#cnpj");
        if (cnpj.val()) {
            cnpj.attr("readonly", 'readonly');
            cnpj.attr("disabled", 'disabled');
        }
    }

    $("#tipo-pessoa-fisica").on("change", function () {
        MudarTipoPessoa(this.value, true);
    });

    $("#tipo-pessoa-juridica").on("change", function () {
        MudarTipoPessoa(this.value, true);
    });

    $("#tipo-pessoa-fisica").val("1");
    MudarTipoPessoa($("#tipo-pessoa-fisica").val());

    $("#unidades").change(function () {
        CarregarUnidadesCadastroCliente();
    });

    $('#unidade').change(function () {
        CarregarTabelaPrecoMensalista(this.value);
    });

    MarcaAutoComplete();
    ModeloAutoComplete();

});


function adicionarEnderecoComercial() {
    indiceEnderecosAdicionadosComercial--;
    var endereco = {
        Id: indiceEnderecosAdicionadosComercial,
        Tipo: 2,
        Cep: $("#zipcode-comercial").val(),
        Cidade: {
            Descricao: $("#city-comercial").val(),
            Estado: {
                Sigla: $("#state-comercial").val()
            }
        },
        CidadeDescricao: $("#city-comercial").val(),
        Estado: $("#state-comercial").val(),
        Logradouro: $("#address-comercial").val(),
        Numero: $("#number-comercial").val(),
        Bairro: $("#district-comercial").val(),
        Complemento: $("#complement-comercial").val(),
        Blacklist: $("#blacklist-comercial").val(),
        TipoMotivo: $("#tipoMotivo-comercial").val(),
        DescricaoMotivo: $("#descricaoMotivo-comercial").val()
    };

    enderecos.push(endereco);
}

function adicionarEnderecoResidencial() {
    indiceEnderecosAdicionadosResidencial--;
    var enderecoResidencial = {
        Id: indiceEnderecosAdicionadosResidencial,
        Tipo: 1,
        Cep: $("#zipcode").val(),
        Cidade: {
            Descricao: $("#city").val(),
            Estado: {
                Sigla: $("#state").val()
            }
        },
        CidadeDescricao: $("#city").val(),
        Estado: $("#state").val(),
        Logradouro: $("#address").val(),
        Numero: $("#number").val(),
        Bairro: $("#district").val(),
        Complemento: $("#complement").val(),
        Blacklist: $("#blacklist").val(),
        TipoMotivo: $("#tipoMotivo").val(),
        DescricaoMotivo: $("#descricaoMotivo").val()
    };

    enderecos.push(endereco);
}

function GetCarteirasByFuncao(ele) {
    $("#carteiras").empty();
    $("#carteiras").trigger("chosen:update");
    $("#carteiras").val("").trigger("chosen:updated");

    $.ajax({
        url: "/cliente/Carteiras",
        type: "POST",
        dataType: "json",
        success: function (carteiras) {
            hideLoading();

            $("#lblMensagemCarteiraResultado").text("");

            if (carteiras !== undefined && carteiras !== null && carteiras.length > 0) {
                $(carteiras).each(function () {
                    $("<option />", {
                        val: this.Id,
                        text: this.RazaoSocial
                    }).appendTo("#carteiras");
                });
                ResetCarteirasChosen();

            } else {
                $("#lblMensagemCarteiraResultado").text("Não possui carteiras disponíveis!");
            }
        },
        error: function (error) {
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        }
    });
}

function ResetCarteirasChosen() {
    //Entidade.Uteis.Constantes.Funcao [Enum]
    var optionsCarteira = $("#carteiras > option").length;
    var lojaLength = optionsCarteira === undefined || optionsCarteira === null ? 0 : optionsCarteira;
    var maxSelectedOptions = lojaLength;
    MakeChosen("carteiras", maxSelectedOptions);

    $("#carteiras").off("change");
    $("#carteiras").change(function (event) {
        var obj = [];
        $("select#carteiras option:selected").each(function () {
            obj.push({ Id: $(this).val(), CarteiraProduto: $(this).text() });
        });

        $.ajax({
            url: "/cliente/carteirasSelecionadas",
            type: "POST",
            dataType: "json",
            data: { json: JSON.stringify(obj) },
            success: function (response) {
                hideLoading();
            },
            error: function (error) {
                hideLoading();
            },
            beforeSend: function () {
                showLoading();
            }
        });
    });
}

function AddBlacklist(tipo, parametro, tipoMotivoBusca) {
    $("#hdnParametro").val(parametro);
    $("#hdnTipo").val(tipo);

    $.ajax({
        url: "/cliente/PopularMotivos",
        datatype: "JSON",
        type: "GET",
        data: { tipoMotivoBusca: tipoMotivoBusca },
        success: function (response) {
            if (typeof response === "object") {
                openCustomModal(null,
                    null,
                    response.TipoModal,
                    response.Titulo,
                    response.Mensagem,
                    false,
                    null,
                    function () { });
            }
            else {
                if (tipoMotivoBusca === "2") {
                    $("#msgBlacklist").innerHTML = "Deseja remover o registro da blacklist?";
                } else {
                    $("#msgBlacklist").innerHTML = "Deseja adicionar o registro na blacklist?";
                }

                $("#drop-result").empty();
                $("#drop-result").append(response);
            }
        },
        beforeSend: function () {
            showLoading();
        }
    });
    hideLoading();
    $("#myModalBlacklist").modal();
}

function ComfirmarBlacklist() {
    var parametro = $("#hdnParametro").val();
    var tipo = $("#hdnTipo").val();
    var idMotivo = $("#ddlMotivo").val();
    var descMotivo = $("#ddlMotivo option:selected").text();

    $("#myModalBlacklist").modal("hide");
    showLoading();

    $.ajax({
        url: "/cliente/ComfirmarBlacklist",
        type: "POST",
        //dataType: "json",
        data: {
            tipo: tipo, parametro: parametro, idMotivo: idMotivo, descMotivo: descMotivo
        },
        success: function (result) {
            if (typeof result === "object") {
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
                if (tipo === "endereco") {
                    $("#lista-endereco-result").empty();
                    $("#lista-endereco-result").append(result);
                } else {
                    $("#lista-contato-result").empty();
                    $("#lista-contato-result").append(result);
                }
            }
        },
        error: function (xhr, status, errorThrown) {
            //Here the status code can be retrieved like;
            xhr.status;

            //The message added to Response object in Controller can be retrieved as following.
            xhr.responseText;
        }
    });
    hideLoading();
}

function ModalHistorico(idCliente) {
    showLoading();
    $.ajax({
        url: "/cliente/ObterHistorico",
        datatype: "JSON",
        type: "GET",
        data: { idCliente: JSON.stringify(idCliente) },
        success: function (response) {
            if (typeof response === "object") {
                openCustomModal(null,
                    null,
                    response.TipoModal,
                    response.Titulo,
                    response.Mensagem,
                    false,
                    null,
                    function () { });
            }
            else {
                $("#grid-result").empty();
                $("#grid-result").append(response);
                $("#grid-result table").dataTable();
            }
        },
        complete: function () {
            hideLoading();
            $("#myModalSituacao").modal();
        }
    });
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

function BuscarClientes(pagina = 1) {
    var documento = $("#cpfBusca").val();
    var nome = $("#nomeBusca").val();
    var contrato = $("#contratoBusca").val();

    $.ajax({
        url: "/cliente/BuscarClientes",
        type: "POST",
        dataType: "json",
        data: {
            documento,
            nome,
            contrato,
            pagina
        },
        success: function (result) {
            $("#cpfBusca").val(result.documentoFormatado);

            if (typeof result.resultado === "object" && result.resultado.Titulo !== null && result.resultado.Titulo !== "") {
                openCustomModal(null,
                    null,
                    result.resultado.TipoModal,
                    result.resultado.Titulo,
                    result.resultado.Mensagem,
                    false,
                    null,
                    function () { });
            }
            else {
                $("#lista-clientes").empty();
                $("#lista-clientes").append(result.grid);
            }
        },
        error: function (error) {
            $("#lista-clientes").empty();
            toastr.error("Ocorreu um erro ao fazer a busca dos clientes, tente novamente.", "Atenção");
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

function buscarDivida(id) {
    $.ajax({
        url: "/cliente/BuscarDivida",
        type: "GET",
        dataType: "json",
        data: { id: id },
        success: function (result) {
            if (typeof result === "object") {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () { });
            } else {
                $("#grid-divida").empty();
                $("#grid-divida").append(result);
            }
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }, error: function (error) {
            if (typeof result === "object") {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () { });
            } else {
                $("#grid-divida").empty();
                $("#grid-divida").append(error.responseText);
            }
            hideLoading();
        }
    });
}

function MudarTipoPessoa(tipoPessoa, limpar) {
    switch (tipoPessoa) {
        // Fisica
        case "1":
            $(".container-pessoa-fisica").show();
            $(".container-pessoa-juridica").hide();

            $("#div-endereco-comercial").css("display", "inline");
            $("#div-endereco-residencial").css("display", "inline");

            if (limpar) {
                LimparCamposPessoaFisica();
                LimparCamposEnderecoResidencial();
                LimparCamposEnderecoComercial();
            }
            break;
        // Juridica
        case "2":
            $(".container-pessoa-fisica").hide();
            $(".container-pessoa-juridica").show();

            $("#div-endereco-comercial").css("display", "inline");
            $("#div-endereco-residencial").css("display", "none");
            LimparCamposEnderecoResidencial();

            if (limpar) {
                LimparCamposPessoaJuridica();
                LimparCamposEnderecoComercial();
            }
            break;
        default:
            $(".container-pessoa-fisica").hide();
            $(".container-pessoa-juridica").hide();

            $("#div-endereco-comercial").css("display", "inline");
            $("#div-endereco-residencial").css("display", "inline");

            if (limpar) {
                LimparCamposPessoaFisica();
                LimparCamposPessoaJuridica();
            }
            break;
    }
}

function LimparCamposEnderecoResidencial() {
    $("#zipcode").val("");
    $("#address").val("");
    $("#district").val("");
    $("#city").val("");
    $("#state").val("");
    $("#city-id").val("");
}

function LimparCamposEnderecoComercial() {
    $("#zipcode-comercial").val("");
    $("#address-comercial").val("");
    $("#district-comercial").val("");
    $("#city-comercial").val("");
    $("#state-comercial").val("");
    $("#city-comercial-id").val("");
}

function LimparCamposPessoaFisica() {
    $("#Pessoa_Nome").val("");
    $("#cpf").val("");
    $("#Pessoa_Rg").val("");
    $("#dtNascimento").val("");
    $("[id*=Pessoa_Sexo]").each(function () {
        $(this).prop("checked", "");
        if ($(this).val() === "Masculino")
            $(this).prop("checked", "checked");
    });
}
function LimparCamposPessoaJuridica() {
    $("#nome_fantasia").val("");
    $("#razao_social").val("");
    $("#cnpj").val("");
    $("#inscricao_social").val("");
    $("#inscricao_municipal").val("");
}

function BuscarCPF() {
    var cpf = $("#cpf").val();
    if (cpf !== "") {
        $.ajax({
            url: "/cliente/BuscarCPF",
            type: "POST",
            dataType: "json",
            data: { cpf: cpf },
            success: function (id) {
                if (id > 0) {
                    $("#cpf").val("");
                    toastr.warning("CPF já possue cadastro no Sistema, aguarde enquanto é aberto o cadastro!", "CPF Já Cadastrado!");
                    window.location = "/Cliente/Edit/" + id;
                    return true;
                }
                return false;
            }
        });
    }

    return true;
}

function BuscarCNPJ() {
    var cnpj = $("#cnpj").val();
    if (cnpj !== "") {
        $.ajax({
            url: "/cliente/BuscarCNPJ",
            type: "POST",
            dataType: "json",
            data: { cnpj: cnpj },
            success: function (id) {
                if (id) {
                    $("#cnpj").val("");
                    toastr.warning("CNPJ já possue cadastro no Sistema, aguarde enquanto é aberto o cadastro!", "CNPJ Já Cadastrado!");
                    window.location = "/Cliente/Edit/" + id;
                    return true;
                }
                return false;
            }
        });
    }

    return true;
}

function CarregarContratosMensalistas(documento) {
    $.ajax({
        type: 'POST',
        url: '/Cliente/CarregarContratosMensalistas',
        dataType: 'json',
        data: {
            documento: documento
        },
        success: function (response) {
            if (typeof response === "object") {
                openCustomModal(null,
                    null,
                    response.TipoModal,
                    response.Titulo,
                    response.Mensagem,
                    false,
                    null,
                    function () { });
            }
            else {
                $("#lista-contrato-result").empty();
                $("#lista-contrato-result").append(response);
            }

            hideLoading();
        },
        error: function (ex) {
            $("#lista-contrato-result").empty();
            $("#lista-contrato-result").empty(ex.responseText);
            hideLoading();
            console.log(ex);
        },
        beforeSend: function () {
            showLoading();
        }
    });
}

function adicionarContatocadastroCliente() {
    indiceContatosAdicionados--;

    var contato = {
        Id: indiceContatosAdicionados,
        Email: $("#email").val(),
        Telefone: $("#telephone").val(),
        Celular: $("#cellphone").val()
    };

    contatos.push(contato);

    atualizarContatos(contatos);
}

function AdicionarContaCorrenteClienteCadastro() {

    let valor = $("#conta-corrente-cliente-valor").val();
    let mesReferencia = $("#conta-corrente-mes-referencia").val();
    if (mesReferencia === 0) {
        toastr.error('Selecione o Mês de Referência', 'Campo obrigatório');
        return;
    }
    let tipoOperacaoId = $("input[name='Cliente.ContaCorrenteCliente']:checked").val();
    let contratoId = $("#conta-corrente-contratos").val();
    if (!contratoId) {
        toastr.error('Selecione o Contrato', 'Campo obrigatório');
        return;
    }
    let numeroContrato = $("#conta-corrente-contratos  option:selected").text();
    let dataCompetencia = $("#conta-corrente-cliente-dtCompetencia").val();

    post("AdicionarContaCorrenteClienteCadastro", { tipoOperacaoId: tipoOperacaoId, valor: valor, dataCompetencia: dataCompetencia, contratoId: contratoId, numeroContrato: numeroContrato }, "Cliente")
        .done((response) => {
            $("#lista-conta-corrente-cliente-detalhe").empty().append(response.Grid);
            LimparCamposContaCorrenteClienteCadastro();
        })
        .fail((error) => {
            toastr.warning(error.statusText, "Alerta");
        });

    contaCorrenteClienteEmEdicacao = false;
}

function LimparCamposContaCorrenteClienteCadastro() {    
    $("#conta-corrente-acrescimo").checked();
    $("#conta-corrente-cliente-valor").val("0,00");
    $("#conta-corrente-cliente-dtCompetencia").val("");
    $("#conta-corrente-contratos").val("0");

    MakeChosen("conta-corrente-contratos");
}

function EditarContaCorrenteClienteCadastro(tipoOperacaoId) {
    var tipoBeneficioId = tipoOperacaoId;
    if (contaCorrenteClienteEmEdicacao) {
        toastr.warning("Já possui um item em edição", "Alerta");
        return;
    }

    post("EditarContaCorrenteCliente", { tipoBeneficioId }, "Cliente")
        .done((response) => {
            $("#lista-conta-corrente-cliente-detalhe").empty().append(response.Grid);

            let isAcrescimo = response.Item.TipoOperacaoContaCorrente == 1;
            if (isAcrescimo) {
                $("#conta-corrente-acrescimo").checked();
            } else {
                $("#conta-corrente-decrescimo").checked();
            }

            $("#conta-corrente-cliente-valor").val(response.Item.Valor);
            $("#conta-corrente-cliente-dtCompetencia").val(moment(response.Item.DataCompetencia).format("MM/YYYY"));
            $("#conta-corrente-contratos").val(response.Item.ContratoMensalista.Id);

            MakeChosen("conta-corrente-contratos");
            contaCorrenteClienteEmEdicacao = true;
        });
}

function CarregarContratosContaCorrente() {
    $("#conta-corrente-contratos").empty();

    $.each(contratos,
        function (i, contrato) {
            $("#conta-corrente-contratos").append('<option value="' + contrato.Id + '">' + contrato.NumeroContrato + '</option>');

            //if (i === 0) {
            //    $("#conta-corrente-contratos").append('<option value="">' + contrato.NumeroContrato + '</option>');
            //}
            //else {
            //    $("#conta-corrente-contratos").append('<option value="' + contrato.Id + '">' + contrato.NumeroContrato + '</option>');
            //}
        });

    MakeChosen("conta-corrente-contratos");
}

function CarregarUnidadesCadastroCliente() {

    $("#unidade").empty();
    $("#unidade-ocorrencia").empty();

    let unidades = $("#unidades").val();
    $.ajax({
        type: 'POST',
        url: '/Cliente/CarregarUnidades',
        data: { unidadesId: unidades },
        success: function (result) {
            $.each(result,
                function (i, result) {
                    if (i === 0) {
                        $("#unidade").append('<option value="">' + result.Text + '</option>');
                        $("#unidade-ocorrencia").append('<option value="">' + result.Text + '</option>');
                    }
                    else {
                        $("#unidade").append('<option value="' + result.Value + '">' + result.Text + '</option>');
                        $("#unidade-ocorrencia").append('<option value="' + result.Value + '">' + result.Text + '</option>');
                    }
                });

            if ($('#UnidadeSelecionada').val() !== '') {
                $('#unidade').val($('#UnidadeSelecionada').val());
            } else {
                $('#unidade').val("");
            }

            if ($('#unidade-selecionada-ocorrencia').val() !== '') {
                $('#unidade').val($('#unidade-selecionada-ocorrencia').val());
            } else {
                $('#unidade-ocorrencia').val("");
            }

            MakeChosen("unidade");
            MakeChosen("unidade-ocorrencia");
        },
        error: function (ex) {
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

function ValidarCamposContrato() {
    if ($("#txtCodContrato").val() === "") {
        toastr.error("O campo \"Código de Contrato\" é obrigatório");
        return false;
    }
    //if ($("#cliente").val() === "") {
    //    toastr.error("O campo \"Cliente\" é obrigatório");
    //    return false;
    //}
    if ($("#cbContratoMensalista_TipoMensalista").val() === "") {
        toastr.error("O campo \"Tipo Mensalista\" é obrigatório");
        return false;
    }
    if (!$("#unidade").val()) {
        toastr.error("O campo \"Unidade\" é obrigatório");
        return false;
    }
    if ($("#dtInicio").val() === "") {
        toastr.error("O campo \"Data Inicio\" é obrigatório");
        return false;
    }
    //if ($("#dtVencimento").val() === "") {
    //    toastr.error("O campo \"Data Vencimento\" é obrigatório");
    //    return false;
    //}
    if ($("#txtValor").val() === "") {
        toastr.error("O campo \"Valor\" é obrigatório");
        return false;
    }
    if ($("#numero-vagas").val() === "" || $("#numero-vagas").val() === null || $("#numero-vagas").val() === undefined) {
        toastr.error("O campo \"Número de Vagas\" é obrigatório");
        return false;
    }
    else if (parseInt($("#numero-vagas").val()) <= 0) {
        toastr.error("O campo \"Número de Vagas\" deve receber um valor acima de zero");
        return false;
    }
    if ($('#TabelaPrecoMensalistaSelecionada').val() === "") {
        toastr.error("O campo \"Tabela de Preço\" é obrigatório");
        return false;
    }
    if ($("#nroRecibo").val() === "") {
        toastr.error("O campo \"Nº Recibo\" é obrigatório");
        return false;
    }
    if ($("#vlrPago").val() === "") {
        toastr.error("O campo \"Valor Pago\" é obrigatório");
        return false;
    }

    return true;
}

function AdicionarContratoMensalista() {

    indiceContratosAdicionados--;


    if (!ValidarCamposContrato())
        return;

    var contrato = {
        TipoMensalista: { Id: $("#cbContratoMensalista_TipoMensalista").val(), Descricao: $("#cbContratoMensalista_TipoMensalista option:selected").text() },
        Unidade: { Id: $("#unidade").val(), Nome: $("#unidade option:selected").text() },
        UnidadeSelecionada: $("#UnidadeSelecionada").val(),
        NumeroVagas: $("#numero-vagas").val(),
        NumeroContrato: $("#txtCodContrato").val(),
        DataInicio: $("#dtInicio").val(),
        DataFim: $("#dtFim").val(),
        Id: indiceContratosAdicionados,
        TabelaPrecoMensalista: { Id: $("#cbContratoMensalista_TabelaPrecoMensalista").val(), Nome: $("#cbContratoMensalista_TabelaPrecoMensalista option:selected").text() },
        Frota: document.getElementById('frota').checked,
        Ativo: document.getElementById('ativo-contrato').checked,
        Valor: document.getElementById("valor-contrato").value,
        NumeroRecibo: $("#nroRecibo").val(),
        ValorPago: $("#vlrPago").val().replace(".", "").replace(",", "."),
        Veiculos: veiculosCliente
    };
   
    contratos.push(contrato);

    $.when(atualizaContrato())
        .done(function () {
            ocorrenciaEmEdicacao = false;

            LimparCamposContrato();
        });
}

function LimparCamposContrato() {
    document.getElementById("cbContratoMensalista_TipoMensalista").selectedIndex = "0";
    document.getElementById("unidade").selectedIndex = "0";
    MakeChosen("unidade");

    //$("#unidade").change();

    document.getElementById("numero-vagas").value = '';
    document.getElementById("txtCodContrato").value = '';
    document.getElementById("dtInicio").value = '';
    document.getElementById("dtFim").value = '';
    document.getElementById("valor-contrato").value = '';
    document.getElementById("frota").checked = false;
    document.getElementById("ativo-contrato").checked = true;
    document.getElementById("nroRecibo").value = '';
    document.getElementById("vlrPago").value = '';

    veiculosCliente = [];
    atualizarveiculos(veiculosCliente);

    document.getElementById("cbContratoMensalista_TabelaPrecoMensalista").value = "";
    MakeChosen("cbContratoMensalista_TabelaPrecoMensalista");

    MakeChosen("cbContratoMensalista_TipoMensalista");
    MakeChosen("unidade-ocorrencia");
}

function atualizaContrato() {
    showLoading();
    $.post('/Cliente/AdicionarContrato', { jsonContratos: JSON.stringify(contratos) })
        .done(() => {
            CarregaContratos();
        })
        .always(() => { hideLoading(); });
}

function CarregaContratos() {

    $('#data_table_address tbody').empty();
    $.each(contratos, function (i, contrato) {

        $('#data_table_address tbody')
            .append('<tr>'
                + '<td style="display:none">'
                + contrato.Id
                + '</td>'
                + '<td col-xs-3">'
                + contrato.Unidade.Nome
                + '</td>'
                + '<td col-xs-3>'
                + contrato.NumeroContrato
                + '</td>'
                + '<td class="col-xs-1" style="text-align:center;">'
                + '<span class="btn btn-primary" style="margin-right: 15px;" onclick="EditarContrato(' + contrato.Id + ')")><i class="fa fa-edit"></i></span>'
                + '<span class="btn btn-danger" onclick="RemoverContrato(' + contrato.Id + ')"><i class="fa fa-remove"></i></span>'
                + '</td>'
                + '</tr>'
            );
    });

    CarregarContratosContaCorrente();
}

function EditarContrato(Id) {
    veiculosCliente = [];

    var contrato = contratos.find(x => x.Id === Id);
  
    $("#unidade").val(contrato.Unidade.Id);
    MakeChosen("unidade");
    MakeChosen("unidade-ocorrencia");

    CarregarTabelaPrecoMensalista(contrato.Unidade.Id, contrato.TabelaPrecoMensalista.Id);

    $("#cbContratoMensalista_TipoMensalista").val(contrato.TipoMensalista.Id);    
    
    $("#numero-vagas").val(contrato.NumeroVagas);
    $("#txtCodContrato").val(contrato.NumeroContrato);
    $("#dtInicio").val(contrato.DataInicio);
    $("#dtFim").val(contrato.DataFim);
    $("#valor-contrato").val(contrato.Valor);
    $("#nroRecibo").val(contrato.NumeroRecibo);
    $("#vlrPago").val(contrato.ValorPago);

    document.getElementById("frota").checked = contrato.Frota;
    document.getElementById("ativo-contrato").checked = contrato.Ativo;

    veiculosCliente = contrato.Veiculos;

    MakeChosen("cbContratoMensalista_TipoMensalista");

    RemoverContrato(Id);

    atualizarveiculos(veiculosCliente);
}

function RemoverContrato(id) {
    contratos = contratos.filter(x => x.Id !== id);
    atualizaContrato();
}

function RemoverContaCorrenteClienteCadastroCliente(tipoOperacaoId) {
    var tipoBeneficioId = tipoOperacaoId;
    post("RemoverContaCorrenteCliente", { tipoBeneficioId }, "Cliente")
        .done((response) => {
            $("#lista-conta-corrente-cliente-detalhe").empty().append(response.Grid);
        });
}