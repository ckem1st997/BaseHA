using AutoMapper;
using BaseHA.Application.ModelDto;
using BaseHA.Domain.Entity;

namespace BaseHA.Application.AutoMapper.WareHouses
{
    public class BeginningWareHouseProfile : Profile
    {
        public BeginningWareHouseProfile()
        {
            // ForAllMaps(CommonProfile.AllMapsAction);

            #region Beginning

            CreateMap<BeginningCommands, BeginningWareHouse>();

            CreateMap<BeginningWareHouse, BeginningCommands>()
                .ForMember(x => x.AvailableWareHouses, opt => opt.Ignore())
                .ForMember(x => x.AvailableItem, opt => opt.Ignore())
                .ForMember(x => x.AvailableUnit, opt => opt.Ignore());

            #endregion

        }
    }

    public static class MappingExtensionsBeginning
    {

        public static BeginningCommands ToModel(this BeginningWareHouse entity)
        {
            return AutoMapperConfiguration.Mapper.Map<BeginningWareHouse, BeginningCommands>(entity);
        }

        public static BeginningWareHouse ToEntity(this BeginningCommands model)
        {
            return AutoMapperConfiguration.Mapper.Map<BeginningCommands, BeginningWareHouse>(model);
        }

        public static BeginningWareHouse ToEntity(this BeginningCommands model, BeginningWareHouse destination)
        {
            return AutoMapperConfiguration.Mapper.Map(model, destination);
        }

    }
}
