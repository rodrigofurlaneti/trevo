$(document).ready(function () {
    // Validation
    $("#cpf").mask("999.999.999-99", { reverse: true });

    if ($('.section-cpf')) {
        $("#login-form").validate({
            // Rules for form validation
            rules: {
                cpf: {
                    required: true,
                    cpf: true
                },
            },

            // Messages for form validation
            messages: {
                cpf: {
                    required: 'Por favor entre com seu login (cpf)',
                    email: 'Por favor entre com um cpf valido'
                },
                password: {
                    required: 'Por favor entre com sua senha'
                }
            },

            // Do not change code below
            errorPlacement: function (error, element) {
                error.insertAfter(element.parent());
            }
        });
    }

    if ($('.section-password')) {
        $("#login-form").validate({
            // Rules for form validation
            rules: {
                password: {
                    required: true,
                    maxlength: 20
                }
            },

            // Messages for form validation
            messages: {
                password: {
                    required: 'Por favor entre com sua senha'
                }
            },

            // Do not change code below
            errorPlacement: function (error, element) {
                error.insertAfter(element.parent());
            }
        });
    } else {

    }
});