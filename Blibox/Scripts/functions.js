function obtenerArticulos(idArticulos, idPreciosUnitarios ) {
    var strSelected = "";
    $("#ID_cliente option:selected").each(function () {
        strSelected += $(this)[0].value;
    });
    $.ajax({
        type: "POST",
        url: "obtenerArticulos",
        data: "{'idCliente': " + strSelected + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data != null) {
                var selectArticulos = $('#'+idArticulos);
                var selectPreciosUnitarios = $('#' + idPreciosUnitarios);

                for (var articulo in data) {
                    var option = document.createElement('option');
                    option.innerHTML = data[articulo].Descripcion;
                    option.value = data[articulo].ID_articulo;
                    selectArticulos.append(option);

                    var option2 = document.createElement('option');
                    option2.innerHTML = data[articulo].Precio_lista;
                    option2.value = data[articulo].ID_articulo;
                    selectPreciosUnitarios.append(option2);
                }
                selectArticulos.change();
            }
        },
        error: function (request, status, error) {
            alert(request.responseText);
        }
    });
}

function setPrecioUnitario(idOrigen, idArticulos, idDestino) {
    var strSelected = "";
    $('#'+ idArticulos + " option:selected").each(function () {
        strSelected += $(this)[0].value;
    });
    var precioUnitario = 0;
    $.ajax({
        type: "POST",
        url: "obtenerPrecioUnitario",
        data: "{'idArticulo': " + strSelected + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false, //FIXME: no usar async false, tratar de devolver el valor de data y asignarlo en precioUnitario
        success: function (data) {
            if (data != null) {
                precioUnitario = data;
            }
        },
        error: function (request, status, error) {
            alert(request.responseText);
        }
    });
    $('#' + idDestino).val(precioUnitario);
    //$('#' + idDestino).change();
    $('#form').formValidation('revalidateField', idDestino);

    
}

function calcularPrecioTotal(idCantidad, idPrecioUnitario, idPrecioTotal) {
    var cantidad = $('#' + idCantidad).val();
    var precioUnitario = $('#' + idPrecioUnitario).val();
    $('#' + idPrecioTotal).val(cantidad * precioUnitario);

    if (cantidad == 0 || precioUnitario == 0) {
        $('#' + idPrecioTotal).val("Cantidad y PU debe ser mayor a 0");
    }
   
    var subtotal = $("#subtotal").val(0);
    $('#tblData tr.dato').each(function () { //filas con clase 'dato', especifica una clase, asi no tomas el nombre de las columnas

        subtotal = $("#subtotal").val();
        var dato = $(this).find('td:eq(3) input').val();
        var iva = parseInt($("#iva").val());
        var porcIva = 0;
        subtotal = parseFloat(subtotal) + parseFloat(dato); //numero de la celda 3

        $('#subtotal').val(Math.round(subtotal * 100) / 100); //numero de la celda 3
        if (iva <= 3) {
            porcIva = 0;
        }
        if (parseInt(iva) == 4) porcIva = 0.105;
        if (parseInt(iva) == 5) porcIva = 0.21;
        if (parseInt(iva) == 6) porcIva = 0.27;

        var total = parseFloat(subtotal) + (parseFloat(subtotal) * (parseFloat(porcIva)));
        $('#total').val(Math.round(total * 100) / 100);
    })

    if (isNaN(subtotal)) {
        $("#subtotal").val(0);
        $("#total").val(0);
       // $("#btnAdd").attr("disabled", true);
    }
    else{
        $("#btnAdd").removeAttr("disabled");
    }

    $('#form').formValidation('revalidateField', 'subtotal');
    $('#form').formValidation('revalidateField', 'total');

}

function Add() {
    var btn = $(this);
    var fila = btn.data('filas') + 1;
    btn.data('filas', fila);
    var idArticulos = 'articulos' + fila;
    var idCantidad = 'cantidad' + fila;
    var idPrecioUnitario = 'precioUnitario' + fila;
    var idPrecioTotal = 'precioTotal' + fila;
    var idPreciosUnitarios = 'preciosUnitarios' + fila;
        

    var nameArticulos = 'articulos[' + fila + '].articulo';
    var nameCantidad = 'articulos[' + fila + '].cantidad';
    var namePrecioUnitario = 'articulos[' + fila + '].precioUnitario';
    var namePrecioTotal = 'articulos[' + fila + '].precioTotal';

    $("#tblData tbody").append("<tr class='dato'>" +
        "<td><select class = 'form-control' id='" + idArticulos + "' name='" + nameArticulos + "' /></td>" +
        "<td><input class = 'form-control' type='text' value='1' onkeypress='return validateFloatKeyPress(this,event);' id='" + idCantidad + "' name ='" + nameCantidad + "' class='precio' /></td>" +
        "<td><input class = 'form-control' type='text' onkeypress='return validateFloatKeyPress(this,event);' id='" + idPrecioUnitario + "' name ='" + namePrecioUnitario + "'  class='precio' min='0' data-fv-greaterthan-message=\"<i class='glyphicon glyphicon-info-sign'></i> El monto no puede ser menor a 0\" data-fv-greaterthan-value='0' data-fv-greaterthan-inclusive='false' data-fv-greaterthan='true'  /></td>" +
        //"<td><input class = 'form-control' type='number' step='any'  id='" + idPrecioUnitario + "' name ='" + namePrecioUnitario + "'></td>"+
        "<td><input class = 'form-control' type='text' onkeypress='return validateFloatKeyPress(this,event);' id='" + idPrecioTotal + "' name ='" + namePrecioTotal + "' readonly='readonly' class='precioTotal' min='0' data-fv-greaterthan-message=\"<i class='glyphicon glyphicon-info-sign'></i> El monto no puede ser menor a 0\" data-fv-greaterthan-value='0' data-fv-greaterthan-inclusive='false' data-fv-greaterthan='true' /></td>" +
        "<td><i class='btnSave glyphiconAddItem glyphicon glyphicon-floppy-disk'></i><i class='btnDelete glyphiconAddItem glyphicon glyphicon-remove'></td>" +
        "</tr>");
    $(".btnSave").bind("click", Save);
    $(".btnDelete").bind("click", Delete);
    

    $('#' + idArticulos).change(function () {
        setPrecioUnitario(idPreciosUnitarios, idArticulos, idPrecioUnitario);
        $('#' + idPrecioUnitario).change();
    });

    $('#' + idCantidad).change(function () {
        calcularPrecioTotal(idCantidad, idPrecioUnitario, idPrecioTotal);
    });

    $('#' + idPrecioUnitario).change(function () {
        calcularPrecioTotal(idCantidad, idPrecioUnitario, idPrecioTotal);
    });

    $('#iva').change(function () {
        calcularPrecioTotal(idCantidad, idPrecioUnitario, idPrecioTotal);
    });

    obtenerArticulos(idArticulos, idPreciosUnitarios);

        var validador = {
            row: '.dobleEntrada',   // The title is placed inside a <div class="col-xs-4"> element
            validators: {
                greaterthan: {
                    message: 'The title is required',
                    min: 0
                },
                notEmpty: {
                    message: 'asd'
                }
            }
        };
    $('#form').formValidation('addField', idPrecioUnitario, validador);
    $('#form').formValidation('addField', idPrecioTotal, validador);

};

function Save(){ 
    var par = $(this).parent().parent();  //tr
    var tdArticulo = par.children("td:nth-child(1)"); 
    var tdCantidad = par.children("td:nth-child(2)"); 
    var tdPrecioU = par.children("td:nth-child(3)");
    var tdPrecioT = par.children("td:nth-child(4)");
    var tdButtons = par.children("td:nth-child(5)");

    tdArticulo.children().attr('readonly', 'readonly');
    tdCantidad.children().attr('readonly', 'readonly');
    tdPrecioU.children().attr('readonly', 'readonly');
    tdPrecioT.children().attr('readonly', 'readonly');
    tdButtons.html("<i class='btnDelete glyphiconAddItem glyphicon glyphicon-remove'></i><i class='btnEdit glyphiconAddItem glyphicon glyphicon-pencil'></i>");
    $(".btnEdit").bind("click", Edit); 
    $(".btnDelete").bind("click", Delete);

};

function Edit(){ 
    var par = $(this).parent().parent(); //tr 
    var tdArticulo = par.children("td:nth-child(1)");
    var tdCantidad = par.children("td:nth-child(2)");
    var tdPrecioU = par.children("td:nth-child(3)");
    var tdPrecioT = par.children("td:nth-child(4)");
    var tdButtons = par.children("td:nth-child(5)");
    tdArticulo.children().attr('readonly', false);
    tdCantidad.children().attr('readonly', false);
    tdPrecioU.children().attr('readonly', false);
    tdPrecioT.children().attr('readonly', false);
    tdButtons.html("<i class='btnSave glyphiconAddItem glyphicon glyphicon-floppy-disk'></i>");
    $(".btnSave").bind("click", Save);
    $(".btnEdit").bind("click", Edit); 
    $(".btnDelete").bind("click", Delete);
};

function Delete() {
    var par = $(this).parent().parent(); //tr 
    var idPrecioUnitario = $($(par.children()[2])[0].innerHTML).attr('id')
    $('#' + idPrecioUnitario).val(0);//seteo el precio en 0 asi actualiza el total
    $('#' + idPrecioUnitario).change();
    par.remove(); 
};

$(function () { //Add, Save, Edit and Delete functions code 
    //$(".btnEdit").bind("click", Edit);
    //$(".btnDelete").bind("click", Delete);
    $("#btnAdd").bind("click", Add);
});

function validateFloatKeyPress(el, evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var number = el.value.split('.');
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    //just one dot
    if (number.length > 1 && charCode == 46) {
        return false;
    }
    //get the carat position
    var caratPos = getSelectionStart(el);
    var dotPos = el.value.indexOf(".");
    if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
        return false;
    }
    return true;
}

//thanks: http://javascript.nwbox.com/cursor_position/
function getSelectionStart(o) {
    if (o.createTextRange) {
        var r = document.selection.createRange().duplicate()
        r.moveEnd('character', o.value.length)
        if (r.text == '') return o.value.length
        return o.value.lastIndexOf(r.text)
    } else return o.selectionStart
}