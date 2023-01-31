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
    public class EmergencyTruckmanifestService
    {

        private LoadDbContext db;
        private MasterDbContext Masterdb;

        public EmergencyTruckmanifestService()
        {
            db = new LoadDbContext();
            Masterdb = new MasterDbContext();
        }

        public EmergencyTruckmanifestService(LoadDbContext db, MasterDbContext Masterdb)
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
                //ProcessStatus = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("processStatus"), filterModel.sJson());


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

                    //Statue = item.Document_Status.ToString();
                    //var ProcessStatusName = ProcessStatus.Where(c => c.processStatus_Id == Statue).FirstOrDefault();
                    //resultItem.processStatus_Name = ProcessStatusName.processStatus_Name;

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

        #region EmergencyTruckMenifest
        public string EmergencyTruckMenifest(PrintOutShipmentModel data, string rootPath = "")
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
                var rpt_data = db.View_RPT_Truck_Menifest.FromSql("sp_RPT_Truck_Menifest_E @TruckLoad_Index", TM_NO).OrderBy(c => c.drop_seq).ToList();

                var result = new List<TruckMenifestModel>();
                //var rpt_data = db.View_RPT_Truck_Menifest.Where(C => C.TruckLoad_Index == data.truckLoad_Index).OrderBy(c => c.drop_seq).ThenBy(c=> c.plan_runing).ToList();
                foreach (var item in rpt_data)
                {
                    cc++;
                    if (cc == 40)
                    {

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
        
    }
}