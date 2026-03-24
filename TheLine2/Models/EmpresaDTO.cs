namespace TheLine2.Models.DTOs
{
    public class EmpresaDTO
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public int EmisorId { get; set; }
        public int Rut_Empresa { get; set; }
        public string Dv_Empresa { get; set; }
    }
}
