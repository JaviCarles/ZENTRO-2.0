<%@ Page Title="Catálogo - Ringo Clothes" Language="C#" MasterPageFile="~/Zentro.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Presentacion.ProductoHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row g-3 mb-2 align-items-end">
        <div class="col-12 col-md-6 col-lg-4">
            <asp:Panel runat="server" DefaultButton="btnBuscar">
                <div class="input-group shadow-sm">

                    <asp:TextBox ID="txtBusqueda" CssClass="form-control input-buscador-oscuro" placeholder="Buscar productos..." runat="server"></asp:TextBox>
                    <asp:Button ID="btnBuscar" CssClass="btn btn-outline-light" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                </div>
            </asp:Panel>
        </div>

        <div class="col-6 col-md-3 col-lg-4">
            <div class="form-group">
                <asp:Label ID="LabelOrden" CssClass="form-label small fw-bold text-black-50 mb-1" runat="server" Text="Ordenar por"></asp:Label>
                <asp:DropDownList ID="DropDownOrden" OnSelectedIndexChanged="DropDownOrden_SelectedIndexChanged" AutoPostBack="true" CssClass="form-select filtroApp" runat="server"></asp:DropDownList>
            </div>
        </div>

        <div class="col-6 col-md-3 col-lg-4">
            <div class="form-group">
                <asp:Label ID="LabelCategoria" CssClass="form-label small fw-bold text-black-50 mb-1" runat="server" Text="Categoría"></asp:Label>
                <asp:DropDownList ID="DropDownCategoria" OnSelectedIndexChanged="btnBuscar_Click" AutoPostBack="true" CssClass="form-select filtroApp" runat="server"></asp:DropDownList>
            </div>
        </div>
    </div>

    <div class="mb-3 border-bottom border-secondary border-opacity-25 pb-2">
        <asp:Label ID="lblCantidad" CssClass="fs-5 fw-semibold text-black-50" runat="server" Text="Productos disponibles"></asp:Label>
    </div>

    <div class="row g-3 g-md-4">
        <asp:Repeater ID="rptProductos" runat="server" OnItemCommand="rptProductos_ItemCommand">
    <ItemTemplate>
        <div class="col-6 col-md-4 col-lg-2 mb-2">
            <div class="card h-100 border-0 producto-card">

                <div class="img-contenedor-catalogo">
                    <img src='<%# Eval("ImagenUrl") %>' alt='<%# Eval("Nombre") %>' onerror="this.onerror=null; this.src='Content/Imagenes/Imagen no disponible.png';" />

                    <%-- Botón favorito corregido contra NullReferenceException usando la sesión directa --%>
                    <asp:LinkButton ID="btnFavorito" runat="server" 
                        CssClass="btn-favorito-flotante position-absolute top-0 end-0 m-2 shadow-sm" 
                        CommandName="Favorito" 
                        CommandArgument='<%# Eval("Id") %>'
                        Visible='<%# Session["usuario"] != null %>'>
                        <i class='<%# Session["usuario"] != null && Negocio.ProductoNegocio.esFavorito((int)Eval("Id"), ((Entidades.Usuario)Session["usuario"]).Id) ? "bi bi-heart-fill" : "bi bi-heart" %>'></i>
                    </asp:LinkButton>
                </div>

                <div class="card-body d-flex flex-column p-2 p-md-3">
                    
                    <%-- Línea de Marca y Código alineados perfectamente --%>
                    <div class="d-flex justify-content-between align-items-center mb-1">
                        <h5 class="marca-catalogo mb-0"><%# Eval("Marca.Descripcion") %></h5>
                        <span class="text-black-50" style="font-size: 0.65rem; font-weight: 500;">ART. <%# Eval("Codigo") %></span>
                    </div>

                    <h5 class="titulo-catalogo mb-2" title='<%# Eval("Nombre") %>'><%# Eval("Nombre") %></h5>
                    
                    <div class="d-flex justify-content-between align-items-center mb-3">
                        <p class="precio-catalogo mb-0">$ <%# Eval("Precio", "{0:N0}") %></p>
                        
                        <%-- Botón Compartir adaptado a la estética Ringo (Azul pastel en el style) --%>
                        <button type="button" 
                                class="btn btn-sm text-black-50 p-1 btn-compartir-dinamico border-0 bg-transparent"
                                data-nombre='<%# Eval("Nombre") %>' 
                                data-id='<%# Eval("Id") %>'
                                title="Compartir producto"
                                style="outline: none; box-shadow: none;">
                            <i class="bi bi-share-fill" style="color: #7da2ff; font-size: 1.1rem;"></i>
                        </button>
                    </div>

                    <div class="d-flex flex-column gap-2 mt-auto">
                        <a href='DetalleProducto.aspx?id=<%# Eval("Id") %>' class="btn btn-sm btn-outline-dark w-100">Ver detalles</a>
                        <%-- Link de WhatsApp optimizado para Ringo Clothes mandando el código automáticamente --%>
                        <a href='<%# "https://wa.me/5493446593767?text=" + HttpUtility.UrlEncode("Hola Ringo Clothes, me interesa este producto: " + Eval("Nombre").ToString() + " (Código: " + Eval("Codigo").ToString() + ")") %>' target="_blank" class="btn btn-sm btn-success w-100">
                            <i class="bi bi-whatsapp me-1"></i>Consultar
                        </a>
                    </div>
                </div>

            </div>
        </div>
    </ItemTemplate>
</asp:Repeater>
    </div>
    <script>document.addEventListener("DOMContentLoaded", function () {

            // Escuchamos los clics en todo el documento (es ideal por si usás UpdatePanels)
            document.addEventListener("click", function (event) {

                // Buscamos si el elemento clickeado es nuestro botón de compartir (o su ícono interno)
                const boton = event.target.closest(".btn-compartir-dinamico");

                if (boton) {
                    // Extraemos los datos que inyectó ASP.NET en el HTML plano
                    const nombreProducto = boton.getAttribute("data-nombre");
                    const idProducto = boton.getAttribute("data-id");
                    
                    // 🔥 CONSTRUIMOS LA URL APUNTANDO DIRECTO AL DETALLE DEL OBJETO
                    const urlDetalle = window.location.origin + '/DetalleProducto.aspx?id=' + idProducto;

                    const datosCompartir = {
                        title: nombreProducto + ' - Ringo Clothes',
                        text: '¡Mirá este producto en Ringo CLothes: ' + nombreProducto + '!',
                        url: urlDetalle
                    };

                    // Intentamos usar la API de compartir nativa (para celulares)
                    if (navigator.share) {
                        navigator.share(datosCompartir)
                            .catch((error) => console.log('Error al compartir:', error));
                    }
                    // Plan B para computadoras: Copiar link directo al portapapeles
                    else {
                        navigator.clipboard.writeText(datosCompartir.url)
                            .then(() => {
                                alert('¡Enlace de "' + nombreProducto + '" copiado al portapapeles!');
                            })
                            .catch((err) => {
                                console.error('No se pudo copiar el enlace:', err);
                            });
                    }
                }
            });
        });</script>
</asp:Content>


