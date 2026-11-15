<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Compras.aspx.cs" Inherits="WebApplication1.Compras" MasterPageFile="~/Site.Master" %>

<asp:Content
    ID="BodyContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="app-card">

    <h2 class="section-title">Registrar compra</h2>

        <div class="form-group">
            <asp:Label
                runat="server"
                Text="Proveedor"
                AssociatedControlID="ddlProveedor" />

            <asp:DropDownList
                ID="ddlProveedor"
                runat="server"
                CssClass="form-select form-control-app">
            </asp:DropDownList>
        </div>

        <div class="form-group">
            <asp:Label
                runat="server"
                Text="Producto"
                AssociatedControlID="ddlProducto" />

            <asp:DropDownList
                ID="ddlProducto"
                runat="server"
                CssClass="form-select form-control-app">
            </asp:DropDownList>
        </div>
    
        <div class="form-group">
             <asp:Label
                runat="server"
                text="Cantidad"
                AssociatedControlID="txtCantidad"/>

            <asp:TextBox 
                ID="txtCantidad" 
                runat="server" 
                CssClass="form-control form-control-app"/>
        </div>

        <div class="form-group">
             <asp:Label
                runat="server"
                text="Precio Unitario"
                AssociatedControlID="txtPrecioUnitario"/>

            <asp:TextBox 
                ID="txtPrecioUnitario" 
                runat="server" 
                CssClass="form-control form-control-app"/>
        </div>

        <div class="form-group">
             <asp:Label
                runat="server"
                text="Observacion"
                AssociatedControlID="txtObservacion"/>

            <asp:TextBox 
                ID="txtObservacion" 
                runat="server" 
                CssClass="form-control form-control-app"/>
        </div>
        <asp:Button
            ID="btnRegistrarCompra"
            runat="server"
            Text="Registrar compra"
            CssClass="btn btn-primary"
            OnClick="btnRegistrarCompra_Click" />

        <div class="mt-3">
            <asp:Label
                ID="lblMensaje"
                runat="server"
                CssClass="message mt-3">
            </asp:Label>
        </div>
        
</div>
<div class="app-card">

    <h2 class="section-title">Historial de compras</h2>

    <div class="table-responsive">
        <asp:GridView
            ID="gvHistorialCompras"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="table table-striped table-hover">

            <Columns>
                <asp:BoundField DataField="CompraId" HeaderText="Compra" />
                <asp:BoundField DataField="Fecha" HeaderText="Fecha"
                    DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                <asp:BoundField DataField="RazonSocial" HeaderText="Proveedor" />
                <asp:BoundField DataField="CodigoProducto" HeaderText="Código" />
                <asp:BoundField DataField="Producto" HeaderText="Producto" />
                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                <asp:BoundField DataField="PrecioUnitario" HeaderText="Precio unitario"
                    DataFormatString="{0:N2}" />
                <asp:BoundField DataField="SubTotal" HeaderText="Subtotal"
                    DataFormatString="{0:N2}" />
                <asp:BoundField DataField="Total" HeaderText="Total"
                    DataFormatString="{0:N2}" />
            </Columns>

        </asp:GridView>
    </div>

</div>
</asp:Content>
