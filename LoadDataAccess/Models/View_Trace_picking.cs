using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace LoadDataAccess.Models
{
    public partial class View_Trace_picking
    {
        [Key]
        public long? RowIndex { get; set; }

        public string Goodsissue_No { get; set; }

        public string TruckLoad_No { get; set; }

        public DateTime? TruckLoad_Date { get; set; }

        public string PlanGoodsIssue_No { get; set; }

        public string Pallet_No { get; set; }

        public string TagOut_No { get; set; }
        
        public string Product_Id { get; set; }
        
        public string Product_Name { get; set; }

        public decimal? Qty { get; set; }
        
        public string ProductConversion_Name { get; set; }
        
        public string Product_Lot { get; set; }

        public string LocationType { get; set; }

        public string Pick_location { get; set; }

        public string Old_location { get; set; }

        public string Current_location { get; set; }
        
        public string status { get; set; }

        public string Chute_Id { get; set; }

        public string RollCage_Id { get; set; }

        public string DocumentRef_No5 { get; set; }

        public string DocumentRef_No2 { get; set; }

        public string TagOutRef_No1 { get; set; }
        public string PickingPickQty_By { get; set; }
        public DateTime? PickingPickQty_Date { get; set; }
    }
}
