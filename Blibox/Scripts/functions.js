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

    var precioUnitario = $('#' + idOrigen + ' option[value="' + strSelected + '"]').text();

    $('#'+ idDestino).val(precioUnitario);
            
}

function calcularPrecioTotal(idCantidad, idPrecioUnitario, idPrecioTotal) {
    var cantidad = $('#' + idCantidad).val();
    var precioUnitario = $('#' + idPrecioUnitario).val();
    $('#' + idPrecioTotal).val(cantidad * precioUnitario);
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

    $("#tblData tbody").append("<tr>" +
        "<td><select class = 'form-control' id='" + idArticulos + "' name='" + nameArticulos + "' /></td>" +
        "<td><input class = 'form-control' type='number' value='1' id='" + idCantidad + "' name ='" + nameCantidad + "' class='precio' /></td>" +
        "<td><input class = 'form-control' type='number' id='" + idPrecioUnitario + "' name ='" + namePrecioUnitario + "'  class='precio' /></td>" +
        "<td><input class = 'form-control' type='number' id='" + idPrecioTotal + "' name ='" + namePrecioTotal + "' readonly='readonly' class='precioTotal' /></td>" +
        "<td><i class='btnSave glyphicon glyphicon-floppy-disk'></i><i class='btnDelete glyphicon glyphicon-remove'></i> <select id='" + idPreciosUnitarios + "' class='hidden' /> </td>" +
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

    obtenerArticulos(idArticulos, idPreciosUnitarios);
    setPrecioUnitario(idPreciosUnitarios, idArticulos, idPrecioUnitario);
    calcularPrecioTotal(idCantidad, idPrecioUnitario, idPrecioTotal);
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
    tdButtons.html("<i class='btnDelete glyphicon glyphicon-remove'></i><i class='btnEdit glyphicon glyphicon-pencil'></i>");
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
    tdButtons.html("<i class='btnSave glyphicon glyphicon-floppy-disk'></i>");
    $(".btnSave").bind("click", Save);
    $(".btnEdit").bind("click", Edit); 
    $(".btnDelete").bind("click", Delete);
};

function Delete(){ 
    var par = $(this).parent().parent(); //tr 
    par.remove(); 
};

$(function () { //Add, Save, Edit and Delete functions code 
    //$(".btnEdit").bind("click", Edit);
    //$(".btnDelete").bind("click", Delete);
    $("#btnAdd").bind("click", Add);
});

  