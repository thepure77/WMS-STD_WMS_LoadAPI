using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace LoadDataAccess.Models
{

    public partial class View_CheckReport_Status
    {
        public long? RowIndex { get; set; }

        [StringLength(50)]
        public string TruckLoad_No { get; set; }

        [Key]
        public Guid TruckLoad_Index { get; set; }

        [StringLength(200)]
        public string GoodsIssue_No { get; set; }

        public int? Document_StatusScanPick { get; set; }

        public int? Document_StatusLabeling { get; set; }

        public int? Document_StatusPickQty { get; set; }

        public int? Document_StatusDocktoStg { get; set; }
    }
}
