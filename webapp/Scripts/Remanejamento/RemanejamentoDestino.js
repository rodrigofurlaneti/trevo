var idEquipeUnidadeDestino;
var idEquipeDestino;


$(document).ready(function () {

 
    $('#unidadeDestino').change(function () {
        var idUnidade = $(this).find(':selected').val();
        idEquipeUnidadeDestino = idUnidade;
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
        var equipeSelect = document.getElementById("equipeDestino");
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
        MakeChosen("equipeDestino");
        hideLoading();
    }



    $('#equipeDestino').change(function () {
        var IdEquipe = $(this).find(':selected').val();
        idEquipeDestino = IdEquipe;
        showLoading();
        $.post("/Remanejamento/BuscarColaboradores", { IdEquipe })

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
                    $("#lista-colaboradores-result").empty();
                    $("#lista-colaboradores-result").append(response);

                }
            })
            .fail(() => { })
            .always(() => { hideLoading(); });
    });

    function CarregaTipoEquipe(equipe) {
        var equipeSelect = document.getElementById("tipoEsquipeDestino");
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
        MakeChosen("tipoEsquipeDestino");
    }



    $('#tipoEsquipeDestino').change(function () {
        
        var IdTipoEquipe = $(this).find(':selected').val();
        showLoading();
        $.ajax({
            url: '/Remanejamento/BuscarEquipe',
            type: 'POST',
            dataType: 'json',
            data: { idUnidade: idEquipeUnidadeDestino, IdTipoEquipe: IdTipoEquipe },
            success: function (response) {
                CarregaEquipe(response);
            }
        });
        showLoading();
        $.ajax({
            url: '/Remanejamento/SalvarTipoEquipeDestino',
            type: 'POST',
            dataType: 'json',
            data: { IdTipoEquipe: IdTipoEquipe },
            success: function () {
                hideLoading();
            }
        });
        

    });


   

    //$('#selecionarTodosDestino').change(function () {
    //    
    //    var marcado = $(this).prop('checked');

    //    alert(marcado);

    //    //$('.colaboradorSelecionado').each(function () {
    //    //    var ckbOrcamento = $(this);

    //    //    if (marcado) {
    //    //        ckbOrcamento.prop('checked', true);
    //    //    }

    //    //    ckbOrcamento.change();
    //    //});
    //});

});



