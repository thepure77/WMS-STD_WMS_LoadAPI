using System;
using System.Collections.Generic;
using System.Text;

namespace LoadBusiness.Load
{
    public class ScanLoadItemViewModel
    {
        public Guid truckLoadItem_Index { get; set; }

        public Guid truckLoad_Index { get; set; }

        public Guid? goodsIssue_Index { get; set; }

        public string goodsIssue_No { get; set; }

        public string goodsIssue_Date { get; set; }

        public string documentType_Name { get; set; }

        public string processStatus_Name { get; set; }

        public string documentRef_No1 { get; set; }

        public string documentRef_No2 { get; set; }

        public string documentRef_No3 { get; set; }

        public string documentRef_No4 { get; set; }

        public string documentRef_No5 { get; set; }

        public string document_Remark { get; set; }

        public int? document_Status { get; set; }

        public string udf_1 { get; set; }

        public string udf_2 { get; set; }

        public string udf_3 { get; set; }

        public string udf_4 { get; set; }

        public string udf_5 { get; set; }


        public string create_By { get; set; }

        public DateTime? create_Date { get; set; }

        public string update_By { get; set; }

        public DateTime? update_Date { get; set; }

        public string cancel_By { get; set; }

        public DateTime? cancel_Date { get; set; }

        public Guid? planGoodsIssue_Index { get; set; }

        public string planGoodsIssue_No { get; set; }

        public string planGoodsIssue_Date { get; set; }

        public string truckLoad_No { get; set; }
        public string product_Id { get; set; }

        public string planGoodsIssueItem_SN { get; set; }

        public Guid planGoodsIssueItemSN_Index { get; set; }
        public string productConvertionBarcode { get; set; }
        public string product_Name { get; set; }

        public decimal? qty { get; set; }
        public string vehicle_Registration { get; set; }
        public int carton { get; set; }
        public string tote { get; set; }
        public bool plan_check { get; set; }


        public List<ScanLoadItemViewModel> listTruckLoadItemViewModel { get; set; }


        public class actionResultScanLoad
        {
            public Boolean Message { get; set; }
            public string msg { get; set; }

        }


    }
}
