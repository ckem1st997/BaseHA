using AutoMapper;
using BaseHA.Application.ModelDto;
using BaseHA.Domain.Entity;

namespace BaseHA.Application.AutoMapper.WareHouses
{
    public class VendorCommandProfile : Profile
    {

        public VendorCommandProfile() 
        {
            CreateMap<VendorCommands, Vendor>()
             .ForMember(x => x.WareHouseItems, opt => opt.Ignore())
             .ForMember(x => x.Inwards, opt => opt.Ignore());


            CreateMap<Vendor, VendorCommands>()
                .ForMember(x => x.AvailableWareHouses, opt => opt.Ignore());

        }
    }
    public static class MappingExtensionsVendor
    {
        public static VendorCommands ToModel(this Vendor entity)
        {
            return AutoMapperConfiguration.Mapper.Map<Vendor, VendorCommands>(entity);
        }

        public static Vendor ToEntity(this VendorCommands model)
        {
            return AutoMapperConfiguration.Mapper.Map<VendorCommands, Vendor>(model);
        }

        public static Vendor ToEntity(this VendorCommands model, Vendor destination)
        {
            return AutoMapperConfiguration.Mapper.Map(model, destination);
        }

    }

}
