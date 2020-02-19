function Salvar(dataFields, acaoSalvar, urlController, titulo, htmlMensagem) {
    var atualizacao = function (response) {
        if (response.sucesso) {
            acaoSalvar(response);
            showPopUpMessage(htmlMensagem, titulo);
        }
        else {
            showPopUpMessage("Ocorreu um erro ao Salvar: " + response.mensagem, titulo);
        }
    };

    doRequest(urlController, dataFields, atualizacao, "POST");
}
function Excluir(id, urlController, titulo, htmlMensagem) {
    var data = {
        id: id
    }
    var atualizacao = function (response) {
        if (response.sucesso) {
            showPopUpMessage(htmlMensagem, titulo);
        }
        else {
            showPopUpMessage("Ocorreu um erro ao Excluir: " + response.mensagem, titulo);
        }
    };

    doRequest(urlController, data, atualizacao, "POST");
}
function Editar(id, acaoSucesso, urlController, titulo) {
    var data = {
        id: id
    }
    var acao = function (response) {
        if (response.sucesso) {
            acaoSucesso(response);
        }
        else {
            showPopUpMessage("Ocorreu um erro ao Excluir: " + response.mensagem, titulo);
        }
    };

    doRequest(urlController, data, acao, "POST");
}