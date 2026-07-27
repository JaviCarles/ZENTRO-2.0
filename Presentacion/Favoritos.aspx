<%--<%@ Page Title="Mis Favoritos - Ringo Clothes" Language="C#" MasterPageFile="~/Zentro.Master" AutoEventWireup="true" CodeBehind="Favoritos.aspx.cs" Inherits="Presentacion.Favoritos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container my-4">
        
        <% if (usuario != null) { %>

            <div class="text-center mb-4">
                <h2 class="titulo-admin text-uppercase" style="color: #cca97c; letter-spacing: 2px;"><i class="bi bi-heart-fill text-danger me-2"></i>Mis Favoritos</h2>
                <p class="text-white-50 small">Tus piezas artesanales seleccionadas y listas para encargar</p>
            </div>

            <div class="row g-3 mb-4 justify-content-between align-items-center p-3 rounded" style="background-color: rgba(43, 30, 22, 0.4); border: 1px solid rgba(243, 235, 224, 0.1);">
                
                <div class="col-12 col-md-6 col-lg-5">
                    <asp:Panel runat="server" DefaultButton="btnBuscar">
                        <div class="input-group">
                            <asp:TextBox ID="txtBusqueda" CssClass="form-control" placeholder="Buscar entre tus favoritos..." runat="server"></asp:TextBox>
                            <asp:LinkButton ID="btnBuscar" CssClass="btn btn-pampa-primary" runat="server" OnClick="btnBuscar_Click">
                                <i class="bi bi-search"></i>
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>

                <div class="col-12 col-md-5 col-lg-4">
                    <div class="d-flex align-items-center">
                        <asp:Label ID="LabelOrden" class="text-black-50 small text-nowrap me-2 mb-0" runat="server" Text="Ordenar por:"></asp:Label>
                        <asp:DropDownList ID="DropDownOrden" OnSelectedIndexChanged="btnBuscar_Click" AutoPostBack="true" CssClass="form-select" runat="server"></asp:DropDownList>
                    </div>
                </div>
            </div>

            <div class="row row-cols-1 row-cols-sm-2 row-cols-md-3 row-cols-lg-4 row-cols-xl-5 g-4">
                <asp:Repeater ID="rptProductos" runat="server" OnItemCommand="rptProductos_ItemCommand">
                    <ItemTemplate>
                        <div class="col animate__animated animate__fadeIn">
                            <div class="card card-pampa h-100 position-relative overflow-hidden">
                                
                                <asp:LinkButton ID="btnFavorito" runat="server"
                                    CssClass="btn-quitar-fav"
                                    CommandName="Favorito"
                                    CommandArgument='<%# Eval("Id") %>'
                                    title="Quitar de favoritos">
                                    <i class="bi bi-heartbreak-fill"></i>
                                </asp:LinkButton>

                                <div class="pampa-img-container bg-black d-flex align-items-center justify-content-center">
                                    <img src='<%# Eval("ImagenUrl") %>' 
                                         class="card-img-top p-2" 
                                         alt='<%# Eval("Nombre") %>'
                                         onerror="this.onerror=null; this.src='Content/Imagenes/Imagen no disponible.png';" />
                                </div>

                                <div class="card-body d-flex flex-column text-center p-3">
                                    <span class="text-uppercase tracking-wider text-warning small mb-1" style="font-size: 0.75rem; color: #cca97c !important;">
                                        <%# Eval("Marca.Descripcion") %>
                                    </span>
                                    <h5 class="card-title text-black-50 fs-6 text-truncate mb-2" title='<%# Eval("Nombre") %>'>
                                        <%# Eval("Nombre") %>
                                    </h5>
                                    <p class="fs-5 fw-bold mb-3" style="color: #cca97c;">
                                        $ <%# Eval("Precio", "{0:N0}") %>
                                    </p>
                                    
                                    <div class="mt-auto d-grid gap-2">
                                        <a href='DetalleProducto.aspx?id=<%# Eval("Id") %>' class="btn btn-sm btn-outline-dark rounded-pill">
                                            Ver Detalles
                                        </a>
                                        <a href='https://wa.me/5493446400000?text=Hola!%20Me%20interesa%20el%20producto:%20<%# Server.UrlEncode(Eval("Nombre").ToString()) %>' 
                                           target="_blank" 
                                           class="btn btn-sm btn-success rounded-pill">
                                            <i class="bi bi-whatsapp me-1"></i>Comprar
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <% if (lista == null || lista.Count == 0) { %>
                <div class="row my-5 py-5">
                    <div class="col-12 text-center text-black-50">
                        <i class="bi bi-heart-prohibited display-1 text-muted mb-3"></i>
                        <h3 class="fw-light">No tenés productos guardados</h3>
                        <p class="small">Explorá nuestro catálogo y dale click al corazón para guardarlos acá.</p>
                        <a href="Default.aspx" class="btn btn-pampa-primary mt-2 rounded-pill px-4">Ver Catálogo</a>
                    </div>
                </div>
            <% } %>

        <% } else { %>
            
            <div class="row my-5 py-5">
                <div class="col-12 text-center text-white-50">
                    <i class="bi bi-person-fill-lock display-1 text-warning mb-3"></i>
                    <h2>Iniciá Sesión</h2>
                    <p class="lead">Debes ingresar a tu cuenta para poder gestionar tus productos favoritos.</p>
                    <a href="Login.aspx" class="btn btn-outline-light mt-3 rounded-pill px-4">Ir al Login</a>
                </div>
            </div>

        <% } %>

    </div>
</asp:Content>--%>

<%@ Page Title="Mis Favoritos - Ringo Clothes" Language="C#" MasterPageFile="~/Zentro.Master" AutoEventWireup="true" CodeBehind="Favoritos.aspx.cs" Inherits="Presentacion.Favoritos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container my-4">
        
        <% if (usuario != null) { %>

            <%-- Encabezado Estilo Ringo Clothes --%>
            <div class="text-center mb-4">
                <h2 class="titulo-admin text-uppercase tipografiaTitulo fs-3" style="color: #2c3e50; letter-spacing: 2px;">
                    <i class="bi bi-heart-fill text-danger me-2"></i>Mis Favoritos
                </h2>
                <p class="text-muted small subtitlo">Tus prendas seleccionadas listas para consultar o encargar</p>
            </div>

            <%-- Barra de Filtros Limpia y Moderna (Fondo claro/traslúcido) --%>
            <div class="row g-3 mb-4 justify-content-between align-items-center p-3 rounded shadow-sm" style="background-color: rgba(255, 255, 255, 0.8); border: 1px solid rgba(125, 162, 255, 0.2); backdrop-filter: blur(8px);">
                
                <div class="col-12 col-md-6 col-lg-5">
                    <asp:Panel runat="server" DefaultButton="btnBuscar">
                        <div class="input-group">
                            <asp:TextBox ID="txtBusqueda" CssClass="form-control" placeholder="Buscar entre tus favoritos..." runat="server"></asp:TextBox>
                            <%-- Botón de búsqueda adaptado al azul Ringo --%>
                            <asp:LinkButton ID="btnBuscar" CssClass="btn btn-outline-dark" style="border-color: #ced4da;" runat="server" OnClick="btnBuscar_Click">
                                <i class="bi bi-search"></i>
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>

                <div class="col-12 col-md-5 col-lg-4">
                    <div class="d-flex align-items-center">
                        <asp:Label ID="LabelOrden" class="text-muted small text-nowrap me-2 mb-0" runat="server" Text="Ordenar por:"></asp:Label>
                        <asp:DropDownList ID="DropDownOrden" OnSelectedIndexChanged="btnBuscar_Click" AutoPostBack="true" CssClass="form-select" runat="server"></asp:DropDownList>
                    </div>
                </div>
            </div>

            <%-- Grid de Tarjetas (Idéntico al Catálogo Principal de Ringo) --%>
            <div class="row row-cols-2 row-cols-sm-2 row-cols-md-3 row-cols-lg-4 row-cols-xl-5 g-3">
                <asp:Repeater ID="rptProductos" runat="server" OnItemCommand="rptProductos_ItemCommand">
                    <ItemTemplate>
                        <div class="col animate__animated animate__fadeIn">
                            <div class="card h-100 border-0 producto-card">
                                
                                <%-- Botón flotante para Desmarcar/Quitar de Favoritos --%>
                                <asp:LinkButton ID="btnFavorito" runat="server"
                                    CssClass="btn-favorito-flotante position-absolute top-0 end-0 m-2 shadow-sm"
                                    CommandName="Favorito"
                                    CommandArgument='<%# Eval("Id") %>'
                                    title="Quitar de favoritos">
                                    <i class="bi bi-heartbreak-fill text-danger"></i>
                                </asp:LinkButton>

                                <%-- Contenedor de Imagen Limpio (Fondo blanco) --%>
                                <div class="img-contenedor-catalogo">
                                    <img src='<%# Eval("ImagenUrl") %>' 
                                         alt='<%# Eval("Nombre") %>'
                                         onerror="this.onerror=null; this.src='Content/Imagenes/Imagen no disponible.png';" />
                                </div>

                                <div class="card-body d-flex flex-column p-2 p-md-3">
                                    
                                    <%-- Línea de Marca y Código Orgánico Alineados --%>
                                    <div class="d-flex justify-content-between align-items-center mb-1">
                                        <h5 class="marca-catalogo mb-0"><%# Eval("Marca.Descripcion") %></h5>
                                        <span class="codigo-catalogo text-muted" style="font-size: 0.65rem; font-weight: 500;">ART. <%# Eval("Codigo") %></span>
                                    </div>

                                    <h5 class="titulo-catalogo mb-2" title='<%# Eval("Nombre") %>'><%# Eval("Nombre") %></h5>
                                    
                                    <%-- Precio y Compartir Dinámico --%>
                                    <div class="d-flex justify-content-between align-items-center mb-3">
                                        <p class="precio-catalogo mb-0">$ <%# Eval("Precio", "{0:N0}") %></p>
                                        
                                        <button type="button" 
                                                class="btn btn-sm text-black-50 p-1 btn-compartir-dinamico border-0 bg-transparent"
                                                data-nombre='<%# Eval("Nombre") %>' 
                                                data-id='<%# Eval("Id") %>'
                                                title="Compartir producto"
                                                style="outline: none; box-shadow: none;">
                                            <i class="bi bi-share-fill" style="color: #7da2ff; font-size: 1.1rem;"></i>
                                        </button>
                                    </div>
                                    
                                    <%-- Botonera: Contorno Negro y Botón WhatsApp con Código --%>
                                    <div class="d-flex flex-column gap-2 mt-auto">
                                        <a href='DetalleProducto.aspx?id=<%# Eval("Id") %>' class="btn btn-sm btn-outline-dark w-100">Ver detalles</a>
                                        
                                        <a href='<%# "https://wa.me/5493446593767?text=" + HttpUtility.UrlEncode("Hola Ringo Clothes, me interesa este producto que tengo en mis favoritos: " + Eval("Nombre").ToString() + " (Código: " + Eval("Codigo").ToString() + ")") %>' target="_blank" class="btn btn-sm btn-success w-100">
                                            <i class="bi bi-whatsapp me-1"></i>Consultar
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <%-- Estado Vacío de Favoritos Estilizado --%>
            <% if (lista == null || lista.Count == 0) { %>
                <div class="row my-5 py-5">
                    <div class="col-12 text-center text-muted">
                        <i class="bi bi-heart-prohibited display-1 text-muted mb-3" style="opacity: 0.4;"></i>
                        <h3 class="fw-light text-dark">No tenés productos guardados</h3>
                        <p class="small text-secondary">Explorá nuestra nueva colección y dale click al corazón para guardarlos acá.</p>
                        <a href="Default.aspx" class="btn btn-outline-dark mt-2 px-4">Ver Catálogo</a>
                    </div>
                </div>
            <% } %>

        <% } else { %>
            
            <%-- Estado de Sesión No Iniciada Estilizado --%>
            <div class="row my-5 py-5">
                <div class="col-12 text-center text-muted">
                    <i class="bi bi-person-fill-lock display-1 mb-3" style="color: #7da2ff;"></i>
                    <h2 class="text-dark">Iniciá Sesión</h2>
                    <p class="lead text-secondary">Debes ingresar a tu cuenta para poder gestionar tus productos favoritos.</p>
                    <a href="Login.aspx" class="btn btn-outline-dark mt-3 px-4">Ir al Login</a>
                </div>
            </div>

        <% } %>

    </div>
</asp:Content>