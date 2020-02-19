$(document).ready(function () {
    callbackPaginacao = BuscarClientes;

    MakeChosen("unidades");
    ResetCarteirasChosen();
    MakeChosen("type");
    MakeChosen("tipoContato");
    MakeChosen("conta-corrente-cliente-tipo-operacao");
    //FormatarReal("valmoney");
    
    if ($("#tipo-pessoa").val() === "" || $("#tipo-pessoa").val() === undefined && $("#tipo-pessoa").val() === null)
        $("#tipo-pessoa").val("1");
    MudarTipoPessoa($("#tipo-pessoa").val());

    $("#cnpj").on("keyup", function (e) {
        $(this).val(
            $(this).val()
                .replace(/\D/g, '')
                .replace(/^(\d{2})(\d{3})?(\d{3})?(\d{4})?(\d{2})?/, "$1.$2.$3/$4-$5"));
    });

    $("#tipo-pessoa").on("change", function () {
        MudarTipoPessoa(this.value, true);
    });

    FormatarCampoData("dtNascimento");
    FormataCampoCpf("cpf");
    FormataCampoRg("Pessoa_Rg");
    //FormataCampoCpf("cpfBusca");

    $("#Pessoa_Nome").keyup(function () {
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

        // Pessoa Fisica
        if ($("#tipo-pessoa").val() === "1") {
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

            //else if (!verificaData($("#dtNascimento").val())) {
            //    toastr.error('Preencha uma Data de Nascimento válida!', 'Data de Nascimento Inválida!');
            //    dadosValidos = false;
            //}
        }
        // Pessoa Juridica
        else if ($("#tipo-pessoa").val() === "2") {
            if (!$("#nome_fantasia").val()) {
                toastr.error('Informe o Nome Fantasia', 'Campo obrigatório');
                dadosValidos = false;
            }
            if (!$("#razao_social").val()) {
                toastr.error('Informe a Razão Social', 'Campo obrigatório');
                dadosValidos = false;
            }
            if (!$("#cnpj").val()) {
                toastr.error('Informe o CNPJ', 'Campo obrigatório');
                dadosValidos = false;
            }
            //else if (!$("#inscricao_social").val()) {
            //    toastr.error('Informe a Inscrição Estadual', 'Campo obrigatório');
            //    dadosValidos = false;
            //}
            //else if (!$("#inscricao_municipal").val()) {
            //    toastr.error('Informe a Inscrição Municipal', 'Campo obrigatório');
            //    dadosValidos = false;
            //}

        } else {
            toastr.warning('Selecione um tipo de pessoa', 'Alerta');
            dadosValidos = false;
        }

        if ($("#tipo-pessoa").val() === "1" || $("#tipo-pessoa").val() === "2") {
            if (!enderecos.length) {
                toastr.error('Informe pelo menos um endereço', 'Campo obrigatório');
                dadosValidos = false;
            }
            else if (!contatos.length) {
                toastr.error('Informe pelo menos um contato', 'Campo obrigatório');
                dadosValidos = false;
            }
        }

        if ($("#unidades_chosen li.search-choice").length === 0) {
            toastr.error('Informe pelo menos uma unidade', 'Campo obrigatório');
            dadosValidos = false;
        }

        if (!dadosValidos) {
            e.preventDefault();
            return false;
        }
    });

    MakeChosen("credores", null);
    MakeChosen("produtos", null);
    MakeChosen("carteirasBusca", null);

    $("#marca").change(function () {
        $('#IdMarca').val($('#marca').val());
    });

    $.ajax({
        type: 'POST',
        url: '/cliente/CarregarModelos',
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

    let idCliente = $("#hdnCliente").val();
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


    if ($("#hdnCliente").val() !== undefined
        && $("#hdnCliente").val() !== null
        && $("#hdnCliente").val() !== ""
        && $("#hdnCliente").val() !== "0") {

        FormatarCampoData("dtInicioFerias");
        FormatarCampoData("dtFimFerias");

        $("#qtdVagas").change(function () {
            var qtdVagas = $(this).val();
            var contratoTextoSelecionado = $("#contrato-ferias option:selected").text();
            var vagasContratoSelecionado = parseInt(contratoTextoSelecionado !== "" && contratoTextoSelecionado !== undefined && contratoTextoSelecionado.indexOf(":") >= 0
                                            ? contratoTextoSelecionado.split(":")[1].trim() : "0");
            var idContrato = $("#contrato-ferias option:selected").val();

            if (vagasContratoSelecionado <= 0)
            {
                toastr.error("Selecione um Contrato, para depois informar a Quantidade de Vagas necessárias!", "Férias - Quantidade de Vagas");
                $(this).val("");
            }
            else if (qtdVagas <= 0
                || (idContrato === undefined || idContrato === null || idContrato === 0)
                || qtdVagas > vagasContratoSelecionado) {
                toastr.error("Quantidade de Vagas informada, excede o total de vagas do Contrato selecionado!", "Férias - Quantidade de Vagas");
                $(this).val("");
            }
        });

        $("#chkTodasAsVagas").change(function () {
            var checked = $(this).prop("checked");
            if (checked === true) {
                $("#qtdVagas").val($("#numeroVagas-contrato-ferias").val()).attr("readonly", "readonly");
                $("#div-contratos-ferias").css("display", "none");
            }
            else {
                $("#qtdVagas").val("").removeAttr("readonly");
                $("#div-contratos-ferias").css("display", "inline");
            }
            
            $("#contrato-ferias").val("").trigger("chosen:updated");
        });

        CarregaContratosClienteFerias($("#hdnCliente").val());
    }

    $("#unidades").change(function () {
        CarregarUnidadesCadastroCliente();
    });

    if ($("#hdnCliente").val() !== undefined
        && $("#hdnCliente").val() !== null
        && $("#hdnCliente").val() !== ""
        && $("#hdnCliente").val() !== "0") {
        $("#unidades").change();
        atualizaVeiculos();
    }
});

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

function CarregaContratosClienteFerias(idCliente) {
    $.ajax({
        type: 'POST',
        url: '/Cliente/BuscaContratosCliente',
        dataType: 'json',
        data: {
            idCliente: idCliente
        },
        success: function (result) {
            $("#numeroVagas-contrato-ferias").val(result.TotalVagas);

            $("#contrato-ferias").append('<option value="">Selecione um Contrato</option>');
            $.each(result.Contratos, function (i, item) {
                $("#contrato-ferias").append('<option value="' + item.Id + '">Nº ' + item.NumeroContrato + ' - Vagas: ' + item.NumeroVagas + '</option>');
            });
            MakeChosen("contrato-ferias");

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

            if (limpar)
                LimparCamposPessoaFisica();
            break;
        // Juridica
        case "2":
            $(".container-pessoa-fisica").hide();
            $(".container-pessoa-juridica").show();

            if (limpar)
                LimparCamposPessoaJuridica();
            break;
        default:
            $(".container-pessoa-fisica").hide();
            $(".container-pessoa-juridica").hide();

            if (limpar) {
                LimparCamposPessoaFisica();
                LimparCamposPessoaJuridica();
            }
            break;
    }
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
                MakeChosen("conta-corrente-contratos");
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

function AbrirContratoMensalista(contratoMensalistaId) {
    window.open("/ContratoMensalista/Edit/" + contratoMensalistaId);
}

function AdicionarFerias() {
    var dadosValidos = true;

    var dataInicio = $("#dtInicioFerias").val();
    var dataFim = $("#dtFimFerias").val();

    if (!$("#dtInicioFerias").val()) {
        toastr.error("O campo \"Início\" deve ser preenchido!", "Validação - Férias do Cliente");
        dadosValidos = false;
    } else if (verificaData($("#dtInicioFerias").val()) === false) {
        toastr.error("O campo \"Início\" deve ser preenchido com uma data válida!", "Validação - Férias do Cliente");
        dadosValidos = false;
    }

    if (!$("#dtFimFerias").val()) {
        toastr.error("O campo \"Fim\" deve ser preenchido!");
        dadosValidos = false;
    } else if (verificaData($("#dtFimFerias").val()) === false) {
        toastr.error("O campo \"Fim\" deve ser preenchido com uma data válida!", "Validação - Férias do Cliente");
        dadosValidos = false;
    }

    var dataParseInicio = new Date(dataInicio.split("/")[1] + '-' + dataInicio.split("/")[0] + '-' + dataInicio.split("/")[2]);
    var dataParseFim = new Date(dataFim.split("/")[1] + '-' + dataFim.split("/")[0] + '-' + dataFim.split("/")[2]);
    
    if (dataParseInicio > dataParseFim) {
        toastr.error("O campo \"Fim\" deve ser MAIOR que o campo \"Início\"!", "Validação - Férias do Cliente");
        dadosValidos = false;
    }

    var dataCompetenciaInicio = new Date(dataInicio.split("/")[1] + '-01-' + dataInicio.split("/")[2]);
    //var dataCompetenciaFim = new Date(dataFim.split("/")[1] + '-01-' + dataFim.split("/")[2]);

    //if (dataCompetenciaInicio < dataCompetenciaFim) {
    //    toastr.error("As datas de \"Início\" e \"Fim\" devem ser do mesmo mês/ano!", "Validação - Férias do Cliente");
    //    dadosValidos = false;
    //}

    var dataParseCompetencia = new Date(dataFim.split("/")[1] + '-01-' + dataFim.split("/")[2]);
    dataParseCompetencia.setMonth(dataParseCompetencia.getMonth() + 1);
    var d = new Date();
    var year = d.getFullYear();
    var month = d.getMonth();
    var day = 1;
    var firstDayOfCurrentDate = new Date(year, month, day, 0, 0, 0, 0);

    if (dataCompetenciaInicio.getTime() < firstDayOfCurrentDate.getTime()
        || dataParseCompetencia.getTime() <= firstDayOfCurrentDate.getTime()) {
        toastr.error("As férias não podem ser de um mês retroativo!", "Validação - Férias do Cliente");
        dadosValidos = false;
    }

    var qtdVagas = $("#qtdVagas").val();
    if (qtdVagas === undefined || qtdVagas === null || qtdVagas === "" || parseInt(qtdVagas) <= 0) {
        toastr.error("O campo Número de Vagas, deve ser preenchido!", "Validação - Férias do Cliente");
        dadosValidos = false;
    }

    var contratoSelecionado = 0;
    var checked = $("#chkTodasAsVagas").prop("checked");
    if (checked === false) {
        var selecionado = $("#contrato-ferias").val();
        if (selecionado !== undefined && selecionado !== null && selecionado !== "" && parseInt(selecionado) > 0) {
            contratoSelecionado = selecionado;
        }
        else {
            toastr.error("Selecione um contrato!", "Validação - Férias do Cliente");
            dadosValidos = false;
        }
    }

    if (!dadosValidos) {
        return false;
    }

    var isEdited = $("#hdnIsEditedFerias").val();
    var usId = $("#hdnUsId").val();
    var dados = {
        Id: $("hdn-id-ferias").val(),
        DataInicio: dataInicio,
        DataFim: dataFim,
        InutilizarTodasVagas: checked,
        TotalVagas: qtdVagas,
        Cliente: {
            Id: $("#hdnCliente").val()
        },
        ContratoMensalista: {
            Id: contratoSelecionado
        },
        IsEdited: isEdited,
        UsuarioCadastro: {
            Id: usId
        }
    };

    $.ajax({
        type: 'POST',
        url: '/Cliente/AdicionarFerias',
        dataType: 'json',
        data: {
            jsonDados: JSON.stringify(dados)
        },
        success: function (response) {
            if (typeof response === "object" && response.TipoModal !== undefined) {
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
                toastr.success("Férias adicionada! Para efetivar, salve o cadastro posteriormente.", "Férias do Cliente");

                $("#hdn-id-ferias").val("0");
                $("#hdn-index-ferias").val("0");
                $("#hdnIsEditedFerias").val("false");
                $("#hdnUsId").val("0");
                $("#dtInicioFerias").val("");
                $("#dtFimFerias").val("");
                $("#chkTodasAsVagas").prop("checked", false);
                $("#chkTodasAsVagas").trigger("change");
                $("#qtdVagas").val("");
                $("#contrato-ferias").val("").trigger("chosen:updated");

                $("#lista-ferias-result").empty().append(response.Grid);
            }

            hideLoading();
        },
        error: function (ex) {
            hideLoading();
            console.log(ex);
        },
        beforeSend: function () {
            showLoading();
        }
    });
}

function EditarItemFerias(index, idFerias) {
    $.ajax({
        type: 'POST',
        url: '/Cliente/EditarFerias',
        dataType: 'json',
        data: {
            index: index,
            idFerias: idFerias
        },
        success: function (response) {
            if (typeof response === "object" && response.TipoModal !== undefined) {
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
                $("#hdn-id-ferias").val(response.Item.Id);
                $("#hdn-index-ferias").val("");
                $("#hdnIsEditedFerias").val(response.Item.IsEdited);
                $("#hdnUsId").val(response.Item.UsuarioCadastro.Id);
                $("#dtInicioFerias").val(response.Item.DataInicioStr);
                $("#dtFimFerias").val(response.Item.DataFimStr);
                $("#chkTodasAsVagas").prop("checked", response.Item.InutilizarTodasVagas);
                $("#chkTodasAsVagas").trigger("change");
                $("#qtdVagas").val(response.Item.TotalVagas);

                if (response.Item.InutilizarTodasVagas === true) {
                    $("#div-contratos-ferias").css("display", "none");
                    $("#contrato-ferias").val("").trigger("chosen:updated");
                }
                else {
                    $("#div-contratos-ferias").css("display", "inline");
                    $("#contrato-ferias").val(response.Item.ContratoMensalista.Id).trigger("chosen:updated");
                }

                $("#lista-ferias-result").empty().append(response.Grid);
            }

            hideLoading();
        },
        error: function (ex) {
            hideLoading();
            console.log(ex);
        },
        beforeSend: function () {
            showLoading();
        }
    });
}

function ExcluirItemFerias(index, idFerias) {
    $.ajax({
        type: 'POST',
        url: '/Cliente/ExcluirFerias',
        dataType: 'json',
        data: {
            index: index,
            idFerias: idFerias
        },
        success: function (response) {
            if (typeof response === "object" && response.TipoModal !== undefined) {
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
                toastr.success("Férias removida da listagem. Salve o cadastro para efetivar a exclusão.", "Férias do Cliente");

                $("#lista-ferias-result").empty().append(response.Grid);
            }

            hideLoading();
        },
        error: function (ex) {
            hideLoading();
            console.log(ex);
        },
        beforeSend: function () {
            showLoading();
        }
    });
}