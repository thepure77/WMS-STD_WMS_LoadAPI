using System;
using System.Collections.Generic;
using System.Text;

namespace LoadBusiness.ShortShip
{
    public class ShortShipItemViewModel
    {
        public Guid? shortShipItem_Index { get; set; }
        public Guid? plangoodsIssue_index { get; set; }
        public string planGoodsIssue_No { get; set; }
        public Guid? planGoodsIssueItem_Index { get; set; }
        public Guid? goodsIssueItemLocation_Index { get; set; }
        public string product_Id { get; set; }
        public string product_name { get; set; }
        public Guid? tagOut_Index { get; set; }
        public string tagOut_No { get; set; }
        public decimal? issueQty { get; set; }
        public decimal? orderQty { get; set; }
        public decimal? shortQty { get; set; }
        public decimal? receiveQty { get; set; }
        public string productConversion_Name { get; set; }
        public string toteSize { get; set; }
        public string drop_orderSeq { get; set; }
        public string orderSeq { get; set; }
        public string dropSeq { get; set; }
        public string statusItem { get; set; }
        public string update_By { get; set; }
        public string update_Date { get; set; }
        public string tagOutNo { get; set; }
        public string dropOrderSeq { get; set; }
        public class actionResult
        {
            public Boolean Message { get; set; }
        }
    }
}
