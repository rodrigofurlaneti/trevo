let oisCategorias = [];

let TipoOISCategoria = {
    "Roubo": 1,
    "Furto": 2,
    "QuebraDePeca": 3,
    "Colisao": 4,
    "Incidente": 5,
    "Outros": 6
}

$(document).ready(function () {
    MakeChosen("categoria", null, "100%");

    if (isEdit() || isSave()) {
        post("BuscarOisCategorias")
            .done((result) => {
                oisCategorias = result;
            })
    }

    $("#categoria").change(function () {
        $("#outra-categoria").val("");
        if (TipoOISCategoria[$(this).find(":selected").text()] == TipoOISCategoria["Outros"])
            $("#container-outra-categoria").show();
        else
            $("#container-outra-categoria").hide();
    });
});

function atualizarOisCategoria(oisCategorias) {
    return post("AtualizarOisCategorias", { oisCategorias })
        .done((response) => {
            $("#lista-ois-categorias").empty().append(response);
        });
}

function validarOisCategoria({ TipoCategoria, Descricao, OutraCategoria }) {
    if (!TipoCategoria) {
        toastr.warning("Selecione um Categoria", "Alerta");
        return false;
    }
    if (oisCategorias.find(x => x.Descricao == Descricao) != null) {
        toastr.warning("Esta categoria já foi adicionada", "Alerta");
        return false;
    }
    if (TipoCategoria == TipoOISCategoria["Outros"] && !OutraCategoria) {
        toastr.warning("Informe a descrição da outra categoria", "Alerta");
        return false;
    }
    return true;
}

function adicionarOisCategoria() {
    let descricao = $("#outra-categoria").val() ? $("#outra-categoria").val() : $("#categoria option:selected").text();

    let oisCategoria = {
        TipoCategoria: $("#categoria").val(),
        Descricao: descricao,
        OutraCategoria: $("#outra-categoria").val()
    }

    if (!validarOisCategoria(oisCategoria))
        return;

    oisCategorias.push(oisCategoria);

    atualizarOisCategoria(oisCategorias);

    clearFieldsCategoria();
}

function removerOisCategoria(descricao) {
    console.log(descricao);
    console.table(oisCategorias);
    oisCategorias = oisCategorias.filter(x => x.Descricao != descricao);

    return atualizarOisCategoria(oisCategorias);
}

function clearFieldsCategoria() {
    $("#outra-categoria").val("");
    $("#container-outra-categoria").hide();
    $('#categoria').val("");
    MakeChosen("categoria", null, "100%");
}