var idEquipeUnidade;
var idEquipeOrigem;


$(document).ready(function () {

    $('#unidadeOrigem').change(function () {
        debugger;
        var idUnidade = $(this).find(':selected').val();
        idEquipeUnidade = idUnidade;
        showLoading();
        $.ajax({
            url: '/Remanejamento/BuscarTipoEquipe',
            type: 'POST',
            dataType: 'json',
            data: { idUnidade: idUnidade },
            success: function (response) {
                CarregaTipoEquipe(response);
                hideLoading();
            }
        });
    });

    function CarregaEquipe(equipe) {
        var equipeSelect = document.getElementById("equipeOrigem");
        equipeSelect.innerHTML = "";

        var option = document.createElement("option");
        option.text = "Selecione a Equipe";
        option.value = 0;
        equipeSelect.options.add(option);
        $.each(equipe, function (i, item) {
            option = document.createElement("option");
            option.text = item.Nome;
            option.value = item.Id;
            equipeSelect.options.add(option);
        });
        MakeChosen("equipeOrigem");
        hideLoading();
    }



    $('#equipeOrigem').change(function () {
        var IdEquipe = $(this).find(':selected').val();
        idEquipeOrigem = IdEquipe;
        showLoading();

        $.post("/Remanejamento/BuscarColaboradoresOrigem", { IdEquipe })

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
                    $("#lista-colaboradores-origem-result").empty();
                    $("#lista-colaboradores-origem-result").append(response);

                }
            })
            .fail(() => { })
            .always(() => { hideLoading(); });

    });

    function CarregaTipoEquipe(equipe) {
       
        var equipeSelect = document.getElementById("tipoEquipeOrigem");
        equipeSelect.innerHTML = "";

        var option = document.createElement("option");
        option.text = "Selecione o tipo Equipe";
        option.value = 0;
        equipeSelect.options.add(option);
        $.each(equipe, function (i, item) {
            option = document.createElement("option");
            option.text = item.Descricao;
            option.value = item.Id;
            equipeSelect.options.add(option);
        });
        MakeChosen("tipoEquipeOrigem");
    }



    $('#tipoEquipeOrigem').change(function () {
        debugger;
        var IdTipoEquipe = $(this).find(':selected').val();
        showLoading();
        $.ajax({
            url: '/Remanejamento/BuscarEquipe',
            type: 'POST',
            dataType: 'json',
            data: { idUnidade: idEquipeUnidade, IdTipoEquipe: IdTipoEquipe },
            success: function (response) {
                CarregaEquipe(response);
            }
        });
        $.ajax({
            url: '/Remanejamento/SalvarTipoEquipeOrigem',
            type: 'POST',
            dataType: 'json',
            data: { IdTipoEquipe: IdTipoEquipe },
            success: function () {
                hideLoading(); 
            }
        });
        
    });

});

