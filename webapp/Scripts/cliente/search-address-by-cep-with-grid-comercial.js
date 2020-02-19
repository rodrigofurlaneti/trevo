var enderecos = [];

$(document).ready(function () {
    $("#zipcode-comercial").on("keyup", function (e) {
        $(this).val(
            $(this).val()
                .replace(/\D/g, "")
                .replace(/^(\d{5})?(\d{3})/, "$1-$2"));
    });


    $("#search-icon-comercial").on("click",
        function () {
            getInformationFromCEPComercial();
        });

    $("#zipcode-comercial").blur(function () {
        getInformationFromCEPComercial();
    });
});

function clear_form_comercial() {
    $("#address-comercial, #district-comercial, #city-comercial, #state-comercial").val("");
}

function getInformationFromCEPComercial() {
    var cep = $("#zipcode-comercial").val().replace(/\D/g, "");
    if (cep !== "") {
        var validacep = /^[0-9]{8}$/;
        if (validacep.test(cep)) {
            $("#invalid_zipcode-comercial").hide();
            $("#address-comercial, #district-comercial, #city-comercial, #state-comercial").val("...");

            $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?", function (dados) {

                if (!("erro" in dados)) {
                    $("#address-comercial").val(dados.logradouro);
                    $("#district-comercial").val(dados.bairro);
                    $("#city-comercial").val(dados.localidade);
                    $("#state-comercial").val(dados.uf);
                }
                else {
                    clear_form_comercial();
                    toastr.error("CEP não encontrado.");
                }
            });
        }
        else {
            clear_form_comercial();
        }
    }
    else {
        $("#invalid_zipcode-comercial").hide();
        clear_form_comercial();
    }
}

function clearFieldsAddress() {
    enderecoEmEdicao = {};
    clear_form_comercial();
    selecionarTipoDeEndereco("0");
    $("#zipcode-comercial, #number-comercial, #complement-comercial").val("");
    $("#cancel-comercial").hide();
}


function VisibilidadeMsgSemEnderecos() {
    var totalEnderecos = $("#data_table_address tbody tr").length;
    var totalEnderecosInvisiveis = $("#data_table_address tbody tr:hidden").length;
    if (totalEnderecos === 0 || totalEnderecos === totalEnderecosInvisiveis) {
        $("#data_table_address tbody")
            .append(
                "<tr class=\"odd\"><td valign=\"top\" colspan=\"2\" class=\"dataTables_empty\">Nenhum registro encontrado</td></tr>");
    } else {
        $("#data_table_address tbody tr.odd").remove();
    }
}

