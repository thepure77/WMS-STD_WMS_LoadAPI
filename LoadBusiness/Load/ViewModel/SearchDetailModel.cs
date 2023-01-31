using System;
using System.Collections.Generic;
using System.Text;

namespace LoadBusiness.Load
{
    public partial class SearchDetailModel : Pagination
    {
        public SearchDetailModel()
        {
            sort = new List<sortViewModel>();

            status = new List<statusViewModel>();

        }

        public Guid truckLoad_Index { get; set; }

        public string truckLoad_No { get; set; }

        public string truckLoad_Date { get; set; }

        public string truckLoad_Date_To { get; set; }

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
        public string Dock_Name { get; set; }
        public string Appointment_Time { get; set; }

        public DateTime? cancel_Date { get; set; }

        public string approve_By { get; set; }

        public DateTime? approve_Date { get; set; }

        public string key { get; set; }

        public string processStatus_Name { get; set; }

        public bool advanceSearch { get; set; }
        public DateTime? time { get; set; }

        public List<sortViewModel> sort { get; set; }
        public List<statusViewModel> status { get; set; }


        public class actionResultViewModel
        {
            public IList<SearchDetailModel> itemsPlanGI { get; set; }
            public Pagination pagination { get; set; }
        }

        public class sortViewModel
        {
            public string value { get; set; }
            public string display { get; set; }
            public int seq { get; set; }
        }

        public class statusViewModel
        {
            public int? value { get; set; }
            public string display { get; set; }
            public int seq { get; set; }
        }

        public class SortModel
        {
            public string ColId { get; set; }
            public string Sort { get; set; }

            public string PairAsSqlExpression
            {
                get
                {
                    return $"{ColId} {Sort}";
                }
            }
        }

        public class statusModel
        {
            public string Name { get; set; }
        }
    }
}
