


$(document).ready(function () { // will trigger when the document is ready
    $('.datepicker').datepicker({
        dateFormat: "mm/dd/yy",
        format: "dd/mm/yy",
        showOtherMonths: true,
        selectOtherMonths: true,
        autoclose: true,
        changeMonth: true,
        changeYear: true,
    }); //Initialise any date pickers

    $('.timepicker').timepicker({
        timeFormat: 'hh:mm p',
        interval: 60,
        minTime: '08',
        maxTime: '6:00pm',
        defaultTime: '08',
        startTime: '08:00',
        dynamic: true,
        dropdown: true,
        scrollbar: true
    });

    //function formatNumber(n) {
    //    n = String(n).replace(/\D/g, "");
    //    return n === '' ? n : Number(n).toLocaleString();
    //}
    //number.addEventListener('keyup', (e) => {
    //    const element = e.target;
    //    const value = element.value;
    //    element.value = formatNumber(value);
    //});

    var cleaveNumeral = new Cleave('#precio', {
        numeral: true,
        numeralThousandsGroupStyle: 'thousand',
        numeralDecimalMark: ',',
        delimiter: '.'
    });

});

function Clean() {
    console.log("limpio");
    document.getElementById("nombre").value = "";
    document.getElementById("descripcion").value = "";
    document.getElementById("hora_desde").value = "8:00 a. m.";
    document.getElementById("hora_hasta").value = "8:00 a. m.";
    document.getElementById("precio").value = "0";
    document.getElementById("invitados").value = "";
}

//document.getElementById("hora_desde").value = "10:00 PM";

