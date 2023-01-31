using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace LoadBusiness.ShortShip
{
    public class ShortShipModel
    {
        public ShortShipModel()
        {
            items = new List<Item>();
        }

        public bool resultIsUse { get; set; }

        public string resultMsg { get; set; }

        public string truckLoad_No { get; set; }
        public string planGoodsissue_no { get; set; }

        public List<Item> items { get; set; }

    }

    public class Item
    {

        public Guid TruckLoad_Index { get; set; }

        public string TruckLoad_No { get; set; }

        public string Driver_name { get; set; }

        public string Vehicle_Registration { get; set; }

        public string VehicleCompany_Id { get; set; }

        public string VehicleType_Name { get; set; }

        public string Route { get; set; }

        public string Appointment_Date { get; set; }

        public string Appointment_Time { get; set; }

        public string Dock_name { get; set; }

        public int? count_drop { get; set; }

        public int? count_plan { get; set; }

        public int count_retrun { get; set; }

        public int? count_carton { get; set; }

        public int? count_tote_L { get; set; }

        public int? count_tote_M { get; set; }

        public string branch { get; set; }

        public string drop_seq { get; set; }

        public int? plan_runing { get; set; }

        public string PlanGoodsIssue_No { get; set; }

        public string ShipTo_Name { get; set; }

        public string ShipTo_Address { get; set; }

        public string ShipTo_Tel { get; set; }

        public int? carton { get; set; }

        public int? toteL { get; set; }

        public int? toteM { get; set; }

        public int Row_Runing_product { get; set; }

        public string Product_Id { get; set; }

        public string Product_Name { get; set; }

        public decimal? Order_Qty { get; set; }

        public decimal? Pick { get; set; }

        public string ProductConversion_Name { get; set; }

        public string Tote_Size { get; set; }

        public string ShippingMethod_Name { get; set; }

        public string Return { get; set; }
        public string Product_Lot { get; set; }
        public string Tagout_Runing { get; set; }
        public int? total_Box { get; set; }
        public string PGI_set { get; set; }
        public string status_shortship { get; set; }
        public string status_Doc { get; set; }
        public string user { get; set; }
        public string update_date { get; set; }
    }

}
