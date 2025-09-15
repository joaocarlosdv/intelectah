$(document).ready(function () {

    $('#myTable').DataTable({
        processing: false,
        serverSide: true,
        ajax: {
            url: '/Usuario/Consultar',
            type: 'GET',
            data: function (d) {
                return {
                    limit: d.length,
                    offset: d.start,
                    search: d.search.value,
                    colOrder: d.order[0].column,
                    dirOrder: d.order[0].dir,
                };
            },
            dataSrc: function (json) {
                console.log('json', json);
                return json.lista;
            }
        },
        columns: [
            { data: 'id', title: 'Id' },
            { data: 'nome', title: 'Nome' },
            { data: 'email', title: 'E-Mail' },
            { data: 'nivelAcessoName', title: 'Acesso' },
            {
                data: null,
                title: 'Ações',
                render: function (data, type, row) {

                    var btnEditar = '';
                    var btnVisualizar = '';
                    var btnExcluir = '';

                    const urlEditar = `/Usuario/Alterar?id=${row.id}`;
                    btnEditar = `
                            <a class="d-block px-1"
                                style="color: black"
                                href="${urlEditar}"
                            >
                                <i class="bi bi-pencil-square"
                                    data-toggle="tooltip"
                                    data-placement="top"
                                    title="Editar">
                                </i>
                            </a>
                        `;

                    const urlVisualizar = `/Usuario/Visualizar?id=${row.id}`;
                    btnVisualizar = `
                                <a class="d-block px-1"
                                    style="color: black"
                                        href="${urlVisualizar}"
                                >
                                        <i class="bi bi-eye"
                                        data-toggle="tooltip"
                                        data-placement="top"
                                            title="Visualizar">
                                    </i>
                                </a>
                            `;

                    const urlExcluir = `/Usuario/Excluir?id=${row.id}`;
                    btnExcluir = `
                            <a class="d-block px-1"
                                style="color: black"
                                href="${urlExcluir}"
                            >
                                <i class="bi bi-trash"
                                    data-toggle=""
                                    data-placement=""
                                    title="Excluir">
                                </i>
                            </a>
                        `;


                    return `<div class="d-flex">` + btnVisualizar + btnEditar + btnExcluir + `<\div>`;
                }
            }
        ],
        order: [[3, 'asc']],
        pageLength: 10,
        lengthMenu: [10, 50, 100],
        language: {
            zeroRecords: "Nenhum registro encontrado.",
            search: "Localizar",
            lengthMenu: "Itens por página _MENU_ ",
            paginate: {
                previous: "< Anterior ",
                next: " Próxima >",
            },
            sInfo: "Exibindo de _START_ até _END_ de _TOTAL_ registros",
            "infoFiltered": "",
            sInfoEmpty: "Exibindo 0 registros"
        }
    });
});