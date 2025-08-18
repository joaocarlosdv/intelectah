$(document).ready(function () {
    $('#cep').mask('00000-000', { reverse: false });
    $('#telefone').mask('(00)00000-0000', { reverse: false });
});

$("#btnCep").click(function () {
    var cep = $("#cep").val();

    if (!cep) {
        alert("Informe um CEP válido!");
        return;
    }

    $.ajax({
        url: '/Concessionaria/ConsultarCep',
        type: 'GET',
        data: { cep: cep },
        success: function (data) {
            if (data) {
                $("#logradouro").val(data.logradouro);
                $("#bairro").val(data.bairro);
                $("#cidade").val(data.localidade);
                $("#uf").val(data.uf);
            } else {
                alert("CEP não encontrado!");
                $("#logradouro").val('');
                $("#bairro").val('');
                $("#cidade").val('');
                $("#uf").val('');
            }
        },
        error: function () {
            alert("Erro ao buscar o CEP.");
        }
    });
});