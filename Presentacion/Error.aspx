<%@ Page Title="Hubo un problema - Ringo Clothes" Language="C#" MasterPageFile="~/Zentro.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="Presentacion.Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container my-5 text-black-50" style="max-width: 600px;">
        
        <div class="text-center p-5 rounded shadow" style="background-color: rgba(43, 30, 22, 0.6); border: 1px solid rgba(243, 235, 224, 0.15);">
            
            <div class="mb-4">
                <i class="bi bi-exclamation-triangle-fill d-block mb-2" style="font-size: 3rem; color: #cca97c;"></i>
                <h3 class="text-uppercase tracking-wider fw-bold" style="color: #cca97c;">
                    Hubo un inconveniente
                </h3>
                <hr style="border-color: rgba(243, 235, 224, 0.15); width: 60%; margin: 1rem auto;" />
            </div>

            <div class="mb-5 px-3">
                <p class="text-black-50 small mb-2 text-uppercase fw-bold tracking-wide">Detalle del error:</p>
                <asp:Label ID="lblError" runat="server" CssClass="fs-6 fw-semibold text-dark d-inline-block p-3 rounded w-100" style="background-color: rgba(0, 0, 0, 0.2); border-left: 4px solid #cca97c; word-break: break-word;" />
            </div>

            <div class="mt-4">
                <asp:Button ID="btnVolver" runat="server" Text="VOLVER AL INICIO" CssClass="btn btn-outline-dark px-4 rounded-pill fw-bold shadow-sm" OnClick="btnVolver_Click" />
            </div>

        </div>
    </div>
</asp:Content>