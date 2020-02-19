$(document).ready(function () {
    ConfigTabelaGridSemCampoFiltroPrincipal();
    ResetLojasChosen();
    
    FormatarCampoData("dtNascimento");
    formataCpf("CPFPesquisa");
    formataCpf("cpf");
    
    $("#Nome").keyup(function () {
        if (!somenteLetra(this.value)) {
            if (!somenteLetra(this.value.substring(this.value.length - 1, this.value.length)))
                this.value = this.value.substring(0, this.value.length - 1);
        }
    });
    
    $('#cpf').on('blur', function (ele) {
        VerificarCPF();
    });

    $("#PessoaForm").submit(function (e) {
        var dadosValidos = true;
        
        if (cpfJaExistente()){
            dadosValidos = false;
        }
        
        if (!somenteLetra($("#Nome").val())) {
            toastr.error("O campo nome não permite caracteres númericos!", "Nome inválido!");
            dadosValidos = false;  
        }

        if (!verificaData($("#dtNascimento").val())) {
            toastr.error('Preencha uma Data de Nascimento Inválida!', 'Data de Nascimento Inválida!');
            dadosValidos = false;
        }

        if (!CPFValido($("#cpf").val())) {
            toastr.error('Preencha um CPF válido!', 'CPF Inválido!');
            dadosValidos = false;
        }

        if (!dadosValidos) {
            e.preventDefault();
            return false;
        }
    });
});

function cpfJaExistente(){
    var cpf = document.getElementById('cpf').value;
    var idPessoa = document.getElementById('hdnPessoa').value;
    return validarCpfExistente(idPessoa ? idPessoa : 0, cpf);
}

function validarCpfExistente(idPessoa, cpf) {
    $.ajax({
        url: '/Pessoa/VerificarCPFExistente',
        data: { idPessoa: idPessoa, cpf: cpf},
        type: 'GET',
        success: function (response) {
            if (response.pessoaExistente && response.pessoaId && response.pessoaId > 0){
                $('#cpf').val('');
                openCustomModal(null, "carregarEdicao('/Pessoa/Edit/" + response.pessoaId + "')", 'warning', 'CPF em uso!', 'Este CPF já está sendo utilizado! \n\n Deseja carregar os dados desta pessoa?', true, 'Sim', null);
                return true;
            }

            return false;
        }
    });
}

function GetLojasByFuncao(ele) {
    $("#lojas").empty();
    $("#lojas").trigger("chosen:update");
    $("#lojas").val('').trigger('chosen:updated');

    $.ajax({
        url: "/pessoa/Lojas",
        type: "POST",
        dataType: "json",
        success: function (lojas) {
            hideLoading();

            $("#lblMensagemLojaResultado").text("");

            if (lojas !== undefined && lojas !== null && lojas.length > 0) {
                $(lojas).each(function () {
                    $("<option />", {
                        val: this.Id,
                        text: this.RazaoSocial
                    }).appendTo("#lojas");
                });
                ResetLojasChosen();

            } else {
                $("#lblMensagemLojaResultado").text("Não possui lojas disponíveis!");
            }
        },
        error: function (error) {
            hideLoading();
            //alert(JSON.stringify(error));
        },
        beforeSend: function () {
            showLoading();
        }
    });
}

function ResetLojasChosen() {
    //Entidade.Uteis.Constantes.Funcao [Enum]
    var optionsLoja = $("#lojas > option").length;
    var lojaLength = optionsLoja === undefined || optionsLoja === null ? 0 : optionsLoja;
    var maxSelectedOptions = (lojaLength);
    MakeChosen("lojas", maxSelectedOptions, "230px");

    $("#lojas").off("change");
    $("#lojas").change(function (event) {
        var obj = [];
        $("select#lojas option:selected").each(function () {
            obj.push({ Id: $(this).val(), RazaoSocial: $(this).text() });
        });

        $.ajax({
            url: '/pessoa/lojasSelecionadas',
            type: 'POST',
            dataType: 'json',
            data: { json: JSON.stringify(obj) },
            success: function (response) {
                hideLoading();
            },
            error: function (error) {
                hideLoading();
                //alert(JSON.stringify(error));
            },
            beforeSend: function () {
                showLoading();
            }
        });
    });
}

function BuscarPessoas() {
    var NomePesquisa = $("#NomePesquisa").val();
    var CPFPesquisa = $("#CPFPesquisa").val();

    $.ajax({
        url: "/pessoa/Pesquisar",
        type: "POST",
        data: {
            nome: NomePesquisa,
            CPF: CPFPesquisa
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
                $(".lista-pessoas").empty();
                $(".lista-pessoas").append(result);
                MetodoUtil();
            }
        },
        error: function (error) {
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

function VerificarCPF() {
    var cpf = $("#cpf").val();

    if (!CPFValido(cpf)) {
        toastr.error("Preencha um CPF válido!", "CPF Inválido!");
        return false;
    }

    return !cpfJaExistente();
}

