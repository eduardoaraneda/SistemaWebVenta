console.log("cliente-modal.js cargado ✅");
console.log("flatpickr existe?", typeof flatpickr);
declare const flatpickr: any;
declare const bootstrap: any;
declare const Swal: any;

// Tipos para Flatpickr en el input (evita any por _flatpickr)
type FlatpickrInstance = {
    clear: () => void;
    setDate: (date: any, triggerChange?: boolean) => void;
    selectedDates?: Date[];
    formatDate: (d: Date, fmt: string) => string;
};

type FlatpickrInput = HTMLInputElement & { _flatpickr?: FlatpickrInstance };

// =================== MODELOS ===================
interface ParamItem {
    [key: string]: any;
    codigo?: string | number;
    descripcion?: string;
    cod_Parametro?: string | number;
}

interface ClienteResponse {
    Rut_Cliente?: number;
    Dv_Cliente?: string;
    Nombres?: string;
    ApellidoPat?: string;
    ApellidoMat?: string;
    Giro?: string;
    Direccion?: string;
    Numero?: string;
    Departamento?: string;
    Depto?: string;
    Celular?: string;
    Mail?: string;

    Cod_Cliente?: string; // Guid
    EstadoCivil?: string; // "S" / "C"
    Sexo?: "M" | "F" | string;

    FechaNacimiento?: string; // ISO o similar

    Tipo_Cliente?: string;
    Estado?: string;

    Cod_Region?: string | number;
    Cod_Ciudad?: string | number;
    Cod_Comuna?: string | number;

    Cod_Profesion?: string | number;
}

interface ClientePayload {
    Rut_Cliente: number;
    Dv_Cliente: string;

    Cod_Cliente: string | null;

    Nombres: string;
    ApellidoPat: string;
    ApellidoMat: string;
    Giro: string;
    Direccion: string;
    Numero: string;
    Depto: string;
    Poblacion: string;

    Cod_Region: string;
    Cod_Ciudad: string;
    Cod_Comuna: string;

    EstadoCivil: string;
    Sexo: string;
    FechaNacimiento: string; // "Y-m-d" o ""

    FaceBook: string;
    Publicidad: string;
    Cod_Profesion: string;

    Telefono: string;
    Celular: string;
    Mail: string;

    Tipo_Cliente: string;
    Fax: string;
    Observaciones: string;

    Estado: string;
    Convenio: string;
    FechaConvenioDesde: string;
    FechaConvenioHasta: string;

    Cod_ClienteConvenio: string | null;

    Banco: string;
    TipoCuenta: string;
    NroCuenta: string;
    RutCuenta: string;
}

interface GuardarClienteResponse {
    success: boolean;
    message?: string;
}

// =================== HELPERS ===================
function toPromiseGet<T>(url: string, params: Record<string, any>): Promise<T> {
    return new Promise<T>((resolve, reject) => {
        $.get(url, params).done((data: T) => resolve(data)).fail((err: any) => reject(err));
    });
}

function llenarSelect<T extends Record<string, any>>(
    $sel: JQuery,
    items: T[] | null | undefined,
    valueKey: keyof T,
    textKey: keyof T,
    placeholder?: string
): void {
    $sel.empty();
    $sel.append(`<option value="">${placeholder || "Seleccione..."}</option>`);
    (items || []).forEach((it) => {
        const v = it[valueKey] ?? "";
        const t = it[textKey] ?? "";
        $sel.append(`<option value="${String(v)}">${String(t)}</option>`);
    });
}

function setDateFlatpickr(inputId: string, isoOrDate: any): void {
    const el = document.getElementById(inputId) as FlatpickrInput | null;
    if (!el) return;

    const fp = el._flatpickr;
    if (!fp) return;

    if (!isoOrDate || String(isoOrDate).startsWith("1900-01-01")) {
        fp.clear();
        return;
    }
    fp.setDate(isoOrDate, true);
}

function limpiarRut(rut: string | null | undefined): string {
    return (rut || "").replace(/\./g, "").trim();
}

function getStr(sel: string): string {
    const v = ($(sel).val() ?? "").toString();
    return v.trim();
}

function getOrEmpty(sel: string): string {
    const v = getStr(sel);
    return v === "" ? "" : v;
}

function getGuidOrNull(sel: string): string | null {
    const v = getStr(sel);
    return v === "" ? null : v;
}

function getFechaYmdDesdeFlatpickr(inputId: string): string {
    const el = document.getElementById(inputId) as FlatpickrInput | null;
    const fp = el?._flatpickr;
    const d = fp?.selectedDates?.[0];
    if (!d || !fp) return "";
    return fp.formatDate(d, "Y-m-d");
}

// =================== LLENAR FORM CON DATOS CLIENTE ===================
function llenarInputsCliente(d: ClienteResponse): void {
    const rutFormateado = d.Rut_Cliente ? `${d.Rut_Cliente}-${d.Dv_Cliente ?? ""}` : "";
    $("#RutModal").val(rutFormateado);

    $("#Nombres").val(d.Nombres ?? "");
    $("#ApellidoPaterno").val(d.ApellidoPat ?? "");
    $("#ApellidoMaterno").val(d.ApellidoMat ?? "");

    $("#Giro").val(d.Giro ?? "");

    $("#Direccion").val(d.Direccion ?? "");
    $("#Numero").val(d.Numero ?? "");
    $("#Depto").val(d.Departamento ?? d.Depto ?? "");

    $("#Celular").val(d.Celular ?? "");
    $("#Mail").val(d.Mail ?? "");

    $("#CodCliente").val(d.Cod_Cliente ?? "");

    $("#EstadoCivil").val(d.EstadoCivil ?? "");

    if (d.Sexo === "M" || d.Sexo === "F") {
        $(`input[name="sexo"][value="${d.Sexo}"]`).prop("checked", true);
    }

    setDateFlatpickr("fechaNacimiento", d.FechaNacimiento);

    $("#Tipo").val(d.Tipo_Cliente ?? "");
    $("#Estado").val(d.Estado ?? "");
}

// =================== CARGA COMBOS (ENCADENADO) ===================
async function cargarRegionesYSeleccionar(codRegion: string | number | null): Promise<void> {
    const data = await toPromiseGet<ParamItem[]>("/PuntoVenta/GetRegiones", {});
    llenarSelect($("#Region"), data, "codigo", "descripcion", "Seleccione región...");
    if (codRegion !== null && codRegion !== undefined && codRegion !== "") $("#Region").val(String(codRegion));
}

async function cargarCiudadesYSeleccionar(
    codRegion: string | number | null,
    codCiudad: string | number | null
): Promise<void> {
    const $ciudad = $("#Ciudad");
    const $comuna = $("#Comuna");

    $ciudad.prop("disabled", !codRegion);
    $comuna.prop("disabled", true);

    llenarSelect($ciudad, [], "codigo", "descripcion", "Seleccione ciudad...");
    llenarSelect($comuna, [], "codigo", "descripcion", "Seleccione comuna...");

    if (!codRegion) return;

    const data = await toPromiseGet<ParamItem[]>("/PuntoVenta/GetCiudades", { codRegion });
    llenarSelect($ciudad, data, "codigo", "descripcion", "Seleccione ciudad...");

    $ciudad.prop("disabled", false);
    if (codCiudad) $ciudad.val(String(codCiudad));
}

async function cargarComunasYSeleccionar(
    codCiudad: string | number | null,
    codComuna: string | number | null
): Promise<void> {
    const $comuna = $("#Comuna");

    $comuna.prop("disabled", !codCiudad);
    llenarSelect($comuna, [], "codigo", "descripcion", "Seleccione comuna...");

    if (!codCiudad) return;

    const data = await toPromiseGet<ParamItem[]>("/PuntoVenta/GetComunas", { codCiudad });
    llenarSelect($comuna, data, "codigo", "descripcion", "Seleccione comuna...");

    $comuna.prop("disabled", false);
    if (codComuna) $comuna.val(String(codComuna));
}

async function cargarProfesionesYSeleccionar(codProfesion: string | number | null): Promise<void> {
    const data = await toPromiseGet<ParamItem[]>("/PuntoVenta/cargarProfesiones", {});
    llenarSelect($("#Profesion"), data, "cod_Parametro", "descripcion", "Seleccione Profesión...");
    if (codProfesion) $("#Profesion").val(String(codProfesion));
}

async function cargarClienteYFormulario(rut: string): Promise<void> {
    const d = await toPromiseGet<ClienteResponse>("/PuntoVenta/cargarDatosCliente", { rut });
    console.log("Cliente response:", d);

    llenarInputsCliente(d);

    await cargarRegionesYSeleccionar(d.Cod_Region ?? null);
    await cargarCiudadesYSeleccionar(d.Cod_Region ?? null, d.Cod_Ciudad ?? null);
    await cargarComunasYSeleccionar(d.Cod_Ciudad ?? null, d.Cod_Comuna ?? null);
    await cargarProfesionesYSeleccionar(d.Cod_Profesion ?? null);
}

// =================== GUARDAR (MISMO VB) ===================
function obtenerClienteDesdeModal_VB(): ClientePayload {
    const rutModal = limpiarRut(getStr("#RutModal"));
    const parts = rutModal.split("-");

    const rut = parts[0] ? parseInt(parts[0], 10) : 0;
    const dv = parts[1] ? parts[1].toUpperCase() : "";

    return {
        Rut_Cliente: Number.isFinite(rut) ? rut : 0,
        Dv_Cliente: dv,

        Cod_Cliente: getGuidOrNull("#CodCliente"),

        Nombres: getOrEmpty("#Nombres"),
        ApellidoPat: getOrEmpty("#ApellidoPaterno"),
        ApellidoMat: getOrEmpty("#ApellidoMaterno"),
        Giro: getOrEmpty("#Giro"),
        Direccion: getOrEmpty("#Direccion"),
        Numero: getOrEmpty("#Numero"),
        Depto: getOrEmpty("#Depto"),
        Poblacion: getOrEmpty("#Poblacion"),

        Cod_Region: getOrEmpty("#Region"),
        Cod_Ciudad: getOrEmpty("#Ciudad"),
        Cod_Comuna: getOrEmpty("#Comuna"),

        EstadoCivil: getOrEmpty("#EstadoCivil"),
        Sexo: (String($('input[name="sexo"]:checked').val() ?? "") || "M"),
        FechaNacimiento: getFechaYmdDesdeFlatpickr("fechaNacimiento"),

        FaceBook: getOrEmpty("#FaceBook"),
        Publicidad: getOrEmpty("#Publicidad") || "N",
        Cod_Profesion: getOrEmpty("#Profesion"),

        Telefono: getOrEmpty("#Telefono"),
        Celular: getOrEmpty("#Celular"),
        Mail: getOrEmpty("#Mail"),

        Tipo_Cliente: getOrEmpty("#Tipo"),
        Fax: getOrEmpty("#Fax"),
        Observaciones: getOrEmpty("#Observaciones"),

        Estado: getOrEmpty("#Estado"),
        Convenio: getOrEmpty("#Convenio") || "N",
        FechaConvenioDesde: getOrEmpty("#FechaConvenioDesde"),
        FechaConvenioHasta: getOrEmpty("#FechaConvenioHasta"),

        Cod_ClienteConvenio: getGuidOrNull("#CodClienteConvenio"),

        Banco: getOrEmpty("#Banco"),
        TipoCuenta: getOrEmpty("#TipoCuenta"),
        NroCuenta: getOrEmpty("#NroCuenta"),
        RutCuenta: getOrEmpty("#RutCuenta"),
    };
}

function validarClienteMinimo(c: ClientePayload): string | null {
    if (!c.Rut_Cliente || !c.Dv_Cliente) return "Debe ingresar RUT válido.";
    if (!c.Nombres) return "Debe ingresar Nombre(s).";
    if (!c.ApellidoPat) return "Debe ingresar Apellido Paterno.";
    return null;
}

// =================== INIT ===================
document.addEventListener("DOMContentLoaded", () => {
    flatpickr("#fechaNacimiento", {
        dateFormat: "d-m-Y",
        maxDate: "today",
        locale: "es",
        allowInput: true,
    });

    $("#Region").on("change", async function () {
        const codRegion = String($(this).val() ?? "");
        await cargarCiudadesYSeleccionar(codRegion || null, null);
        await cargarComunasYSeleccionar(null, null);
    });

    $("#Ciudad").on("change", async function () {
        const codCiudad = String($(this).val() ?? "");
        await cargarComunasYSeleccionar(codCiudad || null, null);
    });

    const modalEl = document.getElementById("modalCliente");
    modalEl?.addEventListener("shown.bs.modal", async () => {
        const rut = getStr("#RutModal");

        try {
            await cargarRegionesYSeleccionar(null);
            await cargarProfesionesYSeleccionar(null);

            $("#Ciudad").prop("disabled", true);
            $("#Comuna").prop("disabled", true);

            if (rut) {
                await cargarClienteYFormulario(rut);
            }
        } catch (e) {
            console.error(e);
            Swal.fire("Error", "No se pudo cargar la ficha del cliente.", "error");
        }
    });

    $(document)
        .off("click", "#btnGrabarCliente")
        .on("click", "#btnGrabarCliente", () => {
            const payload = obtenerClienteDesdeModal_VB();
            const msg = validarClienteMinimo(payload);

            if (msg) {
                Swal.fire("Validación", msg, "warning");
                return;
            }

            $.ajax({
                url: "/PuntoVenta/GuardarClienteEx1",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(payload),
                success: function (resp: GuardarClienteResponse) {
                    if (resp && resp.success) {
                        Swal.fire("OK", resp.message || "Cliente guardado.", "success");

                        const m = document.getElementById("modalCliente");
                        bootstrap.Modal.getInstance(m)?.hide();

                        $("#NombreCompleto").val(
                            `${payload.Nombres} ${payload.ApellidoPat} ${payload.ApellidoMat}`.trim()
                        );
                    } else {
                        Swal.fire("Aviso", resp?.message || "No se pudo guardar.", "warning");
                    }
                },
                error: function (xhr: any) {
                    console.error(xhr);
                    Swal.fire("Error", "Error al guardar el cliente.", "error");
                },
            });
        });
});