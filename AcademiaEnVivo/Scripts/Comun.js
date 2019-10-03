
$(function () {
    $('#datatables').dataTable({
        "pageLength": 5,
        "lengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]]
    });
    $('#datatables_wrapper .table-caption').text('E-Mails');
    $('#datatables_wrapper .dataTables_filter input').attr('placeholder', 'Buscar...');
});
