using LoadBusiness;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PlanGIBusiness.Reports.Trace_loading
{
    public class Trace_loading : Pagination
    {
        public long? RowIndex { get; set; }

        public string TruckLoad_No { get; set; }

        public string TruckLoad_Index { get; set; }

        public string Dock_Index { get; set; }

        public string Dock_Id { get; set; }

        public string Dock_Name { get; set; }

        public string Appointment_Time { get; set; }

        public DateTime? TruckLoad_Date { get; set; }

        public string PlanGoodsIssue_No { get; set; }

        public string PlanGoodsIssue_Index { get; set; }

        public string Chute_Id { get; set; }

        public string RollCage_Name { get; set; }

        public string RollCage_Index { get; set; }

        public string RollCage_Id { get; set; }

        public string IsTote { get; set; }

        public string TagOut_No { get; set; }

        public string Product_Id { get; set; }

        public string Product_Name { get; set; }

        public decimal? Qty { get; set; }

        public string ProductConversion_Name { get; set; }

        public string Product_Lot { get; set; }

        public string status { get; set; }

        public string LocationType { get; set; }

        public string TagOutRef_No1 { get; set; }

        public string Address { get; set; }

        public string load_Date { get; set; }

        public string load_Date_To { get; set; }

        public bool export { get; set; }

    }

    public class actionResultTrace_loadingViewModel
    {
        public IList<Trace_loading> itemsTrace { get; set; }
        public Pagination pagination { get; set; }
    }
}
