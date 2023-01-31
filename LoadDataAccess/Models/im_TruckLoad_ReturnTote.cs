using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace LoadDataAccess.Models
{

    public partial class im_TruckLoad_ReturnTote
    {
        [Key]
        public Guid TruckReturn_Index { get; set; }

        public Guid TruckLoad_Index { get; set; }

        public string TruckLoad_No { get; set; }

        public string Vehicle_Registration { get; set; }

        public DateTime? TruckLoad_Date { get; set; }


        public string Return_Tote_MAX { get; set; }

        public string Return_Tote_Size { get; set; }

        public string Return_Tote_Qty { get; set; }

        public string DocReturn { get; set; }

        public string Retrun_DocReturn { get; set; }

        public string Create_By { get; set; }

        public DateTime? Create_Date { get; set; }

        public string Update_By { get; set; }

        public DateTime? Update_Date { get; set; }

        public string Cancel_By { get; set; }

        public DateTime? Cancel_Date { get; set; }

    }
}
