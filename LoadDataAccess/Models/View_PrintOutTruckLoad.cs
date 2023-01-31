using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace PlanGIDataAccess.Models
{

    public partial class View_PrintOutTruckLoad
    {

        [Key]
        public long? Row_Index { get; set; }

        public Guid? TruckLoad_Index { get; set; }

        public string TruckLoad_No { get; set; }
        public string Create_By { get; set; }
        public DateTime? Create_Date { get; set; }
        public DateTime? Expect_Delivery_Date { get; set; }
        public string Document_Remark { get; set; }
        public Guid? TruckLoadItem_Index { get; set; }
        public Guid? PlanGoodsIssue_Index { get; set; }

    }
}
