using System;
using System.Collections.Generic;
using System.Text;

namespace LoadBusiness.Reports.ShortShip
{
    public class ShortShip_ReportModel
    {
        public long? RowIndex { get; set; }
        public string copy_int { get; set; }
        public string for_report_int { get; set; }
        public string for_report { get; set; }
        public string appointment_Id { get; set; }
        public string appointment_Date { get; set; }
        public string appointment_Time { get; set; }
        public decimal? diff { get; set; }
        public string truckLoad_No { get; set; }
        public string truckLoad_Barcode { get; set; }
        public string order_Seq { get; set; }
        public string planGoodsIssue_No { get; set; }
        public string planGoodsIssue_Date { get; set; }
        public string branch_Code { get; set; }
        public string shipTo_Id { get; set; }
        public string shipTo_Name { get; set; }
        public string shipTo_Address { get; set; }
        public string lineNum { get; set; }
        public int? Row_Runing_product { get; set; }
        public string product_Id { get; set; }
        public string product_Name { get; set; }
        public decimal? bu_Order_TotalQty { get; set; }
        public decimal? bu_GI_TotalQty { get; set; }
        public decimal? su_Order_TotalQty { get; set; }
        public decimal? su_GI_TotalQty { get; set; }
        public string su_Unit { get; set; }
        public string erp_Location { get; set; }
        public string product_Lot { get; set; }
        public string goodsIssue_No { get; set; }

    }
}
