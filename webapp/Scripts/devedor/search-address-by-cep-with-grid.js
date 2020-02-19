$(document).ready(function () {
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

    $("#zipcode").blur(function () {
        getInformationFromCEP();
    });

    $("#add-address").click(function (ele) {

        var id = $("#id").val();
        var type = $("#type option:selected").val();
        var zipcode = $("#zipcode").val();
        var city = $("#city").val();
        var state = $("#state").val();
        var address = $("#address").val();
        var number = $("#number").val();
        var district = $("#district").val();
        var complement = $("#complement").val();
        var blacklist = $("#blacklist").val();
        var tipoMotivo = $("#tipoMotivo").val();
        var descricaoMotivo = $("#descricaoMotivo").val();

        var nIdx = $("table#data_table_address tbody tr").length;

        $("#data_table_address tbody")
            .append("<tr>" +
                "<td style=\"display: none;\"></td>" +
                "<td></td>" +
                "<td></td>" +
                "<td></td>" +
                "<td style=\"display: none;\">" +
                "<input type=\"hidden\" id=\"" + nIdx + "Id\" value=\"" + id + "\">" +
                "<input type=\"hidden\" id=\"" + nIdx + "Tipo\" value=\"" + type + "\">" +
                "<input type=\"hidden\" id=\"" + nIdx + "Cep\" value=\"" + zipcode + "\">" +
                "<input type=\"hidden\" id=\"" + nIdx + "Logradouro\" value=\"" + address + "\">" +
                "<input type=\"hidden\" id=\"" + nIdx + "Numero\" value=\"" + number + "\">" +
                "<input type=\"hidden\" id=\"" + nIdx + "Bairro\" value=\"" + district + "\">" +
                "<input type=\"hidden\" id=\"" + nIdx + "Cidade\" value=\"" + city + "\">" +
                "<input type=\"hidden\" id=\"" + nIdx + "Estado\" value=\"" + state + "\">" +
                "<input type=\"hidden\" id=\"" + nIdx + "Complemento\" value=\"" + complement + "\">" +
                "<input type=\"hidden\" id=\"" + nIdx + "Blacklist\" value=\"" + blacklist + "\">" +
                "<input type=\"hidden\" id=\"" + nIdx + "TipoMotivo\" value=\"" + tipoMotivo + "\">" +
                "<input type=\"hidden\" id=\"" + nIdx + "DescricaoMotivo\" value=\"" + descricaoMotivo + "\">" +
                "</td>" +
                "</tr>");

        removeHiddenItemsAddress();
        saveAddress();
        clearFieldsAddress();
    });
});

function clear_form() {
    $("#address, #district, #city, #state").val("");
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

//Function do Grid
function saveAddress() {
    //if ($("#data_table_address tbody tr.odd").length > 0)
    //    $("#data_table_address tbody tr.odd").remove();

    var dt = $("#data_table_address tbody tr");
    dt.splice(0, 0);
    var obj = [];

    for (var i = 0; i < dt.length; i++) {
        var id = $("#" + i + "Id").val() !== "" ? $("#" + i + "Id").val() : 0;
        var blacklist = $("#" + i + "Blacklist").val() !== "" ? $("#" + i + "Blacklist").val() : false;
        var tipoMotivo = $("#" + i + "TipoMotivo").val() !== "" ? $("#" + i + "TipoMotivo").val() : 1;
        var descricaoMotivo = $("#" + i + "DescricaoMotivo").val();

        obj.push({
            Id: id,
            Tipo: $("#" + i + "Tipo").val(),
            Cep: $("#" + i + "Cep").val(),
            Logradouro: $("#" + i + "Logradouro").val(),
            Numero: $("#" + i + "Numero").val(),
            Bairro: $("#" + i + "Bairro").val(),
            Cidade:{ Descricao: $("#" + i + "Cidade").val(),}, 
            Estado: $("#" + i + "Estado").val(),
            Complemento: $("#" + i + "Complemento").val(),
            Blacklist: blacklist,
            TipoMotivo: tipoMotivo,
            DescricaoMotivo: descricaoMotivo
        });
    }
    
    showLoading();
    $.ajax({
        url: "/Devedor/AdicionarEndereco",
        datatype: "JSON",
        type: "GET",
        data: { json: JSON.stringify(obj) },
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
            } else {
                $("#lista-endereco-result").empty();
                $("#lista-endereco-result").append(response);
                $("#lista-endereco-result table").dataTable();
            }
        }
    });
    hideLoading();

    if (obj.length === 0)
        VisibilidadeMsgSemEnderecos();
}

function editAddress(ele, index) {
    var hiddenItems = $("#data_table_address tbody tr:hidden");
    if (hiddenItems) hiddenItems.show();
    clearFieldsAddress();

    $("#type").val($("#" + index + "Tipo").val());
    $("#id").val($("#" + index + "Cep").val());
    $("#zipcode").val($("#" + index + "Cep").val());
    $("#city").val($("#" + index + "Cidade").val());
    $("#state").val($("#" + index + "Estado").val());
    $("#address").val($("#" + index + "Logradouro").val());
    $("#number").val($("#" + index + "Numero").val());
    $("#district").val($("#" + index + "Bairro").val());
    $("#complement").val($("#" + index + "Complemento").val());
    $("#blacklist").val($("#" + index + "Blacklist").val());
    $("#tipoMotivo").val($("#" + index + "TipoMotivo").val());
    $("#descricaoMotivo").val($("#" + index + "DescricaoMotivo").val());

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

