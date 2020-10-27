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
                    option => option.MapFrom(src => src.TimeEntries)
                ).ForMember(
                    dest => dest.VendorDto,
                    option => option.MapFrom(src => src.Vendor)
                );
                
                

            CreateMap<TimeEntry, TimeEntryDto>();
            
            CreateMap<Vendor, VendorDto>();
        }
    }
}