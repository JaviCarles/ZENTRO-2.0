<%@ Page Title="" Language="C#" MasterPageFile="~/Zentro.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="Presentacion.Error" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="mt-1 md-2">Error !!</h2>
    <asp:Label class="form-label fw-bold mt-2 me-3 mb-3" ID="lblError" runat="server" Text="Error"></asp:Label>
</asp:Content>
