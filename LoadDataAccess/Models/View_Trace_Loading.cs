using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace LoadDataAccess.Models
{
    public partial class View_Trace_Loading
    {
        [Key]
        public long? RowIndex { get; set; }

        public string TruckLoad_No { get; set; }
        
        public string Dock_Name { get; set; }
        
        public string Appointment_Time { get; set; }

        public DateTime? TruckLoad_Date { get; set; }

        public string PlanGoodsIssue_No { get; set; }

        public string Chute_Id { get; set; }

        public string RollCage_Name { get; set; }
        
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
    }
}
