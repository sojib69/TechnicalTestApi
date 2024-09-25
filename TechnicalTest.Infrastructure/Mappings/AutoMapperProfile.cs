using AutoMapper;
using TechnicalTest.Domain.Entities;
using TechnicalTest.Shared.Models.ContactGroups;
using TechnicalTest.Shared.Models.Contacts;

namespace TechnicalTest.Infrastructure.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ContactAddRequest, Contact>();
            CreateMap<ContactEditRequest, Contact>();
            CreateMap<ContactGroupAddRequest, ContactGroup>();
            CreateMap<ContactGroupEditRequest, ContactGroup>();
        }
    }
}
