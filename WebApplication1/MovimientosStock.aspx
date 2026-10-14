<%@ Page
    Title="Stock"
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="MovimientosStock.aspx.cs"
    Inherits="WebApplication1.MovimientosStock"
    MasterPageFile="~/Site.Master" %>

<asp:Content
    ID="BodyContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div>

        <div class="page-header">
            <h1>Stock</h1>
            <p>
                Registro y consulta de movimientos de stock.
            </p>
        </div>

        <div class="app-card">
            <h2 class="section-title">Registrar movimiento</h2>

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
                    Text="Tipo de movimiento"
                    AssociatedControlID="ddlTipoMovimiento" />

                <asp:DropDownList
                    ID="ddlTipoMovimiento"
                    runat="server"
                    CssClass="form-select form-control-app">
                </asp:DropDownList>
            </div>

            <div class="form-group">
                <asp:Label
                    runat="server"
                    Text="Motivo"
                    AssociatedControlID="ddlMotivo" />

                <asp:DropDownList
                    ID="ddlMotivo"
                    runat="server"
                    CssClass="form-select form-control-app">
                </asp:DropDownList>
            </div>

            <div class="form-group">
                <asp:Label
                    runat="server"
                    Text="Cantidad"
                    AssociatedControlID="txtCantidad" />

                <asp:TextBox
                    ID="txtCantidad"
                    runat="server"
                    CssClass="form-control form-control-app">
                </asp:TextBox>
            </div>

            <div class="form-group">
                <asp:Label
                    runat="server"
                    Text="Observación"
                    AssociatedControlID="txtObservacion" />

                <asp:TextBox
                    ID="txtObservacion"
                    runat="server"
                    CssClass="form-control form-control-app">
                </asp:TextBox>
            </div>

            <asp:Button
                ID="btnRegistrar"
                runat="server"
                Text="Registrar movimiento"
                CssClass="btn btn-primary"
                OnClick="btnRegistrar_Click" />

            <asp:Label
                ID="lblMensaje"
                runat="server"
                CssClass="message">
            </asp:Label>
        </div>

        <div class="app-card">
            <h2 class="section-title">Historial de movimientos</h2>

            <div class="table-responsive">
                <asp:GridView
                    ID="gvHistorial"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table table-striped table-hover">

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
        </div>

    </div>

</asp:Content>