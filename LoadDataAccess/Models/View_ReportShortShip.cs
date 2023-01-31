using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LoadDataAccess.Models
{
    public class View_ReportShortShip
    {
        [Key]
        public long? RowIndex { get; set; }
        public string appointment_id { get; set; }
        public DateTime? appointment_date { get; set; }
        public string appointment_time { get; set; }
        public decimal? diff { get; set; }
        public string truckload_no { get; set; }
        public string Order_Seq { get; set; }
        public Guid? plangoodsissue_index { get; set; }
        public string plangoodsissue_no { get; set; }
        public DateTime? plangoodsissue_date { get; set; }
        public string branch_code { get; set; }
        public string shipto_id { get; set; }
        public string shipto_name { get; set; }
        public string shipto_address { get; set; }
        public string linenum { get; set; }
        public int? Row_Runing_product { get; set; }
        public string product_id { get; set; }
        public string product_name { get; set; }
        public decimal? BU_Order_TotalQty { get; set; }
        public decimal? BU_GI_TotalQty { get; set; }
        public decimal? SU_Order_TotalQty { get; set; }
        public decimal? SU_GI_TotalQty { get; set; }
        public string SU_Unit { get; set; }
        public string erp_location { get; set; }
        public string product_lot { get; set; }
        public string GoodsIssue_No { get; set; }
    }
}
