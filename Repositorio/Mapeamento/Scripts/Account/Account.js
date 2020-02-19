$(document).ready(function () {
    $('#password, #password-confirm').on('keyup', function () {
        if ($('#password').val() == $('#password-confirm').val()) {
            $('#no-match').hide();
            $('#send').removeAttr('disabled');
        } else {
            $('#no-match').show();
            $('#send').attr('disabled', 'disabled');
        }
    });

    $(function () {
        $('#password')[0].oninvalid = function () {
            this.setCustomValidity('O campo senha é obrigatorio.');
            this.setCustomValidity('');
        };

        $('#password-confirm')[0].oninvalid = function () {
            this.setCustomValidity('O campo confirmação de senha é obrigatorio.');
            this.setCustomValidity('');
        };
    });

    $("#uploadImgAvatar").change(function () {
        GetAvatarImage(this);
    });
});

function GetAvatarImage(file) {
    var input = file;
    var url = $(file).val();
    var ext = url.substring(url.lastIndexOf('.') + 1).toLowerCase();
    if (input.files && input.files[0] && (ext == "gif" || ext == "png" || ext == "jpeg" || ext == "jpg")) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imgAvatar').attr('src', e.target.result);
            ArmazenarImagem(e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
    else {
        $('#imgAvatar').attr('src', '../../Content/img/avatars/sunny-big.png');
    }
}

function ArmazenarImagem(dataBase64) {
    $.ajax({
        url: '/usuario/ArmazenarImagem',
        type: 'POST',
        data: { file: dataBase64 },
        success: function (response) {
            //var retorno = JSON.stringify(response);

            //$('#imgAvatar').attr("src", retorno.Data);
            console.log("armazenadis");

            hideLoading();
        },
        error: function (error) {
            hideLoading();
            //alert(JSON.stringify(error));
        },
        beforeSend: function () {
            showLoading();
        }
    });
}
