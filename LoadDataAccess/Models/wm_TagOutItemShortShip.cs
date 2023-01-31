using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LoadDataAccess.Models
{
    public class wm_TagOutItemShortShip
    {
        [Key]
        public Guid? ShortShipItem_Index { get; set; }
        public Guid? PlanGoodsIssueItem_Index { get; set; }
        public Guid? PlangoodsIssue_index { get; set; }
        public string PlanGoodsIssue_No { get; set; }
        public Guid? GoodsIssueItemLocation_Index { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public Guid? TagOut_Index { get; set; }
        public string TagOut_No { get; set; }
        public decimal? IssueQty { get; set; }
        public decimal? OrderQty { get; set; }
        public decimal? ShortQty { get; set; }
        public string ProductConversion_Name { get; set; }
        public string OrderSeq { get; set; }
        public string DropSeq { get; set; }
        public string Create_By { get; set; }
        public DateTime? Create_Date { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_Date { get; set; }
        public string Cancel_By { get; set; }
        public DateTime? Cancel_Date { get; set; }
    }
}
