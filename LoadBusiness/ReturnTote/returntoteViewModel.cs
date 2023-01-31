using LoadBusiness.Upload;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoadBusiness.Load
{
    public class returntoteViewModel : Result
    {

        public returntoteViewModel()
        {
            status = new List<statusViewModel>();
            returntoteitemLists = new List<returntoteitemListitem>();
            tran_returntoteitemLists = new List<returntoteitemListitem>();
           
        }
        public Guid? truckReturn_Index { get; set; }
        public Guid? truckLoad_Index { get; set; }
        public string shipment_no { get; set; }
        public string truckLoad_no { get; set; }
        public string vehicle_Registration { get; set; }
        public string truckLoad_Date { get; set; }
        public string truckLoad_Date_To { get; set; }
        public string truckLoad_Return_Date { get; set; }
        public string truckLoad_Date_Return { get; set; }
        public string truckLoad_Date_To_Return { get; set; }
        public bool? truckLoad_Return_Status { get; set; }
        public string docReturn { get; set; }
        public string return_docReturn { get; set; }
        public string return_Tote_Size { get; set; }
        public string return_Tote_Qty { get; set; }
        public string return_qty_damage { get; set; }
        public string return_qty { get; set; }
        public int? re_doc { get; set; }
        public int? return_docReturn_XX { get; set; }
        public string user { get; set; }
        public string isDateLoad { get; set; }
        public string isDateRerurn { get; set; }
        public string load_date { get; set; }
        public bool? diff_Doc { get; set; }
        public bool? diff_L { get; set; }
        public bool? diff_M { get; set; }

        public List<statusViewModel> status { get; set; }

        public List<returntoteitemListitem> returntoteitemLists { get; set; }
        public List<returntoteitemListitem> tran_returntoteitemLists { get; set; }
        
    }
    public class statusViewModel
    {
        public int? value { get; set; }
        public string display { get; set; }
        public int seq { get; set; }
    }

    public class returntoteitemListitem
    {
        public Guid? truckReturn_Index { get; set; }
        public Guid? truckLoad_Index { get; set; }
        public string vehicle_Registration { get; set; }
        public string shipment_no { get; set; }
        public string truckLoad_no { get; set; }
        public string truckLoad_Date { get; set; }
        public string truckLoad_Return_Date { get; set; }
        public bool? truckLoad_Return_Status { get; set; }
        public string return_Tote_Size { get; set; }
        public string return_Tote_MAX { get; set; }
        public string return_Tote_Qty { get; set; }
        public string return_qty_damage { get; set; }
        public string docReturn { get; set; }
        public string return_qty { get; set; }
        public string return_docReturn { get; set; }
        public int? re_doc { get; set; }
        public int? return_docReturn_XX { get; set; }
        public string user { get; set; }
        public string remark { get; set; }
        public string load_date { get; set; }

    }
}
