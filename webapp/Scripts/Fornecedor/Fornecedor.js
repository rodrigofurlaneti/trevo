$(document).ready(function () {
    MakeChosen("banco");
    FormataCampoCpf("cpf");
    MudarTipoPessoa($("#tipo-pessoa").val());

    $('#cpf').on('blur', function () {
        VerificarCPF();
    });

    $("#tipo-pessoa").on("change", function () {
        MudarTipoPessoa(this.value);
    })

    if (isEdit()) {
        showLoading();
        var id = location.pathname.split('/').pop();

        $.post(`/fornecedor/BuscarContatos/${id}`)
            .done((response) => {
                contatos = response.contatos;
            })
            .always(() => { hideLoading(); })
    } else if (location.pathname.includes("salvardados")) {
        showLoading();
        $.post(`/fornecedor/BuscarContatosEmSessao`)
            .done((response) => {
                contatos = response.contatos;
            })
            .always(() => { hideLoading(); })
    }

    $("#fornecedor-form").submit(function (e) {
        var dadosValidos = true;
        var tipoPessoa = $("#tipo-pessoa").val();

        // Pessoa Fisica
        if (tipoPessoa == 1) {
            if (!$("#pessoa-nome").val()) {
                toastr.error('Preencha o Nome', 'Nome é Obrigatório');
                dadosValidos = false;
            }

            else if (!somenteLetra($("#pessoa-nome").val())) {
                toastr.error("O campo nome não permite caracteres númericos!", "Nome inválido!");
                dadosValidos = false;
            }

            else if (!VerificarCPF()) {
                dadosValidos = false;
            }
        }
        // Pessoa Juridica
        else if (tipoPessoa == 2) {
            if (!$("#nome-fantasia").val()) {
                toastr.error('Informe o Nome Fantasia', 'Campo obrigatório');
                dadosValidos = false;
            }
            else if (!$("#razao-social").val()) {
                toastr.error('Informe a Razão Social', 'Campo obrigatório');
                dadosValidos = false;
            }
            else if (!$("#cnpj").val()) {
                toastr.error('Informe o CNPJ', 'Campo obrigatório');
                dadosValidos = false;
            }
            else if (!$("#zipcode").val()) {
                toastr.error('Informe o Cep', 'Campo obrigatório');
                dadosValidos = false;
            }
            else if (!$("#address").val()) {
                toastr.error('Informe o Endereço', 'Campo obrigatório');
                dadosValidos = false;
            }

        } else {
            toastr.warning('Selecione um tipo de pessoa', 'Alerta');
            dadosValidos = false;
        }

        if (tipoPessoa == 1 || tipoPessoa == 2) {
            if (!contatos.length) {
                toastr.error('Informe pelo menos um contato', 'Campo obrigatório');
                dadosValidos = false;
            }
        }
        
        if (!dadosValidos) {
            e.preventDefault();
            return false;
        }
    });
});

function MudarTipoPessoa(tipoPessoa) {
    switch (tipoPessoa) {
        // Pessoa Fisica
        case "1":
            $(".container-pessoa-fisica").show();
            $(".container-pessoa-juridica").hide();
            break;
        // Pessoa Juridica
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

function VerificarCPF() {
    var cpf = $("#cpf").val();

    if (!CPFValido(cpf)) {
        toastr.error("Preencha um CPF válido!", "CPF Inválido!");
        return false;
    }

    return true;
}