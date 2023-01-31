using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MasterDataBusiness.Product
{
    public class ProductDetailViewModel
    {
        [Key]
        [Column(Order = 0)]
        public Guid productConversionBarcode_Index { get; set; }


        [Column(Order = 1)]
        public Guid product_Index { get; set; }


        [Column(Order = 2)]
        [StringLength(50)]
        public string product_Id { get; set; }


        [Column(Order = 3)]
        [StringLength(200)]
        public string product_Name { get; set; }

        [StringLength(200)]
        public string product_SecondName { get; set; }

        [StringLength(200)]
        public string product_ThirdName { get; set; }


        [Column(Order = 4)]
        public Guid productConversion_Index { get; set; }


        [Column(Order = 5)]
        [StringLength(50)]
        public string productConversion_Id { get; set; }


        [Column(Order = 6)]
        [StringLength(200)]
        public string productConversion_Name { get; set; }


        [Column(Order = 7, TypeName = "numeric")]
        public decimal productConversion_Ratio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? productConversion_Weight { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? productConversion_Width { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? productConversion_Length { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? productConversion_Height { get; set; }


        [Column(Order = 8, TypeName = "numeric")]
        public decimal productConversion_VolumeRatio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? productConversion_Volume { get; set; }


        [Column(Order = 9)]
        [StringLength(50)]
        public string productConversionBarcode_Id { get; set; }


        [Column(Order = 10)]
        [StringLength(200)]
        public string productConversionBarcode { get; set; }


        [Column(Order = 11)]
        public Guid owner_Index { get; set; }

        [StringLength(50)]
        public string owner_Id { get; set; }

        [StringLength(200)]
        public string owner_Name { get; set; }


        [Column(Order = 12)]
        public Guid productType_Index { get; set; }


        [Column(Order = 13)]
        [StringLength(50)]
        public string productType_Id { get; set; }


        [Column(Order = 14)]
        [StringLength(200)]
        public string productType_Name { get; set; }


        [Column(Order = 15)]
        public Guid productSubType_Index { get; set; }


        [Column(Order = 16)]
        [StringLength(50)]
        public string productSubType_Id { get; set; }


        [Column(Order = 17)]
        [StringLength(200)]
        public string productSubType_Name { get; set; }


        [Column(Order = 18)]
        public Guid productCategory_Index { get; set; }


        [Column(Order = 19)]
        [StringLength(50)]
        public string productCategory_Id { get; set; }


        [Column(Order = 20)]
        [StringLength(200)]
        public string productCategory_Name { get; set; }


        public int? isExpDate { get; set; }

        public int? isLot { get; set; }

        public int? productItemLife_Y { get; set; }

        public int? productItemLife_M { get; set; }

        public int? productItemLife_D { get; set; }

        public string baseProductConversion { get; set; }

        public int? isMfgDate { get; set; }

        public string suggestLocation { get; set; }

        public int? isCatchWeight { get; set; }
    }
}
