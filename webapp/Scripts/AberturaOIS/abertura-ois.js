$(document).ready(function () {
    FormatarTelefoneFixo("#telefone-fixo");
    FormatarCelular("#celular");
    FormatarPlaca();
    MakeChosen("marca", null, "100%");
    MakeChosen("modelo", null, "100%");
    MakeChosen("tipo", null, "100%");
    MakeChosen("unidade", null, "100%");
    MakeChosen("status", null, "100%");

    BuscarOis()
        .done(() => {
            if (isEdit() || isSave()) {
                AtualizarModelos($("#marca").val())
            }
        });

    $("#marca").on("change", function () {
        AtualizarModelos(this.value);
    });

    $("#abertura-ois-form").submit(() => {
        return ValidarCampos();
    });
});

function ValidarCampos() {
    if (!$("#nome-cliente").val()) {
        toastr.warning("Informe o nome do cliente", "Campo obrigatório!");
        return false;
    }
    if (!$("#marca").val()) {
        toastr.warning("Informe a Marca", "Campo obrigatório!");
        return false;
    }
    if (!$("#modelo").val()) {
        toastr.warning("Informe o Modelo", "Campo obrigatório!");
        return false;
    }
    if (!$("#tipo").val()) {
        toastr.warning("Informe o Tipo", "Campo obrigatório!");
        return false;
    }
    if (!$("#placa").val()) {
        toastr.warning("Informe a Placa", "Campo obrigatório!");
        return false;
    }
    if (!$("#cor").val()) {
        toastr.warning("Informe a Cor", "Campo obrigatório!");
        return false;
    }
    if (!$("#ano").val()) {
        toastr.warning("Informe o Ano", "Campo obrigatório!");
        return false;
    }
    if (!$("#unidade").val()) {
        toastr.warning("Informe a Unidade", "Campo obrigatório!");
        return false;
    }
    if (!$("#lista-ois-categorias tbody tr").length) {
        toastr.warning("Adicione pelo menos 1 categoria", "Campo obrigatório!");
        return false;
    }
    if (!$("#lista-ois-funcionarios tbody tr").length) {
        toastr.warning("Adicione pelo menos 1 funcionario", "Campo obrigatório!");
        return false;
    }
    if (!$("#observacao").val()) {
        toastr.warning("Informe a Observação", "Campo obrigatório!");
        return false;
    }

    return true;
}

function BuscarOis() {
    return post("BuscarOis")
        .done((response) => {
            $("#lista-ois").empty().append(response);
            ConfigTabelaGridSemCampoFiltroPrincipal();
        })
}

function AtualizarModelos() {
    let marcaSelect = document.getElementById("marca");
    let listaMarca = [];
    let marca = {
        Id: marcaSelect.options[marcaSelect.selectedIndex].value,
        Nome: marcaSelect.options[marcaSelect.selectedIndex].text
    };
    listaMarca.push(marca);

    let modeloSelect = document.getElementById("modelo");
    modeloSelect.innerHTML = "";
    let option = document.createElement("option");
    option.text = "Selecione o Modelo";
    option.value = 0;
    modeloSelect.options.add(option);

    if (marca.Id) {
        return post('ListaModelo', { marca: JSON.stringify(listaMarca) }, "Veiculo")
            .done((response) => {
                $.each(response, function (i, item) {
                    option = document.createElement("option");
                    option.text = item.Descricao;
                    option.value = item.Id;
                    modeloSelect.options.add(option);
                });

                $("#modelo").val($("#hdn-modelo").val());
                MakeChosen("modelo", null, "100%");
            });
    }

    MakeChosen("modelo", null, "100%");
}

function CarregarImagem(indice) {
    var filesSelected = document.getElementById("fupload"+indice).files;
    if (filesSelected.length > 0) {
        var fileToLoad = filesSelected[0];

        var fileReader = new FileReader();

        fileReader.onload = function (fileLoadedEvent) {
            var imagem = document.getElementById("imagem" + indice);

            imagem.src = fileLoadedEvent.target.result;

            if (imagem.src.indexOf("data:") >= 0) {
                var hdnImagem = document.getElementById('hdnImagem' + indice);
                hdnImagem.value = imagem.src;
            }
        };
        
        fileReader.readAsDataURL(fileToLoad);
    }
}

function RemoverImagem(indice) {
    var imagem = document.getElementById("imagem" + indice);
    imagem.src = "../../content/img/avatars/sunny-big.png";

    var hdnImagem = document.getElementById('hdnImagem' + indice);
    hdnImagem.value = "";
}