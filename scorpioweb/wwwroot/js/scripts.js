/* ========================================================================= */
/*	Codigo extra
/* ========================================================================= */
function confirmacionQR() {
    if (confirm('¿Estas seguro de Guardar los datos?')) {
        document.QRForm.submit();
    }
}

function probando() {
    alert('si funciona');
}

$(function () {
    $('#btnGuardar').click(function (e) {
        e.preventDefault();
    });
});

$("#confirmDialog").dialog({
    autoOpen: false,
    modal: true,
    resizable: false,
    buttons: {
        "Ok": function () {
            $('QRForm').submit();
        },
        "Cancel": function (e) {
            $(this).dialog("close");
        }
    },
}))