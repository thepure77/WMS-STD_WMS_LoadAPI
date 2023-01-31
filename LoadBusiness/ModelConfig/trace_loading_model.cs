using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LoadBusiness.Load
{
    public class trace_loading_model
    {
        public string TruckLoad_No { get; set; }

        public string TruckLoad_Index { get; set; }

        public string Dock_Index { get; set; }

        public string Dock_Id { get; set; }

        public string Dock_Name { get; set; }

        public string Chute_Id { get; set; }

        public string RollCage_Name { get; set; }

        public string RollCage_Index { get; set; }

        public string RollCage_Id { get; set; }
    }

    public class Dock
    {
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

    public partial class RollCage
    {
        public Guid RollCage_Index { get; set; }

        public string RollCage_Id { get; set; }

        public string RollCage_Name { get; set; }

        public string RollCage_SecondName { get; set; }

        public int? RollCage_Status { get; set; }

        public Guid RollCageType_Index { get; set; }

        public Guid? Location_Index { get; set; }

        public string Location_Id { get; set; }

        public string Location_Name { get; set; }

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

        public string Location_Name_Before { get; set; }

    }

    public partial class Appointment_time
    {
        public string Appointment_Time { get; set; }

    }
}
