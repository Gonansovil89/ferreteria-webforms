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
        <h2>Ingreso de productos</h2>
        <br />

        <asp:Label Text="Nombre" runat="server" />
        <asp:TextBox ID="txtNombre" runat="server" />
        <br /><br />

        <asp:Label Text="Código" runat="server" />
        <asp:TextBox ID="txtCodigo" runat="server" />
        <br /><br />

        <asp:Label Text="Precio" runat="server" />
        <asp:TextBox ID="txtPrecio" runat="server" />
        <br /><br />

        <asp:Label Text="Stock inicial" runat="server" />
        <asp:TextBox ID="txtStock" runat="server" />
        <br /><br />
        <asp:Label runat="server" ID="lblMensaje" />

        <h3>Productos disponibles</h3>

        <br /><br />
        <asp:GridView
            ID="gvProductos"
            runat="server"
            AutoGenerateColumns="false"
            DataKeyNames="Id"
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
                    HeaderText="Stock" />

                <asp:CommandField
                    ShowEditButton="true"
                    EditText="Editar" />
                <asp:ButtonField
                    Text="Desactivar"
                    CommandName="Desactivar" />
            </Columns>

        </asp:GridView>
        <br /><br />
        <asp:Button ID="btnAgregar" Text="Agregar" runat="server" OnClick="btnAgregar_Click" />
        <br /><br />

        <h3>Listado de productos Inactivos</h3>

        <br /><br />
        <asp:GridView
            ID="gvProductosInactivos"
            runat="server"
            AutoGenerateColumns="false"
            DataKeyNames="Id"
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
                    ReadOnly="true"/>

                <asp:ButtonField
                    Text="Reactivar"
                    CommandName="Reactivar" />
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
