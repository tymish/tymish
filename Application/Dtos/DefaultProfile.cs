using AutoMapper;
using Tymish.Domain.Entities;

namespace Tymish.Application.Dtos
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<Invoice, InvoiceDto>()
                .ForMember(
                    dest => dest.TimeEntryDtos,
                    opt => opt.MapFrom(src => src.TimeEntries));
            CreateMap<TimeEntry, TimeEntryDto>();
        }
    }
}