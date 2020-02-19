$("#user-autocomplete").autocomplete({
    source: function (request, response) {
        $.ajax({
            url: '/utils/suggestionperson',
            type: 'POST',
            dataType: 'json',
            data: { param: request.term },
            success: function (data) {
                response($.map(data, function (item) {
                    pessoa = item;
                    return { label: item.Email, value: item.Email };
                }))

            }
        })
    },
    noResults: function () { },
    results: function () { },
    select: function (elem, obj) {
        //alert('Usuario selecionado: ' + pessoa.Email);
    }
});