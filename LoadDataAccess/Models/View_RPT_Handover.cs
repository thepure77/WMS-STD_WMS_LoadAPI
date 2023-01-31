using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace PlanGIDataAccess.Models
{
    public partial class View_RPT_Handover
    {
        [Key]
        public long? RowIndex { get; set; }
        
        public long? IsShow { get; set; }

        public Guid TruckLoad_Index { get; set; }
        
        public string TruckLoad_No { get; set; }
        
        public string Driver_name { get; set; }
        
        public string Vehicle_Registration { get; set; }

        public string VehicleCompany_Id { get; set; }

        public string VehicleType_Name { get; set; }
        
        public string Route { get; set; }

        public DateTime? Appointment_Date { get; set; }
        
        public string Appointment_Time { get; set; }
        
        public string Dock_name { get; set; }

        public int? count_drop { get; set; }

        public int? count_plan { get; set; }
        
        public int count_retrun { get; set; }

        public int? count_carton { get; set; }

        public int? count_tote_L { get; set; }

        public int? count_tote_M { get; set; }
        
        public string branch { get; set; }
        
        public int? drop_seq { get; set; }
        
        public int? plan_runing { get; set; }
        
        public string PlanGoodsIssue_No { get; set; }
        
        public string ShipTo_Name { get; set; }
        
        public string ShipTo_Address { get; set; }

        public string ShipTo_Tel { get; set; }

        public int? carton { get; set; }

        public int? toteL { get; set; }

        public int? toteM { get; set; }
        
        public int? Row_Runing_product { get; set; }
        
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
    }
}
