using AspNetCore.Reporting;
using Business.Library;
using Comone.Utils;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;


namespace LoadBusiness.Load
{
    public class LoadItemService
    {

        private LoadDbContext db;

        public LoadItemService()
        {
            db = new LoadDbContext();
        }

        public LoadItemService(LoadDbContext db)
        {
            this.db = db;
        }

        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));

            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        #region find
        public List<LoadItemViewModel> find(Guid id)
        {

            try
            {
                var result = new List<LoadItemViewModel>();

                //var queryResult = db.im_TruckLoadItem.Where(c => c.TruckLoad_Index == id && c.Document_Status != -1).ToList();

                var queryResult = from tli in db.im_TruckLoadItem
                                  join planGi in db.im_PlanGoodsIssue on tli.PlanGoodsIssue_Index equals planGi.PlanGoodsIssue_Index
                                  join planGii in db.im_PlanGoodsIssueItem on planGi.PlanGoodsIssue_Index equals planGii.PlanGoodsIssue_Index
                                  where tli.TruckLoad_Index == id && tli.Document_Status != -1
                                  group planGii by new
                                  {
                                      tli.TruckLoadItem_Index,
                                      tli.TruckLoad_Index,
                                      planGi.PlanGoodsIssue_Index,
                                      planGi.PlanGoodsIssue_No,
                                      planGi.PlanGoodsIssue_Date,
                                      planGi.DocumentType_Name,
                                      planGi.Document_Status,
                                      tli.DocumentRef_No1,
                                      tli.DocumentRef_No2,
                                      tli.DocumentRef_No3,
                                      tli.DocumentRef_No4,
                                      tli.DocumentRef_No5,
                                      //tli.Document_Status,
                                      tli.Document_Remark,
                                      tli.UDF_1,
                                      tli.UDF_2,
                                      tli.UDF_3,
                                      tli.UDF_4,
                                      tli.UDF_5,
                                      //planGi.Create_By,
                                      //tli.Create_Date,
                                      //tli.Update_By,
                                      //tli.Update_Date,
                                      planGi.ShipTo_Name
                                  } into g
                                  select new
                                  {
                                      g.Key.TruckLoadItem_Index,
                                      g.Key.TruckLoad_Index,
                                      g.Key.PlanGoodsIssue_Index,
                                      g.Key.PlanGoodsIssue_No,
                                      g.Key.PlanGoodsIssue_Date,
                                      g.Key.DocumentType_Name,
                                      g.Key.Document_Status,
                                      g.Key.DocumentRef_No1,
                                      g.Key.DocumentRef_No2,
                                      g.Key.DocumentRef_No3,
                                      g.Key.DocumentRef_No4,
                                      g.Key.DocumentRef_No5,
                                      totalqay = g.Sum(s => s.TotalQty),//tli.Document_Status,
                                      g.Key.Document_Remark,
                                      g.Key.UDF_1,
                                      g.Key.UDF_2,
                                      g.Key.UDF_3,
                                      g.Key.UDF_4,
                                      g.Key.UDF_5,
                                      //g.Key.Create_By,
                                      //g.Key.Create_Date,
                                      //g.Key.Update_By,
                                      //g.Key.Update_Date,
                                      g.Key.ShipTo_Name
                                  };

                var truckloca = db.im_TruckLoad.Find(queryResult.FirstOrDefault().TruckLoad_Index);
                foreach (var item in queryResult)
                {
                    var resultItem = new LoadItemViewModel();


                    var StatusModel = new ProcessStatusViewModel();

                    var StatusName = new List<ProcessStatusViewModel>();

                    StatusModel.process_Index = new Guid("2E026669-99BD-4DE0-8818-534F29F7B89D");

                    StatusModel.processStatus_Id = item.Document_Status.ToString();

                    //GetConfig
                    StatusName = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("processStatus"), StatusModel.sJson());

                    resultItem.truckLoadItem_Index = item.TruckLoadItem_Index;
                    resultItem.truckLoad_Index = item.TruckLoad_Index;
                    //resultItem.goodsIssue_Index = item.GoodsIssue_Index;
                    //resultItem.goodsIssue_No = item.GoodsIssue_No;
                    //resultItem.goodsIssue_Date = item.GoodsIssue_Date.toString();

                    resultItem.shipTo_Name = item.ShipTo_Name;
                    resultItem.qty = item.totalqay;

                    resultItem.planGoodsIssue_Index = item.PlanGoodsIssue_Index;
                    resultItem.planGoodsIssue_No = item.PlanGoodsIssue_No;
                    resultItem.planGoodsIssue_Date = item.PlanGoodsIssue_Date.toString();

                    resultItem.documentType_Name = item.DocumentType_Name;
                    resultItem.documentRef_No1 = item.DocumentRef_No1;
                    resultItem.documentRef_No2 = item.DocumentRef_No2;
                    resultItem.documentRef_No3 = item.DocumentRef_No3;
                    resultItem.documentRef_No4 = item.DocumentRef_No4;
                    resultItem.documentRef_No5 = item.DocumentRef_No5;
                    resultItem.document_Status = item.Document_Status;
                    resultItem.processStatus_Name = (StatusName.Count > 0) ? StatusName.FirstOrDefault().processStatus_Name : "";
                    resultItem.document_Remark = item.Document_Remark;
                    resultItem.udf_1 = item.UDF_1;
                    resultItem.udf_2 = item.UDF_2;
                    resultItem.udf_3 = item.UDF_3;
                    resultItem.udf_4 = item.UDF_4;
                    resultItem.udf_5 = item.UDF_5;
                    resultItem.create_By = truckloca.Create_By;
                    resultItem.create_Date = truckloca.Create_Date;
                    resultItem.update_By = truckloca.Update_By;
                    resultItem.update_Date = truckloca.Update_Date;

                    result.Add(resultItem);

                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public List<View_TLCheckPlanGIViewModel> findDetail(Guid id)
        {
            try
            {
                var result = new List<View_TLCheckPlanGIViewModel>();

                var query = db.View_TLCheckPlanGI.Where(c => c.TruckLoad_Index == id).ToList();

                foreach (var item in query)
                {
                    var resultItem = new View_TLCheckPlanGIViewModel();

                    resultItem.planGoodsIssue_No = item.PlanGoodsIssue_No;
                    resultItem.tagOut_No = item.TagOut_No;
                    resultItem.qty = item.Qty;
                    resultItem.product_Size = item.Product_Size;
                    resultItem.product_Id = item.Product_Id;
                    resultItem.product_Name = item.Product_Name;
                    resultItem.productConversion_Name = item.ProductConversion_Name;

                    result.Add(resultItem);

                }

                return result;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}