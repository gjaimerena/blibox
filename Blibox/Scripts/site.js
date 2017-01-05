function validarForm(id) {
    $(document).ready(function() {
        $(id) 
            .on('init.field.fv', function(e, data) {
                // data.fv      --> The FormValidation instance
                // data.field   --> The field name
                // data.element --> The field element

                var $icon      = data.element.data('fv.icon'),
                    options    = data.fv.getOptions(),                      // Entire options
                    validators = data.fv.getOptions(data.field).validators; // The field validators

                if (validators.notEmpty && options.icon && options.icon.required) {
                    // The field uses notEmpty validator
                    // Add required icon
                    $icon.addClass(options.icon.required).show();
                }
            })
            .formValidation({
                framework: 'bootstrap',
                message: 'El valor no es válido',
                icon: {
                    required: 'glyphicon glyphicon-asterisk',
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh',
                },
                //row: '.grupoInput',
            })
            .on('status.field.fv', function(e, data) {
                // Remove the required icon when the field updates its status
                var $icon      = data.element.data('fv.icon'),
                    options    = data.fv.getOptions(),                      // Entire options
                    validators = data.fv.getOptions(data.field).validators; // The field validators

                if (validators.notEmpty && options.icon && options.icon.required) {
                    $icon.removeClass(options.icon.required).addClass('glyphicon');
                }
            });
    });
}
/*
function inicializarDatePicker(id, fechaMinima, fechaMaxima) {
    if (fechaMinima = '')
        fechaMinima = false

    if (fechaMaxima = '')
        fechaMaxima = false

    $(id).ready(function () {
        $(id)
            .datepicker({
                startView: 3,
                maxViewMode: 3,
                language: 'es',
                autoclose: true,
                format: 'dd/mm/yyyy',
                startDate: fechaMinima,
                endDate: fechaMaxima,
                linked: true })
            .on('changeDate', function (e) {
                console.log('asdsa');
                // Revalidate the date field
                $('#form').formValidation('revalidateField', id);//'[name="'+id.substring(1)+'"]');
            })
        });
}
*/

function inicializarDatePicker(id, fechaMinima, fechaMaxima) {
    if (fechaMinima == '')
        fechaMinima = false;
    if (fechaMaxima == '')
        fechaMaxima = false;
    var idBusqueda =  id;
    $(idBusqueda)
        .ready(function () {
            $(idBusqueda)
                .datepicker({
                    startView: 3,
                    maxViewMode: 3,
                    language: "es",
                    autoclose: true,
                    format: 'dd/mm/yyyy',
                    startDate: fechaMinima,
                    endDate: fechaMaxima,
                })
                .on('changeDate', function (e) {
                    // Revalidate the date field
                    $('#form').formValidation('revalidateField', id.substring(1));
                });
        });
}