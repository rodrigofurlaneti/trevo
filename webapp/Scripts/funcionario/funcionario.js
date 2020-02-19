$(document).ready(function () {
    callbackPaginacao = BuscarFuncionarios;
    ConfigTabelaGridSemCampoFiltroPrincipal("#datatable_grid_controleferias");
    ConfigTabelaGridSemCampoFiltroPrincipal("#datatable_grid_faltas");

    $("#Pessoa_Nome").blur(function () {
        let pessoaNome = $("#Pessoa_Nome").val();
        $("#campo-funcionarios").val(pessoaNome);
        $("#beneficio-funcionario-funcionarios").val(pessoaNome);
        $("#ocorrencia-funcionario-funcionarios").val(pessoaNome);
    });

    MakeChosen("supervisor");
    MakeChosen("unidade");
    MakeChosen("status");
    MakeChosen("cargo");
    MakeChosen("tipoContato");
    MakeChosen("situacao-sage");

    FormatarCampoData("data-admissao");
    FormatarCampoData("dtNascimento");
    FormataCampoCpf("cpf");
    FormataCampoCpf("cpfBusca");
    FormatarCampoDataPelaClasse("data");

    $("input[type=radio][name=TipoEscala]").change(function () {
        AtualizarTipoEscala(this.value);
    });

    $("input[class*=valmoney]").maskMoney({
        prefix: "",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });

    $('#cpf').on('blur', function () {
        VerificarCPF();
    });

    $("#FuncionarioForm").submit(function (e) {
        var dadosValidos = true;

        var imagem = document.getElementById('imgImagemFuncionario');

        if (imagem.src.indexOf("data:") >= 0) {
            var hdnImagemFuncionario = document.getElementById('hdnImagemFuncionario');
            hdnImagemFuncionario.value = imagem.src;
        }

        let nome = $("#Pessoa_Nome").val();
        if (nome === "" || $.trim(nome) === "") {
            toastr.error("O campo \"Nome\" é obrigatório!");
            dadosValidos = false;
        } else if (!somenteLetra(nome)) {
            toastr.error("O campo \"Nome\" não permite caracteres númericos!");
            dadosValidos = false;
        }

        if (!$("#cpf").val()) {
            toastr.error("O campo \"CPF\" é obrigatório");
            dadosValidos = false;
        }

        if (!$("#cargo").val()) {
            toastr.error("O campo \"Cargo\" é obrigatório");
            dadosValidos = false;
        }

        if (!$("#status").val()) {
            toastr.error("O campo \"Status\" é obrigatório!");
            dadosValidos = false;
        }

        if (!$("#unidade").val()) {
            toastr.error("O campo \"Unidade\" é obrigatório!");
            dadosValidos = false;
        }

        if (!dadosValidos) {
            e.preventDefault();
            return false;
        }
    });

    $(':input[type="text"]').map(function (i, e) {
        $(e).attr('autocomplete', 'off');
    });

    MudaImagem();

    $('.valmoney').each(function () {
        $(this).maskMoney('mask', $(this).val());
    });
});

function AtualizarTipoEscala(tipoEscala) {
    let funcionarioId = $("#hdnFuncionario").val();

    post("AtualizarTipoEscala", { funcionarioId, tipoEscala })
        .fail((error) => {
            toastr.warning(error.statusText, "Alerta");
        });
}

function EditarObservacaoDia(event, diaId) {
    let funcionarioId = $("#hdnFuncionario").val();
    return post("EditarObservacaoDia", { funcionarioId, diaId, observacao: event.target.value })
        .done((response) => {
            $("#faltas").empty().append(response);
            ConfigTabelaGridSemCampoFiltroPrincipal("#datatable_grid_faltas")
        });
}

function CarregaImagem() {
    var filesSelected = document.getElementById("fupload").files;
    if (filesSelected.length > 0) {
        var fileToLoad = filesSelected[0];

        var fileReader = new FileReader();

        fileReader.onload = function (fileLoadedEvent) {
            var imagem = document.getElementById
                (
                "imgImagemFuncionario"
                );

            imagem.src = fileLoadedEvent.target.result;
        };

        fileReader.readAsDataURL(fileToLoad);
    }
}

function MudaImagem() {
    var input = document.getElementById('hdnImagemFuncionario');
    if (input.value !== '') {
        var imagem = document.getElementById('imgImagemFuncionario');
        imagem.src = input.value;
    }
}

function getBase64(file) {
    var reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = function () {
        return reader.result;
    };
    reader.onerror = function (error) {
        console.log('Error: ', error);
    };

    return null;
}

function AddBlacklist(tipo, parametro, tipoMotivoBusca) {
    $("#hdnParametro").val(parametro);
    $("#hdnTipo").val(tipo);

    $.ajax({
        url: "/funcionario/PopularMotivos",
        datatype: "JSON",
        type: "GET",
        data: { tipoMotivoBusca: tipoMotivoBusca },
        success: function (response) {
            if (typeof (response) === "object") {
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
        url: "/funcionario/ComfirmarBlacklist",
        type: "POST",
        //dataType: "json",
        data: {
            tipo: tipo, parametro: parametro, idMotivo: idMotivo, descMotivo: descMotivo
        },
        success: function (result) {
            if (typeof (result) === "object") {
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
            debugger;
            //Here the status code can be retrieved like;
            xhr.status;

            //The message added to Response object in Controller can be retrieved as following.
            xhr.responseText;
        }
    });
    hideLoading();
}

function ModalHistorico(idFuncionario) {
    showLoading();
    $.ajax({
        url: "/funcionario/ObterHistorico",
        datatype: "JSON",
        type: "GET",
        data: { idFuncionario: JSON.stringify(idFuncionario) },
        success: function (response) {
            if (typeof (response) === "object") {
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

async function VerificarCPF() {
    var cpf = $("#cpf").val();

    if (!CPFValido(cpf)) {
        toastr.error("Preencha um CPF válido!");
        $("#cpf").val("");
    }

    ValidarSeCpfExiste(cpf);
}

function ValidarSeCpfExiste(cpf) {
    return post("ValidarSeCpfExiste", { cpf }, "Pessoa")
        .done((existe) => {
            if (existe) {
                toastr.error("Cpf já possue cadastro no Sistema!", "Cpf Já Cadastrado!");
                $("#cpf").val("");
            }
        })
}

function BuscarFuncionarios(pagina = 1) {
    var cpf = $("#cpfBusca").val();
    var nome = $("#nomeBusca").val();

    return $.ajax({
        url: "/funcionario/BuscarFuncionarios",
        type: "POST",
        data: {
            cpf,
            nome,
            pagina
        },
        success: function (result) {
            if (typeof (result) === "object") {
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
                $("#lista-funcionarios").empty();
                $("#lista-funcionarios").append(result);
            }
        },
        error: function (error) {
            $("#lista-funcionarios").empty();
            $("#lista-funcionarios").append(error.responseText);
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            $("#container-list-funcionarios").show();
            hideLoading();
        }
    });
}