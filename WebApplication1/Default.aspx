<%@ Page
    Title="Inicio"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Default.aspx.cs"
    Inherits="WebApplication1._Default" %>

<asp:Content
    ID="BodyContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="py-4">

        <div class="mb-4">
            <h1>Ferretería Andreu</h1>
            <p class="text-muted">
                Sistema de gestión de productos y stock.
            </p>
        </div>

        <div class="row g-4">

            <div class="col-md-6">
                <div class="card h-100">
                    <div class="card-body">
                        <h2 class="h4">Productos</h2>

                        <p class="text-muted">
                            Administrar productos, precios,
                            stock inicial y estado.
                        </p>

                        <a
                            runat="server"
                            href="~/Productos"
                            class="btn btn-primary">
                            Ir a Productos
                        </a>
                    </div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="card h-100">
                    <div class="card-body">
                        <h2 class="h4">Stock</h2>

                        <p class="text-muted">
                            Registrar movimientos y consultar
                            el historial de stock.
                        </p>

                        <a
                            runat="server"
                            href="~/MovimientosStock"
                            class="btn btn-outline-primary">
                            Ir a Stock
                        </a>
                    </div>
                </div>
            </div>

        </div>

    </div>

</asp:Content>