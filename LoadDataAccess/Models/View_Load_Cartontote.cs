using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIDataAccess.Models
{
    public partial class View_Load_Cartontote
    {
        [Key]
        public long? RowIndex { get; set; }

        public Guid? TruckLoad_Index { get; set; }
        public string TruckLoad_No { get; set; }
        public string Vehicle_Registration { get; set; }
        public string Size_tote { get; set; }
        public int? Document_Status { get; set; }
        public int? total_Carton { get; set; }
        public int? DocReturn { get; set; }
        public DateTime? TruckLoad_Date { get; set; }
        
    }
}
