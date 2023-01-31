using System;
using System.Collections.Generic;
using System.Text;
using LoadBusiness;

namespace LoadBusiness.ShortShip
{
    public partial class ShortShipViewModel : Pagination
    {
        public ShortShipViewModel()
        {

            status = new List<statusViewModel>();

        }
        public Guid? appointment_Index { get; set; }
        public string appointment_Id { get; set; }
        public string appointment_Date { get; set; }
        public string appointment_Time { get; set; }
        public Guid? truckLoad_Index { get; set; }
        public string truckLoad_No { get; set; }
        public Guid? planGoodsIssue_Index { get; set; }
        public string planGoodsIssue_No { get; set; }
        public Guid? shipTo_Index { get; set; }
        public string shipTo_Id { get; set; }
        public string shipTo_Address { get; set; }
        public string planGoodsIssue_Date { get; set; }
        public string planGoodsIssue_Due_Date { get; set; }
        public Guid? round_Index { get; set; }
        public string round_Id { get; set; }
        public string round_Name { get; set; }
        public string billing_No { get; set; }
        public string branch_Code { get; set; }
        public string shotShip_Status { get; set; }
        public Guid? documentType_Index { get; set; }
        public string documentType_Id { get; set; }
        public string documnetType_Name { get; set; }
        public string update_By { get; set; }
        public string update_date { get; set; }
        public string shipment_date { get; set; }
        public Decimal? shortQty { get; set; }
        public string truckLoad_Date { get; set; }
        public string truckLoad_Date_To { get; set; }
        //public string status { get; set; }
        public Guid processStatus_Index { get; set; }
        public string processStatus_Id { get; set; }
        public string processStatus_Name { get; set; }

        public List<statusViewModel> status { get; set; }

        public class statusViewModel
        {
            public int value { get; set; }
            public string display { get; set; }
            public int seq { get; set; }
        }

    }

    public class SearchTruckLoad
    {
        public string truckLoad_No { get; set; }
        public string planGoodsissue_no { get; set; }
        public string truckLoad_Date { get; set; }
        public string truckLoad_Date_To { get; set; }
        public string status { get; set; }
    }

    public class actionResultViewModel : Result
    {
        public IList<ShortShipViewModel> shortShipList { get; set; }
        public Pagination pagination { get; set; }
    }
}
