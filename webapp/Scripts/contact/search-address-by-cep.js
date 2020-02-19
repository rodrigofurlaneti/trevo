$("#zipcode").on("keyup", function (e) {
    $(this).val(
        $(this).val()
            .replace(/\D/g, '')
            .replace(/^(\d{5})?(\d{3})/, "$1-$2"));
});

$('#search-icon').on('click', function () {
    getInformationFromCEP();
})

function clear_form() {
    $("#address, #district, #city, #state").val("");
}

$("#zipcode").blur(function () {
    getInformationFromCEP();
});

function getInformationFromCEP() {
    var cep = $("#zipcode").val().replace(/\D/g, '');
    if (cep != "") {
        var validacep = /^[0-9]{8}$/;
        if (validacep.test(cep)) {
            $('#invalid_zipcode').hide();
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
        $('#invalid_zipcode').hide();
        clear_form();
    }
}