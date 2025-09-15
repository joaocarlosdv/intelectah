$(document).ready(function () {
    
    $('#precoVenda').mask('##.###.###.##0,00', { reverse: true });
   

    $("#fabricanteId").on("change", function () {
        var fabricanteId = $(this).val();
        
        $("#veiculoId").empty().append('<option value="">Carregando...</option>');

        if (fabricanteId) {
            $.ajax({
                url: '/Veiculo/ConsultarPorFabricante', 
                type: 'GET', 
                data: { id: fabricanteId },
                success: function (data) {
                    $("#veiculoId").empty().append('<option value="">Selecione um Veículo</option>');
                    console.log(data);
                    $.each(data, function (i, veiculo) {                        
                        console.log(veiculo);

                        $("#veiculoId").append(
                            $('<option>', {
                                value: veiculo.id,
                                text: veiculo.modelo+' '+veiculo.anoFabricacao
                            })
                        );
                    });
                },
                error: function () {
                    $("#veiculoId").empty().append('<option value="">Erro ao carregar</option>');
                }
            });
        } else {
            $("#veiculoId").empty().append('<option value="">Selecione um Veículo</option>');
        }
    });

});


