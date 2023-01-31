using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIDataAccess.Models
{
    public partial class View_TLCheckPlanGI
    {
        [Key]
        public long? RowIndex { get; set; }
        public Guid? PlanGoodsIssue_Index { get; set; }


        public string PlanGoodsIssue_No { get; set; }
        public Guid? ShipTo_Index { get; set; }

        public string ShipTo_Id { get; set; }

        public string ShipTo_Name { get; set; }

        public Guid? GoodsIssue_Index { get; set; }

        public string GoodsIssue_No { get; set; }
        public int? Document_Status { get; set; }

        public Guid? DocumentType_Index { get; set; }

        public string DocumentType_Id { get; set; }

        public string DocumentType_Name { get; set; }

        public Guid? TagOut_Index { get; set; }

        public string TagOut_No { get; set; }

        public decimal Qty { get; set; }

        public string Update_By { get; set; }

        public DateTime? TruckLoad_Date { get; set; }

        public DateTime? Update_Date { get; set; }
        public Guid? TruckLoad_Index { get; set; }

        public string Product_Name { get; set; }

        public string Product_Id { get; set; }
        public string ProductConversion_Name { get; set; }
        public string Product_Size { get; set; }




    }
}
