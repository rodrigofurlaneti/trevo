$(document).ready(function () {

    MakeChosen("unidade", null, "100%");
    MakeChosen("tiposelo", null, "100%");
    MakeChosen("numerolote", null, "100%");

    //GTE-1810
    $("#tiposelo").change(function () {
        LoadLotes();
    });

    $("#clientes").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/validadeselo/BuscarCliente',
                data: { descricao: request.term },
                type: "POST",
                success: function (data) {
                    if (!data.length) {
                        $("#cliente").val("");

                        var result = [
                            {
                                label: 'Nenhum resultado encontrado',
                                value: response.term
                            }
                        ];
                        response(result);
                    }
                    else {
                        response($.map(data, function (item) {
                            return { label: item.Descricao, value: item.descricao, id: item.Id };
                        }))
                    }
                },
                error: function (response) {
                    console.log(response.responseText);
                },
                failure: function (response) {
                    console.log(response.responseText);
                }
            });
        },
        select: function (e, i) {
            $("#cliente").val(i.item.id);

        },
        minLength: 3
    });
});


//GTE-1810
function LoadLotes() {
    $("#numerolote").html("").trigger("chosen:updated");

    var tiposeloid = $("#tiposelo").val();

    $.ajax({
        url: '/validadeselo/AtualizaLotes',
        data: { tiposeloid },
        dataType: "json",
        type: "POST",
        success: function (data) {
            var equipeSelect = document.getElementById("numerolote");
            equipeSelect.innerHTML = "";

            var option = document.createElement("option");
            option.text = "Selecione...";
            option.value = 0;
            equipeSelect.options.add(option);
            $.each(data, function (i, item) {
                option = document.createElement("option");
                option.text = item.Text;
                option.value = item.Value;
                equipeSelect.options.add(option);
            });

            $("#numerolote").trigger("chosen:updated");

        },
        error: function (response) {
            alert(response.responseText);
        },
        failure: function (response) {
            alert(response.responseText);
        }
    });
}

function Pesquisar() {
    //debugger;
    var unidade = $("#unidade").val();
    var cliente = $("#cliente").val();
    var tiposelo = $("#tiposelo").val();
    var numerolote = $("#numerolote").val();

    //if (!unidade) {
    //    toastr.warning("Informe uma Unidade!", "Campos");
    //    return true;
    //}
    //} else if (Data && !verificaData(Data)) {
    //    toastr.warning("O campo \"Data Vencimento\" deve ser preenchido com uma data válida!", "Campos");
    //    return true;
    //}

    var filtro = {
        Id: numerolote,
        Unidade: { Id: unidade },
        Cliente: { Id: cliente },
        TipoSelo: { Id: tiposelo }
    };

    $.ajax({
        url: "/validadeselo/Pesquisar",
        type: "POST",
        dataType: "json",
        data: {
            filtro: filtro
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
                
                $("#lista-emissoes").empty();
                $("#lista-emissoes").append(result);
            }
        },
        error: function (error) {
            
            $("#lista-emissoes").empty();
            $("#lista-emissoes").append(error.responseText);
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            
            MetodoUtil();
            hideLoading();
        }
    });
}

function ConfirmarAlteracao() {


    var id = $("#idemissaoselo").val();
    var tipodocumento = $("#tipodocumento").val();
    var numerodocumento = $("#numerodocumento").val();

    //if (tipodocumento == null || tipodocumento == 'undefined' || tipodocumento == 0) {
    //    toastr.warning("Informe um Tipo de Documento!", "Tipo Documento");
    //    return true;
    //}

    if (numerodocumento == null || numerodocumento == 'undefined' || numerodocumento == '') {
        toastr.warning("Informe uma data de validade!", "Validade");
        return true;
    }

    var emissaoselo = {
        Id: id,
        Validade: numerodocumento
    }

    $.ajax({
        url: "/validadeselo/ConfirmarAlteracao",
        type: "POST",
        dataType: "json",
        data: { emissaoselo },
        success: function (result) {
            if (typeof (result) === "object") {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () {
                        showLoading();
                    });
            }


            Pesquisar();
        },
        error: function (error) {
            //$("#modalBodyPagamento").empty();
            //$("#modalPagamento").modal('hide');
            //$("#modalPagamento").modal('hide');
            $("#lista-emissoes").empty();
            $("#lista-emissoes").append(error.responseText);
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

function ExecutarModal(id) {

    var idsLancamentosCobranca = id

    $.ajax({
        url: "/validadeselo/ExecutarModal",
        type: "POST",
        dataType: "json",
        data: { idsLancamentosCobranca },
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
                $("#modalBodyPagamento").empty();
                $("#modalBodyPagamento").append(result);
                $("#modalPagamento").modal({ backdrop: 'static', keyboard: false });
            }
            hideLoading();
        },
        error: function (error) {
            $("#modalBodyPagamento").empty();
            $("#modalBodyPagamento").append(error.responseText);
            $("#modalPagamento").modal({ backdrop: 'static', keyboard: false });
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            
        }
    });
}

function ExecutarPagamento() {

    var id = $("#executarpagamento").val();



    $.ajax({
        url: "/contaPagar/ExecutarPagamento",
        type: "POST",
        async: true,
        dataType: "json",
        data: {
            id: id
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
                $("#lista-emissoes").empty();
                $("#lista-emissoes").append(result);
            }
        },
        error: function (error) {
            hideLoading();
            $("#lista-emissoes").empty();
            $("#lista-emissoes").append(error.responseText);

        },
        beforeSend: function () {
        },
        complete: function () {
            MetodoUtil();
        }
    });

}

function disabledChosen(id) {
    $("#" + id).prop('disabled', true).trigger('chosen:updated').prop('disabled', false);
}

function makeReadonly(id) {
    $("#" + id).prop('readonly', true);
}