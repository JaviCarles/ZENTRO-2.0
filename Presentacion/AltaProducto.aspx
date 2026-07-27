<%@ Page Title="Alta de Producto - Ringo Clothes" Language="C#" MasterPageFile="~/Zentro.Master" AutoEventWireup="true" CodeBehind="AltaProducto.aspx.cs" Inherits="Presentacion.AltaProducto" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <% var usuario = Session["usuario"] as Entidades.Usuario;
       if (usuario != null && usuario.Admin) { %>
        
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <div class="container my-4 text-black">
            <div class="mb-4">
                <h3 class="text-uppercase tracking-wider" style="color: #cca97c;">
                    <i class="bi bi-box-seam me-2"></i><asp:Label ID="lblTitulo" runat="server">AGREGAR PRODUCTO</asp:Label>
                </h3>
                <hr style="border-color: rgba(243, 235, 224, 0.15);" />
            </div>

            <div class="row g-4 p-4 rounded" style="background-color: rgba(43, 30, 22, 0.4); border: 1px solid rgba(243, 235, 224, 0.1);">
                
                <div class="col-12 col-md-6">
                    <div class="mb-3" style="display: none;">
                        <label for="txtId" class="form-label text-black-50 small">Id</label>
                        <asp:TextBox TextMode="Number" ID="txtId" CssClass="form-control" runat="server" />
                    </div>
                    <div class="mb-3">
                        <label for="txtCodigo" class="form-label text-black-50 small fw-bold">CÓDIGO</label>
                        <asp:TextBox ID="txtCodigo" CssClass="form-control text-uppercase" placeholder="Ej: ART-102" runat="server" MaxLength="50" />
                    </div>
                    <div class="mb-3">
                        <label for="txtNombre" class="form-label text-black-50 small fw-bold">NOMBRE DEL PRODUCTO</label>
                        <asp:TextBox ID="txtNombre" CssClass="form-control" placeholder="Ej: Buzo canguro, campera, etc.." runat="server" MaxLength="50" />
                    </div>
                    <div class="mb-3">
                        <label for="textAreaDescripcion" class="form-label text-black-50 small fw-bold">DESCRIPCIÓN / DETALLES</label>
                        <asp:TextBox class="form-control" TextMode="MultiLine" ID="textAreaDescripcion" Rows="3" placeholder="Especificaciones del producto..." runat="server" MaxLength="149" />
                    </div>
                    <div class="row">
                        <div class="col-6 mb-3">
                            <label for="DropDownListMarca" class="form-label text-black-50 small fw-bold">MARCA</label>
                            <asp:DropDownList ID="DropDownListMarca" CssClass="form-select" runat="server"></asp:DropDownList>
                        </div>
                        <div class="col-6 mb-3">
                            <label for="DropDownListCategoria" class="form-label text-black-50 small fw-bold">CATEGORÍA</label>
                            <asp:DropDownList ID="DropDownListCategoria" CssClass="form-select" runat="server"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="mb-3">
                        <label for="txtPrecio" class="form-label text-black-50 small fw-bold">PRECIO ($)</label>
                        <asp:TextBox TextMode="Number" ID="txtPrecio" CssClass="form-control" placeholder="0" runat="server" min="0" step="1" />
                    </div>
                </div>

                <div class="col-12 col-md-6 d-flex flex-column justify-content-between">
                    <asp:UpdatePanel ID="updatePanelImagen" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="mb-3">
                                <label for="txtImagen" class="form-label text-black-50 small fw-bold">LINK DE IMAGEN (URL MANUAL)</label>
                                <asp:TextBox ID="txtImagen" CssClass="form-control" placeholder="https://ejemplo.com/imagen.jpg" runat="server" onchange="previewImage(this)" />
                            </div>
                            
                            <div class="mb-3 text-center p-3 rounded" style="background-color: rgba(0,0,0,0.2); border: 1px dashed rgba(243, 235, 224, 0.2);">
                                <label class="form-label d-block text-black-50 small fw-bold text-start">VISTA PREVIA</label>
                                <asp:Image ID="imgProducto"
                                    ImageUrl="Content/Imagenes/Imagen no disponible.png"
                                    runat="server" CssClass="img-fluid rounded shadow-sm m-2" style="max-height: 200px; object-fit: contain;" />
                            </div>

                            <div class="mb-3">
                                <asp:label id="lblCargarImagen" runat="server" for="CargarImagen" class="form-label text-black-50 small fw-bold">O SUBIR ARCHIVO LOCAL A LA NUBE</asp:label>
                                <asp:FileUpload ID="CargarImagen" runat="server" onchange="previewImage(this)" CssClass="form-control" />
                            </div>
                            <%-- JavaScript nativo para previsualización inmediata --%>
                            <script type="text/javascript">
                                function previewImage(input) {
                                    var imgElement = document.getElementById('<%= imgProducto.ClientID %>');
                                    if (!imgElement) return;

                                    if (input.type === 'text') {
                                        if (input.value.trim() !== "") {
                                            imgElement.src = input.value.trim();
                                        }
                                    }
                                    else if (input.files && input.files[0]) {
                                        var reader = new FileReader();
                                        reader.onload = function (e) {
                                            imgElement.src = e.target.result;
                                        };
                                        reader.readAsDataURL(input.files[0]);
                                    }
                                }
                            </script>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="row mt-4 align-items-center bg-dark bg-opacity-20 p-3 rounded" style="border: 1px solid rgba(243, 235, 224, 0.05);">
                <div class="col-12 col-md-8 d-flex flex-wrap align-items-center gap-2">
                    <asp:Button Text="ACEPTAR" CssClass="btn btn-success px-4 rounded-pill fw-bold" ID="btnAceptar" OnClick="btnAceptar_Click" runat="server" />
                    <asp:Button Text="CANCELAR" CssClass="btn btn-outline-light px-4 rounded-pill" ID="btnCancelar" OnClick="btnCancelar_Click" runat="server" />
                    
                    <asp:Label ID="lblError" runat="server" CssClass="ms-2 small fw-bold d-block mt-1"></asp:Label>
                </div>
                
                <div class="col-12 col-md-4 text-md-end mt-3 mt-md-0">
                    <asp:UpdatePanel ID="UPBtnEliminar" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnEliminar" runat="server" Text="Eliminar Producto" CssClass="btn btn-outline-danger px-3 rounded-pill btn-sm"
                                OnClientClick="return confirm('¿Estás seguro de que deseas eliminar permanentemente este producto y su imagen de la nube?');"
                                OnClick="btnEliminar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

    <% } else { %>
         <div class="row my-5 py-5">
     <div class="col-12 text-center text-black-50">
         <i class="bi bi-shield-lock-fill text-danger display-1 mb-3"></i>
         <h2 class="fw-bold">Acceso Restringido</h2>
         <p class="lead">No tienes los permisos administrativos necesarios para ingresar a esta sección.</p>
         <a href="Default.aspx" class="btn btn-outline-light mt-3 rounded-pill px-4">Volver al Inicio</a>
     </div>
 </div>
    <% } %>
</asp:Content>