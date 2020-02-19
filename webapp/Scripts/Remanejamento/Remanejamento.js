var equipeOrigem = "";
var equipeDestino = "";
var colaboradorOrigem = [];
var colaboradorDestino = [];
var indiceColaboradorOrigem = 0;
var indiceColaboradorDestino = 0;
var operacaoTransferencia = false;


$(document).ready(function () {


    $("#remanejamentoForm").submit(function (e) {

        

        if (ValidaCampos()) {

            
            if (operacaoTransferencia === false) {
                
                toastr.error("obrigatório realizar tipo de operação de remanejamento");
                return false;
            }

            return true;
        }

    });

    $('#salvar').attr('disabled', 'disabled');

    FormatarApresentacaoCampos();

});

function FormatarApresentacaoCampos() {

    MakeChosen("tipo-operacao");
    MakeChosen("unidadeOrigem");
    MakeChosen("equipeOrigem");
    MakeChosen("tipoEquipeOrigem");
    MakeChosen("horarioOrigem");
    MakeChosen("unidadeDestino");
    MakeChosen("equipeDestino");
    MakeChosen("tipoEsquipeDestino");
    MakeChosen("horarioDestino");

    FormatarCampoData("DataFinal");

    $('#DataFinal').attr("disabled", true);

    $('#Fixo').click(function () {

        if (this.checked !== true) {
            $('#DataFinal').attr("disabled", false);
        }
        else {
            $('#DataFinal').attr("disabled", true);
            $('#DataFinal').val('');
        }
    });


}

function SetarOperacaoTransferencia() {

    operacaoTransferencia = false;
}


function checkFluencyDestino() {
    var checkbox = document.getElementById('selecionarTodosDestino');
    if (checkbox.checked === true) {

        $('.colaboradorSelecionado').each(function () {
            var ckbOrcamento = $(this);
            ckbOrcamento.prop('checked', true);
            ckbOrcamento.change();
        });
    }
    else {
        $('.colaboradorSelecionado').each(function () {
            var ckbOrcamento = $(this);
            ckbOrcamento.prop('checked', false);
            ckbOrcamento.change();
        });
    }

    adicionarTodosColaboradoresDestinoClick();
}

function checkFluencyOrigem() {



    var checkbox = document.getElementById('selecionarTodosOrigem');
    if (checkbox.checked === true) {

        $('.colaboradorSelecionadoOrigem').each(function () {
            var ckbOrcamento = $(this);
            ckbOrcamento.prop('checked', true);
            ckbOrcamento.change();
        });



    }
    else {
        $('.colaboradorSelecionadoOrigem').each(function () {
            var ckbOrcamento = $(this);
            ckbOrcamento.prop('checked', false);
            ckbOrcamento.change();
        });
    }


    adicionarTodosColaboradoresOrigemClick();
}

function Transferir() {
    if (!ValidaCampos()) {
        return false;
    }
       

    atualizarColaboradorOrigem(colaboradorOrigem, colaboradorDestino);

    $('#salvar').removeAttr('disabled');
    operacaoTransferencia = true;

}

function removerColaboradorOrigemClick(idColaborador) {
    colaboradorOrigem = colaboradorOrigem.filter(x => x.Id !== idColaborador);
}


function adicionarTodosColaboradoresOrigemClick() {

    var table = document.getElementById('tbColaboradorOrigem');
    var aChk = document.getElementsByName('colaboradorOrigemChk');

    var idColaborador = 0;
    var id = 0;

    for (var a = 0; a < table.rows.length - 1; a++) {

        id++;
        idColaborador = aChk[a].value;

        adicionarColaboradorOrigemClick(idColaborador, id);

    }
}

function adicionarTodosColaboradoresDestinoClick() {

    var table = document.getElementById('tbColaboradorDestino');
    var aChk = document.getElementsByName('colaboradorDestinoChk');

    var idColaborador = 0;
    var id = 0;

    for (var a = 0; a < table.rows.length - 1; a++) {

        id++;
        idColaborador = aChk[a].value;

        adicionarColaboradorDestinoClick(idColaborador, id);

    }
}

function adicionarColaboradorOrigemClick(idColaborador, id) {
    var aChk = document.getElementsByName('colaboradorOrigemChk');

    var table = document.getElementById('tbColaboradorOrigem');

    var periodo = "";
    var descricaoColaborador = "";

    

   


    var firstCol = table.rows[id].cells[1];

    descricaoColaborador = firstCol.innerHTML;


    var secondCol = table.rows[id].cells[2];

    periodo = secondCol.innerHTML.replace('Turno', '');

    var idPessoa = table.rows[id].cells[3].innerHTML;

    if (aChk[id - 1].checked === true) {

        var turno = {
            Periodo: periodo

        };

        var pessoa = {
            Nome: descricaoColaborador,
            Id: idPessoa
        };

        var nomecolaborador = {

            Pessoa: pessoa
        };


        var colaborador = {
            Id: idColaborador,
            Descricao: "", //Pegar a descricao do colaborador
            Turno: turno,
            NomeColaborador: nomecolaborador
        };

        colaboradorOrigem.push(colaborador);


    }
    else {

        removerColaboradorOrigemClick(idColaborador);
    }

    SetarOperacaoTransferencia();

}

function removerColaboradorDestinoClick(idColaborador) {
    colaboradorDestino = colaboradorDestino.filter(x => x.Id !== idColaborador);
}


function adicionarColaboradorDestinoClick(idColaborador, id) {


    var aChk = document.getElementsByName('colaboradorDestinoChk');

    var table = document.getElementById('tbColaboradorDestino');

    var periodo = "";
    var descricaoColaborador = "";


   

    var firstCol = table.rows[id].cells[1];

    descricaoColaborador = firstCol.innerHTML;


    var secondCol = table.rows[id].cells[2];

    periodo = secondCol.innerHTML.replace('Turno', '');

    var idPessoa = table.rows[id].cells[3].innerHTML;

    if (aChk[id - 1].checked === true) {

        var turno = {
            Periodo: periodo

        };

        var pessoa = {
            Nome: descricaoColaborador,
            Id: idPessoa
        };

        var nomecolaborador = {

            Pessoa: pessoa
        };


        var colaborador = {
            Id: idColaborador,
            Descricao: "", //Pegar a descricao do colaborador
            Turno: turno,
            NomeColaborador: nomecolaborador
        };

        colaboradorDestino.push(colaborador);


    }
    else {

        removerColaboradorDestinoClick(idColaborador);
    }

    SetarOperacaoTransferencia();
}

function adicionarColaboradorOrigem() {

    var table = document.getElementById('tbColaboradorOrigem');

    var aChk = document.getElementsByName('colaboradorOrigemChk');

    var descricaoColaborador = "";
    var periodo = "";

    for (var i = 0; i < aChk.length; i++) {

        if (aChk[i].checked === true) {

            for (var a = 0; a < table.rows.length; a++) {

                if (a === i) {

                    var firstCol = table.rows[i].cells[1];

                    descricaoColaborador = firstCol.innerHTML;

                    var secondCol = table.rows[i].cells[2];

                    periodo = secondCol.innerHTML.replace('Turno', '');


                }

            }

            var turno = {
                Periodo: periodo

            };

            var colaborador = {
                Id: aChk[i].value,
                Descricao: descricaoColaborador, //Pegar a descricao do colaborador
                Turno: turno
            };

            colaboradorOrigem.push(colaborador);
        }

    }



    var equipeOrigem = {
        Id: $("#equipeOrigem").val(),
        Nome: $('#equipeOrigem :selected').text(),
        Colaboradores: colaboradorOrigem
    };

    //atualizarColaboradorOrigem(equipeOrigem);

}


function adicionarColaboradorDestino() {

    var aChk = document.getElementsByName('colaboradorDestinochk');

    for (var i = 0; i < aChk.length; i++) {

        if (aChk[i].checked === true) {

            var colaborador = {
                Id: aChk[i].value,
                Descricao: "",//Pegar a descricao do colaborador
                Periodo: ""
            };

            colaboradorDestino.push(colaborador);
        }

    }


    var equipeDestino = {
        Id: $(".equipeDestino").val(),
        Nome: $('#equipeDestino :selected').text(),
        Colaboradores: colaboradorDestino
    };

    atualizarColaboradorDestino(equipeDestino);

}

function atualizarColaboradorOrigem(colaboradorOrigem, colaboradorDestino) {

    

    //showLoading();

    if (colaboradorOrigem.length === 0) {

        toastr.error("Selecione ao menos um \"Colaborador da origem\"");
        return;

    }

    if (colaboradorDestino.length === 0) {

        toastr.error("Selecione ao menos um \"Colaborador ou Slot de destino\"");
        return;
    }

    

    equipeOrigem = {
        Id: $("#equipeOrigem").val(),
        Nome: $('#equipeOrigem :selected').text(),
        Colaboradores: colaboradorOrigem
    };

    equipeDestino = {
        Id: $("#equipeDestino").val(),
        Nome: $('#equipeDestino :selected').text(),
        Colaboradores: colaboradorDestino
    };

    $.post("/Remanejamento/AtualizarEquipes", { equipeOrigem, equipeDestino })
        .done((response) => {
            if (response.tipo === "danger") {
                colaboradorOrigem = [];
                colaboradorDestino = [];
                colaboradorOrigem.length = 0;
                colaboradorDestino.length = 0;
                indiceColaboradorOrigem = 0;
                indiceColaboradorDestino = 0;
                toastr.error(response.message);
                hideLoading();
                return;
            } else {
                $("#lista-colaboradores-result").empty();
                $("#lista-colaboradores-origem-result").empty();
                $("#lista-colaboradores-origem-result").html(response.divGridColaboradorOrigem);
                $("#lista-colaboradores-result").html(response.divGridColaboradorDestino);
                operacaoTransferencia = true;
            }
        })
        .fail(() => { })
        .always(() => { hideLoading(); });

    indiceColaboradorOrigem = 0;
    indiceColaboradorDestino = 0;

    for (var i = colaboradorOrigem.length; i > 0; i--) {
        colaboradorOrigem.pop();
    }

    for (var x = colaboradorDestino.length; x > 0; x--) {
        colaboradorDestino.pop();
    }

}


function atualizarColaboradorDestino(colaboradorDestino) {
    showLoading();

    equipeDestino = {
        Id: $(".equipeDestino").val(),
        Nome: $('#equipeDestino :selected').text(),
        Colaboradores: colaboradorDestino
    };



    $.post("/Remanejamento/AtualizarEquipeDestino", { equipeDestino })
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
}


function ExisteSlot(tableColaborador,checkBoxColaborador) {

    var table = document.getElementById(tableColaborador);
    var aChk = document.getElementsByName(checkBoxColaborador);

    var id = 0;

    for (var a = 0; a < table.rows.length - 1; a++) {

        id++;
  
        var firstCol = table.rows[id].cells[1];

        var descricaoColaborador = firstCol.innerHTML;

        if (aChk[id - 1].checked === true) {

            if (descricaoColaborador.toLowerCase().indexOf('slot') > -1) {
                return true;
            }


        }

    }

    return false;
}

function ResetarCampos() {

    $("#unidadeOrigem").val('');
    $("#equipeOrigem").val('');
    $("#tipoEquipeOrigem").val('');
    $("#horarioOrigem").val('');

    $("#unidadeDestino").val('');
    $("#equipeDestino").val('');
    $("#tipoEsquipeDestino").val('');
    $("#horarioDestino").val('');

    $("#DataFinal").val('');

    $("#equipeOrigem").html('<option value="">Selecione a Equipe</option>');
    $("#tipoEquipeOrigem").html('<option value="">Selecione o Tipo Equipe</option>');
    $("#horarioOrigem").html('<option value="">Selecione o Horário</option>');
 
    $("#equipeDestino").html('<option value="">Selecione a Equipe</option>');
    $("#tipoEsquipeDestino").html('<option value="">Selecione o Tipo Equipe</option>');
    $("#horarioDestino").html('<option value="">Selecione o Horário</option>');

    $("div#divlistacolaboradoresorigem").hide();
    $("div#divlistacolaboradoresdestino").hide();

    $('#salvar').attr('disabled', 'disabled');

    FormatarApresentacaoCampos();

    SetarOperacaoTransferencia();

    document.getElementById('Fixo').checked = true;
}


function ValidaCampos() {



    if ($("#tipo-operacao").val() === "") {
        toastr.error("Selecione o \"Tipo Operação\"");
        return false;
    }
    if ($('#Fixo:checkbox:checked').length === 0 && $("#DataFinal").val() === "") {
            toastr.error("O campo \"DataFinal\" é obrigatório");
            return false;
    }
    if ($("#equipeOrigem").val() === "0" || $("#equipeDestino").val() === "0") {
        toastr.error("Selecione as \"Equipes de Origem e Destino\"");
        return false;
    }

    

    if ($("#equipeOrigem").val() === $("#equipeDestino").val()) {

        toastr.error("As \"Equipes de Origem e Destino tem quer ser diferentes\"");
        return false;
    }

    if ($("#tipoEquipeOrigem").val() === "0" || $("#tipoEsquipeDestino").val() === "0") {
        toastr.error("Selecione os \"Tipos Equipes de Origem e Destino\"");
        return false;
    }


    
    

   if (verificarCheckBoxesMarcados("colaboradorOrigemChk") === false && operacaoTransferencia === false ) {

        toastr.error("Selecione ao menos um \"Colaborador da origem\"");
        return false;
    }


    if (verificarCheckBoxesMarcados("colaboradorDestinoChk") === false && operacaoTransferencia === false) {

        toastr.error("Selecione ao menos um \"Colaborador ou Slot de destino\"");
        return false;
    }


    if (ExisteSlot('tbColaboradorOrigem', 'colaboradorOrigemChk') && operacaoTransferencia === false) {
        toastr.error("Selecione apenas \"colaboradores na origem\"");
        return false;
    }


    //Troca ----
    if ($("#tipo-operacao").val() === "1" && operacaoTransferencia=== false) {

      
        if (ExisteSlot('tbColaboradorDestino','colaboradorDestinoChk')) {
            toastr.error("Selecione apenas \"colaboradores no destino\"");
            return false;
        }
        
    }
    
    //Alocação
    if ($("#tipo-operacao").val() === "2" && operacaoTransferencia=== false) {

        if (!ExisteSlot('tbColaboradorDestino', 'colaboradorDestinoChk')) {
            toastr.error("Selecione apenas \"slot no destino\"");
            return false;
        }

    }


    return true;
}

function SalvarTipoEquipeHorario() {

    var idTipoEquipeOrigem = $("#tipoEquipeOrigem").val();
    var idTipoEquipeDestino = $("#tipoEsquipeDestino").val();
    var idHOrarioOrigem = $("#horarioOrigem").val();
    var idHOrarioDestino = $("#horarioDestino").val(); 

    $.ajax({
        url: "/Remanejamento/SalvarTipoEquipeHorario",
        type: "POST",
        dataType: "json",
        data: {
            idTipoEquipeOrigem: idTipoEquipeOrigem,
            idTipoEquipeDestino: idTipoEquipeDestino,
            idHOrarioOrigem: idHOrarioOrigem,
            idHOrarioDestino: idHOrarioDestino
        },
        success: function (response) {
            
        },
        error: function (error) {
            window.alert('Error');
        }
    });
}