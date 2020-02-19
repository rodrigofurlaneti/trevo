$(document).ready(function () {
    
    MakeChosen("filial", null, "100%");
    MakeChosen("emails", null, "100%");

    ConfigTabelaGridSemCampoFiltroPrincipal();

    if ($("#TemPedido").val().toLowerCase() === 'true') {
       // $("#filial").prop("disabled", true); //TODO: fazer funcionar!
        $("#clientes").attr("disabled", true);
    };

    $("#PropostaForm").submit(function (e) {

        let dadosValidos = true;

        let empresaSelecionada = !!$("#empresa").val();

        if (!empresaSelecionada) {
            toastr.error("O campo \"Empresa\" deve ser selecionado!");
            dadosValidos = false;
        }

        let filialSelecionada = !!$("#filial").val();

        if (!filialSelecionada) {
            toastr.error("O campo \"Filial\" deve ser selecionado!");
            dadosValidos = false;
        }

        if (filialSelecionada && !$("#endereco").val()) {
            toastr.error("A Filial selecionada não possui ENDEREÇO, para prosseguir favor efetuar o cadastro, em cadastro Unidade!");
            dadosValidos = false;
        }

        if (filialSelecionada && !$("#horarioFuncionamento").val()) {
            toastr.error("A Filial selecionada não possui HORÁRIO DE FUNCIONAMENTO, para prosseguir favor efetuar o cadastro, em cadastro Unidade!");
            dadosValidos = false;
        }

        if (empresaSelecionada && !$('#emails option').length) {
            toastr.error("A Empresa selecionada não possui EMAIL, para prosseguir favor efetuar o cadastro, em cadastro Cliente!");
            dadosValidos = false;
        } else if (empresaSelecionada && !$("#emails").val()) {
            toastr.error("O campo \"Email\" deve ser preenchido!");
            dadosValidos = false;
        }

        if (!dadosValidos) {
            e.preventDefault();
            return false;
        }

        $("#email").val($("#emails").val());
	});

	$("#clientes").autocomplete({
		source: function (request, response) {
			$.ajax({
				url: '/proposta/BuscarCliente',
				data: { descricao: request.term },
				type: "POST",
				success: function (data) {
					if (!data.length) {
						$("#empresa").val("");

						var result = [
							{
								label: 'Nenhum resultado encontrado',
								value: response.term
							}
						];
						response(result);						
					}
					else {
						response($.map(data, function (item) {
							return { label: item.Descricao, value: item.descricao, id: item.Id };
						}))
					}
				},
				error: function (response) {
					console.log(response.responseText);
				},
				failure: function (response) {
					console.log(response.responseText);
				}
			});
		},
		select: function (e, i) {
			$("#empresa").val(i.item.id);
			buscarTelefone();
			buscarEmail();
		},
		minLength: 3
	});
});

function buscarTelefone() {

    let idEmpresa = $("#empresa").val();
    if (!idEmpresa) {
        $("#telefone").val('');
        return;
    }

    $.ajax({
        url: "/Proposta/BuscarTelefone",
        type: "POST",
        dataType: "json",
        data: { idEmpresa },
        success: function (result) {
            $("#telefone").val(result);
        },
        error: function (error) {
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });
}

function buscarEndereco() {

    let idFilial = $("#filial").val();
    if (!idFilial) {
        $("#endereco").val('');
        return;
    }

    $.ajax({
        url: "/Proposta/BuscarEndereco",
        type: "POST",
        dataType: "json",
        data: { idFilial },
        success: function (result) {
            $("#endereco").val(result);
        },
        error: function (error) {
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });
}

function buscarHorarioFuncionamento() {

    let idFilial = $("#filial").val();
    if (!idFilial) {
        $("#horarioFuncionamento").val('');
        return;
    }

    $.ajax({
        url: "/Proposta/BuscarHorarioFuncionamento",
        type: "POST",
        dataType: "json",
        data: { idFilial },
        success: function (result) {
            $("#horarioFuncionamento").val(result);
        },
        error: function (error) {
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });
}

function buscarEmail() {

    $('#emails').empty();
    $('#emails').trigger("chosen:updated");

    let idEmpresa = $("#empresa").val();
    if (!idEmpresa) {
        return;
    }

    $.ajax({
        url: "/Proposta/BuscarEmail",
        type: "POST",
        dataType: "json",
        data: { idEmpresa },
        success: function (result) {
            for (let index = 0; index < result.length; index++) {
                let email = result[index];
                $("#emails").append(`<option value='${email}'>${email}</option>`);
            }
            $('#emails').trigger("chosen:updated");
        },
        error: function (error) {
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });
}

function buscarCliente() {

	$('#clientes').empty();
	$('#clientes').trigger("chosen:updated");

	let idCliente = $("#cliente").val();
	if (!idCliente) {
		return;
	}

	$.ajax({
		url: "/Proposta/BuscarCliente",
		type: "POST",
		dataType: "json",
		data: { idCliente },
		success: function (result) {
			for (let index = 0; index < result.length; index++) {
				let cliente = result[index];
				$("#clientes").append(`<option value='${cliente}'>${cliente}</option>`);
			}
			$('#clientes').trigger("chosen:updated");
		},
		error: function (error) {
			hideLoading();
		},
		beforeSend: function () {
			showLoading();
		},
		complete: function () {
			hideLoading();
		}
	});
}

function excluirQuandoTemPedido() {
    toastr.error("Proposta não pode ser excluída pois possui pedido(s) associado(s)");
}