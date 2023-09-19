﻿namespace BaseHA.Models.SearchModel
{
    public class WareHouseSearchModel : BaseSearchModel
    {
        public WareHouseSearchModel()
        {
            Keywords = "";
        }

        public ActiveStatus ActiveStatus { get; set; }

    }

    public class UnitSearchModel : BaseSearchModel
    {
        public UnitSearchModel()
        {
            Keywords = "";
        }

        public ActiveStatus ActiveStatus { get; set; }

    }



}