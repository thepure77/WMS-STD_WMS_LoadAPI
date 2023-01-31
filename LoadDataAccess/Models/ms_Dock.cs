using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace LoadDataAccess.Models
{

    public class Ms_Dock
    {
        [Key]
        public Guid Dock_Index { get; set; }

        public string Dock_Id { get; set; }

        public string Dock_Name { get; set; }

        public Guid? DockType_Index { get; set; }

        public Guid? DockZone_Index { get; set; }

        public int? IsActive { get; set; }

        public int? IsDelete { get; set; }

        public int? IsSystem { get; set; }

        public int? Status_Id { get; set; }

        public string Create_By { get; set; }

        public DateTime? Create_Date { get; set; }

        public string Update_By { get; set; }

        public DateTime? Update_Date { get; set; }

        public string Cancel_By { get; set; }

        public DateTime? Cancel_Date { get; set; }

        public DateTime? Last_Checkin { get; set; }

        public DateTime? Last_Checkout { get; set; }

        public int Dock_use { get; set; }
        public int seq { get; set; }

    }
}

