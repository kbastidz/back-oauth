using AutoMapper;
using db.Models;
using Rq = db.dominio;

namespace Core.mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            /*CreateMap<Usuario, Rq.Usuario>()
            .ForMember(dest => dest.persona,
                opt => opt.MapFrom(src => src.IdPersonaNavigation));
            */
            // Mapeo para la clase Persona
            //CreateMap<Persona, Rq.Persona>();
            CreateMap<Rq.Persona, Persona>().ReverseMap();
            /* CreateMap<Rq.Usuario, Usuario>().ReverseMap()
                 .ForMember(dest => dest.persona,
                 opt => opt.MapFrom(src => src.IdPersonaNavigation)); */

            CreateMap<Rq.Usuario, Usuario>()
    .ForMember(dest => dest.IdPersonaNavigation, opt => opt.Ignore()) // Ignora la relación durante la inserción
    .ForMember(dest => dest.IdPersona, opt => opt.MapFrom(src => src.persona != null ? src.persona.Id : (int?)null)) // Mapea solo el ID de Persona si está disponible
    .ReverseMap()
    .ForMember(dest => dest.persona, opt => opt.MapFrom(src => src.IdPersonaNavigation)); // Relación inversa para consultas




        }
    }
}
