class nuevoTurno{
    constructor() {
        this.init();
    }

    init() {
        utils.initSelect2(document.getElementById('slPacientes'), '/turnos/pacientes');

        const selectProfesionales = document.getElementById('slProfesionales');
        selectProfesionales.addEventListener('change', (event) => {
            this.profesionalChange(event.target.value);
        });

        let btnCrearPaciente = document.getElementById("btnCrearPaciente");
        btnCrearPaciente.addEventListener('click', (event) => {
            this.nuevoPacienteClick(btnCrearPaciente);
        });
    }

    nuevoPacienteClick(btnCrearPaciente) {
        var url = btnCrearPaciente.dataset.url;

        var _this = this;

        utils.get(
            url,
            (data) => { _this.abrirNuevoPacienteModal(data) },
            (error) => { alert(error)}
        )
    }

    abrirNuevoPacienteModal(data) {
        $('#modal-placeholder').html('');
        $('#modal-placeholder').html(data);
        $('#modal-placeholder > .modal').modal('show');

        this.initModal();
    }

    initModal() {
        let placeholderElement = document.getElementById('modal-placeholder');

        var $placeholder = $(placeholderElement);

        var _this = this;

        $placeholder.on('click', '[data-save="modal"]', function (event) {
            event.preventDefault();

            var form = $(this).parents('.modal').find('form');
            var actionUrl = form.attr('action');
            var dataToSend = form.serialize();

            $.post(actionUrl, dataToSend).done(function (data) {
                var newBody = $('.modal-body', data);
                $placeholder.find('.modal-body').replaceWith(newBody);

                var isValid = newBody.find('[name="IsValid"]').val() == 'True';
                if (isValid) {
                    $placeholder.find('.modal').modal('hide');

                    var pacienteId = newBody.find('[name="Id"]').val();
                    var pacienteNombre = newBody.find('[name="Nombre"]').val();
                    _this.seleccionarPacienteRecienCreado(pacienteId, pacienteNombre);
                }
            });
        });
    }

    seleccionarPacienteRecienCreado(id, nombre) {
        var newOption = new Option(nombre, id, true, true);
        $('#slPacientes').append(newOption).trigger('change');
    }

    profesionalChange(event) {
        this.buscarHorarios(event);
    }

    buscarHorarios(profesionalId) {
        var fechaElem = document.getElementById('fecha');

        var fecha = fechaElem.value;

        if (!fecha)
            return;

        var _this = this;

        utils.get(
            `/turnos/profesional/horarios?profesionalId=${profesionalId}&fecha=${fecha}`,
            function (data) { _this.llenarHorarios(data) },
            function (error) { alert(error); });
    }

    llenarHorarios(data) {
        var str = ""
        for (var item of data) {
            str += `<option value='${item.id}'>${item.text}</option>`;
        }

        document.getElementById("slHoraInicio").innerHTML = str; 
    }
}