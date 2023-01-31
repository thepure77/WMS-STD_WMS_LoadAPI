using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace LoadBusiness.Upload
{

    public partial class TruckLoadImageViewModel
    {
        //public Guid truckLoadImage_Index { get; set; }

        //public Guid truckLoad_Index { get; set; }

        //public string imageUrl { get; set; }

        //public string imageType { get; set; }

        //public string remark { get; set; }

        //public string create_By { get; set; }

        //public DateTime? create_Date { get; set; }
        //public string name { get; set; }
        //public string base64 { get; set; }
        //public string type { get; set; }
        //public string src { get; set; }

        //public class actionResult
        //{
        //    public string document_No { get; set; }
        //    public string Message { get; set; }
        //}

        public Guid truckLoadImage_Index { get; set; }

        public Guid truckLoad_Index { get; set; }

        public string truckLoad_No { get; set; }

        public int document_Status { get; set; }

        public string imageUrl { get; set; }

        public string imageType { get; set; }

        public string remark { get; set; }

        public string create_By { get; set; }

        public DateTime? create_Date { get; set; }

        public string update_By { get; set; }

        public DateTime? update_Date { get; set; }

        public string cancel_By { get; set; }

        public DateTime? cancel_Date { get; set; }

        public string name { get; set; }
        public string base64 { get; set; }
        public string type { get; set; }
        public string src { get; set; }
    }
}
