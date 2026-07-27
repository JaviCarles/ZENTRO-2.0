
//function togglePassword(id) {
//    var input = document.getElementById(id);
//    if (input.type === "password") {
//        input.type = "text";
//    } else {
//        input.type = "password";
//    }
//}

function compartirPagina() {
    if (navigator.share) {
        // Usa la API nativa del celular si está disponible
        navigator.share({
            title: document.title,
            url: window.location.href
        }).catch(console.error);
    } else {
        // Opción alternativa (ej: copiar al portapapeles)
        var aux = document.createElement("input");
        aux.setAttribute("value", window.location.href);
        document.body.appendChild(aux);
        aux.select();
        document.execCommand("copy");
        document.body.removeChild(aux);
        alert("¡Enlace copiado al portapapeles!");
    }
}