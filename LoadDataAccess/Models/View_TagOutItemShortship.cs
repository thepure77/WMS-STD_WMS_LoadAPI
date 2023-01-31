using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LoadDataAccess.Models
{
    public class View_TagOutItemShortship
    {
        [Key]
        public long? RowIndex { get; set; }
        public Guid? ShortShipItem_Index { get; set; }
        public Guid? PlangoodsIssue_index { get; set; }
        public string PlanGoodsIssue_No { get; set; }
        public Guid? PlanGoodsIssueItem_Index { get; set; }
        public Guid? GoodsIssueItemLocation_Index { get; set; }
        public string LineNum { get; set; }
        public string Product_Id { get; set; }
        public string Product_name { get; set; }
        public Guid? TagOut_Index { get; set; }
        public string TagOut_No { get; set; }
        public decimal? IssueQty { get; set; }
        public decimal? OrderQty { get; set; }
        public decimal? ShortQty { get; set; }
        public decimal? ReceiveQty { get; set; }
        public string productConversion_Name { get; set; }
        public string toteSize { get; set; }
        public string StatusItem { get; set; }
        public string OrderSeq { get; set; }
        public string DropSeq { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_Date { get; set; }
    }
}
