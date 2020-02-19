$(document).ready(function () {
    ConfigTabelaGridSemCampoFiltroPrincipal("#lista-material-historico #datatable_fixed_column");
    ConfigTabelaGridSemCampoFiltroPrincipal("#lista-estoque-materiais #datatable_fixed_column");
    ConfigTabelaGridSemCampoFiltroPrincipal("#lista-materiais #datatable_fixed_column");
    FormatarNumerosInteiros(".numero-inteiro");
    FormatarNumerosDecimais(".numero-dimensao");

    $("#material-form").submit(function (e) {
        var imagem = document.getElementById('imgImagemMaterial');

        if (imagem.src.indexOf("data:") >= 0) {
            var hdnImagemMaterial = document.getElementById('hdnImagemMaterial');
            hdnImagemMaterial.value = imagem.src;
        }

        if (!ValidarCampos())
            e.preventDefault();
    });

    MudaImagem();
    BuscarMateriais()
        .done(() => {
            if (isEdit()) {
                let materialId = location.pathname.split("/").pop();
                BuscarEstoqueMateriaisPeloMaterialId(materialId);
            }
        });
});

function AbrirModalMaterialHistorico() {
    if (isEdit()) {
        let materialId = location.pathname.split("/").pop();
        BuscarMaterialHistoricoPeloMaterialId(materialId)
            .done(() => $("#modal-material-historico").modal("show"));
    }
    else {
        $("#modal-material-historico").modal("show");
    }

}

function BuscarMaterialHistoricoPeloMaterialId(materialId) {
    return post("BuscarMaterialHistoricoPeloMaterialId", { materialId })
        .done((result) => {
            $("#lista-material-historico").empty().append(result);
            ConfigTabelaGridSemCampoFiltroPrincipal("#lista-material-historico #datatable_fixed_column");
        });
}

function BuscarEstoqueMateriaisPeloMaterialId(materialId) {
    return post("BuscarEstoqueMateriaisPeloMaterialId", { materialId })
        .done((result) => {
            $("#lista-estoque-materiais").empty().append(result);
            ConfigTabelaGridSemCampoFiltroPrincipal("#lista-estoque-materiais #datatable_fixed_column");
        });
}

function BuscarMateriais() {
    return post("BuscarMateriais")
        .done((result) => {
            $("#lista-materiais").empty().append(result);
            ConfigTabelaGridSemCampoFiltroPrincipal("#lista-materiais #datatable_fixed_column");
        });
}


function ValidarCampos() {
    let dadosValidos = true;
    let mensagem = "";

    if (!$("#nome").val()) {
        mensagem += "O campo Nome deve ser preenchido!<br/><br/>";
        dadosValidos = false;
    }

    if (!$("#tipo-material").val()) {
        mensagem += "O campo Tipo deve ser preenchido!<br/><br/>";
        dadosValidos = false;
    }

    if (!$("#estoque-maximo").val()) {
        mensagem += "O campo Estoque Máximo deve ser preenchido!<br/><br/>";
        dadosValidos = false;
    }

    if (!$("#estoque-minimo").val()) {
        mensagem += "O campo Estoque Mínimo deve ser preenchido!<br/><br/>";
        dadosValidos = false;
    }

    if (!$(".item-fornecedor").length) {
        mensagem += "Adicione pelo menos 1 fornecedor!<br/><br/>";
        dadosValidos = false;
    }

    if (!dadosValidos) {
        toastr.error(mensagem, "Validação de Campos");
    }

    return dadosValidos;
}

function CarregaImagem() {
    var filesSelected = document.getElementById("fupload").files;
    if (filesSelected.length > 0) {
        var fileToLoad = filesSelected[0];

        var fileReader = new FileReader();

        fileReader.onload = function (fileLoadedEvent) {
            var imagem = document.getElementById
                (
                "imgImagemMaterial"
                );

            imagem.src = fileLoadedEvent.target.result;
        };

        fileReader.readAsDataURL(fileToLoad);
    }
}

function MudaImagem() {
    var input = document.getElementById('hdnImagemMaterial');
    if (input.value !== '') {
        var imagem = document.getElementById('imgImagemMaterial');
        imagem.src = input.value;
    }
}