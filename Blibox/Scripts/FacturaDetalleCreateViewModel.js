var detalleViewModel = function () {
    var self = this;

    self.ID_articulo = ko.observable(0);
    self.Cantidad = ko.observable(0)
    self.Precio_unitario = ko.observable(0);
    self.Precio_total = ko.observable(0);
    

    // se aplican las validaciones definidas en el html
    ko.validatedObservable(self);
};

var articuloViewModel = function () {
    var self = this;

    self.detalle = ko.observableArray();

    self.agregarDetalle = function () {
       
            self.detalle.push(new detalleViewModel());
       
    };

    self.eliminarDetalle = function (detalle) {
        self.detalle.remove(detalle);
    };

    

    self.canCreate = function () {
        var detallesValidos = ((self.detalle().length > 0) && (self.detalle().length < 4));

        //ko.utils.arrayForEach(self.detalle(), function (item) {
        //    detallesValidos = detallesValidos && item.isValid;
        //});

        return detallesValidos;
    };
};