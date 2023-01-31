using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PlanGIBusiness.Reports.TruckMenifest
{
    public class TruckMenifestModel
    {
        public long? RowIndex { get; set; }
        public string truckLoad_Barcode { get; set; }

        public string appointment_Barcode { get; set; }

        public string planGoodsIssue_Barcode { get; set; }



        public Guid TruckLoad_Index { get; set; }


        public string TruckLoad_No { get; set; }


        public string Driver_name { get; set; }


        public string Vehicle_Registration { get; set; }

        public string Remark { get; set; }
        public string VehicleCompany_Id { get; set; }


        public string VehicleType_Name { get; set; }


        public string DocumentRef_No3 { get; set; }


        public string Appointment_Date { get; set; }


        public string Appointment_Time { get; set; }


        public string Dock_name { get; set; }


        public string Appointment_Id { get; set; }

        public int? count_drop { get; set; }

        public int? count_plan { get; set; }




        public int count_retrun { get; set; }

        public int? count_carton { get; set; }

        public int? count_tote_L { get; set; }

        public int? count_tote_M { get; set; }

        public int? drop_seq { get; set; }


        public string branch { get; set; }


        public string ShipTo_Name { get; set; }


        public string ShipTo_Address { get; set; }


        public string Shipto_tel { get; set; }

        public string plan_runing { get; set; }


        public string PlanGoodsIssue_No { get; set; }


        public string ProductConversion_Name { get; set; }


        public string Billing { get; set; }


        public string PGI_type { get; set; }
        public string PGI_set { get; set; }


        public string ShippingMethod_Name { get; set; }
        public string Type_shipment { get; set; }
        public string DocumentRef_No4 { get; set; }

        public DateTime? PlanGoodsIssue_Due_Date { get; set; }
        public string Expect_Delivery_Date { get; set; }
    }

}
