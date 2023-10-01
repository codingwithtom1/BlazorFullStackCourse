using AutoMapper;
using BlazorInvoiceApp.Data;
using BlazorInvoiceApp.DTOS;


namespace BlazorInvoiceApp.Repository
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            CreateMap<Customer, CustomerDTO>();
            CreateMap<CustomerDTO, Customer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<InvoiceTerms, InvoiceTermsDTO>();
            CreateMap<InvoiceTermsDTO, InvoiceTerms>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<InvoiceLineItem, InvoiceLineItemDTO>();
            CreateMap<InvoiceLineItemDTO, InvoiceLineItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Invoice, InvoiceDTO>()
               .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer!.Name))
               .ForMember(dest => dest.InvoiceTermsName, opt => opt.MapFrom(src => src.InvoiceTerms!.Name));
            CreateMap<InvoiceDTO, Invoice>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
