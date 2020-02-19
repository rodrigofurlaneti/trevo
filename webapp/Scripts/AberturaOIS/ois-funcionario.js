let oisFuncionarios = [];

$(document).ready(function () {
    MakeChosen("funcionario", null, "100%");

    if (isEdit() || isSave()) {
        post("BuscarOisFuncionarios")
            .done((result) => {
                oisFuncionarios = result;
            })
    }
});

function atualizarOisFuncionario(oisFuncionarios) {
    return post("AtualizarOisFuncionarios", { oisFuncionarios })
        .done((response) => {
            $("#lista-ois-funcionarios").empty().append(response);
        });
}

function validarOisFuncionario({ Funcionario }) {
    if (!Funcionario.Id) {
        toastr.warning("Selecione um Funcionário", "Alerta");
        return false;
    }
    if (oisFuncionarios.find(x => x.Funcionario.Id == Funcionario.Id) != null) {
        toastr.warning("Este Funcionário já foi adicionado", "Alerta");
        return false;
    }
    return true;
}

function adicionarOisFuncionario() {
    let oisFuncionario = {
        Funcionario: {
            Id: $("#funcionario").val(),
            Pessoa: {
                Nome: $("#funcionario option:selected").text()
            }
        },
    }

    if (!validarOisFuncionario(oisFuncionario))
        return;

    oisFuncionarios.push(oisFuncionario);

    atualizarOisFuncionario(oisFuncionarios);

    clearFieldsFuncionario();
}

function removerOisFuncionario(id) {
    oisFuncionarios = oisFuncionarios.filter(x => x.Funcionario.Id != id);

    return atualizarOisFuncionario(oisFuncionarios);
}

function clearFieldsFuncionario() {
    $('#funcionario').val("");
    MakeChosen("funcionario", null, "100%");
}