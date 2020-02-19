var vagasLivres = 0;
var vagasRestantes = 0;
var vagasSelecionadas = 0;
var vagasIndisponiveis = 0;
var vagaTotal = 0;
var VagaNaoUtilizada = 0;
var VagaAdiquirida = 0;
var VagaOcupada = 0;
var VagaNaoUtilizadaPadrao = 0;
var vagaTotalPadrao = 0;
var vagasRestantesPadrao = 0;


$(document).ready(function () {
    MakeChosen("Condomino");
    MakeChosen("Unidade");


    $("#Unidade").change(function () {
        var idUnidade = $("#Unidade").val();
        showLoading();
        BuscarCondominos(idUnidade);
        CalcularVagasUnidade(idUnidade);
        CarregarVagasUnidade(idUnidade);
    });


    $("#Condomino").change(function () {
        var idCondomino = $("#Condomino").val();
        var idUnidade = $("#Unidade").val();
        showLoading();
        BuscarVagasDetalheCondomino(idCondomino);
        CarregarVagasUnidadeCondomino(idUnidade, idCondomino);
        setInterval(function () {
            hideLoading();
        }, 8000);
    });


});

function CarregarVagasUnidadeCondomino(idUnidade, idCondomino) {
    $.post("/VagasCondominos/CarregarVagasUnidadeCondomino", { idUnidade: idUnidade, idCondomino: idCondomino })
        .done((response) => {
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
                $("#lista-vagas-result").empty();
                $("#lista-vagas-result").append(response);

                $('.vagaLivre').on('change', function () {
                    $(this)
                        .toggleClass('vagaLivre')
                        .toggleClass('vagaSelecionada');
                    vagaTotal = parseInt($("#TotalVagas").val());
                    vagasLivres = $('.vagaLivre').length;
                    vagasSelecionadas = $('.vagaSelecionada').length;
                    vagasIndisponiveis = $('.vagaIndisponivel').length;
                    VagaOcupada = $('.vagaOcupada').length;
                    vagasRestantes = vagaTotal - vagasSelecionadas - vagasIndisponiveis - VagaOcupada;
                    VagaAdiquirida = $("#AdiquiridasVagas").val();


                    if (VagaNaoUtilizadaPadrao === 0 || VagaNaoUtilizadaPadrao === "0") {
                        VagaNaoUtilizadaPadrao = $("#NaoUtilizadasVagas").val();
                    }

                    VagaNaoUtilizada = VagaNaoUtilizadaPadrao - vagasSelecionadas - vagasIndisponiveis;
                    $("#RestantesVagas").val(vagasRestantes);
                    $("#NaoUtilizadasVagas").val(VagaNaoUtilizada);
                });
            }
        })
        .fail(() => { })
        .always(() => { });
}

function BuscarVagasDetalheCondomino(idCondomino) {
    return $.ajax({
        url: '/VagasCondominos/BuscarVagasDetalheCondomino',
        type: 'POST',
        dataType: 'json',
        data: { idCondomino: idCondomino },
        success: function (response) {
            $("#AdiquiridasVagas").val(response.Adiquiridas);
            $("#NaoUtilizadasVagas").val(response.NaoUtilizadas);
            VagaAdiquirida = $("#AdiquiridasVagas").val();
            VagaNaoUtilizada = $("#NaoUtilizadasVagas").val();
            VagaNaoUtilizadaPadrao = $("#NaoUtilizadasVagas").val();
        }
    });
}

function BuscarCondominos(idUnidade) {
    return $.ajax({
        url: '/VagasCondominos/BuscarCondominos',
        type: 'POST',
        dataType: 'json',
        data: { idUnidade: idUnidade },
        success: function (response) {
            var equipeSelect = document.getElementById("Condomino");
            equipeSelect.innerHTML = "";

            var option = document.createElement("option");
            option.text = "Selecione o Condômino";
            option.value = 0;
            equipeSelect.options.add(option);
            $.each(response, function (i, item) {
                option = document.createElement("option");
                option.text = item.Pessoa.Nome;
                option.value = item.Id;
                equipeSelect.options.add(option);
            });
            MakeChosen("Condomino");
        }
    });
}


function CalcularVagasUnidade(idUnidade) {
    return $.ajax({
        url: '/VagasCondominos/CalcularVagasUnidade',
        type: 'POST',
        dataType: 'json',
        data: { idUnidade: idUnidade },
        success: function (response) {
            $("#TotalVagas").val(response.Total);
            $("#RestantesVagas").val(response.Restantes);
            vagaTotal = parseInt($("#TotalVagas").val());
            vagasRestantes = parseInt($("#RestantesVagas").val());
        }
    });
}


function CarregarVagasUnidade(idUnidade) {
    showLoading();
    $.post("/VagasCondominos/CarregarVagasUnidade", { idUnidade })
        .done((response) => {
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
                $("#lista-vagas-result").empty();
                $("#lista-vagas-result").append(response);
            }
        })
        .fail(() => { })
        .always(() => { hideLoading(); });
}

function ModalIndisponibilizar() {

    if ($('.vagaSelecionada').length === 0) {
        toastr.error("Selecione pelo menos uma vaga!");
        return false;
    }

    $("#modalIndisponibilizar").modal({ backdrop: 'static', keyboard: false });
}

function Cancelar() {
    $("#Vigencia").val("");
    $("#Motivo").val("");
    $("#modalIndisponibilizar").modal('hide');
}

function Indisponibilizar() {
    var vigencia = $("#Vigencia").val();
    var motivo = $("#Motivo").val();


    $.ajax({
        url: '/VagasCondominos/RegistrarIndiponibilidade',
        type: 'POST',
        dataType: 'json',
        data: { vigencia: vigencia, motivo: motivo },
        success: function (response) {
        }
    });

    $("div#lista-vagas-result").find(".vagaSelecionada").toggleClass('vagaSelecionada').toggleClass('vagaIndisponivel');
    $("div#lista-vagas-result").find(".vagaIndisponivel").attr("disabled", true);

    vagaTotal = parseInt($("#TotalVagas").val());
    vagasLivres = $('.vagaLivre').length;
    vagasSelecionadas = $('.vagaSelecionada').length;
    vagasIndisponiveis = $('.vagaIndisponivel').length;
    VagaOcupada = $('.vagaOcupada').length;
    vagasRestantes = vagaTotal - vagasSelecionadas - vagasIndisponiveis - VagaOcupada;
    $("#RestantesVagas").val(vagasRestantes);
    $("#Vigencia").val("");
    $("#Motivo").val("");
    $("#modalIndisponibilizar").modal('hide');

}

function SalvarDados() {
    debugger;
    if ($("#Unidade").val() === "") {
        toastr.error("O campo \"Unidade\" deve ser selecionado!", "Unidadeo Inválida!");
    }
    else if ($("#Condomino").val() === "0") {
        toastr.error("O campo \"Condomíno\" deve ser selecionado!", "Condomíno Inválido!");
    }
    else if ((vagasSelecionadas === 0 && vagasIndisponiveis === 0) || vagasSelecionadas === 0) {
        toastr.error("Selecione pelo menos uma vaga!", "Vaga não Selecionada!");
    }
    else {
        
        var idCondomino = $("#Condomino").val();
        var idUnidade = $("#Unidade").val();

        var vagasCondominosVM = {
            Total: vagaTotal,
            Restantes: vagasRestantes,
            Adiquiridas: VagaAdiquirida,
            NaoUtilizadas: VagaNaoUtilizada,
            Indisponivel: vagasIndisponiveis
        };
        $("#salvar").prop("disabled", true);
        $("#btncancelar").prop("disabled", true);
        $("#btnindisponibilizar").prop("disabled", true);
        showLoading();
        $.post("/VagasCondominos/SalvarDados", { vagasCondominosVM: vagasCondominosVM, idUnidade: idUnidade, idCondomino: idCondomino })
            .done((response) => {
                //if (typeof (response) === "object") {
                //    openCustomModal(null,
                //        null,
                //        response.TipoModal,
                //        response.Titulo,
                //        response.Mensagem,
                //        false,
                //        null,
                //        function () { });
                //} else {
                //    $("#lista-vagas-result").empty();
                //    $("#lista-vagas-result").append();
                //}
                openCustomModal(null,
                    null,
                    "Success",
                    "Sucesso",
                    "Vaga(s) Resgritada(s) Com Sucesso",
                    false,
                    null,
                    function () { });
                    location.reload(true);
            })
            .fail(() => { })
            .always(() => { showLoading(); });
        showLoading();
        setInterval(function () {
            hideLoading();
        }, 8000);
    }
    

}

