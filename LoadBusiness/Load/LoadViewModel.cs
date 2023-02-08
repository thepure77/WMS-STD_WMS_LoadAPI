using LoadBusiness.Upload;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoadBusiness.Load
{
    public class LoadViewModel
    {
        public Guid truckLoad_Index { get; set; }

        public string truckLoad_No { get; set; }

        public string truckLoad_Date { get; set; }

        public string vehicle_Registration { get; set; }

        public double? weight_in { get; set; }

        public double? weight_out { get; set; }

        public string time_in { get; set; }

        public string time_out { get; set; }

        public string start_load { get; set; }

        public string end_load { get; set; }

        public Guid? documentType_Index { get; set; }

        public string documentType_Id { get; set; }

        public string documentType_Name { get; set; }

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

        public string approve_By { get; set; }

        public DateTime? approve_Date { get; set; }
        public Guid? vehicleType_Index { get; set; }

        public string vehicleType_Id { get; set; }
        public string vehicleType_Name { get; set; }

        public Guid? vehicleCompany_Index { get; set; }

        public string vehicleCompany_Id { get; set; }
        public string vehicleCompany_Name { get; set; }

        public List<LoadItemViewModel> listTruckLoadItemViewModel { get; set; }

        public List<TruckLoadImageViewModel> docfile { get; set; }
        public string processStatus_Name { get; set; }

        public class actionResult
        {
            public string document_No { get; set; }
            public Boolean Message { get; set; }
        }
        public class listTruckload
        {
            public List<LoadViewModel> ListTruckload { get; set; }
        }

        public class DemoCallbackResponseViewModel
        {
            public string status { get; set; }
            public string message { get; set; }
            public DemoCallbackResponseItemViewModel data { get; set; }
        }

        public class DemoCallbackResponseItemViewModel
        {
            public string logId { get; set; }
        }
    }
}
