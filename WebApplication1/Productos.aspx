<%@ Page
    Title="Productos"
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="Productos.aspx.cs"
    Inherits="WebApplication1.Productos"
    MasterPageFile="~/Site.Master" %>

<asp:Content
    ID="BodyContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div>
        <div class="page-header">
            <h1>Productos</h1>
            <p>
                Administración del catálogo, precios y stock inicial.
            </p>
        </div>

        <div class="app-card">
            <h2 class="section-title">Nuevo producto</h2>

            <div class="form-group">
                <asp:Label
                    Text="Nombre"
                    AssociatedControlID="txtNombre"
                    runat="server" />

                <asp:TextBox
                    ID="txtNombre"
                    runat="server"
                    CssClass="form-control form-control-app" />
            </div>

            <div class="form-group">
                <asp:Label
                    Text="Código"
                    AssociatedControlID="txtCodigo"
                    runat="server" />

                <asp:TextBox
                    ID="txtCodigo"
                    runat="server"
                    CssClass="form-control form-control-app" />
            </div>

            <div class="form-group">
                <asp:Label
                    Text="Precio"
                    AssociatedControlID="txtPrecio"
                    runat="server" />

                <asp:TextBox
                    ID="txtPrecio"
                    runat="server"
                    CssClass="form-control form-control-app" />
            </div>

            <div class="form-group">
                <asp:Label
                    Text="Stock inicial"
                    AssociatedControlID="txtStock"
                    runat="server" />

                <asp:TextBox
                    ID="txtStock"
                    runat="server"
                    CssClass="form-control form-control-app" />
            </div>

            <asp:Button
                ID="btnAgregar"
                Text="Agregar producto"
                runat="server"
                CssClass="btn btn-primary"
                OnClick="btnAgregar_Click" />

            <asp:Label
                ID="lblMensaje"
                runat="server"
                CssClass="message" />
        </div>
        <div class="app-card">
            <h2 class="section-title">Productos disponibles</h2>

            <div class="table-responsive">
                <asp:GridView
                    ID="gvProductos"
                    runat="server"
                    AutoGenerateColumns="false"
                    DataKeyNames="Id"
                    CssClass="table table-striped table-hover"
                    OnRowEditing="gvProductos_RowEditing"
                    OnRowUpdating="gvProductos_RowUpdating"
                    OnRowCancelingEdit="gvProductos_RowCancelingEdit"
                    OnRowCommand="gvProductos_RowCommand">

                    <Columns>
                        <asp:BoundField
                            DataField="Id"
                            HeaderText="Id"
                            ReadOnly="true" />

                        <asp:BoundField
                            DataField="Codigo"
                            HeaderText="Código"
                            ReadOnly="true" />

                        <asp:BoundField
                            DataField="Nombre"
                            HeaderText="Nombre" />

                        <asp:BoundField
                            DataField="Precio"
                            HeaderText="Precio" />

                        <asp:BoundField
                            DataField="Stock"
                            HeaderText="Stock"
                            ReadOnly="true" />

                        <asp:CommandField
                            ShowEditButton="true"
                            EditText="Editar" />

                        <asp:ButtonField
                            Text="Desactivar"
                            CommandName="Desactivar" />
                    </Columns>

                </asp:GridView>
            </div>
        </div>

        <div class="app-card">
            <h2 class="section-title">Productos inactivos</h2>

            <div class="table-responsive">
                <asp:GridView
                    ID="gvProductosInactivos"
                    runat="server"
                    AutoGenerateColumns="false"
                    DataKeyNames="Id"
                    CssClass="table table-striped table-hover"
                    OnRowCommand="gvProductosInactivos_RowCommand">

                    <Columns>
                        <asp:BoundField
                            DataField="Id"
                            HeaderText="Id" />

                        <asp:BoundField
                            DataField="Codigo"
                            HeaderText="Código" />

                        <asp:BoundField
                            DataField="Nombre"
                            HeaderText="Nombre" />

                        <asp:BoundField
                            DataField="Precio"
                            HeaderText="Precio" />

                        <asp:BoundField
                            DataField="Stock"
                            HeaderText="Stock"
                            ReadOnly="true" />

                        <asp:ButtonField
                            Text="Reactivar"
                            CommandName="Reactivar" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

</asp:Content>
