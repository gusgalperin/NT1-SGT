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