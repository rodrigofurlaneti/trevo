let controller = 'TabelaPrecoAvulso';
let periodo;
let listaUnidade = new Array();
let listaHoraValor = new Array();
let listaIdUnidade = new Array();

$(document).ready(function () {

    formatarCampos();

    eventos();

    let id = $('#Id').val();
    carregarTela(id);
});

function carregarTela(id) {

    if ($('#horaAdicional').is(':checked')) {
        $('#quantidadeHora').removeAttr('readonly');
        $('#valorPorQuantidade').removeAttr('readonly');
    }

    if (!id)
        id = 0;

    carregarPeriodo(id);
    carregarGridHoraValor(id);
    carregarGridUnidadeVigencia(id);
    carregarGrid();
}

function formatarCampos() {

    MakeChosen('unidade', null, '100%');

    FormatarCampoHora('.time');

    $('input[class*=valmoney]').maskMoney({
        prefix: '',
        allowNegative: false,
        allowZero: true,
        thousands: '.',
        decimal: ',',
        affixesStay: false
    });
}

function carregarPeriodo(id) {

    let filtro = {
        id: id
    };

    BuscarPartial(`/${controller}/CarregarPeriodo`, '#opcoesPeriodo', filtro)
        .done(function () {
            if (id !== 0) {
                controlaPeriodo();
            }
        });
}

function carregarGridHoraValor(id) {

    let filtro = {
        id: id
    };

    BuscarPartial(`/${controller}/CarregarHoraValor`, '#divHoraValor', filtro)
        .done(function () {
            if (id !== 0) {
                carregarArrayHoraValor();
            }
        });
}

function carregarGridUnidadeVigencia(id) {

    let filtro = {
        id: id
    };

    BuscarPartial(`/${controller}/CarregarUnidadeVigencia`, '#divUnidade', filtro)
        .done(function () {
            if (id !== 0) {
                carregarArrayUnidade();
            }
        });
}

function carregarGrid() {
    return BuscarPartialSemFiltro(`/${controller}/CarregarGrid`, '#grid')
        .done(function () {
            MetodoUtil();
        });
}

function montaObjeto() {
    let obj = {
        id: $('#Id').val(),
        nomeTabela: $('#nomeTabela').val(),
        numero: $('#numero').val(),
        tempoToleranciaPagamento: $('#tempoToleranciaPagamento').val(),
        tempoToleranciaDesistencia: $('#tempoToleranciaDesistencia').val(),
        horaInicioVigencia: $('#horaInicioVigencia').val(),
        horaFimVigencia: $('#horaFimVigencia').val(),
        horaAdicional: $('#horaAdicional').is(':checked'),
        padrao: $('#padrao').is(':checked'),
        quantidadeHoraAdicional: $('#quantidadeHora').val(),
        valorHoraAdicionalString: $('#valorPorQuantidade').val(),
        descricaoHoraValor: $('#campoParaAdicionarNomeLista').val(),
        periodo: periodo,
        listaUnidade: listaUnidade,
        listaHoraValor: listaHoraValor,
        status: $('#Status').val()
    };

    return obj;
}

function salvar() {

    let obj = montaObjeto();
    if (!validarCampoObrigatorios(obj)) {
        e.preventDefault();
        return false;
    }

    $.ajax({
        url: `/${controller}/SalvarDados`,
        type: 'POST',
        data: { model: obj },
        success: function (result) {
            let callback = result.Redirect ? () => window.location.href = '/TabelaPrecoAvulso/Index/' : null;
            openCustomModal(null, null, result.TipoModal, result.Titulo, result.Mensagem, false, null, callback);
        },
        beforeSend: function () {
            showLoading();
        },
        error: function (err) {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });

}

function validarCampoObrigatorios(obj) {

    let valido = true;

    if (!gridPossuiRegistro('#gridUnidade')) {
        toastr.error('A lista de unidade deve ser preenchida!');
        valido = false;
    }

    if (!gridPossuiRegistro('#gridHoraValor')) {
        toastr.error('A lista de hora x valor deve ser preenchida!');
        valido = false;
    }

    if (obj.horaAdicional) {
        if (obj.horaAdicional && !obj.valorHoraAdicionalString) {
            toastr.error('O campo \"Valor por Qtd\" deve ser preenchido!');
            valido = false;
        }

        if (obj.horaAdicional && !obj.quantidadeHoraAdicional) {
            toastr.error('O campo \"Qtd Hora(s)\" deve ser preenchido!');
            valido = false;
        } else if (obj.quantidadeHoraAdicional === '0' || parseInt(obj.quantidadeHoraAdicional) > 24) {
            toastr.error('O campo \"Qtd Hora(s)\" deve ser preenchido com valores entre 1 e 24!');
            valido = false;
        }
    }

    if (!obj.horaFimVigencia) {
        toastr.error('O campo \"Hora Fim Vigência\" deve ser preenchido!');
        valido = false;
    } else if (obj.horaFimVigencia === '0' || parseInt(obj.horaFimVigencia) > 24) {
        toastr.error('O campo \"Hora Fim Vigência\" deve ser preenchido com valores entre 1 e 24!');
        valido = false;
    }

    if (!obj.horaInicioVigencia) {
        toastr.error('O campo \"Hora Início Vigência\" deve ser preenchido!');
        valido = false;
    } else if (obj.horaInicioVigencia === '0' || parseInt(obj.horaInicioVigencia) > 24) {
        toastr.error('O campo \"Hora Início Vigência\" deve ser preenchido com valores entre 1 e 24!');
        valido = false;
    }

    if (!temPeriodoSelecionado()) {
        toastr.error('Selecione ao menos um período!');
        valido = false;
    }

    if (!obj.tempoToleranciaDesistencia) {
        toastr.error('O campo \"Tempo Tolerância Desistência\" deve ser preenchido!');
        valido = false;
    } else if (obj.tempoToleranciaDesistencia === '0' || parseInt(obj.tempoToleranciaDesistencia) > 60) {
        toastr.error('O campo \"Tempo Tolerância Desistência\" deve ser preenchido com valores entre 1 e 60!');
        valido = false;
    }

    if (!obj.tempoToleranciaPagamento) {
        toastr.error('O campo \"Tempo Tolerância Pagamento\" deve ser preenchido!');
        valido = false;
    } else if (obj.tempoToleranciaPagamento === '0' || parseInt(obj.tempoToleranciaPagamento) > 60) {
        toastr.error('O campo \"Tempo Tolerância Pagamento\" deve ser preenchido com valores entre 1 e 60!');
        valido = false;
    }

    if (!obj.nomeTabela) {
        toastr.error('O campo \"Nome da Tabela\" deve ser preenchido!');
        valido = false;
    }

    if (!obj.numero) {
        toastr.error('O campo \"Numero da Tabela\" deve ser preenchido!');
        valido = false;
    }

    return valido;
}

function temPeriodoSelecionado() {

    let periodos = $('.periodo');
    for (let index = 0; index < periodos.length; index++)
        if ($('#' + periodos[index].id).is(':checked'))
            return true;

    return false;
}

function limpaCampos() {

    $('#Id').val(0);
    $('#nomeTabela').val('');
    $('#tempoToleranciaPagamento').val('');
    $('#tempoToleranciaDesistencia').val('');
    $('#horaInicio').val('');
    $('#horaFim').val('');
    $('#valorDiaria').val('');
    $('#horaAdicional').prop('checked', false);
    $('#padrao').prop('checked', false);
    $('#quantidadeHora').val('');
    $('#valorPorQuantidade').val('');
    $('#quantidadeHora').attr('readonly', 'readonly');
    $('#valorPorQuantidade').attr('readonly', 'readonly');
    $('#campoParaAdicionarNomeLista').val('');
    $('.periodo').prop('checked', false);
    $('#horaValorHora').val('');
    $('#valorValorHora').val('');
    $('#unidade').val('').trigger('chosen:updated');
}

function eventos() {

    $('#horaAdicional').change(function () {
        if (this.checked) {
            $('#quantidadeHora').removeAttr('readonly');
            $('#valorPorQuantidade').removeAttr('readonly');
        } else {
            $('#quantidadeHora').attr('readonly', 'readonly');
            $('#valorPorQuantidade').attr('readonly', 'readonly');
        }
    });

    $('#botaoAdicionarUnidade').click(function () {

        adicionarUnidade();
    });

    $('#botaoAdicionarLinha').click(function () {

        adicionarHoraValor();
    });

    $('#tempoToleranciaPagamento').keydown(function (e) {
        if (!SomenteNumero(e)) {
            e.preventDefault();
            return false;
        }
    });

    $('#tempoToleranciaDesistencia').keydown(function (e) {
        if (!SomenteNumero(e)) {
            e.preventDefault();
            return false;
        }
    });

    $('#quantidadeHora').keydown(function (e) {
        if (!SomenteNumero(e)) {
            e.preventDefault();
            return false;
        }
    });
}

function controlaPeriodo() {

    periodo = [];
    let listaIdPeriodo = [];
    let periodos = $('.periodo');
    for (let index = 0; index < periodos.length; index++)
        if ($('#' + periodos[index].id).is(':checked'))
            listaIdPeriodo.push($('#' + periodos[index].id).val());

    if (listaIdPeriodo.length > 0)
        periodo = { listaPeriodoSelecionado: listaIdPeriodo };
}

function adicionarUnidade() {

    let horaInicio = $('#horaInicio').val();
    let horaFim = $('#horaFim').val();
    let valorDiaria = 0;
    let idUnidade = $('#unidade').val();
    let descricaoUnidade = $('#unidade option:selected').text();

    if ($('#valorDiaria').val() != "")
        valorDiaria = $('#valorDiaria').val()

    if (!validarParaAdicionarUnidade(horaInicio, horaFim, idUnidade))
        return;

    if ($.inArray(idUnidade, listaIdUnidade) !== -1) {
        toastr.error('Unidade já foi selecionada!');
        return;
    }

    if (!preencheListaUnidade(idUnidade, descricaoUnidade, horaInicio, horaFim, valorDiaria))
        return;

    $.ajax({
        url: `/${controller}/AdicionarUnidade`,
        type: 'POST',
        data: {  listaUnidade },
        success: function (result) {
            $('#divUnidade').empty();
            $('#divUnidade').append(result);
            $('#unidade').val('').trigger('chosen:updated');
        },
        beforeSend: function () {
            showLoading();
        },
        error: function (err) {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });
}

function validarParaAdicionarUnidade(horaInicio, horaFim, unidade) {

    let valido = true;

    if (!unidade) {
        toastr.error('O campo \"Unidade\" deve ser selecionado!');
        valido = false;
    }

    if (!horaFim) {
        toastr.error('O campo \"Hora Fim\" deve ser preenchido!');
        valido = false;
    } else if (horaFim === '0' || parseInt(horaFim) > 24) {
        toastr.error('O campo \"Hora Fim\" deve ser preenchido com horário no formato \"HH:mm\" do tipo 24hs!');
        valido = false;
    }

    if (!horaInicio) {
        toastr.error('O campo \"Hora Início\" deve ser preenchido!');
        valido = false;
    } else if (horaInicio === '0' || parseInt(horaInicio) > 24) {
        toastr.error('O campo \"Hora Início\" deve ser preenchido com horário no formato \"HH:mm\" do tipo 24hs!');
        valido = false;
    }

    return valido;
}

function preencheListaUnidade(unidade, descricao, horainicio, horafim, valorDiaria) {

    if (!gridPossuiRegistro('#gridUnidade')) {
        listaUnidade.push({ unidade: { Id: unidade, Nome: descricao }, HoraInicio: horainicio, HoraFim: horafim, ValorDiaria: valorDiaria });
    } else {
        listaIdUnidade = new Array();
        listaUnidade = [];

        $('#gridUnidade tbody tr').each(function () {
            let id = $(this).find('td').eq(4).find('.idUnidade').val();
            let nome = $(this).find('td').eq(4).find('.nomeUnidade').val();
            let horainicio = $(this).find('td').eq(4).find('.horaInicio').val();
            let horafim = $(this).find('td').eq(4).find('.horaFim').val();
            let valorDiaria = $(this).find('td').eq(4).find('.valorDiaria').val().replace("R$", "");
            listaUnidade.push({ unidade: { Id: id, Nome: nome }, HoraInicio: horainicio, HoraFim: horafim, ValorDiaria: valorDiaria });
            listaIdUnidade.push(id);
        });

        if ($.inArray(unidade, listaIdUnidade) !== -1) {
            toastr.error('Unidade já foi selecionada!');
            return false;
        } else {
            listaUnidade.push({ unidade: { Id: unidade, Nome: descricao }, HoraInicio: horainicio, HoraFim: horafim, ValorDiaria: valorDiaria });
        }
    }

    return true;
}


function adicionarHoraValor() {

    let hora = $('#horaValorHora').val();
    let valor = $('#valorValorHora').val();

    if (!validarParaAdicionarHoraValor(hora, valor))
        return;

    if (!preencheListaHoraValor(hora, valor))
        return;

    $.ajax({
        url: `/${controller}/AdicionarHoraValor`,
        type: 'POST',
        data: { listaHoraValor: listaHoraValor },
        success: function (result) {
            $('#divHoraValor').empty();
            $('#divHoraValor').append(result);
            $('#horaValorHora').val('');
            $('#valorValorHora').val('');
        },
        beforeSend: function () {
            showLoading();
        },
        error: function (err) {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });
}

function validarParaAdicionarHoraValor(hora, valor) {

    let valido = true;

    if (!valor) {
        toastr.error('O campo \"Valor\" deve ser selecionado!');
        valido = false;
    }

    if (!hora) {
        toastr.error('O campo \"Hora\" deve ser preenchido!');
        valido = false;
    } else if (hora === '0' || parseInt(hora) > 24) {
        toastr.error('O campo \"Hora\" deve ser preenchido com valores entre 1 e 24!');
        valido = false;
    }

    return valido;
}

function preencheListaHoraValor(hora, valor) {

    if (!gridPossuiRegistro('#gridHoraValor')) {
        listaHoraValor.push({ hora, valor });
    } else {
        var listaHora = new Array();
        listaHoraValor = [];

        $('#gridHoraValor tbody tr').each(function () {
            let valorHora = $(this).find('.coluna-hora').text();
            let valorValor = $(this).find('.coluna-valor-hora').text().replace("R$", "");
            listaHoraValor.push({ hora: valorHora, valor: valorValor });
            listaHora.push(valorHora);
        });

        if ($.inArray(hora, listaHora) !== -1) {
            toastr.error('Hora já foi preenchida!');
            return false;
        } else {
            listaHoraValor.push({ hora, valor });
        }
    }

    return true;
}

function gridPossuiRegistro(id) {

    if ($(id + ' tbody tr').length) {
        if ($(id + ' tbody tr td').text().trim() === 'Nenhum registro foi encontrado.')
            return false;
    }

    if ($(id + ' tbody tr').length === 0)
        return false;

    return true;
}

function editarUnidadeVigencia(idUnidade, horaInicio, horaFim, valorDiaria) {

    removeUnidadeDaListaPorIdUnidade(idUnidade);
    $('#horaInicio').val(horaInicio);
    $('#horaFim').val(horaFim);
    $('#valorDiaria').val(valorDiaria);
    $('#unidade').val(idUnidade).trigger('chosen:updated');
}

function excluirUnidadeVigencia(idUnidade) {

    removeUnidadeDaListaPorIdUnidade(idUnidade);
}

function removeUnidadeDaListaPorIdUnidade(idUnidade) {

    $('#gridUnidade tbody tr').each(function () {
        let id = $(this).find('td').eq(4).find('input').val();
        if (idUnidade === id) {
            $(this).remove();
            return false;
        }
    });

    if (!gridPossuiRegistro('#gridUnidade'))
        $('#gridUnidade tbody').append('<tr><td align="center" colspan="3">Nenhum registro foi encontrado.</td></tr>');

    listaUnidade = listaUnidade.filter(function (obj) {
        return obj.unidade.id !== idUnidade;
    });
}

function editarHoraValor(hora, valor) {

    removeHoraValorDaListaPorHora(hora);
    $('#horaValorHora').val(hora);
    $('#valorValorHora').val(valor);
}

function excluirHoraValor(hora) {

    removeHoraValorDaListaPorHora(hora);
}

function removeHoraValorDaListaPorHora(hora) {

    $('#gridHoraValor tbody tr').each(function () {
        let horaAuxiliar = $(this).find('td').eq(2).find('.hora').val();
        if (hora === horaAuxiliar) {
            $(this).remove();
            return false;
        }
    });

    if (!gridPossuiRegistro('#gridHoraValor'))
        $('#gridHoraValor tbody').append('<tr><td align="center" colspan="3">Nenhum registro foi encontrado.</td></tr>');

    listaHoraValor = listaHoraValor.filter(function (obj) {
        return obj.hora !== hora;
    });
}

function carregarArrayUnidade() {

    if (!gridPossuiRegistro('#gridUnidade'))
        return;

    $('#gridUnidade tbody tr').each(function () {
        let id = $(this).find('td').eq(4).find('.idUnidade').val();
        let nome = $(this).find('td').eq(4).find('.nomeUnidade').val();
        let horainicio = $(this).find('td').eq(4).find('.horaInicio').val();
        let horafim = $(this).find('td').eq(4).find('.horaFim').val();
        let valorDiaria = $(this).find('td').eq(4).find('.valorDiaria').val().replace("R$", "");
        
        listaUnidade.push({ unidade: { Id: id, Nome: nome }, HoraInicio: horainicio, HoraFim: horafim, ValorDiaria: valorDiaria });
    });
}

function carregarArrayHoraValor() {

    if (!gridPossuiRegistro('#gridHoraValor'))
        return;

    $('#gridHoraValor tbody tr').each(function () {
        let hora = $(this).find('td').eq(2).find('.hora').val();
        let valor = $(this).find('td').eq(2).find('.valor').val();

        listaHoraValor.push({ hora, valor });
    });
}

function confirmarExclusao(id, idUnidade, exclusaoGeral) {

    if (exclusaoGeral === 'True')
        abrirModalPersonalizado('Remover Registro', 'Tem certeza que deseja realizar esta remoção?', 'warning', function () { return excluir(id) }, 'Sim');
    else
        abrirModalPersonalizado('Remover Registro', 'Tem certeza que deseja realizar esta remoção?', 'warning', function () { return excluir(id, idUnidade) }, 'Sim');
}

function excluir(id, idUnidade) {

    let action = 'Excluir';
    let filtro = { id };

    if (idUnidade) {
        action = 'ExcluirUnidadeDaTabela';
        filtro = { id, idUnidade };
    }

    return post(action, filtro)
        .done((result) => {
            if (action == 'Excluir') {
                ObterNotificacoes();
            }

            if (result.Redirect) {
                abrirModalPersonalizado(result.Titulo, result.Mensagem, result.TipoModal, null, null, carregarGrid);
            }
            else if (!result.Sucesso) {
                abrirModalPersonalizado(result.Titulo, result.Mensagem, result.TipoModal);
            }
        });
}