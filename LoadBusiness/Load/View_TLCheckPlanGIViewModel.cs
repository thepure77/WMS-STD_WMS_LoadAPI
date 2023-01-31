using System;
using System.Collections.Generic;
using System.Text;

namespace LoadBusiness.Load
{
    public class View_TLCheckPlanGIViewModel
    {

        public Guid? planGoodsIssue_Index { get; set; }
        public string planGoodsIssue_No { get; set; }
        public Guid? shipTo_Index { get; set; }

        public string shipTo_Id { get; set; }

        public string shipTo_Name { get; set; }

        public Guid? goodsIssue_Index { get; set; }

        public string goodsIssue_No { get; set; }
        public int? document_Status { get; set; }
   
        public Guid? documentType_Index { get; set; }

        public string documentType_Id { get; set; }

        public string documentType_Name { get; set; }

        public Guid? tagOut_Index { get; set; }

        public string tagOut_No { get; set; }

        public decimal qty { get; set; }

        public string update_By { get; set; }

        public DateTime? truckLoad_Date { get; set; }

        public DateTime? update_Date { get; set; }
        public string processStatus_Name { get; set; }
        public Guid? truckLoad_Index { get; set; }
        public string product_Name { get; set; }
        public string product_Id { get; set; }
        public string productConversion_Name { get; set; }
        public string product_Size { get; set; }


    }
}
