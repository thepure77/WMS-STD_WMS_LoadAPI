using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PlanGIBusiness.Reports.PrintOutShipment
{
    public class PrintOutShipmentModel
    {
        public Guid? truckLoad_Index { get; set; }
        public string truckLoad_No { get; set; }
        public string create_By { get; set; }
        public string create_Date { get; set; }
        public string document_Remark { get; set; }
        public string planGoodsIssue_No { get; set; }
        public string product_Index { get; set; }
        public string product_Id { get; set; }
        public string product_Name { get; set; }
        public string productConversion_Name { get; set; }
        public decimal? qty { get; set; }
        public string lineNum { get; set; }
        public string shipTo_Id { get; set; }
        public string shipTo_Name { get; set; }
        public string shipTo_Address { get; set; }
        public decimal? qtyRatio { get; set; }
        public string truckLoad_Barcode { get; set; }
        public string planGoodsIssue_Barcode { get; set; }
        public string print_Date { get; set; }
        public string total_tote { get; set; }
        public string total_carton { get; set; }
        public bool checkQuery { get; set; }
    }

}
