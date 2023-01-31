using AspNetCore.Reporting;
using Business.Library;
using Comone.Utils;
using DataAccess;
using LoadBusiness.Reports.ShortShip;
using LoadBusiness.ShortShip;
using LoadBusiness;
using LoadDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PlanGIBusiness.Libs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using static LoadBusiness.ShortShip.ShortShipItemViewModel;
using static LoadBusiness.ShortShip.SearchTruckLoad;
using LoadBusiness.Load;

namespace LoadBusiness.ShortShip
{
    public class ShortShipService
    {
        #region LoadDbContext
        private LoadDbContext db;

        public ShortShipService()
        {
            db = new LoadDbContext();
        }

        public ShortShipService(LoadDbContext db)
        {
            this.db = db;
        }
        #endregion


        #region filter
        public ShortShipModel filter(ShortShipModel data)
        {
            logtxt LoggingService = new logtxt();
            ShortShipModel result = new ShortShipModel();
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
            try
            {
                LoggingService.DataLogLines("Short Ship Filter", "Filter" + DateTime.Now.ToString("yyyy-MM-dd"), "Start filter          : " + DateTime.Now.ToString("yyyy-MM-dd-HHmm"));
                LoggingService.DataLogLines("Short Ship Filter", "Filter" + DateTime.Now.ToString("yyyy-MM-dd"), "Filter data           : " + JsonConvert.SerializeObject(data));
                if (string.IsNullOrEmpty(data.truckLoad_No) && string.IsNullOrEmpty(data.planGoodsissue_no))
                {
                    result.resultIsUse = false;
                    result.resultMsg = "กรุณาทำการค้นหาด้วย Shipment No หรือ DO No";
                    LoggingService.DataLogLines("Short Ship Filter", "Filter" + DateTime.Now.ToString("yyyy-MM-dd"), "Filter data Validate  : " + result.resultMsg);
                    return result;
                }
                else {
                    List<Item> resultitem = new List<Item>();
                    List<PlanGIDataAccess.Models.View_ShortShip> rpt_data = db.View_ShortShip.AsQueryable().ToList();

                    var filtershortShip = rpt_data.GroupBy(c => new {
                        c.PlanGoodsIssue_No
                        ,c.TruckLoad_No
                    }).Select(c=> new {
                        c.Key.PlanGoodsIssue_No
                        ,c.Key.TruckLoad_No
                        ,status_shortship = ""
                        ,status_Doc = ""
                        ,user = ""
                        ,update_date = ""
                    }).ToList();

                    if (!string.IsNullOrEmpty(data.truckLoad_No))
                    {
                        filtershortShip = filtershortShip.Where(c => c.TruckLoad_No == data.truckLoad_No).ToList();
                    }
                    if (!string.IsNullOrEmpty(data.planGoodsissue_no))
                    {
                        filtershortShip = filtershortShip.Where(c => c.PlanGoodsIssue_No == data.planGoodsissue_no).ToList();
                    }

                    LoggingService.DataLogLines("Short Ship Filter", "Filter" + DateTime.Now.ToString("yyyy-MM-dd"), "Filter data Count     : " + filtershortShip.Count);
                    foreach (var item in filtershortShip)
                    {
                        var list = new Item();
                        list.TruckLoad_No = item.TruckLoad_No;
                        list.PlanGoodsIssue_No = item.PlanGoodsIssue_No;
                       

                        resultitem.Add(list);
                    }
                    result.resultIsUse = true;
                    result.items = resultitem;
                }
                return result;
            }
            catch (Exception ex)
            {
                LoggingService.DataLogLines("Short Ship Filter", "Filter" + DateTime.Now.ToString("yyyy-MM-dd"), "Exception             : " + ex);
                result.resultIsUse = false;
                result.resultMsg = "ไม่สามารถทำการค้นหาข้อมูลได้";
                return result;
            }
        }
        #endregion
        #region
        public actionResultViewModel filterV2(ShortShipViewModel model)
        {
            
            try
            {
                var query = db.View_ShortShipV2.AsQueryable();
                var statusModels = new List<string>();
                #region Search filter
                if (!string.IsNullOrEmpty(model.truckLoad_No))
                {
                    query = query.Where(c => c.TruckLoad_No.Contains(model.truckLoad_No));
                }

                if (!string.IsNullOrEmpty(model.planGoodsIssue_No))
                {
                    query = query.Where(c => c.PlanGoodsIssue_No.Contains(model.planGoodsIssue_No));
                }

                if (!string.IsNullOrEmpty(model.processStatus_Id))
                {
                    query = query.Where(c => c.ProcessStatus_Id == model.processStatus_Id);
                }

                if (!string.IsNullOrEmpty(model.truckLoad_Date) && !string.IsNullOrEmpty(model.truckLoad_Date_To))
                {
                    var dateStart = model.truckLoad_Date.toBetweenDate();
                    var dateEnd = model.truckLoad_Date_To.toBetweenDate();
                    query = query.Where(c => c.Appointment_Date >= dateStart.start && c.Appointment_Date <= dateEnd.end);
                }
                else if (!string.IsNullOrEmpty(model.truckLoad_Date))
                {
                    var truckLoad_date_From = model.truckLoad_Date.toBetweenDate();
                    query = query.Where(c => c.Appointment_Date >= truckLoad_date_From.start);
                }
                else if (!string.IsNullOrEmpty(model.truckLoad_Date_To))
                {
                    var truckLoad_date_To = model.truckLoad_Date_To.toBetweenDate();
                    query = query.Where(c => c.Appointment_Date <= truckLoad_date_To.start);
                }

                if (model.status.Count > 0)
                {
                    foreach (var item in model.status)
                    {
                        //statusModels.Add(item.value);
                        if (item.value == 0)
                        {
                            statusModels.Add("YES");
                        }
                    }
                    query = query.Where(c => statusModels.Contains(c.ShortShip_Status));
                }
                #endregion

                var TotalRow = new List<View_ShortShipV2>();

                TotalRow = query.OrderByDescending(o => o.Appointment_Date).ToList();
                if (model.CurrentPage != 0 && model.PerPage != 0)
                {
                    query = query.Skip(((model.CurrentPage - 1) * model.PerPage));
                }

                if (model.PerPage != 0)
                {
                    query = query.Take(model.PerPage);

                }
                //if (model.sort.Count > 0)
                //{
                //    Item = query.ToList();
                //}
                //else
                //{
                //    Item = query.OrderBy(c => c.PlanGoodsIssue_No).ToList();
                //}

                var resultquery = query.ToList();

                var listResult = new List<ShortShipViewModel>();

                foreach (var list in resultquery)
                {
                    var resultItem = new ShortShipViewModel();
                    resultItem.appointment_Index = list.Appointment_Index;
                    resultItem.appointment_Id = list.Appointment_Id;
                    resultItem.shipment_date = list.Appointment_Date.toString();
                    resultItem.appointment_Time = list.Appointment_Time;
                    resultItem.truckLoad_Index = list.TruckLoad_Index;
                    resultItem.truckLoad_No = list.TruckLoad_No;
                    resultItem.planGoodsIssue_Index = list.PlanGoodsIssue_Index;
                    resultItem.planGoodsIssue_No = list.PlanGoodsIssue_No;
                   
                    resultItem.shotShip_Status = list.ShortShip_Status;
                    resultItem.documentType_Index = list.processStatus_Index;
                    resultItem.documentType_Id = list.ProcessStatus_Id;
                    resultItem.documnetType_Name = list.processStatus_Name;
                    
                    resultItem.update_By = (!string.IsNullOrEmpty(list.Update_By)) ? list.Update_By : "";
                    resultItem.update_date = (!string.IsNullOrEmpty(list.Update_Date.toString()))? 
                                                    list.Update_Date.Value.ToString("dd/MM/yyyy") : 
                                                    "";
                    resultItem.shortQty = list.ShortQty;
                    listResult.Add(resultItem);
                }
                var count = TotalRow.Count;
                var ResultTruckLoad = new actionResultViewModel();
                ResultTruckLoad.shortShipList = listResult.ToList();
                ResultTruckLoad.pagination = new Pagination() { TotalRow = count, CurrentPage = model.CurrentPage, PerPage = model.PerPage, };
                ResultTruckLoad.resultIsUse = true;
                return ResultTruckLoad;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region find
        //public im_TruckLoad find(Guid id)
        //{
        //    try
        //    {
        //        var queryResult = db.im_TruckLoadItem.Where(c => c.PlanGoodsIssue_No == id.ToString()).FirstOrDefault();

        //    }
        //}
        #endregion
        #region approveStatus
        public Boolean approveStatus(ShortShipViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                var TagoutShortShip = db.wm_TagOutShortShip.Find(data.planGoodsIssue_Index);
                if (TagoutShortShip != null)
                {
                    TagoutShortShip.ProcessStatus_Id = "1";
                    TagoutShortShip.Update_By = data.update_By;
                    TagoutShortShip.Update_Date = DateTime.Now;

                }
                else
                {
                    wm_TagOutShortShip resultItem = new wm_TagOutShortShip();
                    resultItem.Appointment_Index = data.appointment_Index;
                    resultItem.Appointment_Id = data.appointment_Id;
                    resultItem.Appointment_Date = data.shipment_date.toDate();
                    resultItem.Appointment_Time = data.appointment_Time;
                    resultItem.TruckLoad_Index = data.truckLoad_Index;
                    resultItem.TruckLoad_No = data.truckLoad_No;
                    resultItem.PlanGoodsIssue_Index = data.planGoodsIssue_Index;
                    resultItem.PlanGoodsIssue_No = data.planGoodsIssue_No;
                    resultItem.ProcessStatus_Id = "1";
                    resultItem.ShortQty = data.shortQty;
                    resultItem.Create_By = data.update_By;
                    resultItem.Create_Date = DateTime.Now;
                    resultItem.Update_By = data.update_By;
                    resultItem.Update_Date = DateTime.Now;
                    db.wm_TagOutShortShip.Add(resultItem);
                }
                    
                State = "transactionSave";

                var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transactionx.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("RollBackSaveShortShip", msglog);
                    transactionx.Rollback();

                    throw exy;
                    

                }
                return true;
                
            }
            catch (Exception ex)
            {
                msglog = State + " Error " + ex.Message.ToString();
                olog.logging("ErrorSaveShortShip", msglog);
                throw ex;
            }
        }
        #endregion

        #region findItem
        public List<ShortShipItemViewModel> find(ShortShipItemViewModel model)
        {
            try
            {
                var result = new List<ShortShipItemViewModel>();
                var queryResult = db.View_TagOutItemShortship.Where(c => c.PlangoodsIssue_index == model.plangoodsIssue_index);

                if(!string.IsNullOrEmpty(model.product_Id))
                {
                    queryResult = queryResult.Where(c => c.Product_Id == model.product_Id);
                }

                if(!string.IsNullOrEmpty(model.tagOutNo))
                {
                    queryResult = queryResult.Where(c => c.TagOut_No == model.tagOutNo);
                }

                if (!string.IsNullOrEmpty(model.dropOrderSeq))
                {
                    queryResult = queryResult.Where(c => c.DropSeq + "/" + c.OrderSeq == model.dropOrderSeq);
                }

                foreach (var item in queryResult.OrderBy(c => c.LineNum.sParse<int>()))
                {
                    var resultItem = new ShortShipItemViewModel();

                    resultItem.shortShipItem_Index = item.ShortShipItem_Index;
                    resultItem.plangoodsIssue_index = item.PlangoodsIssue_index;
                    resultItem.planGoodsIssue_No = item.PlanGoodsIssue_No;
                    resultItem.planGoodsIssueItem_Index = item.PlanGoodsIssueItem_Index;
                    resultItem.goodsIssueItemLocation_Index = item.GoodsIssueItemLocation_Index;
                    resultItem.product_Id = item.Product_Id;
                    resultItem.product_name = item.Product_name;
                    resultItem.tagOut_Index = item.TagOut_Index;
                    resultItem.tagOut_No = item.TagOut_No;
                    resultItem.issueQty = item.IssueQty;
                    resultItem.orderQty = item.OrderQty;
                    resultItem.shortQty = item.ShortQty;
                    resultItem.receiveQty = item.ReceiveQty;
                    resultItem.productConversion_Name = item.productConversion_Name;
                    resultItem.toteSize = item.toteSize;
                    resultItem.dropSeq = item.DropSeq;
                    resultItem.orderSeq = item.OrderSeq;
                    resultItem.drop_orderSeq = (!string.IsNullOrEmpty(item.DropSeq) || !string.IsNullOrEmpty(item.OrderSeq)) 
                                          ? item.DropSeq + "/" + item.OrderSeq : "";
                    resultItem.statusItem = item.StatusItem;
                    resultItem.update_By = item.Update_By;
                    resultItem.update_Date =(!string.IsNullOrEmpty(item.Update_Date.toString()))
                                            ? item.Update_Date.Value.ToString("dd/MM/yyyy"): "";

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

        #region CreateOrUpdate
        public actionResult CreateOrUpdate(ShortShipItemViewModel data)
        {
            Guid ShortShipItemIndex = new Guid();
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            var actionResult = new actionResult();
            try
            {
                var item = new List<wm_TagOutItemShortShip>();
                var shortshipItem = db.wm_TagOutItemShortShip.Find(data.shortShipItem_Index);
                if (shortshipItem != null)
                {
                    shortshipItem.ShortQty = data.shortQty;
                    shortshipItem.Update_By = data.update_By;
                    shortshipItem.Update_Date = DateTime.Now;
                }
                else
                {
                    ShortShipItemIndex = Guid.NewGuid();
                    wm_TagOutItemShortShip resultItem = new wm_TagOutItemShortShip();
                    resultItem.ShortShipItem_Index = ShortShipItemIndex;
                    resultItem.PlanGoodsIssueItem_Index = data.planGoodsIssueItem_Index;
                    resultItem.PlangoodsIssue_index = data.plangoodsIssue_index;
                    resultItem.PlanGoodsIssue_No = data.planGoodsIssue_No;
                    resultItem.GoodsIssueItemLocation_Index = data.goodsIssueItemLocation_Index;
                    resultItem.Product_Id = data.product_Id;
                    resultItem.Product_Name = data.product_name;
                    resultItem.TagOut_Index = data.tagOut_Index;
                    resultItem.TagOut_No = data.tagOut_No;
                    resultItem.IssueQty = data.issueQty;
                    resultItem.OrderQty = data.orderQty;
                    resultItem.ShortQty = data.shortQty;
                    resultItem.ProductConversion_Name = data.productConversion_Name;
                    resultItem.OrderSeq = data.orderSeq;
                    resultItem.DropSeq = data.dropSeq;
                    resultItem.Create_By = data.update_By;
                    resultItem.Create_Date = DateTime.Now;
                    resultItem.Update_By = data.update_By;
                    resultItem.Update_Date = DateTime.Now;
                    db.wm_TagOutItemShortShip.Add(resultItem);


                }
                State = "transectionSave";

                var transectionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transectionx.Commit();
                }
                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("SaveShortShipItem", msglog);
                    transectionx.Rollback();

                    throw exy;
                }

                actionResult.Message = true;
                return actionResult;
            }
            catch (Exception ex)
            {
                msglog = State + " Error " + ex.Message.ToString();
                olog.logging("ErrorShortShipItem", msglog);
                throw ex;
            }
        }
        #endregion

        #region printShortShip
        public string printShortShip(ShortShipViewModel data, string rootPath ="")
        {
            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                var result = new List<ShortShip_ReportModel>();
                var rpt_data = db.View_ReportShortShip.Where(c => c.plangoodsissue_index == data.planGoodsIssue_Index).OrderBy(c => c.Row_Runing_product).ToList();

                foreach (var item in rpt_data)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        var list = new ShortShip_ReportModel();
                        var copy_int = "";
                        var for_report = "";
                        var for_report_int = "";
                        switch (i)
                        {
                            case 0:
                                copy_int = "ต้นฉบับ";
                                for_report_int = "1";
                                for_report = "สำหรับลูกค้า";
                                break;
                            case 1:
                                copy_int = "สำเนา";
                                for_report_int = "2";
                                for_report = "สำหรับขนส่งและคลัง";
                                break;
                        }
                        list.copy_int = copy_int;
                        list.for_report = for_report;
                        list.for_report_int = for_report_int;
                        list.truckLoad_Barcode = new NetBarcode.Barcode(item.truckload_no, NetBarcode.Type.Code128B).GetBase64Image();
                        list.appointment_Id = item.appointment_id;
                        list.appointment_Date = item.appointment_date.Value.ToString("dd/MM/yyyy");
                        list.appointment_Time = item.appointment_time;
                        list.diff = item.diff;
                        list.truckLoad_No = item.truckload_no;
                        list.order_Seq = item.Order_Seq;
                        list.planGoodsIssue_No = item.plangoodsissue_no;
                        list.planGoodsIssue_Date = item.plangoodsissue_date.Value.ToString("dd/MM/yyyy");
                        list.branch_Code = item.branch_code;
                        list.shipTo_Id = item.shipto_id;
                        list.shipTo_Name = item.shipto_name;
                        list.shipTo_Address = item.shipto_address;
                        list.lineNum = item.linenum;
                        list.Row_Runing_product = item.Row_Runing_product;
                        list.product_Id = item.product_id;
                        list.product_Name = item.product_name;
                        list.bu_Order_TotalQty = item.BU_Order_TotalQty;
                        list.bu_GI_TotalQty = item.BU_GI_TotalQty;
                        list.su_Order_TotalQty = item.SU_Order_TotalQty;
                        list.su_GI_TotalQty = item.SU_GI_TotalQty;
                        list.su_Unit = item.SU_Unit;
                        list.erp_Location = item.erp_location;
                        list.product_Lot = item.product_lot;
                        list.goodsIssue_No = item.GoodsIssue_No;
                        result.Add(list);


                    }
                }
                if (rpt_data.Count < 20)
                {
                    int countAdd = 20 - rpt_data.Count;
                    for (int j = 0; j < countAdd; j++)
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            var listblank = new ShortShip_ReportModel();
                            var copy_int = "";
                            var for_report = "";
                            var for_report_int = "";
                            switch (k)
                            {
                                case 0:
                                    copy_int = "ต้นฉบับ";
                                    for_report_int = "1";
                                    for_report = "สำหรับลูกค้า";
                                    break;
                                case 1:
                                    copy_int = "สำเนา";
                                    for_report_int = "2";
                                    for_report = "สำหรับขนส่งและคลัง";
                                    break;
                            }
                            listblank.copy_int = copy_int;
                            listblank.for_report = for_report;
                            listblank.for_report_int = for_report_int;
                            listblank.truckLoad_Barcode = ""; // new NetBarcode.Barcode(item.truckload_no, NetBarcode.Type.Code128B).GetBase64Image();
                            listblank.appointment_Id = ""; // item.appointment_id;
                            listblank.appointment_Date = ""; //item.appointment_date.Value.ToString("dd/MM/yyyy");
                            listblank.appointment_Time = ""; //item.appointment_time;
                            listblank.diff = null; //item.diff;
                            listblank.truckLoad_No = ""; //item.truckload_no;
                            listblank.order_Seq = ""; //item.Order_Seq;
                            listblank.planGoodsIssue_No = rpt_data.FirstOrDefault().plangoodsissue_no ; //item.plangoodsissue_no;
                            listblank.planGoodsIssue_Date = ""; //item.plangoodsissue_date.Value.ToString("dd/MM/yyyy");
                            listblank.branch_Code = ""; //item.branch_code;
                            listblank.shipTo_Id = ""; //item.shipto_id;
                            listblank.shipTo_Name = ""; //item.shipto_name;
                            listblank.shipTo_Address = ""; //item.shipto_address;
                            listblank.lineNum = ""; //item.linenum;
                            listblank.Row_Runing_product = null; //item.Row_Runing_product;
                            listblank.product_Id = ""; //item.product_id;
                            listblank.product_Name = ""; //item.product_name;
                            listblank.bu_Order_TotalQty = null; //item.BU_Order_TotalQty;
                            listblank.bu_GI_TotalQty = null; //item.BU_GI_TotalQty;
                            listblank.su_Order_TotalQty = null; //item.SU_Order_TotalQty;
                            listblank.su_GI_TotalQty = null; //item.SU_GI_TotalQty;
                            listblank.su_Unit = ""; // item.SU_Unit;
                            listblank.erp_Location = ""; //item.erp_location;
                            listblank.product_Lot = ""; // item.product_lot;
                            listblank.goodsIssue_No = ""; //item.GoodsIssue_No;
                            result.Add(listblank);

                        }


                    }
                }
                rootPath = rootPath.Replace("\\LoadAPI", "");
                var reportPath = rootPath + new AppSettingConfig().GetUrl("RPT_ShortShip");
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
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region dropdownProcessStatus
        public List<ProcessStatusViewModel> dropdownProcessStatus(ProcessStatusViewModel data)
        {
            try
            {
                var result = new List<ProcessStatusViewModel>();
                var filterModel = new ProcessStatusViewModel();

                filterModel.process_Index = new Guid("6323757E-667C-4D5A-8B3B-1156B07070BA");
                //Get config
                result = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("processStatus"), filterModel.sJson());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region AutoProduct
        public List<ItemListViewModel> autoProductfilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();


                var filterModel = new ItemListViewModel();

                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }
                //if (!string.IsNullOrEmpty(data.key2))
                //{
                //    filterModel.key2 = data.key2;
                //}
                //else
                //{
                //    filterModel.key2 = "00000000-0000-0000-0000-000000000000";
                //}

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoSkufilter"), filterModel.sJson());
                //result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoProductfilter"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }


}