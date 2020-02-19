function ValidaEndereco() {
    if (document.getElementById('CEP').text == '') {
        toastr.error("O campo \"CEP\" é obrigatório");
        return false;
    }

    if (document.getElementById('logradouro').text == '') {
        toastr.error("O campo \"Logradouro\" é obrigatório");
        return false;
    }

    if (document.getElementById('CEP').text == '') {
        toastr.error("O campo \"CEP\" é obrigatório");
        return false;
    }

    if (document.getElementById('logradouro').text == '') {
        toastr.error("O campo \"Logradouro\" é obrigatório");
        return false;
    }

    if (document.getElementById('numeroEndereco').text == '') {
        toastr.error("O campo \"Número\" é obrigatório");
        return false;
    }

    if (document.getElementById('bairro').text == '') {
        toastr.error("O campo \"Bairro\" é obrigatório");
        return false;
    }

    if (document.getElementById('cidade').text == '') {
        toastr.error("O campo \"Cidade\" é obrigatório");
        return false;
    }
}