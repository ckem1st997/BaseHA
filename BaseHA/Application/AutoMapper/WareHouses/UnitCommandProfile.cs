using AutoMapper;
using BaseHA.Application.ModelDto;
using BaseHA.Domain.Entity;

namespace BaseHA.Application.AutoMapper.WareHouses
{
    public class UnitCommandProfile : Profile
    {
        public UnitCommandProfile()
        {
            
            #region Unit

            CreateMap<UnitCommands, Unit>()
               .ForMember(x => x.WareHouseItems, opt => opt.Ignore())
               .ForMember(x => x.InwardDetails, opt => opt.Ignore())
               .ForMember(x => x.OutwardDetails, opt => opt.Ignore())
               .ForMember(x => x.WareHouseItemUnits, opt => opt.Ignore())
               .ForMember(x => x.WareHouseLimits, opt => opt.Ignore())
               .ForMember(x => x.BeginningWareHouses, opt => opt.Ignore());


            CreateMap<Unit, UnitCommands>();
            //.ForMember(x => x.AvailableWareHouses, opt => opt.Ignore());

            #endregion

        }

    }
    public static class MappingExtensionsUnit
    {
        public static UnitCommands ToModel(this Unit entity)
        {
            return AutoMapperConfiguration.Mapper.Map<Unit, UnitCommands>(entity);
        }

        public static Unit ToEntity(this UnitCommands model)
        {
            return AutoMapperConfiguration.Mapper.Map<UnitCommands, Unit>(model);
        }

        public static Unit ToEntity(this UnitCommands model, Unit destination)
        {
            return AutoMapperConfiguration.Mapper.Map(model, destination);
        }

    }
}
