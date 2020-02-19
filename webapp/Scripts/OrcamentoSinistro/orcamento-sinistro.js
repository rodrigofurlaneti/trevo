$(document).ready(function () {
    MakeChosen("ois", null, "100%");

    $("#ois").change(function () {
        $("#unidade, #cliente, #modelo, #placa").val("");

        BuscarOisSelecionado(this.value);
    });

    BuscarOrcamentoSinistro()
        .done(() => {
            if (isEdit())
                BuscarOisSelecionado($("#ois").val())
            else if (location.pathname.includes("OrcamentoSinistro/Cotacao"))
                AbrirModalCotacao(location.pathname.split("/").pop());
        });

    $("#orcamento-sinistro-form").submit(function () {
        let valido = true;

        if (!$("#ois").val()) {
            toastr.warning("Selecione um Id da Abertura OIS");
            valido = false;
        }

        else if (!$(".peca-servico-nome").length) {
            toastr.warning("Informe pelo menos uma Peça a ser comprada/Serviço");
            valido = false;
        }

        if (!valido)
            event.preventDefault();
    });
});

function BuscarOrcamentoSinistro() {
    return post("BuscarOrcamentoSinistro")
        .done((response) => {
            $("#lista-orcamento-sinistro").empty().append(response);
            ConfigTabelaGridSemCampoFiltroPrincipal();
        })
}

function BuscarOisSelecionado(oisId) {
    if (oisId) {
        return post("BuscarOisSelecionado", { oisId })
            .done((response) => {
                $("#unidade").val(response.NomeUnidade);
                $("#cliente").val(response.NomeCliente);
                $("#modelo").val(response.DescricaoModelo);
                $("#placa").val(response.Placa);
            });
    }

    return null;
}

function AbrirModalCotacao(orcamentoSinistroId) {
    modalOrcamentoSinistroId = orcamentoSinistroId;
    $("#modal-cotacao").modal("show");
}

function Cancelar(id) {
    let acaoConfirmar = function () {
        return post("Cancelar", { id });
    }

    abrirModalPersonalizado("Cancelar", "Deseja cancelar o orçamento e as Contas geradas?", "danger", acaoConfirmar, "Sim", BuscarOrcamentoSinistro);
}