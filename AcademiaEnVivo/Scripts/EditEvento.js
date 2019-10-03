$(document).ready(function () {

    

    var cleaveNumeral = new Cleave('#precio', {
        numeral: true,
        numeralThousandsGroupStyle: 'thousand',
        numeralDecimalMark: ',',
        delimiter: '.'
    });

});

function blockelement(el) {

    $(el).block(
        {
            message: '',
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff',
                top: ($(window).height() - 400) / 2 + 'px',
                left: ($(window).width() - 400) / 2 + 'px',
            }
        });
}

function reinvitar(id) {
    blockelement('#divinvitados')
    $.ajax({
        url: '../Invitar',
        data: { id: id }
    }).done(function () {
        swal("Exito", "Invitación Enviada", "success");
        $('#divinvitados').unblock()
    });
    
}

function confirmdelete(id) {
    swal({
        title: "Esta seguro de eliminar el registro?",
        text: "",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: '../DeleteInvitado',
                data: { id: id }
            }).done(function () {
                $('#datatables').DataTable().row("#" + id).remove().draw();
            });
        }
    });
}