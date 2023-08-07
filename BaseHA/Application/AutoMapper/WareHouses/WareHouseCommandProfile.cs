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
            CreateMap<WareHouseCommands, WareHouse>()
                .ForMember(x => x.OutwardWareHouse, opt => opt.Ignore())
                .ForMember(x => x.OutwardToWareHouse, opt => opt.Ignore())
                .ForMember(x => x.BeginningWareHouse, opt => opt.Ignore())
                .ForMember(x => x.Audit, opt => opt.Ignore())
                .ForMember(x => x.Inward, opt => opt.Ignore())
                .ForMember(x => x.OnDelete, opt => opt.Ignore())
                .ForMember(x => x.WareHouseLimit, opt => opt.Ignore());


            CreateMap<WareHouse, WareHouseCommands>();
            //

        }
    }
}