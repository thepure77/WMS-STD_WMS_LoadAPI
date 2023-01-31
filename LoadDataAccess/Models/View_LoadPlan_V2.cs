using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIDataAccess.Models
{
    public partial class View_LoadPlan_V2
    {
        [Key]
        public long? RowIndex { get; set; }

        public Guid? TruckLoad_Index { get; set; }
        public string TruckLoad_No { get; set; }


        public Guid? PlanGoodsIssue_Index { get; set; }
        public string PlanGoodsIssue_No { get; set; }
        public decimal? Qty { get; set; }
        public int carton { get; set; }
        public string tote { get; set; }


    }
}
