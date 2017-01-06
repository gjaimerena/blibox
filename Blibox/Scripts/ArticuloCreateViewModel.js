var detalleViewModel = function () {
    var self = this;

    self.ID_material = ko.observable(0);
    self.Peso = ko.observable(0)
    self.ID_marco = ko.observable(0);
    self.ID_matriz = ko.observable(0);
    self.ID_cortante = ko.observable(0);
    self.Ciclos = ko.observable(0);
    self.Bocas = ko.observable(0)
    self.Espesor = ko.observable(0);
    self.Color = ko.observable(0);

    // se aplican las validaciones definidas en el html
    ko.validatedObservable(self);
};


var articuloViewModel = function () {
    var self = this;

    self.detalle = ko.observableArray();

    self.agregarDetalle = function () {
        if (self.detalle().length < 3)
            self.detalle.push(new detalleViewModel());
        else alert("Maximo de componente permitidos.")
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