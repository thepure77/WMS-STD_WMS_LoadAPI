using System;
using System.Collections.Generic;
using System.Text;

namespace MasterDataBusiness.VehicleCompany
{
    public class VehicleCompanyViewModel
    {
        public Guid vehicleCompany_Index { get; set; }

        public string vehicleCompany_Id { get; set; }

        public string vehicleCompany_Name { get; set; }

        public string vehicleCompany_Address { get; set; }

        public Guid? vehicleCompanyType_Index { get; set; }

        public Guid? country_Index { get; set; }

        public string country_Id { get; set; }

        public string country_Name { get; set; }

        public Guid? province_Index { get; set; }

        public string province_Id { get; set; }

        public string province_Name { get; set; }

        public Guid? district_Index { get; set; }

        public string district_Id { get; set; }

        public string district_Name { get; set; }

        public Guid? subDistrict_Index { get; set; }

        public string subDistrict_Id { get; set; }

        public string subDistrict_Name { get; set; }

        public string postcode { get; set; }

        public string vehicleCompany_TaxID { get; set; }

        public string vehicleCompany_Email { get; set; }

        public string vehicleCompany_Fax { get; set; }

        public string vehicleCompany_Tel { get; set; }

        public string vehicleCompany_Mobile { get; set; }

        public string vehicleCompany_Barcode { get; set; }

        public string contact_Person { get; set; }

        public string contact_Person2 { get; set; }

        public string contact_Person3 { get; set; }

        public string contact_Tel { get; set; }

        public string contact_Tel2 { get; set; }

        public string contact_Tel3 { get; set; }

        public string contact_Email { get; set; }

        public string contact_Email2 { get; set; }

        public string contact_Email3 { get; set; }

        public int? isActive { get; set; }

        public int? isDelete { get; set; }

        public int? isSystem { get; set; }

        public int? status_Id { get; set; }

        public string create_By { get; set; }

        public DateTime? create_Date { get; set; }

        public string update_By { get; set; }

        public DateTime? update_Date { get; set; }

        public string cancel_By { get; set; }

        public DateTime? cancel_Date { get; set; }
    }
}
