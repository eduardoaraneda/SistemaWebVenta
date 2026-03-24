"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
console.log("cliente-modal.js cargado ✅");
console.log("flatpickr existe?", typeof flatpickr);
function toPromiseGet(url, params) {
    return new Promise((resolve, reject) => {
        $.get(url, params).done((data) => resolve(data)).fail((err) => reject(err));
    });
}
function llenarSelect($sel, items, valueKey, textKey, placeholder) {
    $sel.empty();
    $sel.append(`<option value="">${placeholder || "Seleccione..."}</option>`);
    (items || []).forEach((it) => {
        var _a, _b;
        const v = (_a = it[valueKey]) !== null && _a !== void 0 ? _a : "";
        const t = (_b = it[textKey]) !== null && _b !== void 0 ? _b : "";
        $sel.append(`<option value="${String(v)}">${String(t)}</option>`);
    });
}
function setDateFlatpickr(inputId, isoOrDate) {
    const el = document.getElementById(inputId);
    if (!el)
        return;
    const fp = el._flatpickr;
    if (!fp)
        return;
    if (!isoOrDate || String(isoOrDate).startsWith("1900-01-01")) {
        fp.clear();
        return;
    }
    fp.setDate(isoOrDate, true);
}
function limpiarRut(rut) {
    return (rut || "").replace(/\./g, "").trim();
}
function getStr(sel) {
    var _a;
    const v = ((_a = $(sel).val()) !== null && _a !== void 0 ? _a : "").toString();
    return v.trim();
}
function getOrEmpty(sel) {
    const v = getStr(sel);
    return v === "" ? "" : v;
}
function getGuidOrNull(sel) {
    const v = getStr(sel);
    return v === "" ? null : v;
}
function getFechaYmdDesdeFlatpickr(inputId) {
    var _a;
    const el = document.getElementById(inputId);
    const fp = el === null || el === void 0 ? void 0 : el._flatpickr;
    const d = (_a = fp === null || fp === void 0 ? void 0 : fp.selectedDates) === null || _a === void 0 ? void 0 : _a[0];
    if (!d || !fp)
        return "";
    return fp.formatDate(d, "Y-m-d");
}
function llenarInputsCliente(d) {
    var _a, _b, _c, _d, _e, _f, _g, _h, _j, _k, _l, _m, _o, _p, _q;
    const rutFormateado = d.Rut_Cliente ? `${d.Rut_Cliente}-${(_a = d.Dv_Cliente) !== null && _a !== void 0 ? _a : ""}` : "";
    $("#RutModal").val(rutFormateado);
    $("#Nombres").val((_b = d.Nombres) !== null && _b !== void 0 ? _b : "");
    $("#ApellidoPaterno").val((_c = d.ApellidoPat) !== null && _c !== void 0 ? _c : "");
    $("#ApellidoMaterno").val((_d = d.ApellidoMat) !== null && _d !== void 0 ? _d : "");
    $("#Giro").val((_e = d.Giro) !== null && _e !== void 0 ? _e : "");
    $("#Direccion").val((_f = d.Direccion) !== null && _f !== void 0 ? _f : "");
    $("#Numero").val((_g = d.Numero) !== null && _g !== void 0 ? _g : "");
    $("#Depto").val((_j = (_h = d.Departamento) !== null && _h !== void 0 ? _h : d.Depto) !== null && _j !== void 0 ? _j : "");
    $("#Celular").val((_k = d.Celular) !== null && _k !== void 0 ? _k : "");
    $("#Mail").val((_l = d.Mail) !== null && _l !== void 0 ? _l : "");
    $("#CodCliente").val((_m = d.Cod_Cliente) !== null && _m !== void 0 ? _m : "");
    $("#EstadoCivil").val((_o = d.EstadoCivil) !== null && _o !== void 0 ? _o : "");
    if (d.Sexo === "M" || d.Sexo === "F") {
        $(`input[name="sexo"][value="${d.Sexo}"]`).prop("checked", true);
    }
    setDateFlatpickr("fechaNacimiento", d.FechaNacimiento);
    $("#Tipo").val((_p = d.Tipo_Cliente) !== null && _p !== void 0 ? _p : "");
    $("#Estado").val((_q = d.Estado) !== null && _q !== void 0 ? _q : "");
}
function cargarRegionesYSeleccionar(codRegion) {
    return __awaiter(this, void 0, void 0, function* () {
        const data = yield toPromiseGet("/PuntoVenta/GetRegiones", {});
        llenarSelect($("#Region"), data, "codigo", "descripcion", "Seleccione región...");
        if (codRegion !== null && codRegion !== undefined && codRegion !== "")
            $("#Region").val(String(codRegion));
    });
}
function cargarCiudadesYSeleccionar(codRegion, codCiudad) {
    return __awaiter(this, void 0, void 0, function* () {
        const $ciudad = $("#Ciudad");
        const $comuna = $("#Comuna");
        $ciudad.prop("disabled", !codRegion);
        $comuna.prop("disabled", true);
        llenarSelect($ciudad, [], "codigo", "descripcion", "Seleccione ciudad...");
        llenarSelect($comuna, [], "codigo", "descripcion", "Seleccione comuna...");
        if (!codRegion)
            return;
        const data = yield toPromiseGet("/PuntoVenta/GetCiudades", { codRegion });
        llenarSelect($ciudad, data, "codigo", "descripcion", "Seleccione ciudad...");
        $ciudad.prop("disabled", false);
        if (codCiudad)
            $ciudad.val(String(codCiudad));
    });
}
function cargarComunasYSeleccionar(codCiudad, codComuna) {
    return __awaiter(this, void 0, void 0, function* () {
        const $comuna = $("#Comuna");
        $comuna.prop("disabled", !codCiudad);
        llenarSelect($comuna, [], "codigo", "descripcion", "Seleccione comuna...");
        if (!codCiudad)
            return;
        const data = yield toPromiseGet("/PuntoVenta/GetComunas", { codCiudad });
        llenarSelect($comuna, data, "codigo", "descripcion", "Seleccione comuna...");
        $comuna.prop("disabled", false);
        if (codComuna)
            $comuna.val(String(codComuna));
    });
}
function cargarProfesionesYSeleccionar(codProfesion) {
    return __awaiter(this, void 0, void 0, function* () {
        const data = yield toPromiseGet("/PuntoVenta/cargarProfesiones", {});
        llenarSelect($("#Profesion"), data, "cod_Parametro", "descripcion", "Seleccione Profesión...");
        if (codProfesion)
            $("#Profesion").val(String(codProfesion));
    });
}
function cargarClienteYFormulario(rut) {
    return __awaiter(this, void 0, void 0, function* () {
        var _a, _b, _c, _d, _e, _f;
        const d = yield toPromiseGet("/PuntoVenta/cargarDatosCliente", { rut });
        console.log("Cliente response:", d);
        llenarInputsCliente(d);
        yield cargarRegionesYSeleccionar((_a = d.Cod_Region) !== null && _a !== void 0 ? _a : null);
        yield cargarCiudadesYSeleccionar((_b = d.Cod_Region) !== null && _b !== void 0 ? _b : null, (_c = d.Cod_Ciudad) !== null && _c !== void 0 ? _c : null);
        yield cargarComunasYSeleccionar((_d = d.Cod_Ciudad) !== null && _d !== void 0 ? _d : null, (_e = d.Cod_Comuna) !== null && _e !== void 0 ? _e : null);
        yield cargarProfesionesYSeleccionar((_f = d.Cod_Profesion) !== null && _f !== void 0 ? _f : null);
    });
}
function obtenerClienteDesdeModal_VB() {
    var _a;
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
        Sexo: (String((_a = $('input[name="sexo"]:checked').val()) !== null && _a !== void 0 ? _a : "") || "M"),
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
function validarClienteMinimo(c) {
    if (!c.Rut_Cliente || !c.Dv_Cliente)
        return "Debe ingresar RUT válido.";
    if (!c.Nombres)
        return "Debe ingresar Nombre(s).";
    if (!c.ApellidoPat)
        return "Debe ingresar Apellido Paterno.";
    return null;
}
document.addEventListener("DOMContentLoaded", () => {
    flatpickr("#fechaNacimiento", {
        dateFormat: "d-m-Y",
        maxDate: "today",
        locale: "es",
        allowInput: true,
    });
    $("#Region").on("change", function () {
        return __awaiter(this, void 0, void 0, function* () {
            var _a;
            const codRegion = String((_a = $(this).val()) !== null && _a !== void 0 ? _a : "");
            yield cargarCiudadesYSeleccionar(codRegion || null, null);
            yield cargarComunasYSeleccionar(null, null);
        });
    });
    $("#Ciudad").on("change", function () {
        return __awaiter(this, void 0, void 0, function* () {
            var _a;
            const codCiudad = String((_a = $(this).val()) !== null && _a !== void 0 ? _a : "");
            yield cargarComunasYSeleccionar(codCiudad || null, null);
        });
    });
    const modalEl = document.getElementById("modalCliente");
    modalEl === null || modalEl === void 0 ? void 0 : modalEl.addEventListener("shown.bs.modal", () => __awaiter(void 0, void 0, void 0, function* () {
        const rut = getStr("#RutModal");
        try {
            yield cargarRegionesYSeleccionar(null);
            yield cargarProfesionesYSeleccionar(null);
            $("#Ciudad").prop("disabled", true);
            $("#Comuna").prop("disabled", true);
            if (rut) {
                yield cargarClienteYFormulario(rut);
            }
        }
        catch (e) {
            console.error(e);
            Swal.fire("Error", "No se pudo cargar la ficha del cliente.", "error");
        }
    }));
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
            success: function (resp) {
                var _a;
                if (resp && resp.success) {
                    Swal.fire("OK", resp.message || "Cliente guardado.", "success");
                    const m = document.getElementById("modalCliente");
                    (_a = bootstrap.Modal.getInstance(m)) === null || _a === void 0 ? void 0 : _a.hide();
                    $("#NombreCompleto").val(`${payload.Nombres} ${payload.ApellidoPat} ${payload.ApellidoMat}`.trim());
                }
                else {
                    Swal.fire("Aviso", (resp === null || resp === void 0 ? void 0 : resp.message) || "No se pudo guardar.", "warning");
                }
            },
            error: function (xhr) {
                console.error(xhr);
                Swal.fire("Error", "Error al guardar el cliente.", "error");
            },
        });
    });
});
