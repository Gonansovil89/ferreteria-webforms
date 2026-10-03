<%@ Page Title="Stock" Language="C#" AutoEventWireup="true" CodeBehind="MovimientosStock.aspx.cs" Inherits="WebApplication1.MovimientosStock" MasterPageFile="~/Site.Master" %>

<asp:Content
    ID="BodyContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div>

        <h2>Movimiento de stock</h2>

        <asp:Label
            runat="server"
            Text="Producto" />

        <asp:DropDownList
            ID="ddlProducto"
            runat="server">
        </asp:DropDownList>

        <br /><br />

        <asp:Label
            runat="server"
            Text="Tipo de movimiento" />

        <asp:DropDownList
            ID="ddlTipoMovimiento"
            runat="server">
        </asp:DropDownList>

        <br /><br />

        <asp:Label
            runat="server"
            Text="Motivo" />

        <asp:DropDownList
            ID="ddlMotivo"
            runat="server">
        </asp:DropDownList>

        <br /><br />

        <asp:Label
            runat="server"
            Text="Cantidad" />

        <asp:TextBox
            ID="txtCantidad"
            runat="server">
        </asp:TextBox>

        <br /><br />

        <asp:Label
            runat="server"
            Text="Observación" />

        <asp:TextBox
            ID="txtObservacion"
            runat="server">
        </asp:TextBox>

        <br /><br />

        <asp:Button
            ID="btnRegistrar"
            runat="server"
            Text="Registrar" 
            OnClick="btnRegistrar_Click" />

        <br /><br />

        <asp:Label
            ID="lblMensaje"
            runat="server">
        </asp:Label>

    </div>

</asp:Content>
