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
            #region Vendor

            CreateMap<VendorCommands, Vendor>()
               .ForMember(x => x.WareHouseItems, opt => opt.Ignore())
               .ForMember(x => x.Inwards, opt => opt.Ignore());


            CreateMap<Vendor, VendorCommands>()
                .ForMember(x => x.AvailableWareHouses, opt => opt.Ignore());

            #endregion

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


            CreateMap<Category, CategoryCommands>();

            CreateMap<CategoryCommands, Category>()
                 .ForMember(x => x.Answers, opt => opt.Ignore())
                 .ForMember(x => x.Intents, opt => opt.Ignore());


            CreateMap<Answer, AnswerCommands>();

            CreateMap<AnswerCommands, Answer>()
                 .ForMember(x => x.IntentCodeEnNavigation, opt => opt.Ignore());



            CreateMap<Intent, IntentCommands>();

            CreateMap<IntentCommands, Intent>()
                 .ForMember(x => x.IntentCodeEnNavigation, opt => opt.Ignore());

        }
    }
}