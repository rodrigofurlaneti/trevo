
$(document).ready(function () {
    
    MakeChosen("nome");
    MakeChosen("tipolocacao");
    MakeChosen("unidade");
    MakeChosen("type");

    $("#zipcode").on("keyup", function (e) {
        $(this).val(
            $(this).val()
                .replace(/\D/g, "")
                .replace(/^(\d{5})?(\d{3})/, "$1-$2"));
    });

    $("#zipcode").blur(function () {
        getInformationFromCEP();
    });

    $("#estoque-principal").change(function () {
        console.log(this.checked);
        if (this.checked) {
            get("VerificarSeExisteEstoquePrincipal")
                .done((response) => {
                    if (response.Existe) {
                        this.checked = false;
                        openCustomModal(null, "ConfirmarSubstituicaoEstoquePrincipal()", "warning", "Confirmar Substituição", `O Estoque "${response.Estoque}" já está marcado como padrão, deseja substituir por esse?`, true, "Sim", null, "Não")
                    }
                });
        }
    });
});

function ConfirmarSubstituicaoEstoquePrincipal() {
    $("#estoque-principal").prop("checked", true);
}

function getInformationFromCEP() {
    var cep = $("#zipcode").val().replace(/\D/g, "");
    if (cep !== "") {
        var validacep = /^[0-9]{8}$/;
        if (validacep.test(cep)) {
            $("#invalid_zipcode").hide();
            $("#address, #district, #city, #state").val("...");

            $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?", function (dados) {

                if (!("erro" in dados)) {
                    $("#address").val(dados.logradouro);
                    $("#district").val(dados.bairro);
                    $("#city").val(dados.localidade);
                    $("#state").val(dados.uf);
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

function clear_form() {
    $("#address, #district, #city, #state").val("");
}