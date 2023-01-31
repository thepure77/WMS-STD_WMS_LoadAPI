using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace PlanGIDataAccess.Models
{

    public class im_PlanGoodsIssue_Ref
    {

        [Key]
        public Guid PlanGoodsIssue_Ref_Index { get; set; }

        public Guid PlanGoodsIssue_Index { get; set; }

        public string PlanGoodsIssue_No { get; set; }
        public string SO_No { get; set; }
        public string SO_Type { get; set; }
        public string SHIP_CON { get; set; }
        public string DO_Type { get; set; }
        public string RTN_Flag { get; set; }
        public string CREDIT_STATUS { get; set; }
        public string EXPORT_FLAG { get; set; }
        public string FOC_FLAG { get; set; }
        public string CLAIM_FLAG { get; set; }
        public string Delivery_Date { get; set; }
        public string Ref_Type { get; set; }

        public string GV_KNDNR { get; set; }
        public string GV_OIFBPR{ get; set; }
        public string GV_OIFPBL { get; set; }
        public string GV_OIFWE { get; set; }
        public string GV_OPERATINGCONCERN { get; set; }
        public string TAR_VAL { get; set; }
    }
}
