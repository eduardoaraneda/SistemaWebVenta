using AutoMapper;
using Domain.DTOs;    // Para TiendaDTO y EmpresaDTO
using Domain.Entidades;
using TheLine2.Models.DTOs;
using Web.Models;      // Para TiendaViewModel
// using Infrastructure.Persistence; // Para la entidad Empresas

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TiendaViewModel, TiendaDTO>();

        // Si los nombres en la base de datos son distintos a Id y Nombre, 
        // debes mapearlos manualmente así:
        CreateMap<Empresas, EmpresaDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Cod_Empresa)) // Cambia Cod_Empresa por el nombre real en tu tabla
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Descripcion)); // Cambia Descripcion por el nombre real
    }
}