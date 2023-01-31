using LoadBusiness.Upload;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoadBusiness.Load
{
    public class SearchreturntoteViewModel : Result
    {


        public string TruckLoad_No { get; set; }

        public string TruckLoad_Return_Date { get; set; }


        public string Return_Tote_MAX_XL { get; set; }


        public string Return_Tote_MAX_M { get; set; }


        public string DocReturn_Max { get; set; }


        public int Return_Tote_Qty_XL { get; set; }


        public int Return_Tote_Qty_M { get; set; }


        public int Return_Tote_Qty_DMG_XL { get; set; }


        public int Return_Tote_Qty_DMG_M { get; set; }


        public int Return_Doc { get; set; }

        public string Appointment_date { get; set; }
        public string VehicleCompany_Name { get; set; }


    }
}

