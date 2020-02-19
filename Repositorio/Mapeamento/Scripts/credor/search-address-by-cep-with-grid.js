$("#zipcode").on("keyup", function (e) {
    $(this).val(
        $(this).val()
            .replace(/\D/g, "")
            .replace(/^(\d{5})?(\d{3})/, "$1-$2"));
});

$("#search-icon").on("click",
    function () {
        getInformationFromCEP();
    });

function clear_form() {
    $("#address, #district, #city, #state").val("");
}

$("#zipcode").blur(function () {
    getInformationFromCEP();
});

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

//Function do Grid
var idAddressInEdit;

$("#add-address").click(function (ele) {

    var zipcode = document.getElementById("zipcode").value;
    var city = document.getElementById("city").value;
    var state = document.getElementById("state").value;
    var address = document.getElementById("address").value;
    var number = document.getElementById("number").value;
    var district = document.getElementById("district").value;
    var complement = document.getElementById("complement").value;
    var type = $("#type option:selected").val();
    var typeText = $("#type option:selected").text();

    var resumo = "Tipo : " + typeText +
        " - Endereço : " + address +
        ", " +
        number +
        ((complement !== "") ? " - " + complement : "") +
        " - " +
        zipcode +
        ((district !== "") ? " - " + district : "") +
        ((city !== "") ? " - " + city : "") +
        ((state !== "") ? " - " + state : "");

    var nextIndex = $("table#data_table_address tbody tr").length;

    $("#data_table_address tbody")
        .append("<tr>" +
        "<td style=\"display: none;\" id=\"id_address\">" + (idAddressInEdit ? idAddressInEdit : 0) + "</td>" +
        "<td class=\"col - xs - 10\">" +
        resumo +
        "</td>" +
        "<td class=\"col - xs - 2\">" +
        "<span class=\"btn btn-primary\" style=\"margin-right: 10px;\" onclick=\"editAddress(this, " + nextIndex + ")\"><i class=\"fa fa-edit\"></i></span>" +
        "<span class=\"btn btn-danger\" onclick=\"removeAddress(this)\"><i class=\"fa fa-remove\"></i></span>" +
        "</td>" +
        "<td style=\"display: none;\">" +
        "<input type=\"hidden\" id=\"" + nextIndex + "Tipo\" value=\"" + type + "\">" +
        "<input type=\"hidden\" id=\"" + nextIndex + "Cep\" value=\"" + zipcode + "\">" +
        "<input type=\"hidden\" id=\"" + nextIndex + "Logradouro\" value=\"" + address + "\">" +
        "<input type=\"hidden\" id=\"" + nextIndex + "Numero\" value=\"" + number + "\">" +
        "<input type=\"hidden\" id=\"" + nextIndex + "Bairro\" value=\"" + district + "\">" +
        "<input type=\"hidden\" id=\"" + nextIndex + "Cidade\" value=\"" + city + "\">" +
        "<input type=\"hidden\" id=\"" + nextIndex + "Estado\" value=\"" + state + "\">" +
        "<input type=\"hidden\" id=\"" + nextIndex + "Complemento\" value=\"" + complement + "\">" +
        "</td>" +
        "</tr>");

    removeHiddenItemsAddress();
    saveAddress();
    clearFieldsAddress();
});

function saveAddress() {
    if ($("#data_table_address tbody tr.odd").length > 0)
        $("#data_table_address tbody tr.odd").remove();

    var dt = $("#data_table_address tbody tr");

    var obj = [];

    for (var i = 0; i < dt.length; i++) {
        obj.push({
            Id: dt[i].children[0] != undefined ? dt[i].children[0].innerText ? parseInt(dt[i].children[0].innerText) : 0 : 0,
            Cep: $("#" + i + "Cep").val(),
            Logradouro: $("#" + i + "Logradouro").val(),
            Numero: $("#" + i + "Numero").val(),
            Bairro: $("#" + i + "Bairro").val(),
            Cidade: $("#" + i + "Cidade").val(),
            Estado: $("#" + i + "Estado").val(),
            Complemento: $("#" + i + "Complemento").val(),
            Tipo: $("#" + i + "Tipo").val()
        });
    }

    $.ajax({
        url: "/credor/AdicionarEndereco",
        type: "POST",
        dataType: "json",
        data: { json: JSON.stringify(obj) },
        success: function (data) { }
    });

    if (obj.length === 0)
        VisibilidadeMsgSemEnderecos();
}

function editAddress(ele, index) {
    var hiddenItems = $("#data_table_address tbody tr:hidden");
    if (hiddenItems) hiddenItems.show();
    clearFieldsAddress();

    idAddressInEdit = ele.parentElement.parentElement.children[0].innerText.trim();

    document.getElementById("zipcode").value = $("#" + index + "Cep").val();
    document.getElementById("city").value = $("#" + index + "Cidade").val();
    document.getElementById("state").value = $("#" + index + "Estado").val();
    document.getElementById("address").value = $("#" + index + "Logradouro").val();
    document.getElementById("number").value = $("#" + index + "Numero").val();
    document.getElementById("district").value = $("#" + index + "Bairro").val();
    document.getElementById("complement").value = $("#" + index + "Complemento").val();
    $("#type").val($("#" + index + "Tipo").val());

    $("#cancel").show();
    $(ele.parentElement.parentElement).hide();

    VisibilidadeMsgSemEnderecos();
}

function removeAddress(ele) {
    ele.parentElement.parentElement.remove();
    saveAddress();

    VisibilidadeMsgSemEnderecos();
}

function cancelEditAddress() {
    var hiddenItems = $("#data_table_address tbody tr:hidden");
    if (hiddenItems) hiddenItems.show();
    clearFieldsAddress();

    VisibilidadeMsgSemEnderecos();
}

function removeHiddenItemsAddress() {
    var hiddenItems = $("#data_table_address tbody tr:hidden");
    if (hiddenItems) hiddenItems.remove();
}

function clearFieldsAddress() {
    idAddressInEdit = 0;
    clear_form();
    $("#type").val("0");
    $("#zipcode, #number, #complement").val("");
    $("#cancel").hide();
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
