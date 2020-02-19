$(document).ready(function () {
    AlterarCampoSeForFuncionario();
    ConfigTabelaGridDefault();
    MakeChosen("perfil", null, "100%");
    MakeChosen("funcionario", null, "100%");

    $("#funcionario").change(function (event) {
        VerificarSeUsuarioExiste($(this).val())
            .done((existe) => {
                if (existe) {
                    toastr.warning("Já existe um usuário para este funcionário", "Alerta");
                    $("#salvar").disabled();
                } else {
                    $("#salvar").unDisabled();
                    BuscarDadosDaPessoa($(this).val());
                }
            });
    });

    $("#uploadImgAvatar").change(function () {
        GetAvatarImage(this);
    });

    $("#tem-acesso-ao-pdv").change(function () {
        if (this.checked) {
            $("#operador-perfil").unDisabled();
            $("#unidade").unDisabled();
        } else {
            $("#operador-perfil").disabled().val("");
            $("#unidade").disabled().val("");
        }
    });

    //$("#Login").blur(function () {
    //    if (!CPFValido(this.value))
    //        this.value = "";

    //    validaCPF
    //});

    $("#Form").submit(function (e) {
        let ehFuncionario = $("#eh-funcionario").isChecked();
        var dadosValidos = true;

        if (ehFuncionario) {
            if (!$("#funcionario").val()) {
                toastr.error("O combo \"Nome do Funcionário\" deve ser selecionado!");
                dadosValidos = false;
            }
        } else {
            if (!$("#nome-completo").val()) {
                toastr.error("O combo \"Nome do Complete\" deve ser preenchido!");
                dadosValidos = false;
            }
            if (!$("#Login").val()) {
                toastr.error("O combo \"Login\" deve ser preenchido!");
                dadosValidos = false;
            }
        }

        if (!$("#perfil").val()) {
            toastr.error("O combo \"Perfis\" deve ser selecionado!");
            dadosValidos = false;
        }

        if ($("#Senha").val() === '' || $.trim($("#Senha").val()) === "") {
            toastr.error("O campo \"Senha\" deve ser preenchido!");
            dadosValidos = false;
        }

        let email = $("#Email").val();
        if (email === '' || $.trim(email) === "" || EmailValido(email) === false) {
            toastr.error("O campo \"Email\" deve ser preenchido com um email válido!");
            dadosValidos = false;
        }

        if ($("#tem-acesso-ao-pdv").is(":checked")) {
            if (!$("#operador-perfil").val()) {
                toastr.error("Seleciona um Operador Perfil!");
                dadosValidos = false;
            }

            if (!$("#unidade").val()) {
                toastr.error("Seleciona uma Unidade!");
                dadosValidos = false;
            }
        }

        if (!dadosValidos) {
            e.preventDefault();
            return false;
        }
    });

    $("#eh-funcionario").change(function () {
        AlterarCampoSeForFuncionario();
    });
});

function VerificarSeUsuarioExiste(funcionarioId) {
    return get(`VerificarSeUsuarioExiste?funcionarioId=${funcionarioId}`);
}

function BuscarDadosDaPessoa(funcionarioId) {
    if (!funcionarioId) {
        $('#Login').val("");
        $('#Email').val("");
        return;
    }

    return post('BuscarDadosDaPessoa', { funcionarioId })
        .done((response) => {
            if (!response.Login) {
                toastr.warning("Funcionário não possui CPF cadastrado", "Alerta");
                $("#salvar").disabled();
            } else {
                $("#salvar").unDisabled();
            }

            $('#Login').val(response.Login);
            $('#Email').val(response.Email);
        });
}

function GetAvatarImage(file) {
    var input = file;
    var url = $(file).val();
    var ext = url.substring(url.lastIndexOf('.') + 1).toLowerCase();
    if (input.files && input.files[0] && (ext == "gif" || ext == "png" || ext == "jpeg" || ext == "jpg")) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imgAvatar').attr('src', e.target.result);
            ArmazenarImagem(e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
    else {
        $('#imgAvatar').attr('src', '../../Content/img/avatars/sunny-big.png');
    }
}

function ArmazenarImagem(dataBase64) {

    $.ajax({
        url: '/usuario/ArmazenarImagem',
        type: 'POST',
        data: { file: dataBase64 },
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
}

function AlterarCampoSeForFuncionario() {
    let ehFuncionario = $("#eh-funcionario").isChecked();

    if (ehFuncionario) {
        $("#container-nome-funcionario").show();
        $("#container-nome-completo").hide();
        $("#nome-completo").val("");
        $("#Login").disabled();
    } else {
        $("#container-nome-completo").show();
        $("#container-nome-funcionario").hide();
        $("#Login").unDisabled();
        $("#funcionario").val("");
        FormataCampoCpf("Login");
        CPFValido
        MakeChosen("funcionario");
    }
}

function ToggleSenha() {
    if ($("#btn-visualizar .fa.fa-eye").length) {
        $("#btn-visualizar .fa.fa-eye").removeClass("fa-eye").addClass("fa-eye-slash");
        $("#Senha").attr("type", "text");
    }
    else {
        $("#btn-visualizar .fa.fa-eye-slash").removeClass("fa-eye-slash").addClass("fa-eye");
        $("#Senha").attr("type", "password");
    }
}