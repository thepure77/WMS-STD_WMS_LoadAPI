using AspNetCore.Reporting;
using Business.Library;
using Comone.Utils;
using DataAccess;
using GIDataAccess.Models;
using GRBusiness.ConfigModel;
using LoadBusiness.AutoNumber;
using LoadBusiness.Upload;
using LoadDataAccess.Models;
using MasterDataBusiness.Product;
using MasterDataBusiness.VehicleCompany;
using MasterDataBusiness.VehicleCompanyType;
using MasterDataBusiness.VehicleType;
using MasterDataBusiness.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PlanGIBusiness.Libs;
using PlanGIBusiness.Reports.DeliveryNote;
using PlanGIBusiness.Reports.Handover;
using PlanGIBusiness.Reports.PrintOutShipment;
using PlanGIBusiness.Reports.Trace_loading;
using PlanGIBusiness.Reports.Trace_picking;
using PlanGIBusiness.Reports.TruckMenifest;
using PlanGIDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using static LoadBusiness.Load.LoadViewModel;
using static LoadBusiness.Load.ScanLoadItemViewModel;
using static LoadBusiness.Load.SearchDetailModel;

namespace LoadBusiness.Load
{
    public class LoadService
    {

        private LoadDbContext db;
        private MasterDbContext Masterdb;

        public LoadService()
        {
            db = new LoadDbContext();
            Masterdb = new MasterDbContext();
        }

        public LoadService(LoadDbContext db, MasterDbContext Masterdb)
        {
            this.db = db;
            this.Masterdb = Masterdb;
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

        #region  autoGoodIssueNo
        public List<ItemListViewModel> autoGoodIssueNo(ItemListViewModel data)
        {
            try
            {
                var query = db.View_GoodsIssueTruckload.AsQueryable();

                if (!string.IsNullOrEmpty(data.key))
                {
                    query = query.Where(c => c.GoodsIssue_No.Contains(data.key) && c.Document_Status != -1);

                }

                var items = new List<ItemListViewModel>();

                var result = query.Select(c => new { c.GoodsIssue_Index, c.GoodsIssue_No }).Distinct().Take(10).ToList();


                foreach (var item in result)
                {
                    var resultItem = new ItemListViewModel
                    {
                        index = item.GoodsIssue_Index,
                        name = item.GoodsIssue_No
                    };
                    items.Add(resultItem);

                }

                return items;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region  checkGI
        public planGoodIssueViewModel checkPlanGI(ItemListViewModel data)
        {
            try
            {
                var query = db.im_PlanGoodsIssue.AsQueryable();

                if (!string.IsNullOrEmpty(data.key))
                {
                    //query = query.Where(c => c.GoodsIssue_No.Contains(data.key));
                    query = query.Where(c => c.PlanGoodsIssue_No == data.key);
                }

                var items = new planGoodIssueViewModel();

                var result = query.Select(c => new
                {
                    c.PlanGoodsIssue_Index,
                    c.PlanGoodsIssue_No,
                    c.PlanGoodsIssue_Date,
                    c.DocumentType_Index,
                    c.DocumentType_Id,
                    c.DocumentType_Name,
                    c.Document_Status,
                    c.Update_By,
                    c.ShipTo_Index,
                    c.ShipTo_Id,
                    c.ShipTo_Name,
                    c.Create_By,
                }).FirstOrDefault();

                var queryPlanItem = db.im_PlanGoodsIssueItem.Where(c => c.PlanGoodsIssue_Index == result.PlanGoodsIssue_Index)
                    .GroupBy(o => o.PlanGoodsIssue_Index)
                    .Select(c => new { SumQty = c.Sum(s => s.Qty) }).FirstOrDefault();

                var StatusModel = new ProcessStatusViewModel();

                var StatusName = new List<ProcessStatusViewModel>();

                StatusModel.process_Index = new Guid("80E8E627-6A2D-4075-9BA6-04B7178C1203");

                StatusModel.processStatus_Id = result.Document_Status.ToString();

                //GetConfig
                StatusName = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("processStatus"), StatusModel.sJson());

                items.planGoodsIssue_Index = result.PlanGoodsIssue_Index;
                items.planGoodsIssue_No = result.PlanGoodsIssue_No;
                items.planGoodsIssue_Date = result.PlanGoodsIssue_Date.toString();
                items.documentType_Index = result.DocumentType_Index;
                items.documentType_Id = result.DocumentType_Id;
                items.documentType_Name = result.DocumentType_Name;
                items.shipTo_Index = items.shipTo_Index;
                items.shipTo_Id = result.ShipTo_Id;
                items.shipTo_Name = result.ShipTo_Name;
                items.update_By = result.Update_By;
                items.qty = queryPlanItem.SumQty;
                items.document_Status = result.Document_Status;
                items.processStatus_Name = (StatusName.Count > 0) ? StatusName.FirstOrDefault().processStatus_Name : "";

                return items;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region find
        public LoadViewModel find(Guid id)
        {

            try
            {
                var queryResult = db.View_TruckLoadProcessStatus.Where(c => c.TruckLoad_Index == id).FirstOrDefault();

                var resultItem = new LoadViewModel();

                var StatusModel = new ProcessStatusViewModel();

                var StatusName = new List<ProcessStatusViewModel>();

                StatusModel.process_Index = new Guid("1150720E-EE32-426D-A98E-6CC659D9AAD5");

                StatusModel.processStatus_Id = queryResult.Document_Status.ToString();

                //GetConfig
                StatusName = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("processStatus"), StatusModel.sJson());

                resultItem.truckLoad_Index = queryResult.TruckLoad_Index;
                resultItem.truckLoad_No = queryResult.TruckLoad_No;
                resultItem.truckLoad_Date = queryResult.TruckLoad_Date.toString();
                resultItem.vehicle_Registration = queryResult.Vehicle_Registration;
                resultItem.weight_in = queryResult.Weight_in;
                resultItem.weight_out = queryResult.Weight_out;
                resultItem.time_in = queryResult.Time_in;
                resultItem.time_out = queryResult.Time_out;
                resultItem.start_load = queryResult.Start_load;
                resultItem.end_load = queryResult.End_load;
                resultItem.documentType_Index = queryResult.DocumentType_Index;
                resultItem.documentType_Name = queryResult.DocumentType_Name;
                resultItem.document_Status = queryResult.Document_Status;
                resultItem.document_Remark = queryResult.Document_Remark;
                resultItem.create_By = queryResult.Create_By;
                resultItem.vehicleType_Index = queryResult.VehicleType_Index;
                resultItem.vehicleType_Id = queryResult.VehicleType_Id;
                resultItem.vehicleType_Name = queryResult.VehicleType_Name;

                resultItem.vehicleCompany_Index = queryResult.VehicleCompany_Index;
                resultItem.vehicleCompany_Id = queryResult.VehicleCompany_Id;
                resultItem.vehicleCompany_Name = queryResult.VehicleCompany_Name;

                resultItem.documentRef_No1 = queryResult.DocumentRef_No1;

                resultItem.processStatus_Name = (StatusName.Count > 0) ? StatusName.FirstOrDefault().processStatus_Name : "";

                resultItem.docfile = new List<TruckLoadImageViewModel>();
                var image = db.im_TruckLoadImages.Where(c => c.TruckLoad_Index == queryResult.TruckLoad_Index && c.Document_Status == 0).ToList();
                foreach (var i in image)
                {
                    var fileimage = new TruckLoadImageViewModel();
                    fileimage.truckLoadImage_Index = i.TruckLoadImage_Index;
                    fileimage.truckLoad_Index = i.TruckLoad_Index.sParse<Guid>();
                    fileimage.imageUrl = i.ImageUrl;
                    fileimage.imageType = i.ImageType;
                    fileimage.name = i.ImageUrl;
                    fileimage.src = i.ImageUrl;
                    fileimage.type = i.ImageType;
                    resultItem.docfile.Add(fileimage);

                }

                return resultItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region filter
        public actionResultViewModel filter(SearchDetailModel model)
        {
            try
            {
                var query = db.View_TruckLoadProcessStatus.AsQueryable();

                //query = query.Where(c => c.Document_Status != -1);

                #region advanceSearch
                if (model.advanceSearch == true)
                {
                    if (!string.IsNullOrEmpty(model.truckLoad_No))
                    {
                        query = query.Where(c => c.TruckLoad_No == (model.truckLoad_No));
                    }

                    if (!string.IsNullOrEmpty(model.document_Status.ToString()))
                    {
                        query = query.Where(c => c.Document_Status == (model.document_Status));
                    }

                    if (!string.IsNullOrEmpty(model.processStatus_Name))
                    {
                        int DocumentStatue = 0;

                        var StatusName = new List<ProcessStatusViewModel>();

                        var StatusModel = new ProcessStatusViewModel();

                        StatusModel.process_Index = new Guid("1150720E-EE32-426D-A98E-6CC659D9AAD5");

                        StatusModel.processStatus_Name = model.processStatus_Name;

                        //GetConfig
                        StatusName = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("processStatus"), StatusModel.sJson());

                        if (StatusName.Count > 0)
                        {
                            DocumentStatue = StatusName.FirstOrDefault().processStatus_Id.sParse<int>();
                        }

                        query = query.Where(c => c.Document_Status == DocumentStatue);
                    }

                    if (!string.IsNullOrEmpty(model.documentType_Index.ToString()) && model.documentType_Index.ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        query = query.Where(c => c.DocumentType_Index == (model.documentType_Index));
                    }

                    if (!string.IsNullOrEmpty(model.truckLoad_Date) && !string.IsNullOrEmpty(model.truckLoad_Date_To))
                    {
                        var dateStart = model.truckLoad_Date.toBetweenDate();
                        var dateEnd = model.truckLoad_Date_To.toBetweenDate();
                        query = query.Where(c => c.TruckLoad_Date >= dateStart.start && c.TruckLoad_Date <= dateEnd.end);
                    }
                    else if (!string.IsNullOrEmpty(model.truckLoad_Date))
                    {
                        var truckLoad_Date_From = model.truckLoad_Date.toBetweenDate();
                        query = query.Where(c => c.TruckLoad_Date >= truckLoad_Date_From.start);
                    }
                    else if (!string.IsNullOrEmpty(model.truckLoad_Date_To))
                    {
                        var truckLoad_Date_To = model.truckLoad_Date_To.toBetweenDate();
                        query = query.Where(c => c.TruckLoad_Date <= truckLoad_Date_To.start);
                    }

                    if (!string.IsNullOrEmpty(model.create_By))
                    {
                        query = query.Where(c => c.Create_By == (model.create_By));
                    }
                }

                #endregion

                #region Basic
                else
                {
                    if (!string.IsNullOrEmpty(model.key))
                    {
                        query = query.Where(c => c.TruckLoad_No.Contains(model.key));
                    }

                    if (!string.IsNullOrEmpty(model.truckLoad_Date) && !string.IsNullOrEmpty(model.truckLoad_Date_To))
                    {
                        var dateStart = model.truckLoad_Date.toBetweenDate();
                        var dateEnd = model.truckLoad_Date_To.toBetweenDate();
                        query = query.Where(c => c.TruckLoad_Date >= dateStart.start && c.TruckLoad_Date <= dateEnd.end);
                    }
                    else if (!string.IsNullOrEmpty(model.truckLoad_Date))
                    {
                        var truckLoad_Date_From = model.truckLoad_Date.toBetweenDate();
                        query = query.Where(c => c.TruckLoad_Date >= truckLoad_Date_From.start);
                    }
                    else if (!string.IsNullOrEmpty(model.truckLoad_Date_To))
                    {
                        var truckLoad_Date_To = model.truckLoad_Date_To.toBetweenDate();
                        query = query.Where(c => c.TruckLoad_Date <= truckLoad_Date_To.start);
                    }

                    var statusModels = new List<int?>();
                    var sortModels = new List<SortModel>();

                    if (model.status.Count > 0)
                    {
                        foreach (var item in model.status)
                        {

                            if (item.value == 0)
                            {
                                statusModels.Add(0);
                            }
                            if (item.value == 1)
                            {
                                statusModels.Add(1);
                            }
                            if (item.value == -1)
                            {
                                statusModels.Add(-1);
                            }
                        }

                        query = query.Where(c => statusModels.Contains(c.Document_Status));
                    }

                    if (model.sort.Count > 0)
                    {
                        foreach (var item in model.sort)
                        {

                            if (item.value == "TruckLoad_No")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "TruckLoad_No",
                                    Sort = "desc"
                                });
                            }
                            if (item.value == "TruckLoad_Date")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "TruckLoad_Date",
                                    Sort = "desc"
                                });
                            }
                            if (item.value == "Weight_Out")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "Weight_Out",
                                    Sort = "desc"
                                });
                            }
                        }
                        query = query.KWOrderBy(sortModels);

                    }

                }

                #endregion

                var Item = new List<View_TruckLoadProcessStatus>();
                var TotalRow = new List<View_TruckLoadProcessStatus>();


                TotalRow = query.OrderByDescending(o => o.Create_Date).ThenByDescending(o => o.Create_Date).ToList();


                if (model.CurrentPage != 0 && model.PerPage != 0)
                {
                    query = query.Skip(((model.CurrentPage - 1) * model.PerPage));
                }

                if (model.PerPage != 0)
                {
                    query = query.Take(model.PerPage);

                }

                Item = query.OrderByDescending(o => o.Create_Date).ThenByDescending(o => o.Create_Date).ToList();
                //var perpages = model.PerPage == 0 ? query.ToList() : query.OrderByDescending(o => o.Create_Date).ThenByDescending(o => o.Create_Date).Skip((model.CurrentPage - 1) * model.PerPage).Take(model.PerPage).ToList();

                var ProcessStatus = new List<ProcessStatusViewModel>();

                var filterModel = new ProcessStatusViewModel();

                filterModel.process_Index = new Guid("1150720E-EE32-426D-A98E-6CC659D9AAD5");

                //GetConfig
                ProcessStatus = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("processStatus"), filterModel.sJson());


                String Statue = "";
                var result = new List<SearchDetailModel>();

                foreach (var item in Item)
                {
                    var resultItem = new SearchDetailModel();
                    resultItem.truckLoad_Index = item.TruckLoad_Index;
                    resultItem.truckLoad_No = item.TruckLoad_No;
                    resultItem.truckLoad_Date = item.TruckLoad_Date.toString();
                    resultItem.vehicle_Registration = item.Vehicle_Registration;
                    resultItem.weight_in = item.Weight_in;
                    resultItem.weight_out = item.Weight_out;
                    resultItem.time_in = item.Time_in;
                    resultItem.time_out = item.Time_out;
                    resultItem.start_load = item.Start_load;
                    resultItem.end_load = item.End_load;
                    resultItem.documentType_Index = item.DocumentType_Index;
                    resultItem.documentType_Name = item.DocumentType_Name;
                    resultItem.document_Status = item.Document_Status;

                    Statue = item.Document_Status.ToString();
                    var ProcessStatusName = ProcessStatus.Where(c => c.processStatus_Id == Statue).FirstOrDefault();
                    resultItem.processStatus_Name = ProcessStatusName.processStatus_Name;

                    resultItem.document_Remark = item.Document_Remark;
                    resultItem.create_By = item.Create_By;
                    resultItem.update_By = item.Update_By;
                    resultItem.cancel_By = item.Cancel_By;
                    resultItem.Dock_Name = item.Dock_Name;
                    resultItem.Appointment_Time = item.Appointment_Time;
                    resultItem.time = item.Appointment_Time == null ? DateTime.Now.TrimTime() : DateTime.Parse(item.Appointment_Time.Split('-')[0].Trim());
                    result.Add(resultItem);
                }
                var count = TotalRow.Count;

                var actionResult = new actionResultViewModel();
                actionResult.itemsPlanGI = result.OrderByDescending(o => o.truckLoad_Date.Substring(0, 8)).ThenByDescending(o => o.time).ToList();
                actionResult.pagination = new Pagination() { TotalRow = count, CurrentPage = model.CurrentPage, PerPage = model.PerPage, };

                return actionResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CreateOrUpdate
        public actionResult CreateOrUpdate(LoadViewModel data)
        {
            Guid TruckLoadIndex = new Guid();
            String TruckLoadNo = "";

            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            Boolean IsNew = false;

            var actionResult = new actionResult();

            try
            {

                //var itemDetail = new List<im_TruckLoadItem>();

                var TruckLoadOld = db.im_TruckLoad.Find(data.truckLoad_Index);

                if (TruckLoadOld == null)
                {
                    IsNew = true;
                    TruckLoadIndex = data.truckLoad_Index;

                    var result = new List<GenDocumentTypeViewModel>();

                    var filterModel = new GenDocumentTypeViewModel();

                    filterModel.process_Index = new Guid("1150720E-EE32-426D-A98E-6CC659D9AAD5");
                    filterModel.documentType_Index = data.documentType_Index;
                    //GetConfig
                    result = utils.SendDataApi<List<GenDocumentTypeViewModel>>(new AppSettingConfig().GetUrl("dropDownDocumentType"), filterModel.sJson());

                    var genDoc = new AutoNumberService();
                    string DocNo = "";
                    DateTime DocumentDate = (DateTime)data.truckLoad_Date.toDate();
                    DocNo = genDoc.genAutoDocmentNumber(result, DocumentDate);



                    im_TruckLoad itemHeader = new im_TruckLoad();
                    var document_status = 0;

                    TruckLoadNo = DocNo;
                    itemHeader.TruckLoad_Index = TruckLoadIndex;
                    itemHeader.TruckLoad_No = DocNo;
                    itemHeader.TruckLoad_Date = data.truckLoad_Date.toDate();
                    itemHeader.Vehicle_Registration = data.vehicle_Registration;
                    itemHeader.Weight_in = data.weight_in;
                    itemHeader.Weight_out = data.weight_out;
                    itemHeader.Time_in = data.time_in;
                    itemHeader.Time_out = data.time_out;
                    //itemHeader.Start_load = data.start_load;
                    //itemHeader.End_load = data.end_load;
                    itemHeader.DocumentType_Index = data.documentType_Index;
                    itemHeader.DocumentType_Id = data.documentType_Id;
                    itemHeader.DocumentType_Name = data.documentType_Name;

                    //itemHeader.DocumentRef_No1 = data.documentRef_No1;
                    //itemHeader.DocumentRef_No2 = data.documentRef_No2;
                    //itemHeader.DocumentRef_No3 = data.documentRef_No3;
                    //itemHeader.DocumentRef_No4 = data.documentRef_No4;
                    //itemHeader.DocumentRef_No5 = data.documentRef_No5;
                    itemHeader.Document_Status = document_status;
                    //itemHeader.UDF_1 = data.udf_1;
                    //itemHeader.UDF_2 = data.udf_2;
                    //itemHeader.UDF_3 = data.udf_3;
                    //itemHeader.UDF_4 = data.udf_4;
                    //itemHeader.UDF_5 = data.udf_5;
                    itemHeader.Document_Remark = data.document_Remark;
                    itemHeader.VehicleCompany_Index = data.vehicleCompany_Index;
                    itemHeader.VehicleCompany_Id = data.vehicleCompany_Id;
                    itemHeader.VehicleCompany_Name = data.vehicleCompany_Name;
                    itemHeader.VehicleType_Index = data.vehicleType_Index;
                    itemHeader.VehicleType_Id = data.vehicleType_Id;
                    itemHeader.VehicleType_Name = data.vehicleType_Name;

                    if (IsNew == true)
                    {
                        itemHeader.Create_By = data.create_By;
                        itemHeader.Create_Date = DateTime.Now;
                    }
                    db.im_TruckLoad.Add(itemHeader);

                    if (data.listTruckLoadItemViewModel != null)
                    {
                        foreach (var item in data.listTruckLoadItemViewModel)
                        {

                            im_TruckLoadItem resultItem = new im_TruckLoadItem();

                            resultItem.TruckLoadItem_Index = Guid.NewGuid();
                            resultItem.TruckLoad_Index = TruckLoadIndex;
                            resultItem.GoodsIssue_Index = item.goodsIssue_Index;
                            resultItem.GoodsIssue_No = item.goodsIssue_No;
                            //resultItem.DocumentRef_No1 = item.documentRef_No1;
                            //resultItem.DocumentRef_No2 = item.documentRef_No2;
                            //resultItem.DocumentRef_No3 = item.documentRef_No3;
                            //resultItem.DocumentRef_No4 = item.documentRef_No4;
                            //resultItem.DocumentRef_No5 = item.documentRef_No5;
                            resultItem.Document_Status = 0;
                            resultItem.Document_Remark = item.document_Remark;
                            //resultItem.UDF_1 = item.udf_1;
                            //resultItem.UDF_2 = item.udf_2;
                            //resultItem.UDF_3 = item.udf_3;
                            //resultItem.UDF_4 = item.udf_4;
                            //resultItem.UDF_5 = item.udf_5;
                            resultItem.PlanGoodsIssue_Index = item.planGoodsIssue_Index;
                            resultItem.PlanGoodsIssue_No = item.planGoodsIssue_No;

                            if (IsNew == true)
                            {
                                resultItem.Create_By = data.create_By;
                                resultItem.Create_Date = DateTime.Now;
                            }
                            db.im_TruckLoadItem.Add(resultItem);

                        }
                    }


                    //save image4
                    if (data.docfile != null)
                    {
                        if (data.docfile.Count > 0)
                        {
                            foreach (var d in data.docfile)
                            {
                                byte[] img = Convert.FromBase64String(d.base64);
                                var path = Directory.GetCurrentDirectory();
                                path += "\\" + "ImageFolder" + "\\";
                                if (!System.IO.Directory.Exists(path))
                                {
                                    System.IO.Directory.CreateDirectory(path);
                                }
                                System.IO.File.WriteAllBytes(new AppSettingConfig().GetUrl("configUploadImg") + d.name, img);

                                im_TruckLoadImages tli = new im_TruckLoadImages();

                                tli.TruckLoadImage_Index = Guid.NewGuid();
                                tli.TruckLoad_Index = TruckLoadIndex;
                                tli.ImageUrl = new AppSettingConfig().GetUrl("configGetImg") + d.name.ToString();
                                tli.ImageType = d.type;
                                tli.Document_Status = 0;
                                tli.Create_By = data.create_By;
                                tli.Create_Date = DateTime.Now;
                                db.im_TruckLoadImages.Add(tli);
                            }
                        }
                    }
                }
                else
                {
                    TruckLoadOld.TruckLoad_Index = data.truckLoad_Index;
                    TruckLoadOld.TruckLoad_No = data.truckLoad_No;
                    TruckLoadOld.TruckLoad_Date = data.truckLoad_Date.toDate();
                    TruckLoadOld.Vehicle_Registration = data.vehicle_Registration;
                    TruckLoadOld.Weight_in = data.weight_in;
                    TruckLoadOld.Weight_out = data.weight_out;
                    TruckLoadOld.Time_in = data.time_in;
                    TruckLoadOld.Time_out = data.time_out;
                    TruckLoadOld.Start_load = data.start_load;
                    TruckLoadOld.End_load = data.end_load;
                    TruckLoadOld.DocumentType_Index = data.documentType_Index;
                    TruckLoadOld.DocumentType_Id = data.documentType_Id;
                    TruckLoadOld.DocumentType_Name = data.documentType_Name;
                    //TruckLoadOld.DocumentRef_No1 = data.documentRef_No1;
                    //TruckLoadOld.DocumentRef_No2 = data.documentRef_No2;
                    //TruckLoadOld.DocumentRef_No3 = data.documentRef_No3;
                    //TruckLoadOld.DocumentRef_No4 = data.documentRef_No4;
                    //TruckLoadOld.DocumentRef_No5 = data.documentRef_No5;
                    TruckLoadOld.Document_Status = data.document_Status;
                    //TruckLoadOld.UDF_1 = data.udf_1;
                    //TruckLoadOld.UDF_2 = data.udf_2;
                    //TruckLoadOld.UDF_3 = data.udf_3;
                    //TruckLoadOld.UDF_4 = data.udf_4;
                    //TruckLoadOld.UDF_5 = data.udf_5;
                    TruckLoadOld.Document_Remark = data.document_Remark;
                    TruckLoadOld.VehicleCompany_Index = data.vehicleCompany_Index;
                    TruckLoadOld.VehicleCompany_Id = data.vehicleCompany_Id;
                    TruckLoadOld.VehicleCompany_Name = data.vehicleCompany_Name;
                    TruckLoadOld.VehicleType_Index = data.vehicleType_Index;
                    TruckLoadOld.VehicleType_Id = data.vehicleType_Id;
                    TruckLoadOld.VehicleType_Name = data.vehicleType_Name;

                    if (IsNew != true)
                    {
                        TruckLoadOld.Update_By = data.update_By;
                        TruckLoadOld.Update_Date = DateTime.Now;
                    }

                    foreach (var item in data.listTruckLoadItemViewModel)
                    {

                        var TruckLoadItemOld = db.im_TruckLoadItem.Find(item.truckLoadItem_Index);

                        if (TruckLoadItemOld != null)
                        {

                            im_TruckLoadItem resultItem = new im_TruckLoadItem();

                            TruckLoadItemOld.TruckLoadItem_Index = item.truckLoadItem_Index;
                            TruckLoadItemOld.TruckLoad_Index = item.truckLoad_Index;
                            TruckLoadItemOld.GoodsIssue_Index = item.goodsIssue_Index;
                            TruckLoadItemOld.GoodsIssue_No = item.goodsIssue_No;
                            //TruckLoadItemOld.DocumentRef_No1 = item.documentRef_No1;
                            //TruckLoadItemOld.DocumentRef_No2 = item.documentRef_No2;
                            //TruckLoadItemOld.DocumentRef_No3 = item.documentRef_No3;
                            //TruckLoadItemOld.DocumentRef_No4 = item.documentRef_No4;
                            //TruckLoadItemOld.DocumentRef_No5 = item.documentRef_No5;
                            TruckLoadItemOld.Document_Status = 0;
                            TruckLoadItemOld.Document_Remark = item.document_Remark;
                            //TruckLoadItemOld.UDF_1 = item.udf_1;
                            //TruckLoadItemOld.UDF_2 = item.udf_2;
                            //TruckLoadItemOld.UDF_3 = item.udf_3;
                            //TruckLoadItemOld.UDF_4 = item.udf_4;
                            //TruckLoadItemOld.UDF_5 = item.udf_5;
                            TruckLoadItemOld.PlanGoodsIssue_Index = item.planGoodsIssue_Index;
                            TruckLoadItemOld.PlanGoodsIssue_No = item.planGoodsIssue_No;

                            if (IsNew != true)
                            {
                                TruckLoadItemOld.Update_By = data.update_By;
                                TruckLoadItemOld.Update_Date = DateTime.Now;
                            }
                        }
                        else
                        {

                            im_TruckLoadItem resultItem = new im_TruckLoadItem();

                            resultItem.TruckLoadItem_Index = Guid.NewGuid();
                            resultItem.TruckLoad_Index = data.truckLoad_Index;
                            resultItem.GoodsIssue_Index = item.goodsIssue_Index;
                            resultItem.GoodsIssue_No = item.goodsIssue_No;
                            //resultItem.DocumentRef_No1 = item.documentRef_No1;
                            //resultItem.DocumentRef_No2 = item.documentRef_No2;
                            //resultItem.DocumentRef_No3 = item.documentRef_No3;
                            //resultItem.DocumentRef_No4 = item.documentRef_No4;
                            //resultItem.DocumentRef_No5 = item.documentRef_No5;
                            resultItem.Document_Status = 0;
                            resultItem.Document_Remark = item.document_Remark;
                            //resultItem.UDF_1 = item.udf_1;
                            //resultItem.UDF_2 = item.udf_2;
                            //resultItem.UDF_3 = item.udf_3;
                            //resultItem.UDF_4 = item.udf_4;
                            //resultItem.UDF_5 = item.udf_5;
                            resultItem.PlanGoodsIssue_Index = item.planGoodsIssue_Index;
                            resultItem.PlanGoodsIssue_No = item.planGoodsIssue_No;

                            if (IsNew != true)
                            {
                                resultItem.Update_By = data.update_By;
                                resultItem.Update_Date = DateTime.Now;
                            }
                            db.im_TruckLoadItem.Add(resultItem);
                        }


                    }

                    var deleteItem = db.im_TruckLoadItem.Where(c => !data.listTruckLoadItemViewModel.Select(s => s.truckLoadItem_Index).Contains(c.TruckLoadItem_Index)
                                        && c.TruckLoad_Index == TruckLoadOld.TruckLoad_Index).ToList();

                    foreach (var c in deleteItem)
                    {
                        var deleteTruckLoadItem = db.im_TruckLoadItem.Find(c.TruckLoadItem_Index);

                        deleteTruckLoadItem.Document_Status = -1;
                        deleteTruckLoadItem.Update_By = data.update_By;
                        deleteTruckLoadItem.Update_Date = DateTime.Now;

                    }

                    ////save image4
                    //if (data.docfile != null)
                    //{
                    //    if (data.docfile.Count > 0)
                    //    {
                    //        foreach (var d in data.docfile)
                    //        {
                    //            byte[] img = Convert.FromBase64String(d.base64);
                    //            var path = Directory.GetCurrentDirectory();
                    //            path += "\\" + "ImageFolder" + "\\";
                    //            if (!System.IO.Directory.Exists(path))
                    //            {
                    //                System.IO.Directory.CreateDirectory(path);
                    //            }
                    //            System.IO.File.WriteAllBytes(new AppSettingConfig().GetUrl("configUploadImg") + d.name, img);

                    //            im_TruckLoadImages tli = new im_TruckLoadImages();

                    //            tli.TruckLoadImage_Index = Guid.NewGuid();
                    //            tli.TruckLoad_Index = TruckLoadIndex;
                    //            tli.ImageUrl = new AppSettingConfig().GetUrl("configGetImg") + d.name.ToString();
                    //            tli.ImageType = d.type;
                    //            tli.Document_Status = 0;
                    //            tli.Create_By = data.create_By;
                    //            tli.Create_Date = DateTime.Now;
                    //            db.im_TruckLoadImages.Add(tli);
                    //        }
                    //    }
                    //}

                    //save image
                    if (data.docfile != null)
                    {
                        if (data.docfile.Count > 0)
                        {
                            var chkTruckloadImage = db.im_TruckLoadImages.Where(c => c.TruckLoad_Index == data.docfile.FirstOrDefault().truckLoad_Index && c.Document_Status == 0).ToList();
                            if (chkTruckloadImage.Count() > 0)
                            {
                                foreach (var updateTruckloadImage in chkTruckloadImage)
                                {
                                    updateTruckloadImage.Document_Status = -1;
                                    updateTruckloadImage.Update_By = data.create_By;
                                    updateTruckloadImage.Update_Date = DateTime.Now;
                                }
                            }
                            foreach (var d in data.docfile)
                            {
                                var chkinsert = db.im_TruckLoadImages.Find(d.truckLoadImage_Index);

                                if (chkinsert == null)
                                {
                                    byte[] img = Convert.FromBase64String(d.base64);
                                    var path = Directory.GetCurrentDirectory();
                                    path += "\\" + "ImageFolder" + "\\";
                                    if (!System.IO.Directory.Exists(path))
                                    {
                                        System.IO.Directory.CreateDirectory(path);
                                    }
                                    System.IO.File.WriteAllBytes(new AppSettingConfig().GetUrl("configUploadImg") + d.name, img);

                                    im_TruckLoadImages tli = new im_TruckLoadImages();

                                    tli.TruckLoadImage_Index = Guid.NewGuid();
                                    tli.TruckLoad_Index = TruckLoadIndex;
                                    tli.ImageUrl = new AppSettingConfig().GetUrl("configGetImg") + d.name.ToString();
                                    tli.ImageType = d.type;
                                    tli.Document_Status = 0;
                                    tli.Create_By = data.create_By;
                                    tli.Create_Date = DateTime.Now;
                                    db.im_TruckLoadImages.Add(tli);
                                }
                                else
                                {
                                    chkinsert.Document_Status = 0;
                                    chkinsert.Update_By = null;
                                    chkinsert.Update_Date = null;
                                }
                            }
                        }
                        else
                        {
                            var chkTruckloadImage = db.im_TruckLoadImages.Where(c => c.TruckLoad_Index == data.truckLoad_Index && c.Document_Status == 0).ToList();
                            if (chkTruckloadImage.Count() > 0)
                            {
                                foreach (var updateTruckloadImage in chkTruckloadImage)
                                {
                                    updateTruckloadImage.Document_Status = -1;
                                    updateTruckloadImage.Update_By = data.create_By;
                                    updateTruckloadImage.Update_Date = DateTime.Now;
                                }
                            }
                        }
                    }
                }



                var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transactionx.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("SaveTruckLoad", msglog);
                    transactionx.Rollback();

                    throw exy;

                }

                actionResult.document_No = TruckLoadNo;
                actionResult.Message = true;

                return actionResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region delete
        public Boolean delete(LoadViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                var TL = db.im_TruckLoad.Find(data.truckLoad_Index);

                if (TL != null)
                {
                    TL.Document_Status = -1;
                    TL.Cancel_By = data.cancel_By;
                    TL.Cancel_Date = DateTime.Now;

                    var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                        return true;
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("DeleteTruckLoad", msglog);
                        transaction.Rollback();
                        throw exy;
                    }

                }


                return false;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region comfirmStatus
        public Boolean comfirmStatus(LoadViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                var TL = db.im_TruckLoad.Find(data.truckLoad_Index);

                if (TL != null)
                {
                    TL.Document_Status = 1;
                    TL.Approve_By = data.approve_By;
                    TL.Approve_Date = DateTime.Now;

                    var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                        return true;
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("comfirmStatusTruckLoad", msglog);
                        transaction.Rollback();
                        throw exy;
                    }

                }
                else
                {
                    return false;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region listcomfirmStatus
        public Boolean listcomfirmStatus(listTruckload data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                foreach (var item in data.ListTruckload)
                {
                    var TL = db.im_TruckLoad.Find(item.truckLoad_Index);

                    if (TL != null)
                    {
                        TL.Document_Status = 1;
                        TL.Approve_By = item.approve_By;
                        TL.Approve_Date = DateTime.Now;

                        var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                        try
                        {
                            db.SaveChanges();
                            transaction.Commit();
                        }

                        catch (Exception exy)
                        {
                            msglog = State + " ex Rollback " + exy.Message.ToString();
                            olog.logging("listcomfirmStatusTruckLoad", msglog);
                            transaction.Rollback();
                            throw exy;
                        }

                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region  autoPlanGoodIssueNo
        public List<ItemListViewModel> autoPlanGoodIssueNo(ItemListViewModel data)
        {
            try
            {
                var query = db.im_PlanGoodsIssue.AsQueryable();

                if (!string.IsNullOrEmpty(data.key))
                {
                    query = query.Where(c => c.PlanGoodsIssue_No.Contains(data.key) && c.Document_Status == 3);

                }

                var items = new List<ItemListViewModel>();

                var result = query.Select(c => new { c.PlanGoodsIssue_Index, c.PlanGoodsIssue_No }).Distinct().Take(10).ToList();


                foreach (var item in result)
                {
                    var resultItem = new ItemListViewModel
                    {
                        index = item.PlanGoodsIssue_Index,
                        name = item.PlanGoodsIssue_No
                    };
                    items.Add(resultItem);

                }

                return items;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region scanLoadNo
        public LoadViewModel scanLoadNo(LoadViewModel data)
        {
            try
            {
                var query = db.im_TruckLoad.Where(c => c.TruckLoad_No == data.truckLoad_No && (c.Document_Status == 0 || c.Document_Status == 1)).FirstOrDefault();

                var result = new LoadViewModel();

                if (query != null)
                {
                    result.truckLoad_Index = query.TruckLoad_Index;
                    result.truckLoad_No = query.TruckLoad_No;
                    result.documentRef_No1 = query.DocumentRef_No1;
                    result.vehicle_Registration = query.Vehicle_Registration;

                }


                return result;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region scanLoadNo
        //public LoadViewModel scanLoadNo(LoadViewModel data)
        //{
        //    try
        //    {
        //        var query = db.im_TruckLoad.Where(c => c.TruckLoad_No == data.truckLoad_No && c.Document_Status == 1).FirstOrDefault();

        //        var result = new LoadViewModel();

        //        if (query != null)
        //        {
        //            result.truckLoad_Index = query.TruckLoad_Index;
        //            result.truckLoad_No = query.TruckLoad_No;
        //            result.documentRef_No1 = query.DocumentRef_No1;
        //            result.vehicle_Registration = query.Vehicle_Registration;

        //        }


        //        return result;

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        #endregion

        #region scanSoNo
        //public LoadItemViewModel scanSoNo(LoadItemViewModel data)
        //{
        //    try
        //    {
        //        var query = db.im_TruckLoadItem.Where(c => c.TruckLoad_Index == data.truckLoad_Index && c.PlanGoodsIssue_No == data.planGoodsIssue_No).FirstOrDefault();

        //        var result = new LoadItemViewModel();

        //        if (query != null)
        //        {
        //            var plan = db.im_PlanGoodsIssue.Find(query.PlanGoodsIssue_Index);

        //            result.truckLoad_Index = query.TruckLoad_Index;
        //            result.planGoodsIssue_Index = query.PlanGoodsIssue_Index;
        //            result.shipTo_Name = plan.ShipTo_Name;

        //        }


        //        return result;

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        #endregion

        #region  autoTruckloadNo
        public List<ItemListViewModel> autoTruckloadNo(ItemListViewModel data)
        {
            try
            {
                var query = db.im_TruckLoad.Where(c=> c.Document_Status != -1).AsQueryable();

                if (!string.IsNullOrEmpty(data.key))
                {
                    query = query.Where(c => c.TruckLoad_No.Contains(data.key));

                }

                var items = new List<ItemListViewModel>();

                var result = query.Select(c => new { c.TruckLoad_Index, c.TruckLoad_No }).Distinct().Take(10).ToList();


                foreach (var item in result)
                {
                    var resultItem = new ItemListViewModel
                    {
                        index = item.TruckLoad_Index,
                        name = item.TruckLoad_No
                    };
                    items.Add(resultItem);

                }

                return items;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region scanProduct
        public ScanLoadItemViewModel scanProduct(ScanLoadItemViewModel data)
        {
            try
            {
                var query = db.im_PlanGoodsIssueItem.Where(c => c.Product_Id == data.product_Id && c.PlanGoodsIssue_No == data.planGoodsIssue_No).FirstOrDefault();

                var result = new ScanLoadItemViewModel();

                if (query != null)
                {
                    result.planGoodsIssue_Index = query.PlanGoodsIssue_Index;

                }


                return result;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region scanProductId
        public ScanLoadItemViewModel scanProductId(ScanLoadItemViewModel data)
        {
            try
            {

                var result = new ScanLoadItemViewModel();

                var queryResult = new List<ProductDetailViewModel>();
                var filterModel = new ProductDetailViewModel();

                filterModel.productConversionBarcode = data.productConvertionBarcode;
                queryResult = utils.SendDataApi<List<ProductDetailViewModel>>(new AppSettingConfig().GetUrl("ConfigViewProductDetail"), filterModel.sJson());

                if (queryResult.Count > 0)
                {
                    var query = db.im_PlanGoodsIssueItem.Where(c => c.Product_Index == queryResult.FirstOrDefault().product_Index && c.PlanGoodsIssue_No == data.planGoodsIssue_No).FirstOrDefault();


                    if (query != null)
                    {
                        result.planGoodsIssue_Index = query.PlanGoodsIssue_Index;
                        result.product_Id = query.Product_Id;
                        result.product_Name = query.Product_Name;
                    }
                }



                return result;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region scanSN
        public actionResultScanLoad scanSN(ScanLoadItemViewModel data)
        {

            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            var actionResult = new actionResultScanLoad();

            Decimal? QtySN = 0;

            Decimal? QtyPlan = 0;

            try
            {

                var checkSN = db.im_PlanGoodsIssueItemSN.Where(c => c.PlanGoodsIssue_Index == data.planGoodsIssue_Index && c.PlanGoodsIssueItem_SN == data.planGoodsIssueItem_SN && c.Document_Status != -1).FirstOrDefault();

                if (checkSN != null)
                {
                    actionResult.Message = false;
                    actionResult.msg = "มี S/N นี้แล้ว";

                    return actionResult;

                }
                else
                {


                    var SumQtySN = db.im_PlanGoodsIssueItemSN.Where(c => c.PlanGoodsIssue_Index == data.planGoodsIssue_Index && c.Product_Id == data.product_Id && c.Document_Status != -1).ToList();

                    if (SumQtySN.Count > 0)
                    {
                        QtySN = SumQtySN.Sum(s => s.Qty);
                    }

                    var queryPlan = db.im_PlanGoodsIssueItem.Where(c => c.Product_Id == data.product_Id && c.PlanGoodsIssue_Index == data.planGoodsIssue_Index).ToList();

                    var result = new List<ScanLoadItemViewModel>();

                    if (queryPlan.Count > 0)
                    {
                        QtyPlan = queryPlan.Sum(s => s.Qty);

                        if (QtySN >= QtyPlan)
                        {
                            actionResult.Message = false;
                            actionResult.msg = "Scan Qty ครบแล้ว";

                            return actionResult;
                        }

                        else
                        {
                            foreach (var item in queryPlan)
                            {
                                var resultSN = new im_PlanGoodsIssueItemSN();

                                resultSN.PlanGoodsIssueItemSN_Index = Guid.NewGuid();
                                resultSN.PlanGoodsIssue_Index = item.PlanGoodsIssue_Index;
                                resultSN.PlanGoodsIssueItem_Index = item.PlanGoodsIssueItem_Index;
                                resultSN.LineNum = item.LineNum;
                                resultSN.Product_Index = item.Product_Index;
                                resultSN.Product_Id = item.Product_Id;
                                resultSN.Product_Name = item.Product_Name;
                                resultSN.Product_SecondName = item.Product_SecondName;
                                resultSN.Product_ThirdName = item.Product_ThirdName;
                                resultSN.ProductConversion_Index = item.ProductConversion_Index;
                                resultSN.ProductConversion_Id = item.ProductConversion_Id;
                                resultSN.ProductConversion_Name = item.ProductConversion_Name;
                                resultSN.Qty = 1;
                                resultSN.Ratio = 1;
                                resultSN.TotalQty = 1;
                                resultSN.Document_Status = 0;
                                resultSN.Create_By = data.create_By;
                                resultSN.Create_Date = DateTime.Now;
                                resultSN.PlanGoodsIssue_No = item.PlanGoodsIssue_No;
                                resultSN.PlanGoodsIssueItem_SN = data.planGoodsIssueItem_SN;

                                db.im_PlanGoodsIssueItemSN.Add(resultSN);

                            }
                        }
                    }
                }

                var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transactionx.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("SavePlanSN", msglog);
                    transactionx.Rollback();

                    throw exy;

                }

                actionResult.Message = true;
                return actionResult;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region filterPlanSN
        public List<ScanLoadItemViewModel> filterplanSN(ScanLoadItemViewModel data)
        {

            try
            {

                var query = db.View_LoadPlan_V2.Where(c => c.TruckLoad_Index == data.truckLoad_Index).ToList();

                var result = new List<ScanLoadItemViewModel>();

                foreach (var item in query)
                {
                    var resultSN = new ScanLoadItemViewModel();

                    resultSN.planGoodsIssue_No = item.PlanGoodsIssue_No;
                    resultSN.truckLoad_No = item.TruckLoad_No;
                    resultSN.carton = item.carton;
                    resultSN.tote = item.tote;
                    resultSN.planGoodsIssue_Index = item.PlanGoodsIssue_Index;

                    result.Add(resultSN);

                }


                return result;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region confirmScan
        public actionResultScanLoad confirmScan(ScanLoadItemViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            var actionResult = new actionResultScanLoad();

            try
            {
                //var OldLoad = db.im_TruckLoadItem.FirstOrDefault(c=> c.TruckLoad_Index == data.truckLoad_Index && c.PlanGoodsIssue_Index == data.planGoodsIssue_Index);
                var OldLoad = db.im_TruckLoadItem.Where(c => c.TruckLoad_Index == data.truckLoad_Index).ToList();
                //var updateConfirmPost = db.im_PlanGoodsIssue.FirstOrDefault(c => c.PlanGoodsIssue_Index == data.planGoodsIssue_Index);
                var scanout = db.im_RollCageOrder.Where(c => OldLoad.Select(x => x.PlanGoodsIssue_No).Contains(c.PlanGoodsIssue_No) && c.Document_Status == 0).ToList();
                //var scanout = db.im_RollCageOrder.Where(c => c.PlanGoodsIssue_No == updateConfirmPost.PlanGoodsIssue_No && c.Document_Status == 0).ToList();
                if (scanout.Count > 0)
                {
                    actionResult.msg = "กรุณา Scan ของ ออกจาก Rollcage ให้ครบ ก่อนทำการปล่อยรถ";
                    actionResult.Message = false;
                    return actionResult;
                }
                var checkall_tasks = db.View_Taskitem_with_Truckload.Where(c => c.TruckLoad_Index == data.truckLoad_Index && c.PickingPickQty_Location_Name != null && c.Document_StatusTracking != 2).ToList();
                if (checkall_tasks.Count > 0)
                {
                    actionResult.msg = "กรุณา Conform Task งาน Selecting to Dock ให้ครบก่อนทำการ Load สินค้า";
                    actionResult.Message = false;
                    return actionResult;
                }
                var get_truckload = db.im_TruckLoad.FirstOrDefault(c => c.TruckLoad_Index == data.truckLoad_Index);
                List<Guid> rollCageOrders = db.im_RollCageOrder.Where(c => c.TruckLoad_No == get_truckload.TruckLoad_No && string.IsNullOrEmpty(c.UDF_1) && c.Document_Status != -1).GroupBy(c => c.RollCage_Index).Select(c => c.Key).ToList();
                //List<ms_RollCage> rollCage = Masterdb.ms_RollCage.Where(c => rollCageOrders.Contains(c.RollCage_Index) && c.Location_Name.Contains("DK")).ToList();
                if (rollCageOrders.Count() > 0)
                {
                    actionResult.msg = "กรุณาส่ง Rollcage ให้ครบก่อนทำการ Load สินค้า";
                    actionResult.Message = false;
                    return actionResult;
                }



                if (OldLoad.Count > 0)
                {
                    foreach (var item in OldLoad)
                    {
                        item.Document_Status = 2;
                        item.Update_By = data.update_By;
                        item.Update_Date = DateTime.Now;
                    }

                }
                else
                {
                    actionResult.msg = "ไม่สามารถทำการ load สินค้าได้";
                    actionResult.Message = false;
                    return actionResult;
                }

                var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transactionx.Commit();
                    

                    foreach (var itemList in OldLoad)
                    {
                        var des = "รอโหลด";
                        try
                        {

                            var resmodel = new
                            {
                                referenceNo = itemList.PlanGoodsIssue_No,
                                status = 103,
                                statusAfter = 104,
                                statusBefore = 102,
                                statusDesc = des,
                                statusDateTime = DateTime.Now
                            };
                            SaveLogRequest(itemList.PlanGoodsIssue_No, JsonConvert.SerializeObject(resmodel), des, 1, des, Guid.NewGuid());
                            var result_api = Utils.SendDataApi<DemoCallbackResponseViewModel>(new AppSettingConfig().GetUrl("TMS_status"), JsonConvert.SerializeObject(resmodel));
                            SaveLogResponse(itemList.PlanGoodsIssue_No, JsonConvert.SerializeObject(result_api), resmodel.statusDesc, 2, resmodel.statusDesc, Guid.NewGuid());
                        }
                        catch (Exception ex)
                        {
                            SaveLogResponse(itemList.PlanGoodsIssue_No, JsonConvert.SerializeObject(ex.Message), des, -1, des, Guid.NewGuid());
                        }
                    }
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("SavePlanSN", msglog);
                    transactionx.Rollback();

                    throw exy;

                }

                var findsucess = db.im_TruckLoadItem.Where(c => c.TruckLoad_Index == data.truckLoad_Index && (c.Document_Status != 2 && c.Document_Status != -1)).ToList();
                if (findsucess.Count <= 0)
                {
                    var updatetruck = db.im_TruckLoad.FirstOrDefault(c => c.TruckLoad_Index == data.truckLoad_Index);
                    updatetruck.Document_Status = 2;
                    updatetruck.Update_By = data.update_By;
                    updatetruck.Update_Date = DateTime.Now;
                }

                var transactionup = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transactionup.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("SavePlanSN", msglog);
                    transactionup.Rollback();

                    throw exy;

                }

                actionResult.Message = true;
                return actionResult;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region confirmScan
        //public actionResultScanLoad confirmScan(ScanLoadItemViewModel data)
        //{
        //    String State = "Start";
        //    String msglog = "";
        //    var olog = new logtxt();

        //    var actionResult = new actionResultScanLoad();

        //    try
        //    {
        //        var OldLoad = db.im_TruckLoad.Find(data.truckLoad_Index);

        //        if (OldLoad != null)
        //        {
        //            OldLoad.Document_Status = 2;
        //            OldLoad.Update_By = data.update_By;
        //            OldLoad.Update_Date = DateTime.Now;
        //            OldLoad.End_load = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

        //        }
        //        else
        //        {
        //            actionResult.Message = false;
        //            return actionResult;
        //        }


        //        var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
        //        try
        //        {
        //            db.SaveChanges();
        //            transactionx.Commit();
        //        }

        //        catch (Exception exy)
        //        {
        //            msglog = State + " ex Rollback " + exy.Message.ToString();
        //            olog.logging("SavePlanSN", msglog);
        //            transactionx.Rollback();

        //            throw exy;

        //        }

        //        actionResult.Message = true;
        //        return actionResult;

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        #endregion

        #region deleteItem
        public actionResultScanLoad deleteItem(ScanLoadItemViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            var actionResult = new actionResultScanLoad();

            try
            {
                var deleteItem = db.im_TruckLoadItem.Where(c => data.listTruckLoadItemViewModel.Select(s => s.planGoodsIssue_Index).Contains(c.PlanGoodsIssue_Index)).ToList();

                foreach (var c in deleteItem)
                {
                    var deleteTruckLoadItem = db.im_TruckLoadItem.Find(c.TruckLoadItem_Index);

                    deleteTruckLoadItem.Document_Status = -1;
                    deleteTruckLoadItem.Update_By = data.update_By;
                    deleteTruckLoadItem.Update_Date = DateTime.Now;

                }


                var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transactionx.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("deleteItem", msglog);
                    transactionx.Rollback();

                    throw exy;

                }

                actionResult.Message = true;
                return actionResult;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region dropdownVehicleCompanyType
        public List<VehicleCompanyTypeViewModel> dropdownVehicleCompanyType(VehicleCompanyTypeViewModel data)
        {
            try
            {
                var result = new List<VehicleCompanyTypeViewModel>();

                var filterModel = new VehicleCompanyTypeViewModel();

                //GetConfig
                result = utils.SendDataApi<List<VehicleCompanyTypeViewModel>>(new AppSettingConfig().GetUrl("dropdownVehicleCompanyType"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region dropdownVehicleCompany
        public List<VehicleCompanyViewModel> dropdownVehicleCompany(VehicleCompanyViewModel data)
        {
            try
            {
                var result = new List<VehicleCompanyViewModel>();

                var filterModel = new VehicleCompanyViewModel();

                //GetConfig
                result = utils.SendDataApi<List<VehicleCompanyViewModel>>(new AppSettingConfig().GetUrl("dropdownVehicleCompany"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region dropdownVehicleType
        public List<VehicleTypeViewModel> dropdownVehicleType(VehicleTypeViewModel data)
        {
            try
            {
                var result = new List<VehicleTypeViewModel>();

                var filterModel = new VehicleTypeViewModel();

                //GetConfig
                result = utils.SendDataApi<List<VehicleTypeViewModel>>(new AppSettingConfig().GetUrl("dropdownVehicleType"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region scanSoNo
        public List<ScanLoadItemViewModel> scanSoNo(ScanLoadItemViewModel data)
        {

            String State = "Start";
            String msglog = "";
            var olog = new logtxt();


            try
            {
                var result = new List<ScanLoadItemViewModel>();


                var checkSN_select = db.View_LoadPlan_V2.Where(c => c.PlanGoodsIssue_No == data.planGoodsIssue_No).FirstOrDefault();

                var result_Check = new ScanLoadItemViewModel();
                result_Check.planGoodsIssue_No = checkSN_select.PlanGoodsIssue_No;
                result_Check.truckLoad_No = checkSN_select.TruckLoad_No;
                result_Check.qty = checkSN_select.Qty;
                result_Check.planGoodsIssue_Index = checkSN_select.PlanGoodsIssue_Index;
                result_Check.plan_check = true;
                result.Add(result_Check);

                var checkSN = db.View_LoadPlan_V2.Where(c => c.PlanGoodsIssue_No != data.planGoodsIssue_No && c.TruckLoad_No == data.truckLoad_No).ToList();

                foreach (var item in checkSN)
                {
                    var resultSN = new ScanLoadItemViewModel();

                    resultSN.planGoodsIssue_No = item.PlanGoodsIssue_No;
                    resultSN.truckLoad_No = item.TruckLoad_No;
                    resultSN.qty = item.Qty;
                    resultSN.planGoodsIssue_Index = item.PlanGoodsIssue_Index;

                    result.Add(resultSN);

                }


                return result;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region scanSoNo
        //public actionResultScanLoad scanSoNo(ScanLoadItemViewModel data)
        //{

        //    String State = "Start";
        //    String msglog = "";
        //    var olog = new logtxt();

        //    var actionResult = new actionResultScanLoad();

        //    Decimal? QtySN = 0;

        //    Decimal? QtyPlan = 0;

        //    try
        //    {

        //        //var checkSN = db.View_LoadPlan.Where(c => c.PlanGoodsIssue_No == data.planGoodsIssue_No && c.TruckLoad_Index == data.truckLoad_Index).FirstOrDefault();

        //        var checkSN = db.View_LoadPlan.Where(c => c.PlanGoodsIssue_No == data.planGoodsIssue_No).FirstOrDefault();


        //        if (checkSN != null)
        //        {
        //            actionResult.Message = false;
        //            actionResult.msg = "มี S/N นี้แล้ว";

        //            return actionResult;

        //        }
        //        else
        //        {
        //            var queryPlan = db.im_PlanGoodsIssue.Where(c => c.PlanGoodsIssue_No == data.planGoodsIssue_No && c.Document_Status != -1).FirstOrDefault();

        //            var result = new im_TruckLoadItem();

        //            result.TruckLoad_Index = data.truckLoad_Index;
        //            result.TruckLoadItem_Index = Guid.NewGuid();
        //            result.Create_By = data.create_By;
        //            result.Create_Date = DateTime.Now;
        //            result.PlanGoodsIssue_Index = queryPlan.PlanGoodsIssue_Index;
        //            result.PlanGoodsIssue_No = data.planGoodsIssue_No;
        //            result.Document_Status = 0;
        //            db.im_TruckLoadItem.Add(result);

        //            var TLold = db.im_TruckLoad.Find(result.TruckLoad_Index);

        //            if (TLold != null)
        //            {

        //                TLold.DocumentRef_No1 = data.documentRef_No1;
        //                TLold.Vehicle_Registration = data.vehicle_Registration;

        //                if (TLold.Start_load == null)
        //                {
        //                    TLold.Start_load = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //                }
        //            }

        //        }

        //        var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
        //        try
        //        {
        //            db.SaveChanges();
        //            transactionx.Commit();
        //        }

        //        catch (Exception exy)
        //        {
        //            msglog = State + " ex Rollback " + exy.Message.ToString();
        //            olog.logging("SavePlanSN", msglog);
        //            transactionx.Rollback();

        //            throw exy;

        //        }

        //        actionResult.Message = true;
        //        return actionResult;


        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        #endregion

        #region ConfirmCutStock
        public string ConfirmCutStock(SearchDetailModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            Guid GI_Index = Guid.Empty;
            try
            {
                var TruckLoad = db.im_TruckLoad.Where(c => c.TruckLoad_Index == model.truckLoad_Index && c.Document_Status != -1);
                if (TruckLoad.Count() == 0)
                {
                    return "สถานนะไม่ถูกต้อง";
                }
                var TruckLoadItem = db.im_TruckLoadItem.Where(c => c.TruckLoad_Index == TruckLoad.FirstOrDefault().TruckLoad_Index).GroupBy(g => new { g.PlanGoodsIssue_Index, g.PlanGoodsIssue_No }).ToList();
                var chktaskitem = db.IM_TaskItem.Where(c => TruckLoadItem.Select(s => s.Key.PlanGoodsIssue_Index).Contains(c.PlanGoodsIssue_Index));
                if (chktaskitem.Count() == 0)
                {
                    return "กรุณาทำ GI ให้เสร็จก่อนทำการตัดสต๊อก";
                }
                else if (chktaskitem.Count() > 0)
                {
                    var countTaskItem = chktaskitem.Where(c => c.Picking_Status != 2).Count();
                    if (countTaskItem > 0)
                    {
                        return "กรุณาทำ GI ให้เสร็จก่อนทำการตัดสต๊อก";
                    }
                }
                foreach (var TL in TruckLoadItem)
                {
                    //var objectLocation = new { locationType_Index = new Guid("2E9338D3-0931-4E36-B240-782BF9508641") };
                    var location = utils.SendDataApi<List<locationViewModel>>(new AppSettingConfig().GetUrl("GetLocation"), new { }.sJson());
                    var taskitem = db.IM_TaskItem.Where(c => c.PlanGoodsIssue_Index == TL.Key.PlanGoodsIssue_Index && c.PlanGoodsIssue_No == TL.Key.PlanGoodsIssue_No && c.Picking_Status == 2).ToList();
                    var result = false;

                    foreach (var ti in taskitem)
                    {
                        var l = location.FirstOrDefault(c => c.location_Name == ti.UDF_5);
                        var GIL = db.IM_GoodsIssueItemLocation.Find(ti.Ref_DocumentItem_Index);
                        var GI = db.IM_GoodsIssue.Find(ti.Ref_Document_Index);
                        GI_Index = GI.GoodsIssue_Index;
                        var datacutslots = new
                        {
                            goodsIssueItemLocation_Index = GIL.GoodsIssueItemLocation_Index,
                            goodsIssue_Index = GIL.GoodsIssue_Index,
                            lineNum = ti.LineNum,
                            tagItem_Index = ti.TagItem_Index,
                            tag_Index = ti.Tag_Index,
                            tag_No = ti.UDF_5,
                            product_Index = ti.Product_Index,
                            product_Id = ti.Product_Id,
                            product_Name = ti.Product_Name,
                            product_SecondName = ti.Product_SecondName,
                            product_ThirdName = ti.Product_ThirdName,
                            product_Lot = ti.Product_Lot,
                            itemStatus_Index = ti.ItemStatus_Index,
                            itemStatus_Id = ti.ItemStatus_Id,
                            itemStatus_Name = ti.ItemStatus_Name,
                            location_Index = l.location_Index,
                            location_Id = l.location_Id,
                            location_Name = l.location_Name,
                            qtyPlan = GIL.QtyPlan,
                            qty = ti.Qty,
                            ratio = ti.Ratio,
                            totalQty = ti.TotalQty,
                            productConversion_Index = ti.ProductConversion_Index,
                            productConversion_Id = ti.ProductConversion_Id,
                            productConversion_Name = ti.ProductConversion_Name,
                            mfg_Date = ti.MFG_Date,
                            exp_Date = ti.EXP_Date,
                            unitWeight = ti.UnitWeight,
                            weight = ti.Weight,
                            unitWidth = ti.UnitWidth,
                            unitLength = ti.UnitLength,
                            unitHeight = ti.UnitHeight,
                            unitVolume = ti.UnitVolume,
                            volume = ti.Volume,
                            unitPrice = ti.UnitPrice,
                            price = ti.Price,
                            documentRef_No1 = ti.DocumentRef_No1,
                            documentRef_No2 = ti.DocumentRef_No2,
                            documentRef_No3 = ti.DocumentRef_No3,
                            documentRef_No4 = ti.DocumentRef_No4,
                            documentRef_No5 = ti.DocumentRef_No5,
                            document_Status = ti.Document_Status,
                            udf_1 = ti.UDF_1,
                            udf_2 = ti.UDF_2,
                            udf_3 = ti.UDF_3,
                            udf_4 = ti.UDF_4,
                            udf_5 = ti.UDF_5,
                            ref_Process_Index = ti.Ref_Process_Index,

                            ref_Document_No = TruckLoad.FirstOrDefault().TruckLoad_No,
                            ref_Document_LineNum = ti.LineNum,
                            ref_Document_Index = GIL.GoodsIssue_Index,
                            ref_DocumentItem_Index = GIL.GoodsIssueItemLocation_Index,
                            goodsReceiveItem_Index = GIL.GoodsReceiveItem_Index,
                            picking_Status = ti.Picking_Status,
                            picking_By = ti.Picking_By,
                            picking_Date = ti.Picking_Date,
                            picking_Ref1 = GIL.Picking_Ref1,
                            picking_Ref2 = GIL.Picking_Ref2,
                            picking_Qty = ti.Picking_Qty,
                            picking_Ratio = ti.Picking_Ratio,
                            picking_TotalQty = ti.Picking_TotalQty,
                            picking_ProductConversion_Index = ti.Pick_ProductConversion_Index,
                            mashall_Status = GIL.Mashall_Status,
                            mashall_Qty = GIL.Mashall_Qty,
                            cancel_Status = GIL.Cancel_Status,
                            goodsIssue_No = GIL.GoodsIssue_No,
                            binBalance_Index = ti.BinBalance_Index_New,
                            process_Index = "69CAD84E-1F80-48FD-8749-1A6009F3B89C",
                            create_By = model.create_By,
                            owner_Index = GI.Owner_Index,
                            owner_Id = GI.Owner_Id,
                            owner_Name = GI.Owner_Name,
                            goodsIssue_Date = GI.GoodsIssue_Date,
                            documentType_Index = GI.DocumentType_Index,
                            documentType_Id = GI.DocumentType_Id,
                            documentType_Name = GI.DocumentType_Name

                        };

                        result = utils.SendDataApi<bool>(new AppSettingConfig().GetUrl("CutSlotsBinBalance"), datacutslots.sJson());

                        if (result)
                        {
                            #region Update Status GIL = 2
                            //Update Status GIL = 1
                            var transaction2 = db.Database.BeginTransaction();
                            try
                            {
                                GIL.Document_Status = 2;
                                db.SaveChanges();
                                transaction2.Commit();
                            }

                            catch (Exception exy)
                            {
                                msglog = State + " ex Rollback 2 " + exy.Message.ToString();
                                olog.logging("ConfirmCutSlots", msglog);
                                transaction2.Rollback();
                                throw exy;
                            }
                            #endregion
                        }
                    }
                }
                var ListGIL2 = db.IM_GoodsIssueItemLocation.Where(c => c.GoodsIssue_Index == GI_Index && c.Document_Status != 2).Count();

                #region Update Status TruckLoad = 3
                //Update Status GIL = 1
                var transaction3 = db.Database.BeginTransaction();
                try
                {

                    if (ListGIL2 == 0)
                    {
                        db.IM_GoodsIssue.Find(GI_Index).Document_Status = 4;
                    }

                    TruckLoad.FirstOrDefault().Document_Status = 3;
                    db.SaveChanges();
                    transaction3.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback 2 " + exy.Message.ToString();
                    olog.logging("ConfirmCutSlots", msglog);
                    transaction3.Rollback();
                    throw exy;
                }
                #endregion

                return "success";
            }
            catch (Exception ex)
            {
                msglog = State + " ex Rollback " + ex.Message.ToString();
                olog.logging("ConfirmCutSlots", msglog);
                throw ex;
            }
        }

        #endregion

        #region CloseDocument
        public string CloseDocument(SearchDetailModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                var TruckLoad = db.im_TruckLoad.Find(model.truckLoad_Index);
                if (TruckLoad == null)
                {
                    return "ไม่พบเลขเอกสารนี้";
                }

                if (TruckLoad.Document_Status == 2)
                {
                    return "กรุณาตัดสต๊อกก่อนทำการปิดเอกสาร";
                }

                var TruckLoadItem = db.im_TruckLoadItem.Where(c => c.TruckLoad_Index == TruckLoad.TruckLoad_Index).GroupBy(g => g.PlanGoodsIssue_Index).ToList();
                var GuidTruckLoadItem = new List<Guid>();
                foreach (var item in TruckLoadItem)
                {
                    GuidTruckLoadItem.Add(item.Key.sParse<Guid>());
                }
                var pgi = db.im_PlanGoodsIssue.Where(c => GuidTruckLoadItem.Contains(c.PlanGoodsIssue_Index)).ToList();

                if (pgi.Count() > 0 && TruckLoad.Document_Status == 0)
                {
                    return "กรุณาตัดสต๊อกก่อนทำการปิดเอกสาร";
                }

                #region Update Status TruckLoad = 4
                //Update Status GIL = 1
                var transaction3 = db.Database.BeginTransaction();
                try
                {
                    TruckLoad.Document_Status = 4;
                    TruckLoad.Start_load = !string.IsNullOrEmpty(TruckLoad.Start_load) ? TruckLoad.Start_load : pgi?.OrderBy(o => o.PlanGoodsIssue_Due_Date)?.FirstOrDefault()?.PlanGoodsIssue_Due_Date?.ToString("dd/MM/yyyy");
                    TruckLoad.End_load = !string.IsNullOrEmpty(TruckLoad.End_load) ? TruckLoad.End_load : pgi?.OrderByDescending(o => o.PlanGoodsIssue_Due_Date)?.FirstOrDefault()?.PlanGoodsIssue_Due_Date?.ToString("dd/MM/yyyy");
                    db.SaveChanges();
                    transaction3.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback 2 " + exy.Message.ToString();
                    olog.logging("ConfirmCutSlots", msglog);
                    transaction3.Rollback();
                    throw exy;
                }
                #endregion

                return "success";
            }
            catch (Exception ex)
            {
                msglog = State + " ex Rollback " + ex.Message.ToString();
                olog.logging("ConfirmCutSlots", msglog);
                throw ex;
            }
        }

        #endregion

        #region ReportPrintOutShipment
        public string ReportPrintOutShipment(PrintOutShipmentModel data, string rootPath = "")
        {

            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                var queryTruckLoad = db.View_PrintOutTruckLoad.AsQueryable();
                var queryPlanGI = db.View_PrintOutPlanGIV2.AsQueryable();
                var findTote = db.View_Load_returntote.AsQueryable();
                var findCarton = new View_Load_Cartontote();

                if (data.truckLoad_Index != null)
                {
                    queryTruckLoad = queryTruckLoad.Where(c => c.TruckLoad_Index == data.truckLoad_Index);
                    findTote = findTote.Where(c => c.TruckLoad_Index == data.truckLoad_Index && !string.IsNullOrEmpty(c.Size_tote));
                    findCarton = db.View_Load_Cartontote.FirstOrDefault(c => c.TruckLoad_Index == data.truckLoad_Index);
                    var aa = queryTruckLoad.ToList();
                }


                var result = new List<PrintOutShipmentModel>();

                decimal? Ratio = 0;

                var ProductConversionModel = new ProductConversionViewModel();
                var resultProductConversion = new List<ProductConversionViewModel>();
                resultProductConversion = utils.SendDataApi<List<ProductConversionViewModel>>(new AppSettingConfig().GetUrl("dropdownProductConversionV2"), ProductConversionModel.sJson());

                var date = DateTime.Now.ToString("dd/MM/yyyy", culture);
                var time = DateTime.Now.ToString("HH:mm");

                var query_TruckLoad = queryTruckLoad.ToList();
                var query_PlanGI = queryPlanGI.ToList();
                var query_Tote = findTote.ToList();
                var query_TaskIten = db.IM_TaskItem.Where(c => query_PlanGI.Select(s => s.PlanGoodsIssue_Index).Contains(c.PlanGoodsIssue_Index)).ToList();

                var sumTask = query_TaskIten.GroupBy(c => new
                {
                    c.PlanGoodsIssue_Index,
                    c.PlanGoodsIssueItem_Index,
                })
               .Select(c => new
               {
                   c.Key.PlanGoodsIssue_Index,
                   c.Key.PlanGoodsIssueItem_Index,

                   SumPickingQty = c.Sum(s => s.Picking_Qty),
               }).ToList();


                var query = (from TL in query_TruckLoad
                             join PlanGI in query_PlanGI on TL.PlanGoodsIssue_Index equals PlanGI.PlanGoodsIssue_Index into ps
                             from r in ps.DefaultIfEmpty()
                             join TaskIten in sumTask on r.PlanGoodsIssueItem_Index equals TaskIten.PlanGoodsIssueItem_Index into ti
                             from t in ti.DefaultIfEmpty()
                             orderby r.PlanGoodsIssue_No ascending
                             group TL by new
                             {
                                 TL?.TruckLoad_No,
                                 TL?.Create_By,
                                 TL?.Create_Date,
                                 TL?.Document_Remark,
                                 TL?.Expect_Delivery_Date,
                                 r?.PlanGoodsIssue_No,
                                 r?.Product_Id,
                                 r?.Product_Name,
                                 r?.Qty,
                                 r?.ProductConversion_Index,
                                 r?.ShipTo_Id,
                                 r?.ShipTo_Name,
                                 r?.ShipTo_Address,
                                 r?.LineNum,
                                 t?.SumPickingQty


                             } into G
                             select new
                             {
                                 G.Key.TruckLoad_No,
                                 G.Key.Create_By,
                                 G.Key.Create_Date,
                                 G.Key.Document_Remark,
                                 G.Key.Expect_Delivery_Date,
                                 G.Key.PlanGoodsIssue_No,
                                 G.Key.Product_Id,
                                 G.Key.Product_Name,
                                 G.Key.Qty,
                                 G.Key.ProductConversion_Index,
                                 G.Key.ShipTo_Id,
                                 G.Key.ShipTo_Name,
                                 G.Key.ShipTo_Address,
                                 G.Key.LineNum,
                                 G.Key.SumPickingQty,
                             }
                             ).ToList();



                if (query.Count == 0)
                {
                    var resultItem = new PrintOutShipmentModel();

                    resultItem.checkQuery = true;


                    result.Add(resultItem);
                }
                else
                {
                    foreach (var item in query.OrderBy(o => o.LineNum).OrderBy(o => o.Product_Id))
                    {
                        var resultItem = new PrintOutShipmentModel();

                        resultItem.truckLoad_No = item.TruckLoad_No;
                        resultItem.create_By = item.Create_By;
                        string creDate = item.Create_Date.toString();
                        string CreateTime = item.Create_Date.Value.ToString("HH:mm");
                        string CreateDate = DateTime.ParseExact(creDate.Substring(0, 8), "yyyyMMdd",
                        System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);
                        resultItem.create_Date = CreateDate + " เวลา : " + CreateTime;
                        resultItem.print_Date = item.Expect_Delivery_Date.GetValueOrDefault().ToString("dd/MM/yyyy");
                        resultItem.document_Remark = item.Document_Remark;
                        resultItem.planGoodsIssue_No = item.PlanGoodsIssue_No;
                        resultItem.product_Id = item.Product_Id;
                        resultItem.product_Name = item.Product_Name;
                        //resultItem.qty = item.Qty;
                        resultItem.qty = item.SumPickingQty;

                        if (resultProductConversion.Count > 0 && resultProductConversion != null)
                        {
                            var DataProductConversion = resultProductConversion.Find(c => c.productConversion_Index == item.ProductConversion_Index);
                            if (DataProductConversion != null)
                            {
                                Ratio = DataProductConversion.productConversion_Ratio;
                                resultItem.productConversion_Name = DataProductConversion.productConversion_Name;

                            }
                        }
                        //resultItem.qtyRatio = Ratio * item.Qty;
                        resultItem.qtyRatio = item.Qty;
                        resultItem.shipTo_Id = item.ShipTo_Id;
                        resultItem.shipTo_Name = item.ShipTo_Name;
                        resultItem.shipTo_Address = item.ShipTo_Address;
                        resultItem.truckLoad_Barcode = new NetBarcode.Barcode(item.TruckLoad_No, NetBarcode.Type.Code128B).GetBase64Image();
                        resultItem.planGoodsIssue_Barcode = new NetBarcode.Barcode(item.PlanGoodsIssue_No, NetBarcode.Type.Code128B).GetBase64Image();
                        resultItem.total_tote = query_Tote.Sum(c => c.total_tote).ToString();
                        if (findCarton != null)
                        {
                            resultItem.total_carton = findCarton.total_Carton.ToString();
                        }
                        else
                        {
                            resultItem.total_carton = "0";
                        }

                        result.Add(resultItem);
                    }
                    result.ToList();
                }
                rootPath = rootPath.Replace("\\LoadAPI", "");
                var reportPath = rootPath + new AppSettingConfig().GetUrl("ReportPrintOutShipment");
                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", result);

                System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                string fileName = "";
                string fullPath = "";
                fileName = "tmpReport" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var renderedBytes = report.Execute(RenderType.Pdf);

                Utils objReport = new Utils();
                fullPath = objReport.saveReport(renderedBytes.MainStream, fileName + ".pdf", rootPath);
                var saveLocation = objReport.PhysicalPath(fileName + ".pdf", rootPath);
                return saveLocation;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region ReportPrintOutHandover
        public string ReportPrintOutHandover(PrintOutShipmentModel data, string rootPath = "")
        {
            var checkDup = "";
            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            db.Database.SetCommandTimeout(360);
            try
            {
                var checkstatus = db.View_CheckReport_Status.Where(c => c.TruckLoad_Index == data.truckLoad_Index && c.Document_StatusDocktoStg == null).ToList();
                if (checkstatus.Count() > 0)
                {
                    return "";
                }
                var result = new List<HandoverModel>();

                var TM_NO = new SqlParameter("@TruckLoad_Index", data.truckLoad_Index);
                var rpt_data = db.View_RPT_Handover.FromSql("sp__RPT_Handover @TruckLoad_Index", TM_NO).OrderBy(c => c.drop_seq).ThenBy(c => c.plan_runing).ThenBy(c => c.Row_Runing_product).ToList();

                foreach (var item in rpt_data)
                {
                    var Splitdate = item.Appointment_Date.ToString().Split('/');
                    int Month = int.Parse(Splitdate[0]);
                    var year = (int.Parse((Splitdate[2].Split(' '))[0])) + 543;
                    var day = Splitdate[1];
                    var thaiMonth = "";
                    switch (Month)
                    {
                        case 1:
                            thaiMonth = "มกราคม";
                            break;
                        case 2:
                            thaiMonth = "กุมภาพันธ์";
                            break;
                        case 3:
                            thaiMonth = "มีนาคม";
                            break;
                        case 4:
                            thaiMonth = "เมษายน";
                            break;
                        case 5:
                            thaiMonth = "พฤษภาคม";
                            break;
                        case 6:
                            thaiMonth = "มิถุนายน";
                            break;
                        case 7:
                            thaiMonth = "กรกฎาคม";
                            break;
                        case 8:
                            thaiMonth = "สิงหาคม";
                            break;
                        case 9:
                            thaiMonth = "กันยายน";
                            break;
                        case 10:
                            thaiMonth = "ตุลาคม";
                            break;
                        case 11:
                            thaiMonth = "พฤศจิกายน";
                            break;
                        case 12:
                            thaiMonth = "ธันวาคม";
                            break;
                    }

                    var list = new HandoverModel();
                    list.truckLoad_Barcode = new NetBarcode.Barcode(item.TruckLoad_No, NetBarcode.Type.Code128B).GetBase64Image();
                    list.planGoodsIssue_Barcode = new NetBarcode.Barcode(item.PlanGoodsIssue_No, NetBarcode.Type.Code128B).GetBase64Image();
                    list.TruckLoad_No = item.TruckLoad_No;
                    list.VehicleCompany_Id = item.VehicleCompany_Id;
                    list.VehicleType_Name = item.VehicleType_Name;
                    list.Route = item.Route;
                    //list.Appointment_Date = item.Appointment_Date;
                    list.Appointment_Date = day + " " + thaiMonth + " " + year;
                    list.Appointment_Time = item.Appointment_Time;
                    list.count_drop = item.count_drop;
                    list.count_plan = item.count_plan;
                    list.count_retrun = item.count_retrun;
                    list.count_carton = item.count_carton ?? 0;
                    list.count_tote_L = item.count_tote_L ?? 0;
                    list.count_tote_M = item.count_tote_M ?? 0;
                    list.drop_seq = item.drop_seq;
                    list.branch = item.branch;
                    list.ShipTo_Name = item.ShipTo_Name;
                    list.ShipTo_Address = item.ShipTo_Address;
                    list.PlanGoodsIssue_No = item.PlanGoodsIssue_No;
                    list.carton = item.carton;
                    list.toteL = item.toteL;
                    list.toteM = item.toteM;
                    list.Product_Id = item.Product_Id;
                    list.Order_Qty = item.Order_Qty;
                    list.ProductConversion_Name = item.ProductConversion_Name;
                    list.pick = item.Pick == 0 ? null : item.Pick;
                    list.Driver_name = item.Driver_name;
                    list.Vehicle_Registration = item.Vehicle_Registration;
                    list.Product_Name = item.Product_Name;
                    list.Dock_name = item.Dock_name;
                    list.DateNow = DateTime.Now;
                    list.Row_Runing_product = item.Pick == 0 ? null : item.Row_Runing_product;
                    list.plan_runing = item.plan_runing + (item.Return == "0" ? null : "-" + "R");
                    list.ShippingMethod_Name = item.ShippingMethod_Name;
                    list.ShipTo_Tel = item.ShipTo_Tel;

                    list.Product_Lot = item.Product_Lot;

                    if (!string.IsNullOrEmpty(item.Tote_Size.Trim()))
                    {
                        if (item.IsShow == 1)
                        {
                            list.Tote_Size = item.Tote_Size;
                            list.Tagout_Runing = item.Tote_Size + " " + item.Tagout_Runing.TrimStart('0') + "/";
                            list.total_Box = item.total_Box;
                        }

                    }
                    list.PGI_set = item.PGI_set;

                    result.Add(list);
                }


                rootPath = rootPath.Replace("\\LoadAPI", "");
                var reportPath = rootPath + new AppSettingConfig().GetUrl("Handover");
                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", result);

                System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                string fileName = "";
                string fullPath = "";
                fileName = "tmpReport" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var renderedBytes = report.Execute(RenderType.Pdf);

                Utils objReport = new Utils();
                fullPath = objReport.saveReport(renderedBytes.MainStream, fileName + ".pdf", rootPath);
                var saveLocation = objReport.PhysicalPath(fileName + ".pdf", rootPath);
                return saveLocation;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region printOutTruckMenifest
        public string printOutTruckMenifest(PrintOutShipmentModel data, string rootPath = "")
        {
            int cc = 0;
            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                db.Database.SetCommandTimeout(360);

                var TM_NO = new SqlParameter("@TruckLoad_Index", data.truckLoad_Index);
                var rpt_data = db.View_RPT_Truck_Menifest.FromSql("sp_RPT_Truck_Menifest @TruckLoad_Index", TM_NO).OrderBy(c => c.drop_seq).ToList();

                var result = new List<TruckMenifestModel>();
                //var rpt_data = db.View_RPT_Truck_Menifest.Where(C => C.TruckLoad_Index == data.truckLoad_Index).OrderBy(c => c.drop_seq).ThenBy(c=> c.plan_runing).ToList();
                foreach (var item in rpt_data)
                {
                    cc++;
                    if (cc == 40)
                    {

                    }
                    if (string.IsNullOrEmpty(item.Billing))
                    {
                        var checkplan = db.im_PlanGoodsIssue_Ref.Where(c => c.PlanGoodsIssue_No == item.PlanGoodsIssue_No).ToList();
                        if (checkplan.Count() <= 0)
                        {
                            item.Billing = item.GoodsIssue_No;
                        }
                        else
                        {
                            return "";
                        }
                    }
                    var Splitdate = item.Appointment_Date.ToString().Split('/');
                    int Month = int.Parse(Splitdate[0]);
                    var year = (int.Parse((Splitdate[2].Split(' '))[0])) + 543;
                    var day = Splitdate[1];
                    var thaiMonth = "";
                    switch (Month)
                    {
                        case 1:
                            thaiMonth = "มกราคม";
                            break;
                        case 2:
                            thaiMonth = "กุมภาพันธ์";
                            break;
                        case 3:
                            thaiMonth = "มีนาคม";
                            break;
                        case 4:
                            thaiMonth = "เมษายน";
                            break;
                        case 5:
                            thaiMonth = "พฤษภาคม";
                            break;
                        case 6:
                            thaiMonth = "มิถุนายน";
                            break;
                        case 7:
                            thaiMonth = "กรกฎาคม";
                            break;
                        case 8:
                            thaiMonth = "สิงหาคม";
                            break;
                        case 9:
                            thaiMonth = "กันยายน";
                            break;
                        case 10:
                            thaiMonth = "ตุลาคม";
                            break;
                        case 11:
                            thaiMonth = "พฤศจิกายน";
                            break;
                        case 12:
                            thaiMonth = "ธันวาคม";
                            break;
                    }

                    var list = new TruckMenifestModel();
                    list.truckLoad_Barcode = new NetBarcode.Barcode(item.TruckLoad_No, NetBarcode.Type.Code128B).GetBase64Image();
                    list.planGoodsIssue_Barcode = new NetBarcode.Barcode(item.PlanGoodsIssue_No, NetBarcode.Type.Code128B).GetBase64Image();
                    list.appointment_Barcode = new NetBarcode.Barcode(item.Appointment_Id, NetBarcode.Type.Code128B).GetBase64Image();
                    list.TruckLoad_No = item.TruckLoad_No;
                    list.VehicleCompany_Id = item.VehicleCompany_Id;
                    list.VehicleType_Name = item.VehicleType_Name;
                    list.DocumentRef_No3 = item.DocumentRef_No3;
                    list.Appointment_Date = day + " " + thaiMonth + " " + year;
                    list.Appointment_Time = item.Appointment_Time;
                    list.count_drop = item.count_drop;
                    list.count_plan = item.count_plan;
                    list.count_retrun = item.count_retrun;
                    list.count_carton = item.count_carton ?? 0;
                    list.count_tote_L = item.count_tote_L ?? 0;
                    list.count_tote_M = item.count_tote_M ?? 0;
                    list.drop_seq = item.drop_seq;
                    list.branch = item.branch;
                    list.ShipTo_Name = item.ShipTo_Name;
                    list.ShipTo_Address = item.ShipTo_Address;
                    list.PlanGoodsIssue_No = item.PlanGoodsIssue_No;
                    list.Driver_name = item.Driver_name;
                    list.Vehicle_Registration = item.Vehicle_Registration;
                    list.Dock_name = item.Dock_name;
                    list.plan_runing = item.plan_runing;
                    list.ShippingMethod_Name = item.ShippingMethod_Name;
                    list.Billing = item.Billing;
                    list.PGI_type = item.PGI_type;
                    list.PlanGoodsIssue_Due_Date = item.PlanGoodsIssue_Due_Date;
                    list.Appointment_Id = item.Appointment_Id;
                    list.Remark = item.Remark;
                    list.Shipto_tel = item.Shipto_tel;
                    list.PGI_set = item.PGI_set;
                    list.Type_shipment = item.Type_shipment;
                    list.DocumentRef_No4 = item.DocumentRef_No4;
                    list.Expect_Delivery_Date = item.Expect_Delivery_Date == null ? null : item.Expect_Delivery_Date.GetValueOrDefault().ToString("dd/MM/yyyy");

                    result.Add(list);
                }

                rootPath = rootPath.Replace("\\LoadAPI", "");
                var reportPath = rootPath + new AppSettingConfig().GetUrl("TruckMenifest");
                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", result);

                System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                string fileName = "";
                string fullPath = "";
                fileName = "tmpReport" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var renderedBytes = report.Execute(RenderType.Pdf);

                Utils objReport = new Utils();
                fullPath = objReport.saveReport(renderedBytes.MainStream, fileName + ".pdf", rootPath);
                var saveLocation = objReport.PhysicalPath(fileName + ".pdf", rootPath);
                return saveLocation;


            }
            catch (Exception ex)
            {
                var xx = cc;
                throw ex;
            }
        }
        #endregion

        #region printOutTracePicking
        public actionResultTrace_pickingViewModel printOutTracePicking(Trace_picking data)
        {
            var culture = new System.Globalization.CultureInfo("en-US");
            var olog = new logtxt();
            try
            {
                db.Database.SetCommandTimeout(360);


                var GI = new SqlParameter("@GI", data.Goodsissue_Index == null ? "" : data.Goodsissue_Index);
                var TL = new SqlParameter("@TL", data.TruckLoad_Index == null ? "" : data.TruckLoad_Index);
                var PGL = new SqlParameter("@PGL", data.PlanGoodsIssue_Index == null ? "" : data.PlanGoodsIssue_Index);
                var PLI = new SqlParameter("@PLI", data.Pallet_No == null ? "" : data.Pallet_No);
                var TAO = new SqlParameter("@TAO", data.TagOut_No == null ? "" : data.TagOut_No);
                var LCTY = new SqlParameter("@LCTY", data.LocationType == null ? "" : data.LocationType);
                var LCTN = new SqlParameter("@LCTN", data.Current_location == null ? "" : data.Current_location);
                var ST = new SqlParameter("@ST", data.status == null ? "" : data.status);
                var CH = new SqlParameter("@CH", data.Chute_Id == null ? "" : data.Chute_Id);


                var rpt_data = db.View_Trace_picking.FromSql("sp_Trace_pick @GI ,@TL ,@PGL ,@PLI ,@TAO ,@LCTY ,@LCTN ,@CH ,@ST ", GI, TL, PGL, PLI, TAO, LCTY, LCTN, CH, ST).ToList();

                if (!string.IsNullOrEmpty(data.load_Date) && !string.IsNullOrEmpty(data.load_Date_To))
                {
                    var dateStart = data.load_Date.toBetweenDate();
                    var dateEnd = data.load_Date_To.toBetweenDate();
                    rpt_data = rpt_data.Where(c => c.TruckLoad_Date >= dateStart.start && c.TruckLoad_Date <= dateEnd.end).ToList();
                }

                var Item = new List<View_Trace_picking>();
                var TotalRow = new List<View_Trace_picking>();


                TotalRow = rpt_data.ToList();
                var Row = 1;

                if (!data.export)
                {
                    if (data.CurrentPage != 0 && data.PerPage != 0)
                    {
                        rpt_data = rpt_data.Skip(((data.CurrentPage - 1) * data.PerPage)).OrderBy(c => c.RowIndex).ToList();
                        Row = (data.CurrentPage == 1 ? 1 : ((data.CurrentPage - 1) * data.PerPage) + 1);
                    }

                    if (data.PerPage != 0)
                    {
                        rpt_data = rpt_data.Take(data.PerPage).OrderBy(c => c.RowIndex).ToList();

                    }
                    else
                    {
                        rpt_data = rpt_data.Take(50).OrderBy(c => c.RowIndex).ToList();
                    }
                }


                var result = new List<Trace_picking>();
                foreach (var item in rpt_data)
                {
                    Trace_picking trace = new Trace_picking();
                    trace.RowIndex = Row;
                    trace.Goodsissue_No = item.Goodsissue_No;
                    trace.TruckLoad_No = item.TruckLoad_No;
                    trace.TruckLoad_Date = item.TruckLoad_Date;
                    trace.PlanGoodsIssue_No = item.PlanGoodsIssue_No;
                    trace.Pallet_No = item.Pallet_No;
                    trace.TagOut_No = item.TagOut_No;
                    trace.Product_Id = item.Product_Id;
                    trace.Product_Name = item.Product_Name;
                    trace.Qty = item.Qty;
                    trace.ProductConversion_Name = item.ProductConversion_Name;
                    trace.Product_Lot = item.Product_Lot;
                    trace.LocationType = item.LocationType;
                    trace.Pick_location = item.Pick_location;
                    trace.Old_location = item.Old_location;
                    trace.Current_location = item.Current_location;
                    trace.status = item.status;
                    trace.Chute_Id = item.Chute_Id;
                    trace.RollCage_Id = item.RollCage_Id;
                    trace.DocumentRef_No5 = item.DocumentRef_No5;
                    trace.DocumentRef_No2 = item.DocumentRef_No2;
                    trace.TagOutRef_No1 = item.TagOutRef_No1;
                    trace.PickingPickQty_By = item.PickingPickQty_By;
                    trace.PickingPickQty_Date = item.PickingPickQty_Date;
                    Row++;
                    result.Add(trace);
                }
                var count = TotalRow.Count;
                var actionResult = new actionResultTrace_pickingViewModel();
                actionResult.itemsTrace = result.ToList();
                actionResult.pagination = new Pagination() { TotalRow = count, CurrentPage = data.CurrentPage, PerPage = data.PerPage, };
                return actionResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region printOutTraceLoading
        public actionResultTrace_loadingViewModel printOutTraceLoading(Trace_loading data)
        {
            var culture = new System.Globalization.CultureInfo("en-US");
            var olog = new logtxt();
            try
            {
                db.Database.SetCommandTimeout(360);


                var TL = new SqlParameter("@TL", data.TruckLoad_Index == null ? "" : data.TruckLoad_Index);
                var PGL = new SqlParameter("@PGL", data.PlanGoodsIssue_Index == null ? "" : data.PlanGoodsIssue_Index);
                var TAO = new SqlParameter("@TAO", data.TagOut_No == null ? "" : data.TagOut_No);
                var DK = new SqlParameter("@DK", data.Dock_Index == null ? "" : data.Dock_Index);
                var CH = new SqlParameter("@CH", data.Chute_Id == null ? "" : data.Chute_Id);
                var RC = new SqlParameter("@RC", data.RollCage_Index == null ? "" : data.RollCage_Index);
                var ST = new SqlParameter("@ST", data.status == null ? "" : data.status);
                var APT = new SqlParameter("@APT", data.Appointment_Time == null ? "" : data.Appointment_Time);


                var rpt_data = db.View_Trace_Loading.FromSql("sp_Trace_Loading @TL ,@PGL ,@TAO ,@DK ,@CH ,@RC ,@ST ,@APT ", TL, PGL, TAO, DK, CH, RC, ST, APT).ToList();

                if (!string.IsNullOrEmpty(data.load_Date) && !string.IsNullOrEmpty(data.load_Date_To))
                {
                    var dateStart = data.load_Date.toBetweenDate();
                    var dateEnd = data.load_Date_To.toBetweenDate();
                    rpt_data = rpt_data.Where(c => c.TruckLoad_Date >= dateStart.start && c.TruckLoad_Date <= dateEnd.end).ToList();
                }

                var Item = new List<View_Trace_Loading>();
                var TotalRow = new List<View_Trace_Loading>();


                TotalRow = rpt_data.ToList();
                var Row = 1;

                if (!data.export)
                {
                    if (data.CurrentPage != 0 && data.PerPage != 0)
                    {
                        rpt_data = rpt_data.Skip(((data.CurrentPage - 1) * data.PerPage)).OrderBy(c => c.RowIndex).ToList();
                        Row = (data.CurrentPage == 1 ? 1 : ((data.CurrentPage - 1) * data.PerPage) + 1);
                    }

                    if (data.PerPage != 0)
                    {
                        rpt_data = rpt_data.Take(data.PerPage).OrderBy(c => c.RowIndex).ToList();

                    }
                    else
                    {
                        rpt_data = rpt_data.Take(50).OrderBy(c => c.RowIndex).ToList();
                    }
                }


                var result = new List<Trace_loading>();
                foreach (var item in rpt_data)
                {
                    Trace_loading trace = new Trace_loading();
                    trace.RowIndex = Row;
                    trace.TruckLoad_No = item.TruckLoad_No;
                    trace.Dock_Name = item.Dock_Name;
                    trace.TruckLoad_Date = item.TruckLoad_Date;
                    trace.PlanGoodsIssue_No = item.PlanGoodsIssue_No;
                    trace.Chute_Id = item.Chute_Id;
                    trace.RollCage_Name = item.RollCage_Name;
                    trace.IsTote = item.IsTote;
                    trace.TagOut_No = item.TagOut_No;
                    trace.Product_Id = item.Product_Id;
                    trace.Product_Name = item.Product_Name;
                    trace.Qty = item.Qty;
                    trace.ProductConversion_Name = item.ProductConversion_Name;
                    trace.Product_Lot = item.Product_Lot;
                    trace.status = item.status;
                    trace.LocationType = item.LocationType;
                    trace.TagOutRef_No1 = item.TagOutRef_No1;
                    trace.Address = item.Address;

                    Row++;
                    result.Add(trace);
                }
                var count = TotalRow.Count;
                var actionResult = new actionResultTrace_loadingViewModel();
                actionResult.itemsTrace = result.ToList();
                actionResult.pagination = new Pagination() { TotalRow = count, CurrentPage = data.CurrentPage, PerPage = data.PerPage, };
                return actionResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region printOutDeliveryNote
        public string printOutDeliveryNote(PrintOutShipmentModel data, string rootPath = "")
        {

            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                var result = new List<DeliveryNote>();
                var rpt_data = db.View_RPT_Delivery_Note.Where(C => C.TruckLoad_Index == data.truckLoad_Index).ToList();
                foreach (var item in rpt_data)
                {
                    var list = new DeliveryNote();
                    list.truckLoad_Barcode = new NetBarcode.Barcode(item.TruckLoad_No, NetBarcode.Type.Code128B).GetBase64Image();
                    list.Billing_Barcode = new NetBarcode.Barcode(item.Billing, NetBarcode.Type.Code128B).GetBase64Image();
                    list.TruckLoad_No = item.TruckLoad_No;
                    list.Appointment_Date = item.Appointment_Date;
                    list.Appointment_Time = item.Appointment_Time;
                    list.branch = item.branch;
                    list.ShipTo_Name = item.ShipTo_Name;
                    list.ShipTo_Address = item.ShipTo_Address;
                    list.PlanGoodsIssue_No = item.PlanGoodsIssue_No;
                    list.Billing = item.Billing;
                    list.Product_Id = item.Product_Id;
                    list.Product_Name = item.Product_Name;
                    list.Product_Lot = item.Product_Lot;
                    list.ProductConversion_Name = item.ProductConversion_Name;
                    list.ShipTo_Id = item.ShipTo_Id;


                    result.Add(list);
                }

                rootPath = rootPath.Replace("\\LoadAPI", "");
                var reportPath = rootPath + new AppSettingConfig().GetUrl("DeliveryNote");
                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", result);

                System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                string fileName = "";
                string fullPath = "";
                fileName = "tmpReport" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var renderedBytes = report.Execute(RenderType.Pdf);

                Utils objReport = new Utils();
                fullPath = objReport.saveReport(renderedBytes.MainStream, fileName + ".pdf", rootPath);
                var saveLocation = objReport.PhysicalPath(fileName + ".pdf", rootPath);
                return saveLocation;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Taskreturntote
        public string Taskreturntote()
        {
            logtxt LoggingService = new logtxt();
            try
            {
                LoggingService.DataLogLines("Task Return Tote", "Task_Return_Tote" + DateTime.Now.ToString("yyyy-MM-dd"), "Start Task       : " + DateTime.Now.ToString("yyyy-MM-dd-HHmm"));
                var get_truckload = db.im_TruckLoad.Where(c => c.UDF_5 == null && c.Document_Status != -1).ToList();
                if (get_truckload.Count() > 0)
                {
                    var getView_returntote = db.View_ToteReturnTMS.Where(c => get_truckload.Select(x => x.TruckLoad_Index).Contains(c.TruckLoad_Index) && c.TruckLoad_Return_Date != null).ToList();
                    LoggingService.DataLogLines("Task Return Tote", "Task_Return_Tote" + DateTime.Now.ToString("yyyy-MM-dd"), "Select Truckload : " + getView_returntote.Count());
                    foreach (var item in getView_returntote)
                    {
                        LoggingService.DataLogLines("Task Return Tote", "Task_Return_Tote" + DateTime.Now.ToString("yyyy-MM-dd"), "Truckload No.    : " + item.TruckLoad_No);
                        LoggingService.DataLogLines("Task Return Tote", "Task_Return_Tote" + DateTime.Now.ToString("yyyy-MM-dd"), "Return Date.     : " + item.TruckLoad_Return_Date);
                        LoggingService.DataLogLines("Task Return Tote", "Task_Return_Tote" + DateTime.Now.ToString("yyyy-MM-dd"), "Tote L Max.      : " + item.Return_Tote_MAX_XL);
                        LoggingService.DataLogLines("Task Return Tote", "Task_Return_Tote" + DateTime.Now.ToString("yyyy-MM-dd"), "Tote M Max.      : " + item.Return_Tote_MAX_M);
                        LoggingService.DataLogLines("Task Return Tote", "Task_Return_Tote" + DateTime.Now.ToString("yyyy-MM-dd"), "Doc Max.         : " + item.DocReturn_Max);
                        LoggingService.DataLogLines("Task Return Tote", "Task_Return_Tote" + DateTime.Now.ToString("yyyy-MM-dd"), "G Tote L.        : " + item.Return_Tote_Qty_XL);
                        LoggingService.DataLogLines("Task Return Tote", "Task_Return_Tote" + DateTime.Now.ToString("yyyy-MM-dd"), "G Tote M.        : " + item.Return_Tote_Qty_M);
                        LoggingService.DataLogLines("Task Return Tote", "Task_Return_Tote" + DateTime.Now.ToString("yyyy-MM-dd"), "D Tote L.        : " + item.Return_Tote_Qty_DMG_XL);
                        LoggingService.DataLogLines("Task Return Tote", "Task_Return_Tote" + DateTime.Now.ToString("yyyy-MM-dd"), "D Tote M.        : " + item.Return_Tote_Qty_DMG_M);
                        LoggingService.DataLogLines("Task Return Tote", "Task_Return_Tote" + DateTime.Now.ToString("yyyy-MM-dd"), "Doc.             : " + item.Return_Doc);

                        var Jnew = new
                        {
                            truckload_index = item.TruckLoad_Index,
                            truckload_no = item.TruckLoad_No,
                            tm_index = item.TM_Index,
                            truckload_return_date = item.TruckLoad_Return_Date,
                            return_tote_max_xl = item.Return_Tote_MAX_XL,
                            return_tote_max_m = item.Return_Tote_MAX_M,
                            docreturn_max = item.DocReturn_Max,
                            return_tote_qty_xl = item.Return_Tote_Qty_XL,
                            return_tote_qty_m = item.Return_Tote_Qty_M,
                            return_tote_qty_dmg_xl = item.Return_Tote_Qty_DMG_XL,
                            return_tote_qty_dmg_m = item.Return_Tote_Qty_DMG_M,
                            return_doc = item.Return_Doc
                        };

                        LoggingService.DataLogLines("Task Return Tote", "Task_Return_Tote" + DateTime.Now.ToString("yyyy-MM-dd"), "Send TMS         : " + JsonConvert.SerializeObject(Jnew));
                        var result_api = Utils.SendDataApi<Message_return_tote>(new AppSettingConfig().GetUrl("Status_task"), JsonConvert.SerializeObject(Jnew));
                        LoggingService.DataLogLines("Task Return Tote", "Task_Return_Tote" + DateTime.Now.ToString("yyyy-MM-dd"), "Send TMS Result  : " + result_api.result);
                        if (result_api.result)
                        {
                            var updateTruckload = db.im_TruckLoad.FirstOrDefault(c => c.TruckLoad_Index == item.TruckLoad_Index);
                            updateTruckload.UDF_5 = "Y";

                            var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                            try
                            {
                                db.SaveChanges();
                                transactionx.Commit();
                            }

                            catch (Exception exy)
                            {
                                transactionx.Rollback();

                                throw exy;

                            }
                        }
                        else
                        {
                            continue;
                        }

                        LoggingService.DataLogLines("Task Return Tote", "Task_Return_Tote" + DateTime.Now.ToString("yyyy-MM-dd"), "End              : --------------------------------------------------------------");
                    }
                }
                else
                {
                    return "No Return";
                }


                return "OK";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Dockfillter
        public List<Dock> Dockfillter(trace_loading_model data)
        {
            var olog = new logtxt();
            String msglog = "";
            try
            {
                var result = new List<Dock>();

                var query = Masterdb.Ms_Dock.Where(c => c.DockType_Index == Guid.Parse("BDB6CC26-B144-4E44-BC3F-F8E78E0E97AE") && c.IsActive == 1 && c.IsDelete == 0).AsQueryable();

                if (!string.IsNullOrEmpty(data.Dock_Index))
                {
                    query = query.Where(c => c.Dock_Index == Guid.Parse(data.Dock_Index));
                }


                var queryResult = query.OrderBy(o => o.Dock_Id).ToList();

                foreach (var item in queryResult)
                {
                    var resultItem = new Dock();
                    resultItem.Dock_Index = item.Dock_Index;
                    resultItem.Dock_Id = item.Dock_Id;
                    resultItem.Dock_Name = item.Dock_Name;

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

        #region Rollcagefillter
        public List<RollCage> Rollcagefillter(trace_loading_model data)
        {
            try
            {
                var result = new List<RollCage>();
                var query = new List<ms_RollCage>();
                if (!string.IsNullOrEmpty(data.TruckLoad_Index))
                {
                    List<Guid> rollCageOrders = db.im_RollCageOrder.Where(c => c.TruckLoad_No ==data.TruckLoad_No).GroupBy(c => c.RollCage_Index).Select(c => c.Key).ToList();
                    query = Masterdb.ms_RollCage.Where(c => rollCageOrders.Contains(c.RollCage_Index) && c.IsActive == 1 && c.IsDelete == 0).ToList();
                }
                else {
                    query = Masterdb.ms_RollCage.Where(c => c.IsActive == 1 && c.IsDelete == 0).ToList();
                }
                
                var queryResult = query.OrderBy(o => int.Parse(o.RollCage_Id)).ToList();

                foreach (var item in queryResult)
                {
                    var resultItem = new RollCage();
                    resultItem.RollCage_Index = item.RollCage_Index;
                    resultItem.RollCage_Id = item.RollCage_Id;
                    resultItem.RollCage_Name = item.RollCage_Name;

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

        #region Appointtimefilter
        public List<Appointment_time> Appointtimefilter(trace_loading_model data)
        {
            try
            {
                var result = new List<Appointment_time>();

                var truckLoadProcessStatuses = db.View_TruckLoadProcessStatus.Where(c => c.Document_Status != -1).GroupBy(c => c.Appointment_Time).Select(c=> c.Key).ToList();

                foreach (var item in truckLoadProcessStatuses)
                {
                    var resultItem = new Appointment_time();
                    resultItem.Appointment_Time = item;

                    result.Add(resultItem);
                }
                return result.OrderBy(c=> c.Appointment_Time).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region CheckCancle_shipment_bymapRound
        public Result CheckCancle_shipment_bymapRound(string data)
        {
            logtxt LoggingService = new logtxt();
            Result result = new Result();
            try
            {
                LoggingService.DataLogLines("Checkmap_Round", "Checkmap_Round" + DateTime.Now.ToString("yyyy-MM-dd"), "Start Check                            : --Time-- " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));

                SearchDetailModel Models = JsonConvert.DeserializeObject<SearchDetailModel>(data);
                LoggingService.DataLogLines("Checkmap_Round", "Checkmap_Round" + DateTime.Now.ToString("yyyy-MM-dd"), "Json 1                                 : "+ data + " --Time-- " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                LoggingService.DataLogLines("Checkmap_Round", "Checkmap_Round" + DateTime.Now.ToString("yyyy-MM-dd"), "Json 2                                 : "+ JsonConvert.SerializeObject(Models) + " --Time-- " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                LoggingService.DataLogLines("Checkmap_Round", "Checkmap_Round" + DateTime.Now.ToString("yyyy-MM-dd"), "Check truckLoad_No                     : "+ Models.truckLoad_No+ " --Time-- " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                List<im_TruckLoad> truckLoad = db.im_TruckLoad.Where(c => c.TruckLoad_No == Models.truckLoad_No).ToList();
                LoggingService.DataLogLines("Checkmap_Round", "Checkmap_Round" + DateTime.Now.ToString("yyyy-MM-dd"), "data truckLoad Count                   : " + truckLoad.Count + " --Time-- " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                if (truckLoad.Count < 1)
                {
                    result.resultIsUse = false;
                    result.reason_code = 2;
                    result.resultMsg = "Shipment not found";
                    LoggingService.DataLogLines("Checkmap_Round", "Checkmap_Round" + DateTime.Now.ToString("yyyy-MM-dd"), "data truckLoad Count E             : " + result.resultMsg + " --Time-- " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    return result;
                }
                else {
                    List<im_TruckLoad> truckLoad_status = truckLoad.Where(c => c.Document_Status == 0).ToList();
                    LoggingService.DataLogLines("Checkmap_Round", "Checkmap_Round" + DateTime.Now.ToString("yyyy-MM-dd"), "data truckLoad_status Count        : " + truckLoad_status.Count + " --Time-- " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    if (truckLoad_status.Count < 1)
                    {
                        result.resultIsUse = false;
                        result.reason_code = 2;
                        result.resultMsg = "สถานะของ shipment ไม่สามารถถอยได้";
                        LoggingService.DataLogLines("Checkmap_Round", "Checkmap_Round" + DateTime.Now.ToString("yyyy-MM-dd"), "data truckLoad_status Count E  : " + result.resultMsg + " --Time-- " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                        return result;
                    }
                    else {
                        List<Guid> truckload_guid = truckLoad_status.GroupBy(c => c.TruckLoad_Index).Select(c => c.Key).ToList();
                        LoggingService.DataLogLines("Checkmap_Round", "Checkmap_Round" + DateTime.Now.ToString("yyyy-MM-dd"), "data truckload_guid            : " + JsonConvert.SerializeObject(truckload_guid) + " --Time-- " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));

                        List<Guid?> planGoodsissue_guid = db.im_TruckLoadItem.Where(c => truckload_guid.Contains(c.TruckLoad_Index)).GroupBy(c=> c.PlanGoodsIssue_Index).Select(c=> c.Key).ToList();
                        LoggingService.DataLogLines("Checkmap_Round", "Checkmap_Round" + DateTime.Now.ToString("yyyy-MM-dd"), "data planGoodsissue_guid       : " + JsonConvert.SerializeObject(planGoodsissue_guid) + " --Time-- " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));

                        List<im_PlanGoodsIssue> planGoodsIssues = db.im_PlanGoodsIssue.Where(c => planGoodsissue_guid.Contains(c.PlanGoodsIssue_Index) && c.Document_Status == 1 && c.Round_Index != null).ToList();
                        LoggingService.DataLogLines("Checkmap_Round", "Checkmap_Round" + DateTime.Now.ToString("yyyy-MM-dd"), "data planGoodsIssues Count     : " + planGoodsIssues.Count + " --Time-- " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                        if (planGoodsIssues.Count > 0)
                        {
                            result.resultIsUse = false;
                            result.reason_code = 2;
                            result.resultMsg = "Shipment ทำการบันทึกรอบรียบร้อยแล้ว กรุณาติดต่อฝ่ายคลัง";
                            LoggingService.DataLogLines("Checkmap_Round", "Checkmap_Round" + DateTime.Now.ToString("yyyy-MM-dd"), "data planGoodsIssue Count E: " + result.resultMsg + " --Time-- " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                            return result;
                        }
                        else {
                            result.resultIsUse = true;
                            result.reason_code = 0;
                            LoggingService.DataLogLines("Checkmap_Round", "Checkmap_Round" + DateTime.Now.ToString("yyyy-MM-dd"), "data planGoodsIssue Count S:  --Time-- " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                        }
                    }
                }


                return result;
            }
            catch (Exception ex)
            {
                result.resultIsUse = false;
                result.reason_code = 2;
                result.resultMsg = ex.Message;
                LoggingService.DataLogLines("Checkmap_Round", "Checkmap_Round" + DateTime.Now.ToString("yyyy-MM-dd"), "EX : " + JsonConvert.SerializeObject(ex) + " --Time-- " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                return result;
            }
        }
        #endregion

        public string SaveLogRequest(string orderno, string json, string interfacename, int status, string txt, Guid logindex)
        {
            try
            {
                log_api_request l = new log_api_request();
                l.log_id = logindex;
                l.log_date = DateTime.Now;
                l.log_requestbody = json;
                l.log_absoluteuri = "";
                l.status = status;
                l.Interface_Name = interfacename;
                l.Status_Text = txt;
                l.File_Name = orderno;
                db.log_api_request.Add(l);
                db.SaveChanges();
                return "";
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public string SaveLogResponse(string orderno, string json, string interfacename, int status, string txt, Guid logindex)
        {
            try
            {
                log_api_reponse l = new log_api_reponse();
                l.log_id = logindex;
                l.log_date = DateTime.Now;
                l.log_reponsebody = json;
                l.log_absoluteuri = "";
                l.status = status;
                l.Interface_Name = interfacename;
                l.Status_Text = txt;
                l.File_Name = orderno;
                db.log_api_reponse.Add(l);

                //var d = db.log_api_request.Where(c => c.log_id == logindex).FirstOrDefault();
                //d.status = status;

                db.SaveChanges();
                return "";
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}