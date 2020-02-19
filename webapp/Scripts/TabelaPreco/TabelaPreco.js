var funcionamentos = [];
var indiceFuncionamentosAdicionados = 0;
var indiceHorarioPrecoAdicionados = 0;
var valorTabelaEventoFeriado = "";
$(document).ready(function () {
    FormatarCampoData("DataInicioFuncionamento");
    FormatarCampoData("DataFimFuncionamento");

    FormatarCampoData("DataInicioEvento");
    FormatarCampoData("DataFimEvento");



    $('#TempoTolerancia').keyup(function () {
        $(this).val(this.value.replace(/\D/g, ''));
    });

    $('.valmoney').maskMoney({ decimal: ',', thousands: '', precision: 2 });
    $('.valor').maskMoney({ decimal: ',', thousands: '', precision: 2 });

    $(function () {
        $("input").on("keydown", function (e) {
            // use which ou charCode ou e.keyCode, dependendo do navegador
            var key = e.which || e.charCode || e.keyCode;
            // 9 é o caracter Unicode da tecla TAB
            if (key === 9) {
                if (e.preventDefault) {
                    e.preventDefault();
                }
                return false;
            }
        });
    });

    $.ajax({
        url: '/TabelaPreco/BuscarFuncionamentos',
        type: 'POST',
        dataType: 'json',
        success: function (response) {
            funcionamentos = response;
        }
    });
    $("#Form").submit(function (e) {
        var dadosValidos = true;

        if (!$("#Nome").val()) {
            toastr.error("O campo \"Nome Departamento\" deve ser preenchido!");
            dadosValidos = false;
        }

        if (!$("#TempoTolerancia").val()) {
            toastr.error("O campo \"Abreviatura:\" deve ser preenchido!");
            dadosValidos = false;
        }

        if (!dadosValidos) {
            e.preventDefault();
            return false;
        }
    });

    var container = $("#container-tables");
    var buttonAdd = $('.add-funcionamento');

    $("#add-evento").click(function () {
        buttonAdd = $("#add-evento");
    });
    $("#add-feriado").click(function () {
        buttonAdd = $("#add-feriado");
    });

    //var inputNomeTable = $('#nomeFuncionamento');
    //var inputDataInicio = $('#DataInicioFuncionamento');
    //var inputDataFim = $('#DataFimFuncionamento');

    jQuery("#horaiosprecoslist").data("placeholder", "teste").chosen();

    $("#horaiosprecoslist")
        .change(function () {
            $("select option:selected:last").each(function () {
                $("div#horaiosprecoslist_chosen").find("a").removeClass("search-choice-close");
                var nomeTable = $(this).text();
                var valorTable = $(this).val();
                var vinculado = $(this).hasClass("vinculado");
                if (vinculado === true) {
                    $(this).removeClass("vinculado");
                }
                else {
                        if (nomeTable === 'Feriado') 
                            $("#inserirFeriado").css("display", "block");
                        else if (nomeTable === 'Evento') 
                            $("#inserirEvento").css("display", "block");
                        
                    else {
                        $("#inserirFeriado").css("display", "none");
                        $("#inserirEvento").css("display", "none");
                        container.append(`
                            <div class="col-lg-3 table-mensalista">
                                <h3 class="nomeFuncionamento">${nomeTable}</h3>
                                    <label class="valorFuncionamento" style="display:none">${valorTable}</label>
                                <div class="col-xs-3">
                                    <table class="table table-striped table-bordered data_table_funcionamento">
                                        <thead>
                                            <tr>
                                                <th style="width: 1%; display:none;"></th>
                                                <th style="width: 50%;">Horario</th>
                                                <th style="width: 30%;">Valor</th>
                                            </tr>
                                        </thead>
                                        <tbody class="dynamicInputs">
                                            <tr>
                                                <td><input type="time" class="horario" onclick="ConfigurarMoney()"  /></td>
                                                <td><input type="text" class="valor" onclick="ConfigurarMoney()"  /></td>
                                            </tr>
                                        </tbody>
                                        <tfoot>
                                            <tr style="width: 100%; margin: 0 auto; text-align: center">
                                                <td>
                                                    <button type="button" class="btn btn-success btn-add-new-row">Periodo</button>
                                                </td>
                                                <td>
                                                    <button type="button" class="btn btn-primary btn-add-row"><i class="fa fa-plus-circle"></i>&nbsp;Adicionar Horário</button>
                                                </td>
                                            </tr>
                                        </tfoot>
                                    </table>
                                </div>
                            </div>
            `               );
                    }
                }


            });
        })
        .change();



    buttonAdd.click(function () {
        var tipoButton = $(this).data('value');
        var nomeTable;
        var dataInicio;
        var dataFim;
        if (tipoButton === 'feriado') {
            valorTabelaEventoFeriado = 4;
            nomeTable = $("#nomeFuncionamento").val();
            dataInicio = $("#DataInicioFuncionamento").val();
            dataFim = $("#DataFimFuncionamento").val();

            $("#nomeFuncionamento").val("");
            $("#DataInicioFuncionamento").val("");
            $("#DataFimFuncionamento").val("");
        }
        else {
            valorTabelaEventoFeriado = 5;
            nomeTable = $("#nomeEvento").val();
            dataInicio = $("#DataInicioEvento").val();
            dataFim = $("#DataFimEvento").val();

            $("#nomeEvento").val("");
            $("#DataInicioEvento").val("");
            $("#DataFimEvento").val("");
        }
       
        dataInicio = 
        container.append(`
             <div class="col-lg-3 table-mensalista">
                <h3 class="nomeFuncionamento">${nomeTable}</h3>
                    <label class="valorFuncionamento" style="display:none">${valorTabelaEventoFeriado}</label>
                    <label class="valorDataInicio" style="display:none">${dataInicio}</label>
                    <label class="valorDataFim" style="display:none">${dataFim}</label>
                                <div class="col-xs-3">
                     <table class="table table-striped table-bordered data_table_funcionamento">
                        <thead>
                            <tr>
                                <th style="width: 1%; display:none;"></th>
                                <th style="width: 25%;">Horario</th>
                                <th style="width: 25%;">Valor</th>
                            </tr>
                        </thead>
                        <tbody class="dynamicInputs">
                            <tr>
                                <td><input type="time" class="horario" onclick="ConfigurarMoney()" /></td>
                                <td><input type="text" class="valor" onclick="ConfigurarMoney()" /></td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr style="width: 100%; margin: 0 auto; text-align: center">
                                <td>
                                    <button type="button" class="btn btn-success btn-add-new-row">Periodo</button>
                                </td>
                                <td>
                                    <button type="button" class="btn btn-primary btn-add-row"><i class="fa fa-plus-circle"></i>&nbsp;Adicionar Horário</button>
                                </td>
                                
                            </tr>
                        </tfoot>
                    </table>
                </div>
            </div>
            `);
    });


    $('#container-tables').on('click', '.btn-add-row', function () {
        debugger;
        var escopoRaiz = $(this).parent().parent().parent().parent();  //('.dynamicInputs');
        //var escopo = $('.dynamicInputs');
        var escopo = escopoRaiz.find(".dynamicInputs");
        var escopo2 = escopo.parent().parent().parent();
        var nomeTabela = escopo2.find('.nomeFuncionamento').text();
        var tabelaId = escopo2.find('.nomeFuncionamento').data('value');
        var dataInicio = escopo2.find('.valorDataInicio').text();
        var dataFim = escopo2.find('.valorDataFim').text();
        var codFuncionamento = escopo2.find('.valorFuncionamento').text();
        var teste = escopo2.find(".data_table_funcionamento tr");
        var qtdLinha = (teste.length) - 3;
        var horariosPrecos = [];



        for (var i = 0; i <= qtdLinha; i++) {
            var idHorarioPreco = escopo.find('.valor:eq(' + i + ')').data('value');
            //alert(idHorarioPreco);
            var horario = escopo.find('.horario:eq(' + i + ')').val();
            var valor = escopo.find('.valor:eq(' + i + ')').val();
            indiceHorarioPrecoAdicionados--;
            var horarioPreco =
            {
                Id: idHorarioPreco,
                Horario: horario,
                Valor: valor
            };

            horariosPrecos.push(horarioPreco);
        }


        indiceFuncionamentosAdicionados--;

        var funcionamento =
        {
            Id: tabelaId,
            Nome: nomeTabela,
            CodFuncionamento: codFuncionamento,
            HorariosPrecos: horariosPrecos,
            DataInicio: dataInicio,
            DataFim: dataFim
        };

        atualizarFuncionamento(funcionamento);

            escopo.append(`
            <tr>
               <td><input type="time" onclick="ConfigurarMoney()" class="horario" /></td>
               <td><input type="text" onclick="ConfigurarMoney()" class="valor" /></td>
            </tr>
        `);

    });


    $('#container-tables').on('click', '.btn-add-new-row', function () {
        debugger;
        var escopoRaiz = $(this).parent().parent().parent().parent();  //('.dynamicInputs');
        //var escopo = $('.dynamicInputs');
        var escopo = escopoRaiz.find(".dynamicInputs");

        var botaoPeriodo = escopoRaiz.find(".btn-add-new-row");

        var escopo2 = escopo.parent().parent().parent();
        var tabelaId = escopo2.find('.nomeFuncionamento').data('value');

        if (tabelaId === null || tabelaId === undefined ) {
            //$(this).parent().parent().parent().parent().find('tr:last').remove();
            escopo.find('tr:last').remove();
            escopo.append(`
            <tr>
               <td><input type="text" class="horario" onclick="ConfigurarMoney()" /></td>
               <td><input type="text" class="valor" onclick="ConfigurarMoney()"  /></td>
            </tr>
        `);
        }
        else
        {
            $(this).parent().parent().parent().parent().find('tbody tr:last').remove();
            escopo.append(`
            <tr>
               <td><input type="text" class="horario" onclick="ConfigurarMoney()" /></td>
               <td><input type="text" class="valor" onclick="ConfigurarMoney()"  /></td>
            </tr>
        `);
        }

        botaoPeriodo.prop('disabled', true)

    });

    buscarTabelaPreco();

});

function atualizarFuncionamento(funcionamento) {
    showLoading();
    $.post("/TabelaPreco/AtualizarFuncionamentos", { funcionamento })
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
                //$("#lista-convenios-result").empty();
                //$("#lista-convenios-result").append(response);
            }
        })
        .fail(() => { })
        .always(() => { hideLoading(); });
}

function buscarTabelaPreco() {
    BuscarPartialSemFiltro("/TabelaPreco/BuscarTabelaPreco", "#lista-tabela-preco")
        .done(function () {
            ConfigTabelaGridSemCampoFiltroPrincipal();
        });
}

function ConfigurarMoney() {
    $('.valmoney').maskMoney({ decimal: ',', thousands: '', precision: 2 });
    $('.valor').maskMoney({ decimal: ',', thousands: '', precision: 2 });
}