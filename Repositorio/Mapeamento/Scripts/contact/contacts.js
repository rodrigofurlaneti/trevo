$("#telephone").mask("(99) 9999-9999");
$("#cellphone").mask("(99) 99999-9999");

var idContactInEdit;

$('#add-contact').click(function (ele) {

    var contact = [
        document.getElementById('email').value,
        document.getElementById('telephone').value,
        document.getElementById('cellphone').value
    ];

    for (var i = 0; i < contact.length; i++) {
        if (contact[i]) {
            $('#data_table_contacts tbody')
                .append('<tr>' +
                '<td style="display:none;" id="id_contact">' + (idContactInEdit ? idContactInEdit : 0) + '</td>' +
                '<td class="col-xs-10">' + contact[i] + '</td>' +
                '<td class="col-xs-2">' +
                '<span class="btn btn-primary" style="margin-right: 10px;" onclick="editContact(this)"><i class="fa fa-edit"></i></span>' +
                '<span class="btn btn-danger" onclick="removeContact(this)"><i class="fa fa-remove"></i></span>' +
                '</td>' +
                '</tr>');
        }
    }

    removeHiddenItems();
    saveContacts();
    clearFields();
});

function saveContacts() {
    var dt = $("#data_table_contacts tbody tr");
    dt.splice(0, 0);

    var obj = [];

    for (var i = 0; i < dt.length; i++) {
        obj.push({
            Id: dt[i].children[0] != undefined ? dt[i].children[0].innerText ? parseInt(dt[i].children[0].innerText) : 0 : 0,
            Email: dt[i].children[1] != undefined ? dt[i].children[1].innerText.indexOf('@') > 0 ? dt[i].children[1].innerText : '' : '',
            Celular: dt[i].children[1] != undefined ? dt[i].children[1].innerText.length == 15 && dt[i].children[1].innerText.indexOf('(') == 0
                ? dt[i].children[1].innerText
                : '' : '',
            Telefone: dt[i].children[1] != undefined ? dt[i].children[1].innerText.length == 14 && dt[i].children[1].innerText.indexOf('(') == 0
                ? dt[i].children[1].innerText
                : '' : ''
        });
    }

    $.ajax({
        url: '/contato/AdicionarContato',
        type: 'POST',
        dataType: 'json',
        data: { json: JSON.stringify(obj) },
        success: function (data) {}
    });

    if (obj.length > 0 && $('#data_table_contacts tbody tr.odd').length > 0)
        $('#data_table_contacts tbody tr.odd').remove();
}

function editContact(ele) {
    var hiddenItems = $('#data_table_contacts tbody tr:hidden');
    if (hiddenItems) hiddenItems.show();
    clearFields();
    $('#cancel').hide();

    idContactInEdit = ele.parentElement.parentElement.children[0].innerText.trim();
    var value = ele.parentElement.parentElement.children[1].textContent.trim();

    if (value.length == 15 && value.indexOf('(') == 0)
        document.getElementById('cellphone').value = value;
    else if (value.length == 14 && value.indexOf('(') == 0)
        document.getElementById('telephone').value = value;
    else if (value.indexOf('@') > 0)
        document.getElementById('email').value = value;

    $('#cancel').show();
    $(ele.parentElement.parentElement).hide();

    VisibilidadeMsgSemContatos();
}

function removeContact(ele) {
    ele.parentElement.parentElement.remove();
    saveContacts();

    VisibilidadeMsgSemContatos();
}

function cancelEdit() {
    var hiddenItems = $('#data_table_contacts tbody tr:hidden');
    if (hiddenItems) hiddenItems.show();
    clearFields();
    $('#cancel').hide();

    VisibilidadeMsgSemContatos();
}

function removeHiddenItems() {
    var hiddenItems = $('#data_table_contacts tbody tr:hidden');
    if (hiddenItems) hiddenItems.remove();
}

function clearFields() {
    idContactInEdit = 0;
    document.getElementById('email').value = "";
    document.getElementById('telephone').value = "";
    document.getElementById('cellphone').value = "";
    $('#cancel').hide();
}

function VisibilidadeMsgSemContatos() {
    var totalContatos = $("#data_table_contacts tbody tr").length;
    var totalContatosInvisiveis = $("#data_table_contacts tbody tr:hidden").length;
    if (totalContatos == 0 || totalContatos == totalContatosInvisiveis)
        $('#data_table_contacts tbody').append("<tr class='odd'><td valign='top' colspan='2' class='dataTables_empty'>Nenhum registro encontrado</td></tr>")
    else
        $('#data_table_contacts tbody tr.odd').remove();
}