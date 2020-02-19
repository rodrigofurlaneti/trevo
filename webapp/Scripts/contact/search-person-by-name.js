
var person;

$("#person-autocomplete").autocomplete({
    source: function (request, response) {
        $.ajax({
            url: "/pessoa/suggestionperson",
            type: "POST",
            dataType: "json",
            data: { param: request.term, exact: false },
            success: function (data) {
                response($.map(data,
                    function (item) {
                        pessoa = item;
                        return { label: item.Nome, value: item.Nome };
                    }));

            }
        });
    },
    noResults: function () { },
    results: function () { },
    select: function (elem, obj) {
        $("#lblMessageResult").text("");
        $("#hdnPersonSelectedId").val(pessoa.Id);
    },
    change: function (event, ui) {
        ConfirmSearchPersonByName(event.currentTarget.value);
    }
});

function ConfirmSearchPersonByName(param) {
    $.ajax({
        url: "/pessoa/suggestionperson",
        type: "POST",
        dataType: "json",
        data: { param: param, exact: true },
        success: function (person) {
            $("#hdnPersonSelectedId").val("");
            $("#person-autocomplete").val("");
            $("#lblMessageResult").text("");

            if (person !== undefined && person !== null && person.length > 0) {
                $("#hdnPersonSelectedId").val(person[0].Id);
                $("#person-autocomplete").val(person[0].Nome);
                $('#emailPessoa').val(person[0].ContatoEmail);
            } else {
                $("#lblMessageResult").text("Pessoa não encontrada pelo nome informado!");
            }
        }
    });
}