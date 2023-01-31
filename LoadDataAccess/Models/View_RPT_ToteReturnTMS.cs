using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace PlanGIDataAccess.Models
{
    public partial class View_RPT_ToteReturnTMS
    {
        [Key]

        public Guid TruckLoad_Index { get; set; }

     
        public string TruckLoad_No { get; set; }

       
        public string TM_Index { get; set; }

        public DateTime? TruckLoad_Return_Date { get; set; }

      
        public int? Return_Tote_MAX_XL { get; set; }

        
        public int? Return_Tote_MAX_M { get; set; }

      
        public int? DocReturn_Max { get; set; }

       
        public int Return_Tote_Qty_XL { get; set; }

      
        public int Return_Tote_Qty_M { get; set; }

        
        public int Return_Tote_Qty_DMG_XL { get; set; }

 
        public int Return_Tote_Qty_DMG_M { get; set; }

     
        public int Return_Doc { get; set; }

        public DateTime? Appointment_date { get; set; }
        public string VehicleCompany_Name { get; set; }
    }
}
