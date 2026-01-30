<%@ Page Title="" Language="C#" MasterPageFile="~/Zentro.Master" AutoEventWireup="true" CodeBehind="DetalleProducto.aspx.cs" Inherits="Presentacion.DetalleProducto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%if (producto != null)
        { %>
    <div class="container my-5">
        <div class="row">
            <!-- Imagen del producto -->
            <div class="col-md-6 text-center">
                <img src="<%= producto.ImagenUrl%>" class="img-fluid rounded shadow" alt="Producto">
            </div>

            <!-- Detalles del producto -->
            <div class="col-md-6">
                <h1 class="fw-bold mb-3"><%= producto.Nombre%></h1>
                <h4 class="text-success mb-4">$ <%= producto.Precio %></h4>
                <p class="text-muted">
                    <%= producto.Descripcion %>
                </p>
                <ul class="list-unstyled mb-4">
                    <%if (producto.Precio > 150000)
                        { %>
                    <li><i class="bi bi-check-circle text-success"></i>ENVÍO GRATIS</li>
                    <li><i class="bi bi-check-circle text-success"></i>HASTA 6 CUOTAS SIN INTÉRES CON TARJETAS BANCARIAS</li>
                    <li><i class="bi bi-check-circle text-success"></i>RESPONDEMOS TU CONSULTA AL INSTANTE</li>
                    <%} %>
                    <%else
                        { %><li><i class="bi bi-check-circle text-success"></i>ENVÍO GRATIS SI TU COMPRA SUPERA $ 150.000</li>
                    <li><i class="bi bi-check-circle text-success"></i>PROMOCIONES CON TARJETAS BANCARIAS</li>
                    <li><i class="bi bi-check-circle text-success"></i>RESPONDEMOS TU CONSULTA AL INSTANTE</li>
                    <%} %>
                </ul>

                <!-- Botón de acción -->
                <a href="https://wa.me/549XXXXXXXXXX"
                    class="btn btn-success btn-lg">
                    <i class="bi bi-whatsapp"></i> Contactar por WhatsApp
                </a>
            </div>
        </div>
    </div>
    <%} %>
</asp:Content>
