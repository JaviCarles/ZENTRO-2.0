<%@ Page Title="Detalle - Ringo Clothes" Language="C#" MasterPageFile="~/Zentro.Master" AutoEventWireup="true" CodeBehind="DetalleProducto.aspx.cs" Inherits="Presentacion.DetalleProducto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* 1. Controlamos el contenedor para que en PC mantenga un tamaño elegante */
        .img-detalle-cuadro {
            max-height: 500px; /* Evita que el cuadro sea más alto que el monitor */
            display: flex;
            align-items: center;
            justify-content: center;
            background-color: #ffffff; /* Fondo blanco limpio para la prenda */
            overflow: hidden;
        }

        /* 2. Forzamos a la imagen a contenerse ENTERA (del cuello a la cintura) */
        .img-detalle-max {
            max-height: 480px; /* Un pelín más chica que el cuadro para dejar aire */
            width: 100%;
            object-fit: contain !important; /* Achica la foto proporcionalmente hasta entrar entera */
            object-position: center !important; /* La centra para que no se corte arriba */
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <% if (producto != null)
        { %>
    <div class="container my-4 my-md-5">
        <div class="row g-4 g-lg-5">

            <div class="col-12 col-md-6 text-center">
                <div class="img-detalle-cuadro rounded shadow-lg p-2">
                    <img src="<%= producto.ImagenUrl %>" alt="<%= producto.Nombre %>" class="img-detalle-max rounded" onerror="this.onerror=null; this.src='Content/Imagenes/Imagen no disponible.png';">
                </div>
            </div>

            <div class="col-12 col-md-6 d-flex flex-column justify-content-center">
                <h1 class="titulo-detalle fw-bold mb-2"><%= producto.Nombre %></h1>
                <h4 class="precio-catalogo mb-4">$ <%= producto.Precio.ToString("N0") %></h4>

                <p class="descripcion-detalle mb-3">
                    <%= producto.Descripcion %>
                </p>

                <p class="mb-4 text-uppercase tracking-wider">
                    <span class="text-muted small">Marca:</span> <strong><%= producto.Marca.Descripcion %></strong>
                </p>

                <ul class="list-unstyled lista-beneficios-detalle mb-4">
                    <li class="mb-2"><i class="bi bi-check-circle-fill text-success me-2"></i><span>REGISTRATE Y OBTENÉ FUTUROS BENEFICIOS</span></li>
                    <li class="mb-2"><i class="bi bi-check-circle-fill text-success me-2"></i><span>FINALIZÁ TU COMPRA A TRAVÉS DE WHATSAPP</span></li>
                    <li class="mb-2"><i class="bi bi-check-circle-fill text-success me-2"></i><span>RESPONDEMOS TU CONSULTA AL INSTANTE</span></li>
                </ul>

                <div class="d-flex flex-column flex-sm-row flex-wrap gap-3 mt-2">

                    <%-- Link de WhatsApp optimizado para Ringo Clothes mandando código, nombre y URL de la página --%>
                    <a href='https://wa.me/5493446593767?text=<%= Server.UrlEncode("Hola Ringo Clothes, me interesa este producto: " + producto.Nombre + " (Código: " + producto.Codigo + "). Acá podés ver el producto: " + Request.Url.AbsoluteUri) %>'
                        target="_blank"
                        class="btn btn-success btn-lg btn-whatsapp-pro px-4 d-inline-flex align-items-center justify-content-center flex-grow-1 flex-sm-grow-0">
                        <i class="bi bi-whatsapp me-2"></i>Consultar</a>

                    <button type="button"
                        class="btn btn-outline-dark btn-lg px-4 d-inline-flex align-items-center justify-content-center flex-grow-1 flex-sm-grow-0"
                        onclick="compartirPagina()">
                        <i class="bi bi-share-fill me-2" style="color: #cca97c;"></i>Compartir
                   
                    </button>

                </div>
            </div>

        </div>
    </div>
    <% } %>
</asp:Content>
