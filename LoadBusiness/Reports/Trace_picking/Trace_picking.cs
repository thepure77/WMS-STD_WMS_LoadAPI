using LoadBusiness;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PlanGIBusiness.Reports.Trace_picking
{
    public class Trace_picking : Pagination
    {
        public long? RowIndex { get; set; }

        public bool export { get; set; }

        public string Goodsissue_No { get; set; }

        public string Goodsissue_Index { get; set; }

        public string TruckLoad_No { get; set; }

        public string TruckLoad_Index { get; set; }

        public string value { get; set; }

        public DateTime? TruckLoad_Date { get; set; }

        public string load_Date { get; set; }

        public string load_Date_To { get; set; }

        public string PlanGoodsIssue_No { get; set; }

        public string PlanGoodsIssue_Index { get; set; }

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

    public class actionResultTrace_pickingViewModel
    {
        public IList<Trace_picking> itemsTrace { get; set; }
        public Pagination pagination { get; set; }
    }
}
