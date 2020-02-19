var listaequipamentos = [];

$(document).ready(function () {


  



    $("#unidade").change(function () {

        var unidade = document.getElementById("unidade");

        if (unidade.selectedIndex <= 0) {
            $("#lista-funcionamentos-result").empty();
            return ;
        }

        var Id = $("#unidade").val();

        $.ajax({
            url: "/equipamentounidade/atualizaequipamentos",
            type: "POST",
            dataType: "json",
            data: {
                Id: Id
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

                    $("#lista-funcionamentos-result").empty();
                    $("#lista-funcionamentos-result").append(result);
                }
            },
            error: function (error) {

                $("#lista-funcionamentos-result").empty();
                $("#lista-funcionamentos-result").append(error.responseText);
                //hideLoading();
            },
            beforeSend: function () {
                //showLoading();
            },
            complete: function () {

                //MetodoUtil();
                //hideLoading();
            }
        });
    })
    

    $(window).load(function () {

        //$("#conferencia").hide();
        //$("#unidadeblockfiltro").hide();

        //var Id = $("#hdnEquipamentoUnidade").val();

        //if (Id != null && Id != 0) {

        //    $("#conferencia").show();

        //}
        //else { $("#unidadeblock").hide();}

        //var perfil = $("#perfilatual").val();

        //if (perfil != 'Root') {

        //    $("#unidadeblockfiltro").show();
        //    $("#unidadeblocklabel").text('Unidade:')
        //    $("#unidadeblock").text($("#unidade option:selected").text())

        //    $("#adiciona-equipamento").hide();
        //    $("#gerarnotificacaofiltro").hide();
        //    $("#periodoequipamentounidadefiltro").hide();
        //    $("#unidadefiltro").hide();
        //    $("span[name=deleta-equipamento]").hide();

        //    $('a[name=botaoexcluir]').hide();


        //}
    });
});

function DeletaEquipamento(x) {


    var listahorarioparametro = [];

    var index = $(x).closest('tr').index();

    $("#teste tbody").find('tr').each(function (rowIndex, r) {

        var periodohorario = {
            Id: $(this).find('input').attr("id"),
            EstruturaGaragem: $(this).find('input[name="equipamento"]').val(),
            Quantidade: $(this).find('input[name="quantidade"]').val(),
            Ativo: $(this).find('input[name="ativo"]:checked').length == 1 ? 'true' : 'false'
        }

        listahorarioparametro.push(periodohorario);

    });


    $.ajax({
        url: "/equipamentounidade/deletaequipamento",
        type: "POST",
        dataType: "json",
        data: {
            index, listahorarioparametro
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

                $("#lista-funcionamentos-result").empty();
                $("#lista-funcionamentos-result").append(result);
            }
        },
        error: function (error) {

            $("#lista-funcionamentos-result").empty();
            $("#lista-funcionamentos-result").append(error.responseText);
            //hideLoading();
        },
        beforeSend: function () {
            //showLoading();
        },
        complete: function () {
            AjustaDropdowDinamicos();
        }
    });

}

//function DeletaEquipamento(idequipamento) {

//    var IdUnidade = $("#unidade").val();

//    $.ajax({
//        url: "/equipamentounidade/deletaequipamento",
//        type: "POST",
//        dataType: "json",
//        data: {
//            IdEquipamento: idequipamento, IdUnidade: IdUnidade
//        },
//        success: function (result) {
//            if (typeof (result) === "object") {

//                openCustomModal(null,
//                    null,
//                    result.TipoModal,
//                    result.Titulo,
//                    result.Mensagem,
//                    false,
//                    null,
//                    function () { });
//            }
//            else {

//                $("#lista-funcionamentos-result").empty();
//                $("#lista-funcionamentos-result").append(result);
//            }
//        },
//        error: function (error) {

//            $("#lista-funcionamentos-result").empty();
//            $("#lista-funcionamentos-result").append(error.responseText);
//            //hideLoading();
//        },
//        beforeSend: function () {
//            //showLoading();
//        },
//        complete: function () {

//            //MetodoUtil();
//            //hideLoading();
//        }
//    });

//}

function Imprimir() {

    $("span[name=deleta-equipamento]").hide();
    var conteudo = document.getElementById('container-tables').innerHTML;
        tela_impressao = window.open('about:blank');

    var perfil = $("#perfilatual").val();

    if (perfil == 'Root')
        $("span[name=deleta-equipamento]").show();

    tela_impressao.document.write(conteudo);
    tela_impressao.window.print();
    tela_impressao.window.close();

    
}

function validarItens(unidade) {
    if (unidade.selectedIndex <= 0) {
        toastr.error('Selecione uma Unidade!', 'Informe a Unidade!');
        return false;
    }

    return true;
}

function SalvarDados() {

    
    var Id = $("#hdnEquipamentoUnidade").val();
    var unidadeId = $("#unidade").val();
    var GerarNotificacao = $("#gerarnotificacao").val();
    var PeriodoEquipamentoUnidade = $("#periodoequipamentounidade").val();
    var Observacao = $("#observacao").val();
    var Usuario = $("#Usuario").val();

    var unidade = document.getElementById("unidade");

    if (!validarItens(unidade))
        return;

    var model = {
        Id: Id,
        GerarNotificacao: GerarNotificacao,
        Observacao: Observacao,
        Unidade:  {
            Id: unidadeId
        },
        PeriodoEquipamentoUnidade: PeriodoEquipamentoUnidade,
        Usuario: Usuario
    }

    listaequipamentos.length = 0;

    $("#teste tbody").find('tr').each(function (rowIndex, r) {

        var periodohorario = {
            Id: $(this).find('input').attr("id"),
            EstruturaGaragem: $(this).find('input[name="equipamento"]').val(),
            Quantidade: $(this).find('input[name="quantidade"]').val(),
            Ativo: $(this).find('input[name="ativo"]:checked').length == 1 ? 'true' : 'false'
        }

        listaequipamentos.push(periodohorario);

    });
    
    $.ajax({
        url: "/equipamentounidade/SalvarDados",
        type: "POST",
        data: { model, listaequipamentos },
        success: function (result) {
            hideLoading();
            openCustomModal(null, null, result.TipoModal, result.Titulo, result.Mensagem, false, null, function () { });

            if (result.Sucesso)
                window.location.href = "/equipamentounidade/Index/";

        },
        beforeSend: function () {
            showLoading();
        }
    });
}

function Conferencia() {

    var Id = $("#hdnEquipamentoUnidade").val();
    var unidadeId = $("#unidade").val();
    var GerarNotificacao = $("#gerarnotificacao").val();
    var PeriodoEquipamentoUnidade = $("#periodoequipamentounidade").val();
    var Observacao = $("#observacao").val();

    var unidade = document.getElementById("unidade");

    if (!validarItens(unidade))
        return;

    var model = {
        Id: Id,
        GerarNotificacao: GerarNotificacao,
        Observacao: Observacao,
        Unidade: {
            Id: unidadeId
        },
        PeriodoEquipamentoUnidade: PeriodoEquipamentoUnidade
    }

    listaequipamentos.length = 0;

    $("#teste tbody").find('tr').each(function (rowIndex, r) {

        var periodohorario = {
            Id: $(this).find('input').attr("id"),
            EstruturaGaragem: $(this).find('input[name="equipamento"]').val(),
            Quantidade: $(this).find('input[name="quantidade"]').val(),
            Ativo: $(this).find('input[name="ativo"]:checked').length == 1 ? 'true' : 'false'
        }

        listaequipamentos.push(periodohorario);

    });

    $.ajax({
        url: "/equipamentounidade/conferencia",
        type: "POST",
        data: { model, listaequipamentos },
        success: function (result) {
            hideLoading();
            openCustomModal(null, null, result.TipoModal, result.Titulo, result.Mensagem, false, null, function () { });

            if (result.Sucesso)
                window.location.href = "/equipamentounidade/index";

        },
        beforeSend: function () {
            showLoading();
        }
    });
}

$('.quant').keyup(function (e) {
    if (/\D/g.test(this.value)) {
        // Filter non-digits from input value.
        this.value = this.value.replace(/\D/g, '');
    }
});
function AdicionaEquipamento() {

    var unidade = document.getElementById("unidade");

    if (!validarItens(unidade))
        return;

    var escopo = $('#teste tbody');  //('.dynamicInputs');
        //var escopo = $('.dynamicInputs');
        //var escopo2 = escopo.parent();

    //var valorPrimeiraLinha = $('#teste tbody').find("td:first").html();

    //if (valorPrimeiraLinha == 'Não Há Equipamentos') {
    //    $('#teste tbody').find("tr:first").text('');
    //    $('#teste tbody').find("tr:first").remove();
    //}

        //$(this).parent().parent().parent().parent().find('tr:last').remove();
        escopo.append(`
           <tr>
               <td><input name="equipamento"  type="text" id="0" class="horario " value=""  placeholder="equipamento" /></td>
               <td><input name="quantidade"  type="number" min="1" id="0" class="horario quant" value=""  placeholder="quantidade" /></td>
               <td><span class="btn btn-danger btn-circle"  onclick="DeletaEquipamento(this)"  data-idequipamento="0" id="deleta-equipamento" ><i class="fa" style="color: white">X</i>&nbsp;</span></td>
           </tr>
        `);


}


