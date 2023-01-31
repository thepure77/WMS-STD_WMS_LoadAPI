using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace PlanGIDataAccess.Models
{

    public partial class View_PrintOutPlanGIV2
    {

        [Key]
        public long? Row_Index { get; set; }

        public Guid? PlanGoodsIssue_Index { get; set; }
        public Guid? PlanGoodsIssueItem_Index { get; set; }
        public string PlanGoodsIssue_No { get; set; }
        public Guid? Product_Index { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public decimal? Qty { get; set; }
        public Guid? ProductConversion_Index { get; set; }
        public string ProductConversion_Id { get; set; }
        public string ProductConversion_Name { get; set; }
        public string LineNum { get; set; }
        public Guid? ShipTo_Index { get; set; }
        public string ShipTo_Id { get; set; }
        public string ShipTo_Name { get; set; }
        public string ShipTo_Address { get; set; }
     

    }
}
