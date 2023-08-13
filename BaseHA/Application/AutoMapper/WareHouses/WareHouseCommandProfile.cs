using AutoMapper;
using BaseHA.Application.ModelDto;
using BaseHA.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BaseHA.Application.AutoMapper.WareHouses
{
    public class WareHouseCommandProfile : Profile
    {
        public WareHouseCommandProfile()
        {
            // ForAllMaps(CommonProfile.AllMapsAction);



            CreateMap<WareHouseCommands, WareHouse>()
                .ForMember(x => x.OutwardToWareHouses, opt => opt.Ignore())
                .ForMember(x => x.OutwardToWareHouses, opt => opt.Ignore())
                .ForMember(x => x.BeginningWareHouses, opt => opt.Ignore())
                .ForMember(x => x.Audits, opt => opt.Ignore())
                .ForMember(x => x.Inwards, opt => opt.Ignore())
                //.ForMember(x => x.OnDelete, opt => opt.Ignore())
                //.ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.WareHouseLimits, opt => opt.Ignore());


            CreateMap<WareHouse, WareHouseCommands>()
                .ForMember(x => x.AvailableWareHouses, opt => opt.Ignore());
            //

            #region Unit

            CreateMap<UnitCommands, Unit>()
               .ForMember(x => x.WareHouseLimits, opt => opt.Ignore())
               .ForMember(x => x.WareHouseItemUnits, opt => opt.Ignore())
               .ForMember(x => x.BeginningWareHouses, opt => opt.Ignore())
               .ForMember(x => x.WareHouseItems, opt => opt.Ignore())
               .ForMember(x => x.InwardDetails, opt => opt.Ignore())
               .ForMember(x => x.OutwardDetails, opt => opt.Ignore())
               .ForMember(x => x.WareHouseLimits, opt => opt.Ignore());


            CreateMap<Unit, UnitCommands>();
                //.ForMember(x => x.AvailableWareHouses, opt => opt.Ignore());

            #endregion



        }
    }

    public static class MappingExtensions
    {

        public static WareHouseCommands ToModel(this WareHouse entity)
        {
            return AutoMapperConfiguration.Mapper.Map<WareHouse, WareHouseCommands>(entity);
        }

        public static WareHouse ToEntity(this WareHouseCommands model)
        {
            return AutoMapperConfiguration.Mapper.Map<WareHouseCommands, WareHouse>(model);
        }

        public static WareHouse ToEntity(this WareHouseCommands model, WareHouse destination)
        {
            return AutoMapperConfiguration.Mapper.Map(model, destination);
        }

    }
    /// <summary>
    /// AutoMapper configuration
    /// </summary>
    public static class AutoMapperConfiguration
    {
        /// <summary>
        /// Mapper
        /// </summary>
        public static IMapper Mapper { get; private set; }

        /// <summary>
        /// Mapper configuration
        /// </summary>
        public static MapperConfiguration MapperConfiguration { get; private set; }

        // Custom-AutoMapper: AddProfile theo từng Project, cần Profile nào thì add Profile đó
        public static IList<Profile> Profiles = new List<Profile>();

        /// <summary>
        /// Initialize mapper
        /// </summary>
        /// <param name="config">Mapper configuration</param>
        public static void Init(MapperConfiguration config)
        {
            MapperConfiguration = config;
            Mapper = config.CreateMapper();
        }
    }
}