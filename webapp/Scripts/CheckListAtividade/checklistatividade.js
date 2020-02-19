var atividades = [];
var atividadeEmEdicao = {};
var indiceAtividadesAdicionados = 0;

$(document).ready(function () {
    MakeChosen("atividade");

    if (isEdit()) {
        let id = location.pathname.split("/").pop();
        post(`BuscarDadosDosGrids/${id}`)
            .done((response) => {
                atividades = response.Atividades;
                atualizaAtividades();
            });
    }

    $.ajax({
        url: '/checklistatividade/ListaTipoAtividade',
        type: 'POST',
        dataType: 'json',
        success: function (response) {
            CarregaAtividade(response)
        }
    });
});

function CarregaAtividade(atividades) {
    var atividadeSelect = document.getElementById("atividade");

    $.each(atividades, function (i, item) {
        var option = document.createElement("option");
        option.text = item.Descricao;
        option.value = item.Id;
        atividadeSelect.options.add(option);
    });
    MakeChosen("atividade");
}

function AdicionaAtividade() {
    indiceAtividadesAdicionados--;
    var atividade = document.getElementById("atividade");

    if (!validarAtividade(atividade))
        return;

    var atividade = {
        Id: atividade.value,
        Descricao: atividade.options[atividade.selectedIndex].text
    }

    atividades.push(atividade);

    atualizaAtividades();

    atividade.selectedIndex = "0";

    MakeChosen("atividade");

    atividadeEmEdicao = {};
}

function ListaAtividades() {
    var element = document.getElementById('hdnCheckAtividade');
    if (typeof (element) != 'undefined' && element != null) {
        if (element.value != "") {
            var obj = [];
            var newObj = {
                Id: element.value
            };
            obj.push(newObj);
            $.ajax({
                url: '/checklistatividade/ListaAtividades',
                type: 'POST',
                dataType: 'json',
                data: { CheckListAtividade: JSON.stringify(obj) },
                success: function (response) {
                    if (response && response.length) {
                        atividades = response;
                        CarregaTipoAtividade()
                    }
                }
            })
        };

    }
}

function CarregaTipoAtividade() {
    $('#gridAtividades tbody').empty();
    $.each(atividades, function (i, atividade) {
        $('#gridAtividades tbody')
            .append('<tr>'
            + '<td style="display:none">'
            + atividade.Id
            + '</td>'
            + '<td class="col-xs-10">'
            + atividade.Descricao
            + '</td>'
            + '<td class="col-xs-2">'
            + '<span class="btn btn-danger" onclick="RemoveAtividade(' + atividade.Id + ')"><i class="fa fa-remove"></i></span>'
            + '</td>'
            + '</tr>'
            );
    });
}

function EditaAtividade(id) {
    if (Object.keys(atividadeEmEdicao).length)
        atividades.push(atividadeEmEdicao);

    atividadeEmEdicao = atividades.find(x => x.Id == id);

    atividades = atividades.filter(x => x.Id != atividadeEmEdicao.Id);

    console.table(atividadeEmEdicao);

    $("#atividade").val(atividadeEmEdicao.Id);
    MakeChosen("atividade");

    atualizaAtividades();
}

function RemoveAtividade(id) {
    atividades = atividades.filter(x => x.Id != id);

    atualizaAtividades();
}

function atualizaAtividades() {
    showLoading();
    $.post('/checklistatividade/AtualizaTipoAtividades', { jsonTipoAtividades: JSON.stringify(atividades) })
        .done(() => {
            CarregaTipoAtividade();
        })
        .always(() => { hideLoading(); });
}

function validarAtividade(atividade) {
    if (atividade.selectedIndex <= 0) {
        toastr.error('Selecione um item!', 'Informe o Tipo Atividade!');
        return false;
    }

    return true;
}


