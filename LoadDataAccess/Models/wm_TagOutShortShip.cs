using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LoadDataAccess.Models
{
    public class wm_TagOutShortShip
    {
        [Key]
        public Guid? PlanGoodsIssue_Index { get; set; }
        public string PlanGoodsIssue_No { get; set; }
        public Guid? Appointment_Index { get; set; }
        public string Appointment_Id { get; set; }
        public DateTime? Appointment_Date { get; set; }
        public string Appointment_Time { get; set; }
        public Guid? TruckLoad_Index { get; set; }
        public string TruckLoad_No { get; set; }
        public string Order_Seq { get; set; }
        public Guid? processStatus_Index { get; set; }
        public string ProcessStatus_Id { get; set; }
        public string processStatus_Name { get; set; }
        public string ShortShip_Status { get; set; }
        public Decimal? ShortQty { get; set; }
        public string Create_By { get; set; }
        public DateTime? Create_Date { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_Date { get; set; }
        public string Cancel_By { get; set; }
        public DateTime? Cancel_Date { get; set; }
    }
}
