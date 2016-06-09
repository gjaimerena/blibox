function Add() {
    $("#tblData tbody").append("<tr>" +
        "<td><input type='text'/></td>" + 
        "<td><input type='text'/></td>" + 
        "<td><input type='text'/></td>" +
        "<td><input type='text'/></td>" +
        "<td> <img src='../Content/dist/img/Save.png' style='width: 25px;' class='btnSave'><img src='../Content/dist/img/Delete.png' style='width: 25px;' class='btnDelete'/></td>" + "</tr>");
    $(".btnSave").bind("click", Save);
    $(".btnDelete").bind("click", Delete);
};

function Save(){ 
    var par = $(this).parent().parent();  //tr
    var tdArticulo = par.children("td:nth-child(1)"); 
    var tdCantidad = par.children("td:nth-child(2)"); 
    var tdPrecioU = par.children("td:nth-child(3)");
    var tdPrecioT = par.children("td:nth-child(4)");
    var tdButtons = par.children("td:nth-child(5)"); 
    tdArticulo.html(tdArticulo.children("input[type=text]").val());
    tdCantidad.html(tdCantidad.children("input[type=text]").val());
    tdPrecioU.html(tdPrecioU.children("input[type=text]").val());
    tdPrecioT.html(tdPrecioT.children("input[type=text]").val());
    tdButtons.html("<img src='../Content/dist/img/Delete.png'   style='width: 25px;' class='btnDelete'/><img src='../Content/dist/img/Edit.png' style='width: 25px;' class='btnEdit'/>");
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
    tdArticulo.html("<input type='text' id='txtArt'" + tdArticulo.html()+"' value='" + tdArticulo.html() + "'/>");
    tdCantidad.html("<input type='text' id='txtCant'" + tdArticulo.html() + "' value='" + tdCantidad.html() + "'/>");
    tdPrecioU.html("<input type='text' id='txtPU'" + tdArticulo.html() + "' value='" + tdPrecioU.html() + "'/>");
    tdPrecioT.html("<input type='text' id='txtPT'" + tdArticulo.html()+"' value='" + tdPrecioT.html() + "'/>");
    tdButtons.html("<img src='../Content/dist/img/Save.png' style='width: 25px;' class='btnSave'/>");
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

  