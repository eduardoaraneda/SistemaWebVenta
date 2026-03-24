// ============================
// VARIABLES GLOBALES
// ============================
window.cambioActual = { cabecera: null, productos: [] };
let miModal = null;

// ============================
// INICIALIZACIÓN
// ============================
document.addEventListener('DOMContentLoaded', function () {

    // Inicializar modal cambio
    miModal = new bootstrap.Modal(document.getElementById('modalCambio'));

    inicializarEventosIndex();
    inicializarEventosCambio();
});

// ============================
// EVENTOS INDEX
// ============================
function inicializarEventosIndex() {

    // ---------- Validación Cliente ----------
    const rutInput = document.getElementById('Rut');
    if (rutInput) {
        rutInput.addEventListener('keydown', function (e) {
            if (e.key === 'Enter' || e.key === 'Tab') {
                const rut = this.value;
                const url = this.dataset.urlValidaCliente; // Agregar data-url-valida-cliente="@Url.Action("ValidaCliente","PuntoVenta")"
                fetch(`${url}?rut=${rut}`)
                    .then(r => r.json())
                    .then(response => {
                        if (response.success) {
                            document.getElementById('NombreCompleto').value = response.cliente.nombre;
                            document.getElementById('idCliente').value = response.cliente.codCliente;
                            if (response.cliente.registrado) {
                                document.getElementById('clienteId').value = response.cliente.rut_Cliente;
                                $('#Clientemodal').modal('show');
                            }
                        } else {
                            Swal.fire("Aviso", response.message, "warning");
                        }
                    });
            }
        });
    }

    // ---------- Validación Vendedor ----------
    const vendedorInput = document.getElementById('VendedorRut');
    if (vendedorInput) {
        vendedorInput.addEventListener('keydown', function (e) {
            if (e.key === 'Enter' || e.key === 'Tab') {
                const rut = this.value;
                const codTienda = document.getElementById('Cod_Tienda').value;
                const url = this.dataset.urlValidaVendedor; // data-url-valida-vendedor="@Url.Action("ValidaVendedor","PuntoVenta")"

                fetch(`${url}?rut=${rut}&cod_tienda=${codTienda}`)
                    .then(r => r.json())
                    .then(r => {
                        if (r.success) {
                            document.getElementById('NombreVendedor').value = r.usuario.nombre;
                        } else {
                            Swal.fire("Aviso", r.message, "warning");
                        }
                    });
            }
        });
    }

    // ---------- Validación Producto ----------
    const prodInput = document.getElementById('Producto');
    if (prodInput) {
        prodInput.addEventListener('keydown', function (e) {
            if (e.key === 'Enter' || e.key === 'Tab') {
                const producto = this.value;
                const codTienda = document.getElementById('Cod_Tienda').value;
                const url = this.dataset.urlValidaProducto; // data-url-valida-producto="@Url.Action("ValidaProducto","PuntoVenta")"

                fetch(`${url}?producto=${producto}&codtienda=${codTienda}`)
                    .then(r => r.json())
                    .then(r => {
                        if (r.success) {
                            const p = r.productoValido;
                            document.getElementById('DescProducto').value = p.descripcion;
                            document.getElementById('PrecioProd').value = p.precioVenta;
                            document.getElementById('CantProducto').value = 1;
                            document.getElementById('DescuProducto').value = p.descuento;
                            document.getElementById('CodProducto').value = p.codigo;
                            document.getElementById('CodBodega').value = p.codBodega;
                            document.getElementById('CantProducto').focus();
                        } else {
                            Swal.fire("Aviso", r.message, "warning");
                        }
                    });
            }
        });
    }

    // ---------- Botones agregar / limpiar ----------
    document.getElementById('btnAgregarProducto').addEventListener('click', agregarProductoATabla);
    document.addEventListener('keydown', function (e) {
        if (e.key === 'F8') {
            e.preventDefault();
            agregarProductoATabla();
        }
        if (e.key === 'F9') {
            e.preventDefault();
            limpiarInputsProducto();
        }
    });

    // ---------- Inputs de pago ----------
    document.querySelectorAll('.input-pago').forEach(i => i.addEventListener('input', calcularPagos));
    document.querySelectorAll('.input-pago').forEach(i => i.addEventListener('change', calcularPagos));

    // ---------- Botón grabar venta ----------
    document.getElementById('btnGrabarVenta').addEventListener('click', grabarVenta);

    // ---------- Inicializar readonly inputs cambio ----------
    document.getElementById('tipoCambio').readOnly = true;
    document.getElementById('numCambio').readOnly = true;
}

// ============================
// FUNCIONES INDEX
// ============================
function agregarProductoATabla() {
    const cod = document.getElementById('CodProducto').value;
    const sku = document.getElementById('Producto').value;
    const desc = document.getElementById('DescProducto').value;
    const precio = parseFloat(document.getElementById('PrecioProd').value) || 0;
    const cantNueva = parseFloat(document.getElementById('CantProducto').value) || 0;
    const descUnit = parseFloat(document.getElementById('DescuProducto').value) || 0;
    const bodega = document.getElementById('CodBodega').value || "01";

    if (!cod || cantNueva <= 0) {
        Swal.fire("Aviso", "Seleccione un producto y cantidad válida", "warning");
        return;
    }

    const tbody = document.getElementById('tbodyDetalle');
    let filaExistente = tbody.querySelector(`tr[data-cod="${cod}"]`);

    if (filaExistente) {
        const cantActual = parseFloat(filaExistente.querySelector('.col-cantidad').textContent);
        const cantTotal = cantActual + cantNueva;
        const descTotal = descUnit * cantTotal;
        const filaTotal = (precio * cantTotal) - descTotal;

        filaExistente.querySelector('.col-cantidad').textContent = cantTotal;
        filaExistente.querySelector('.col-descuento').textContent = `-${descTotal.toLocaleString('es-CL')}`;
        filaExistente.querySelector('.total-fila').textContent = filaTotal.toLocaleString('es-CL');
        filaExistente.querySelector('.total-fila').dataset.valor = filaTotal;

    } else {
        const descTotal = descUnit * cantNueva;
        const filaTotal = (precio * cantNueva) - descTotal;

        const tr = document.createElement('tr');
        tr.dataset.cod = cod;
        tr.dataset.bodega = bodega;
        tr.innerHTML = `
            <td class="text-center">VEN</td>
            <td>${sku}</td>
            <td>${desc}</td>
            <td class="text-end">${precio.toLocaleString('es-CL')}</td>
            <td class="text-center col-cantidad">${cantNueva}</td>
            <td class="text-end text-danger col-descuento">-${descTotal.toLocaleString('es-CL')}</td>
            <td class="text-end fw-bold total-fila" data-valor="${filaTotal}">${filaTotal.toLocaleString('es-CL')}</td>`;
        tbody.appendChild(tr);
    }

    actualizarTotalGeneral();
    limpiarInputsProducto();
    document.getElementById('Producto').focus();
}

function limpiarInputsProducto() {
    ['Producto', 'CodProducto', 'DescProducto', 'PrecioProd', 'CantProducto', 'DescuProducto', 'CodBodega']
        .forEach(id => document.getElementById(id).value = '');
}

function actualizarTotalGeneral() {
    let total = 0;
    document.querySelectorAll('.total-fila').forEach(td => {
        total += parseFloat(td.dataset.valor) || 0;
    });
    document.getElementById('txtTotal').textContent = total.toLocaleString('es-CL');
    calcularPagos();
}

function calcularPagos() {
    const totalVenta = parseInt(document.getElementById('txtTotal').textContent.replace(/\./g, '')) || 0;
    const efectivo = parseInt(document.getElementById('pagoEfectivo').value) || 0;
    const tarjeta = parseInt(document.getElementById('pagoTarjeta').value) || 0;

    const totalPagado = efectivo + tarjeta;
    const vuelto = totalPagado - totalVenta;

    document.getElementById('resumenTotal').textContent = totalVenta.toLocaleString('es-CL');
    document.getElementById('resumenPagos').textContent = totalPagado.toLocaleString('es-CL');
    document.getElementById('resumenVuelto').textContent = (vuelto > 0 ? vuelto : 0).toLocaleString('es-CL');
}

function grabarVenta() {
    const CodClie = document.getElementById('idCliente').value;
    const rutVend = document.getElementById('VendedorRut').value;
    const filas = document.querySelectorAll('#tbodyDetalle tr');

    if (!CodClie || !rutVend) { Swal.fire("Error", "Debe ingresar Cliente y Vendedor", "error"); return; }
    if (filas.length === 0) { Swal.fire("Error", "No hay productos en la venta", "error"); return; }

    const ventaData = {
        TipoVenta: document.getElementById('TipoDocumento').value,
        Total: parseInt(document.getElementById('txtTotal').textContent.replace(/\./g, '')) || 0,
        Vuelto: parseInt(document.getElementById('resumenVuelto').textContent.replace(/\./g, '')) || 0,
        CodCliente: CodClie,
        RutVendedor: rutVend,
        RetiroPosterior: document.getElementById('retiroPosterior').checked,
        Pagos: {
            Efectivo: parseInt(document.getElementById('pagoEfectivo').value) || 0,
            Tarjeta: parseInt(document.getElementById('pagoTarjeta').value) || 0
        },
        Detalles: []
    };

    filas.forEach(f => {
        ventaData.Detalles.push({
            CodProducto: f.dataset.cod,
            CodBodega: f.dataset.bodega,
            Cantidad: parseFloat(f.querySelector('.col-cantidad').textContent) || 0,
            Precio: parseInt(f.children[3].textContent.replace(/\./g, '')) || 0,
            DescuentoMonto: Math.abs(parseInt(f.querySelector('.col-descuento').textContent.replace(/\./g, ''))) || 0,
            TotalFila: parseFloat(f.querySelector('.total-fila').dataset.valor) || 0
        });
    });

    const url = document.getElementById('btnGrabarVenta').dataset.urlGrabar; // data-url-grabar="@Url.Action("GrabarVenta","PuntoVenta")"

    Swal.fire({
        title: '¿Confirmar Venta?',
        showCancelButton: true,
        confirmButtonText: 'Sí, Grabar'
    }).then(r => {
        if (!r.isConfirmed) return;
        fetch(url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(ventaData)
        })
            .then(r => r.json())
            .then(resp => {
                if (resp.success) {
                    window.open(`/PuntoVenta/Imprimir?tipo=BLE&id=${resp.idDoc}`, '_blank');
                    Swal.fire("Éxito", "Documento N° " + resp.folio, "success").then(() => location.reload());
                } else {
                    Swal.fire("Error", resp.message, "error");
                }
            })
            .catch(() => Swal.fire("Error", "Error de comunicación con el servidor", "error"));
    });
}

// ============================
// EVENTOS CAMBIO
// ============================
function inicializarEventosCambio() {

    console.log("✔ Sistema Cambio Inicializado");

    // ---------- Abrir Modal ----------
    document.getElementById('btnBuscarCambio')?.addEventListener('click', abrirModal);

    // ---------- Buscar Documento ----------
    document.getElementById('BuscarDocumento')?.addEventListener('click', function (e) {
        e.preventDefault();
        const tipo = document.getElementById('tipoDoctoSelect').value;
        const numero = document.getElementById('numDoc').value;
        const validador = document.getElementById('valDoc').value;
        const laboratorio = document.getElementById('labDoc').value || 0;

        if (!numero || numero === "0") { Swal.fire("Aviso", "Ingrese número de documento", "warning"); return; }

        const url = this.dataset.urlObtenerDoc; // data-url-obtener-doc="@Url.Action("ObtenerDocumentoCambio","PuntoVenta")"

        fetch(`${url}?tipo=${tipo}&numero=${numero}&validador=${validador}&laboratorio=${laboratorio}`)
            .then(r => r.json())
            .then(response => {
                if (!response.success) { Swal.fire("Aviso", response.message, "info"); return; }
                if (response.multiple) { /* implementar modal selección */ }
                else { renderizarDetalle(response); }
            })
            .catch(() => Swal.fire("Error", "Error de servidor", "error"));
    });

    // ---------- Check productos ----------
    $(document).on('change', '.chk-cambio', function () {
        const $fila = $(this).closest('tr');
        if (!this.checked) {
            $fila.find('.cant-cambio').text('0');
            return;
        }
        window.filaSeleccionadaActual = $fila;
        const max = Number($fila.data('max')) || 0;
        const desc = $fila.find('.desc-prod-tabla').text();
        $('#maxCant').val(max);
        $('#txtProductoModal').text(desc);
        $('#inputCantidadCambio').val(max);
        $('#modalCantidad').fadeIn(200);
        setTimeout(() => $('#inputCantidadCambio').focus().select(), 100);
    });

    // ---------- Botón limpiar ----------
    document.getElementById('btnLimpiarCambio')?.addEventListener('click', limpiarCambio);

    // ---------- Seleccionar Cambio ----------
    document.getElementById('BtnSeleccionar')?.addEventListener('click', function () {

        if (!window.cambioActual.cabecera) { Swal.fire("Aviso", "No hay documento cargado", "warning"); return; }

        let totalCambio = 0;
        let filas = '';

        $('#tablaProductos tbody tr').each(function () {
            const cant = parseInt($(this).find('.cant-cambio').text()) || 0;
            if (cant <= 0) return;
            const precio = Number($(this).children().eq(4).text().replace(/\D/g, ''));
            const total = cant * precio;
            totalCambio += total;

            filas += `<tr>
                        <td class="text-center">CAM</td>
                        <td>${$(this).children().eq(2).text()}</td>
                        <td>${$(this).find('.desc-prod-tabla').text()}</td>
                        <td class="text-end">${precio.toLocaleString('es-CL')}</td>
                        <td class="text-center">${cant}</td>
                        <td class="text-end">0</td>
                        <td class="text-end">${total.toLocaleString('es-CL')}</td>
                     </tr>`;
        });

        if (!filas) { Swal.fire("Aviso", "Seleccione al menos un producto", "warning"); return; }

        document.getElementById('tipoCambio').value = window.cambioActual.cabecera.tipo;
        document.getElementById('numCambio').value = window.cambioActual.cabecera.numero;
        document.getElementById('pagoCambio').value = totalCambio;

        document.getElementById('tbodyCambios').innerHTML = filas;
        document.getElementById('resumenTotal').textContent = totalCambio.toLocaleString('es-CL');

        miModal.hide();

        Swal.fire({ icon: 'success', title: 'Cambio aplicado', timer: 1200, showConfirmButton: false });
    });
}

// ---------- Abrir / Cerrar modal ----------
function abrirModal() { if (miModal) miModal.show(); }
function cerrarModal() { if (miModal) miModal.hide(); }

// ---------- Renderizar Documento Cambio ----------
function renderizarDetalle(res) {
    if (!res?.cabecera) { Swal.fire("Error", "Documento inválido", "error"); return; }

    const cab = res.cabecera;
    window.cambioActual.cabecera = { tipo: cab.tipoDocumento, numero: cab.numeroDocumento };

    document.getElementById('det-rut').value = cab.rut || '';
    document.getElementById('det-cliente').value = cab.cliente || '';
    document.getElementById('det-fecha').value = cab.fEmision || '';
    document.getElementById('det-vendedor').value = cab.vendedor || '';
    document.getElementById('det-cajero').value = cab.cajero || '';
    document.getElementById('det-tienda').value = cab.tienda || '';
    document.getElementById('det-total').value = new Intl.NumberFormat('es-CL').format(cab.total || 0);

    const tbody = document.getElementById('tablaProductos').querySelector('tbody');
    tbody.innerHTML = '';
    (res.productos || []).forEach(p => {
        const cantidad = Number(p.cantidad) || 0;
        const precio = Number(p.precioUnitario || p.precio_Unitario) || 0;
        const total = cantidad * precio;

        tbody.insertAdjacentHTML('beforeend', `
            <tr data-id="${p.idDetalleBoleta}" data-max="${cantidad}">
                <td class="text-center"><input type="checkbox" class="chk-cambio"></td>
                <td class="text-center">VEN</td>
                <td>${p.codigo || ''}</td>
                <td class="desc-prod-tabla">${p.descripcion || ''}</td>
                <td class="text-end">${precio.toLocaleString('es-CL')}</td>
                <td class="text-center">${cantidad}</td>
                <td class="text-end">0</td>
                <td class="text-center cant-cambio">0</td>
                <td class="text-end">${total.toLocaleString('es-CL')}</td>
            </tr>
        `);
    });
}

// ---------- Limpiar Modal Cambio ----------
function limpiarCambio() {
    window.cambioActual = { cabecera: null, productos: [] };
    document.getElementById('tablaProductos').querySelector('tbody').innerHTML = '';
    document.getElementById('tipoDoc').value = '';
    document.getElementById('numDoc').value = '';
    document.getElementById('valDoc').value = '';
}
