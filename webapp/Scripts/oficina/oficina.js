$(document).ready(function () {
    FormatarCampos();

    $("#cep").blur(function () {
        buscarInformacoesPeloCep();
    });

    $("#indicada-pelo-cliente").change(function () {
        if (this.checked) {
            $("#container-cliente-indicador").show();
        }
        else {
            $("#container-cliente-indicador").hide();
        }
    });

    MudarTipoPessoa($("#tipo-pessoa").val());
    $("#cnpj").on("keyup", function (e) {
        $(this).val(
            $(this).val()
                .replace(/\D/g, '')
                .replace(/^(\d{2})(\d{3})?(\d{3})?(\d{4})?(\d{2})?/, "$1.$2.$3/$4-$5"));
    });

    $("#tipo-pessoa").on("change", function () {
        MudarTipoPessoa(this.value);
    });

    $("#Pessoa_Nome").keyup(function () {
        if (!somenteLetra(this.value)) {
            if (!somenteLetra(this.value.substring(this.value.length - 1, this.value.length)))
                this.value = this.value.substring(0, this.value.length - 1);
        }
    });

    $('#cpf').on('blur', function () {
        VerificarCPF();
        BuscarCPF();
    });

    $('#cnpj').on('blur', function (ele) {
        VerificarCNPJ();
        BuscarCNPJ();
    });

    $("#oficina-form").submit(function (e) {
        if (!ValidarCampos())
            e.preventDefault();
    });

    if (isEdit()) {
        let cpf = $("#cpf");
        if (cpf.val()) {
            cpf.attr("readonly", 'readonly');
        }

        let cnpj = $("#cnpj");
        if (cnpj.val()) {
            cnpj.attr("readonly", 'readonly');
        }
    }

    BuscarOficinas();
});


function ValidarCampos() {
    let indicadaPeloCliente = document.getElementById("indicada-pelo-cliente").checked;
    if (indicadaPeloCliente) {
        if (!$("#nome-cliente").val()) {
            toastr.error("Informe o Nome do Cliente que indicou", "Campo Obrigatório");
            return false;
        }

        if (!$("#celular-cliente-descricao").val()) {
            toastr.error("Informe o Celular do Cliente que indicou", "Campo Obrigatório");
            return false;
        }
    }

    // Pessoa Fisica
    if ($("#tipo-pessoa").val() === "1") {
        if (!$("#Pessoa_Nome").val()) {
            toastr.error('Preencha o Nome', 'Nome é Obrigatório');
            return false;
        }
        else if (!indicadaPeloCliente) {
            if (!somenteLetra($("#Pessoa_Nome").val())) {
                toastr.error("O campo nome não permite caracteres númericos!", "Nome inválido!");
                return false;
            }

            else if (!VerificarCPF()) {
                return false;
            }

            else if (!verificaData($("#dtNascimento").val())) {
                toastr.error('Preencha uma Data de Nascimento válida!', 'Data de Nascimento Inválida!');
                return false;
            }
        }
    }
    // Pessoa Juridica
    else if ($("#tipo-pessoa").val() === "2") {
        if (!$("#nome_fantasia").val()) {
            toastr.error('Informe o Nome Fantasia', 'Campo obrigatório');
            return false;
        }
        else if (!indicadaPeloCliente) {
            if (!$("#cnpj").val()) {
                toastr.error('Informe o CNPJ', 'Campo obrigatório');
                return false;
            }
            else if (!$("#inscricao_social").val()) {
                toastr.error('Informe a Inscrição Estadual', 'Campo obrigatório');
                return false;
            }
            else if (!$("#inscricao_municipal").val()) {
                toastr.error('Informe a Inscrição Municipal', 'Campo obrigatório');
                return false;
            }
        }
    } else {
        toastr.error('Selecione um tipo de pessoa', 'Campo Obrigatório');
        return false;
    }

    return true;
}

function BuscarOficinas() {
    return post("BuscarOficina")
        .done((response) => {
            $("#lista-oficinas").empty().append(response);
            ConfigTabelaGridSemCampoFiltroPrincipal();
        });
}

function FormatarCampos() {
    FormatarTelefoneFixo("#telefone-fixo");
    FormatarCelular("#celular, #celular-cliente-descricao");
    $("#cep").mask("00000-000");
    FormatarCampoData("dtNascimento");
    FormataCampoCpf("cpf");
    FormataCampoRg("Pessoa_Rg");
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

function BuscarCPF() {
    var cpf = $("#cpf").val();
    if (cpf !== "") {
        $.ajax({
            url: "/oficina/BuscarCPF",
            type: "POST",
            dataType: "json",
            data: { cpf: cpf },
            success: function (existe) {
                if (existe) {
                    $("#cpf").val("");
                    toastr.error("Cpf já possue cadastro no Sistema!", "Cpf Já Cadastrado!");
                    return false;
                }
            }
        });
    }

    return true;
}

function BuscarCNPJ() {
    var cnpj = $("#cnpj").val();
    if (cnpj !== "") {
        $.ajax({
            url: "/oficina/BuscarCNPJ",
            type: "POST",
            dataType: "json",
            data: { cnpj: cnpj },
            success: function (existe) {
                if (existe) {
                    $("#cnpj").val("");
                    toastr.error("Cnpj já possue cadastro no Sistema!", "Cnpj Já Cadastrado!");
                    return false;
                }
            }
        });
    }

    return true;
}

function buscarInformacoesPeloCep() {
    var cep = $("#cep").val().replace(/\D/g, "");
    if (cep !== "") {
        var validacep = /^[0-9]{8}$/;
        if (validacep.test(cep)) {
            $("#invalid_zipcode").hide();
            $("#address, #district, #city, #state").val("...");

            showLoading();
            $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?", function (dados) {
                if (!("erro" in dados)) {
                    $("#logradouro").val(dados.logradouro);
                    $("#bairro").val(dados.bairro);
                    $("#cidade-descricao").val(dados.localidade);
                    $("#estado").val(dados.uf);
                }
                else {
                    clear_form();
                    toastr.error("CEP não encontrado.");
                }

                hideLoading();
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

function validarEndereco({ Tipo, Cep }) {
    if (Tipo === 0) {
        toastr.warning("Selecione um Tipo de Locação", "Alerta");
    }
    else if (!Cep) {
        toastr.warning("Informe o cep", "Alerta");
    } else {
        return true;
    }

    return false;
}