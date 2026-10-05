<%@ Page Title="Stock" Language="C#" AutoEventWireup="true" CodeBehind="MovimientosStock.aspx.cs" Inherits="WebApplication1.MovimientosStock" MasterPageFile="~/Site.Master" %>

<asp:Content
    ID="BodyContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div>

        <h3>Movimiento de stock</h3>
        <hr />
        <asp:Label
            runat="server"
            Text="Producto" />

        <asp:DropDownList
            ID="ddlProducto"
            runat="server">
        </asp:DropDownList>

        <br />
        <br />

        <asp:Label
            runat="server"
            Text="Tipo de movimiento" />

        <asp:DropDownList
            ID="ddlTipoMovimiento"
            runat="server">
        </asp:DropDownList>

        <br />
        <br />

        <asp:Label
            runat="server"
            Text="Motivo" />

        <asp:DropDownList
            ID="ddlMotivo"
            runat="server">
        </asp:DropDownList>

        <br />
        <br />

        <asp:Label
            runat="server"
            Text="Cantidad" />

        <asp:TextBox
            ID="txtCantidad"
            runat="server">
        </asp:TextBox>

        <br />
        <br />

        <asp:Label
            runat="server"
            Text="Observación" />

        <asp:TextBox
            ID="txtObservacion"
            runat="server">
        </asp:TextBox>

        <br />
        <br />

        <asp:Button
            ID="btnRegistrar"
            runat="server"
            Text="Registrar"
            OnClick="btnRegistrar_Click" />

        <br />
        <br />

        <asp:Label
            ID="lblMensaje"
            runat="server">
        </asp:Label>
        <hr />

        <h3>Historial de movimientos</h3>

        <asp:GridView
            ID="gvHistorial"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="table">

            <Columns>
                <asp:BoundField
                    DataField="Fecha"
                    HeaderText="Fecha"
                    DataFormatString="{0:dd/MM/yyyy HH:mm}" />

                <asp:BoundField
                    DataField="ProductoCodigo"
                    HeaderText="Código" />

                <asp:BoundField
                    DataField="ProductoNombre"
                    HeaderText="Producto" />

                <asp:BoundField
                    DataField="TipoMovimiento"
                    HeaderText="Tipo" />

                <asp:BoundField
                    DataField="MotivoMovimiento"
                    HeaderText="Motivo" />

                <asp:BoundField
                    DataField="Cantidad"
                    HeaderText="Cantidad" />

                <asp:BoundField
                    DataField="StockAnterior"
                    HeaderText="Stock anterior" />

                <asp:BoundField
                    DataField="StockPosterior"
                    HeaderText="Stock posterior" />

                <asp:BoundField
                    DataField="Observacion"
                    HeaderText="Observación" />
            </Columns>

        </asp:GridView>
    </div>

</asp:Content>
