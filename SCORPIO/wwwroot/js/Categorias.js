var localStorage = window.localStorage;
class Categorias {
    constructor(nombre, descripcion, estado, action) {
        this.nombre = nombre;
        this.descripcion = descripcion;
        this.estado = estado;
        this.action = action;
    }

    agregarCategoria(id, funcion) {
        if (this.nombre == "") {
            document.getElementById("Nombre").focus();
        }
        else {
            if (this.descripcion == "") {
                document.getElementById("Descripcion").focus();
            }
            else {
                if (this.estado == "0") {
                    document.getElementById("mensaje").innerHTML = "Seleccione un estado";
                }
                else {
                    var nombre = this.nombre;
                    var descripcion = this.descripcion;
                    var estado = this.estado;
                    var action = this.action;
                    var mensaje = "";

                    $.ajax({
                        type: "POST",
                        url: action,
                        data: {
                            id, nombre, descripcion, estado, funcion
                        },
                        success: (response) => {
                            console.log("Hola");
                            console.log(response);
                            $.each(response, (index, val) => {
                                //console.log("response del each");
                                //console.log(response); 
                                //console.log("index del each");
                                //console.log(index);
                                //console.log("val del each");
                                //console.log(val);
                                mensaje = val.description;
                                console.log("mensaje del each");
                                console.log(mensaje);

                            });
                            if (mensaje === "Save") {
                                this.restablecer();
                            }
                            else {
                                document.getElementById("mensaje").innerHTML = "No se puede guardar la categoria";
                            }
                            //console.log(response);
                        }
                    });
                }
            }
        }
    }

    filtrarDatos(numPagina) {
        var valor = this.nombre;
        var action = this.action;
        if (valor == "") {
            valor = "null";
        }
        $.ajax({
            type: "POST",
            url: action,
            data: { valor, numPagina },
            success: (response) => {
                console.log(response);
                $.each(response, (index, val) => {
                    $("#resultSearch").html(val[0]);
                    $("#paginado").html(val[1]);
                });
            }
        });
    }

    getCategoria(id, funcion) {
        var action = this.action;
        $.ajax({
            type: "POST",
            url: action,
            data: { id },
            success: (response) => {
                console.log(response);
                if (funcion == 0) {
                    if (response[0].estado) {
                        document.getElementById("titleCategoria").innerHTML = "Estas seguro de desactivar la categoría " + response[0].nombre;
                    }
                    else {
                        document.getElementById("titleCategoria").innerHTML = "Estas seguro de habilitar la categoría " + response[0].nombre;
                    }
                } else {
                    document.getElementById("Nombre").value = response[0].nombre;
                    document.getElementById("Descripcion").value = response[0].descripcion;
                    if (response[0].estado) {
                        document.getElementById("Estado").selectedIndex = 1;
                    } else {
                        document.getElementById("Estado").selectedIndex = 2;
                    }
                }
                
                localStorage.setItem("categoria", JSON.stringify(response)); //se almacena en la memoria local del navegador (hasta 5mb)
            }
        });
    }

    editarCategoria(id, funcion) {
        var nombre = null;
        var descripcion = null;
        var estado = null;
        var action = null;
        switch (funcion) {
            case "estado":
                var response = JSON.parse(localStorage.getItem("categoria"));
                nombre = response[0].nombre;
                descripcion = response[0].descripcion;
                estado = response[0].estado;
                localStorage.removeItem("categoria");
                this.editar(id, nombre, descripcion, estado, funcion);
                break;
            default:
                break;
        }
    }

    editar(id, nombre, descripcion, estado, funcion) {
        var action = this.action;
        $.ajax({
            type: "POST",
            url: action,
            data: { id, nombre, descripcion, estado, funcion },
            success: (response) => {
                console.log("Desde el metodo editar en JS");
                console.log(response);
                this.restablecer();
            }
        });
    }

    restablecer() {
        document.getElementById("Nombre").value = "";
        document.getElementById("Descripcion").value = "";
        document.getElementById("mensaje").innerHTML = "";
        document.getElementById("Estado").selectedIndex = 0;
        $('#modalScorpion').modal('hide');
        $('#ModalEstado').modal('hide');
        filtrarDatos(1);
    }
}
