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
using PlanGIBusiness.Libs;
using PlanGIBusiness.Reports.PrintOutShipment;
using PlanGIDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using static LoadBusiness.Load.LoadViewModel;
using static LoadBusiness.Load.ScanLoadItemViewModel;
using static LoadBusiness.Load.SearchDetailModel;

namespace LoadBusiness.Load
{
    public class ReturntoteService
    {

        private LoadDbContext db;

        public ReturntoteService()
        {
            db = new LoadDbContext();
        }

        public ReturntoteService(LoadDbContext db)
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
        
        #region filter
        public returntoteViewModel filter(returntoteViewModel model)
        {
            try
            {
                returntoteViewModel result = new returntoteViewModel();
                List<im_TruckLoad_ReturnTote> findshipment = db.im_TruckLoad_ReturnTote.Where(c => c.TruckLoad_No == model.shipment_no).ToList();
                if (findshipment.Count > 0)
                {
                    
                    result.truckLoad_Date = findshipment[0].TruckLoad_Date.Value.ToString("yyyyMMdd");
                    result.vehicle_Registration = findshipment[0].Vehicle_Registration;
                    result.docReturn = findshipment[0].DocReturn.ToString();
                    result.return_docReturn = findshipment[0].Retrun_DocReturn.ToString();
                    result.truckLoad_Index = findshipment[0].TruckLoad_Index;

                    List<Guid> getGUID = findshipment.Select(c => c.TruckReturn_Index).ToList();
                    List<im_TruckLoad_ReturnTote_Tran> truckLoad_ReturnTote_Trans = db.im_TruckLoad_ReturnTote_Tran.Where(c => getGUID.Contains(c.TruckReturn_Index)).ToList();

                    if (truckLoad_ReturnTote_Trans.Count() > 0)
                    {
                        List<Guid> _getGUID = truckLoad_ReturnTote_Trans.Select(c => c.TruckReturn_Index).ToList();
                        findshipment = findshipment.Where(c => _getGUID.Contains(c.TruckReturn_Index)).ToList();
                    }

                    foreach (im_TruckLoad_ReturnTote item in findshipment)
                    {
                        returntoteitemListitem resultitem = new returntoteitemListitem();
                        resultitem.truckReturn_Index = item.TruckReturn_Index;
                        resultitem.truckLoad_Index = item.TruckLoad_Index;
                        resultitem.truckLoad_no = item.TruckLoad_No;
                        resultitem.return_Tote_Size = item.Return_Tote_Size;
                        resultitem.return_Tote_MAX = item.Return_Tote_MAX;
                        resultitem.return_Tote_Qty = item.Return_Tote_Qty;
                        resultitem.return_qty_damage = "0";
                        result.returntoteitemLists.Add(resultitem);
                    }
                    List<im_TruckLoad_ReturnTote_Tran> find_transhipment = db.im_TruckLoad_ReturnTote_Tran.Where(c => c.TruckLoad_No == model.shipment_no).ToList();
                    if (find_transhipment.Count > 0 )
                    {
                        View_TruckLoadProcessStatus truckLoadProcessStatus = db.View_TruckLoadProcessStatus.FirstOrDefault(c => c.TruckLoad_Index == find_transhipment[0].TruckLoad_Index);
                        //find_transhipment = find_transhipment.Where(c => c.TruckLoad_Return_Date == model.truckLoad_Return_Date.toDate()).ToList();
                        //if (find_transhipment.Count > 0)
                        //{
                            foreach (im_TruckLoad_ReturnTote_Tran item in find_transhipment.OrderBy(c=> c.Return_Tote_Size).ThenBy(c=> c.TruckLoad_Return_Status))
                            {
                                returntoteitemListitem tran_resultitem = new returntoteitemListitem();
                                tran_resultitem.return_Tote_Size = item.Return_Tote_Size;
                                tran_resultitem.return_Tote_Qty = item.Return_Tote_Qty;
                                tran_resultitem.truckLoad_Return_Status = item.TruckLoad_Return_Status == 1 ? true : false;
                                tran_resultitem.truckLoad_Return_Date = item.TruckLoad_Return_Date.Value.ToString("dd-MM-yyyy");
                                tran_resultitem.user = item.Create_By;
                                tran_resultitem.remark = item.Remark;
                                tran_resultitem.load_date = truckLoadProcessStatus.Appointment_Date == null ? null : truckLoadProcessStatus.Appointment_Date.GetValueOrDefault().ToString("dd/MM/yyyy");
                                result.tran_returntoteitemLists.Add(tran_resultitem);
                            }
                            result.resultIsUse = true;
                        //}
                        //else {
                        //    result.resultIsUse = false;
                        //    result.resultMsg = "เกินเวลารับคืน ที่ทำการรับจากครั้งล่าสุด";
                        //    return result;
                        //}                        
                    }
                    result.resultIsUse = true;
                }
                else {
                    List<View_Load_returntote> shipment = db.View_Load_returntote.Where(c => c.TruckLoad_No == model.shipment_no && c.Document_Status == 2).ToList();
                    if (shipment.Count > 0)
                    {
                        result.truckLoad_Date = shipment[0].TruckLoad_Date.Value.ToString("yyyyMMdd");
                        result.vehicle_Registration = shipment[0].Vehicle_Registration;
                        result.docReturn = shipment[0].DocReturn.ToString();
                        result.return_docReturn = "0";
                        result.truckLoad_Index = shipment[0].TruckLoad_Index;
                        foreach (var item in shipment)
                        {
                            im_TruckLoad_ReturnTote insert_return = new im_TruckLoad_ReturnTote();
                            insert_return.TruckReturn_Index = Guid.NewGuid();
                            insert_return.TruckLoad_Index   = item.TruckLoad_Index.GetValueOrDefault();
                            insert_return.TruckLoad_No      = item.TruckLoad_No;
                            insert_return.Vehicle_Registration   = item.Vehicle_Registration;
                            insert_return.TruckLoad_Date    = item.TruckLoad_Date;
                            insert_return.Return_Tote_Size  = item.Size_tote;
                            insert_return.Return_Tote_MAX   = item.total_tote.ToString();
                            insert_return.Return_Tote_Qty   = "0";
                            insert_return.DocReturn   = item.DocReturn.ToString();
                            insert_return.Retrun_DocReturn   = "0";
                            insert_return.Create_By         = model.user;
                            insert_return.Create_Date       = DateTime.Now;

                            db.im_TruckLoad_ReturnTote.Add(insert_return);

                            returntoteitemListitem resultitem = new returntoteitemListitem();
                            resultitem.truckLoad_Index = insert_return.TruckLoad_Index;
                            resultitem.truckReturn_Index = insert_return.TruckReturn_Index;
                            resultitem.truckLoad_no = item.Vehicle_Registration;
                            resultitem.truckLoad_Index = item.TruckLoad_Index.GetValueOrDefault();
                            resultitem.return_Tote_Size = item.Size_tote;
                            resultitem.return_Tote_MAX = item.total_tote.ToString();
                            resultitem.return_Tote_Qty = "0";
                            resultitem.return_qty_damage = "0";
                            result.returntoteitemLists.Add(resultitem);
                            result.resultIsUse = true;

                            
                        }
                        result.resultIsUse = true;
                        var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                        try
                        {
                            db.SaveChanges();
                            transactionx.Commit();
                        }

                        catch (Exception exy)
                        {
                            result.resultIsUse = false;
                            result.resultMsg = exy.Message;
                            return result;
                        }
                    }
                    else {
                        im_TruckLoad getshipment = db.im_TruckLoad.FirstOrDefault(c => c.TruckLoad_No == model.shipment_no && c.Document_Status == 2);
                        if (getshipment != null)
                        {
                            List<im_TruckLoadItem> getitem_Shipment = db.im_TruckLoadItem.Where(c => c.TruckLoad_Index == getshipment.TruckLoad_Index && c.Document_Status != -1).ToList();
                            int GIL = db.IM_GoodsIssueItemLocation.Where(c => getitem_Shipment.Select(x => x.PlanGoodsIssue_Index).Contains(c.Ref_Document_Index) && c.Document_Status != -1).GroupBy(c => c.Ref_Document_Index).Count();
                            if (GIL > 0)
                            {
                                result.truckLoad_Date = getshipment.TruckLoad_Date.Value.ToString("yyyyMMdd");
                                result.vehicle_Registration = getshipment.Vehicle_Registration;
                                result.docReturn = GIL.ToString();
                                result.return_docReturn = "0";
                                result.truckLoad_Index = getshipment.TruckLoad_Index;
                                result.resultIsUse = true;

                                im_TruckLoad_ReturnTote insert_return = new im_TruckLoad_ReturnTote();
                                insert_return.TruckReturn_Index = Guid.NewGuid();
                                insert_return.TruckLoad_Index = getshipment.TruckLoad_Index;
                                insert_return.TruckLoad_No = getshipment.TruckLoad_No;
                                insert_return.Vehicle_Registration = getshipment.Vehicle_Registration;
                                insert_return.TruckLoad_Date = getshipment.TruckLoad_Date;
                                insert_return.Return_Tote_Size = "Doc";
                                insert_return.Return_Tote_MAX = "0";
                                insert_return.Return_Tote_Qty = "0";
                                insert_return.DocReturn = GIL.ToString();
                                insert_return.Retrun_DocReturn = "0";
                                insert_return.Create_By = model.user;
                                insert_return.Create_Date = DateTime.Now;

                                db.im_TruckLoad_ReturnTote.Add(insert_return);

                                returntoteitemListitem resultitem = new returntoteitemListitem();
                                resultitem.truckLoad_Index = insert_return.TruckLoad_Index;
                                resultitem.truckReturn_Index = insert_return.TruckReturn_Index;
                                resultitem.truckLoad_no = getshipment.Vehicle_Registration;
                                resultitem.truckLoad_Index = getshipment.TruckLoad_Index;
                                resultitem.return_Tote_Size = insert_return.Return_Tote_Size;
                                resultitem.return_Tote_MAX = insert_return.Return_Tote_MAX;
                                resultitem.return_Tote_Qty = "0";
                                resultitem.return_qty_damage = "0";
                                result.returntoteitemLists.Add(resultitem);

                                var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                                try
                                {
                                    db.SaveChanges();
                                    transactionx.Commit();
                                }

                                catch (Exception exy)
                                {
                                    result.resultIsUse = false;
                                    result.resultMsg = exy.Message;
                                    return result;
                                }
                            }
                            else {
                                result.resultIsUse = false;
                                result.resultMsg = "ไม่พบ Shipment ที่ทำการแสกน";
                            }
                        }
                        else {
                            result.resultIsUse = false;
                            result.resultMsg = "ไม่พบ Shipment ที่ทำการแสกน";
                        }
                        
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

        #region SavereturnTote
        public returntoteViewModel SavereturnTote(returntoteViewModel model)
        {
            try
            {
                returntoteViewModel result = new returntoteViewModel();
                if (model.returntoteitemLists.Count > 0)
                {
                    foreach (var item in model.returntoteitemLists)
                    {
                        var updatereturn = db.im_TruckLoad_ReturnTote.FirstOrDefault(c => c.TruckReturn_Index == item.truckReturn_Index);
                        if (updatereturn == null)
                        {
                            im_TruckLoad_ReturnTote insert_return = new im_TruckLoad_ReturnTote();
                            insert_return.TruckReturn_Index = Guid.NewGuid();
                            insert_return.TruckLoad_Index = model.truckLoad_Index.GetValueOrDefault();
                            insert_return.TruckLoad_No = model.truckLoad_no;
                            insert_return.TruckLoad_Date = model.truckLoad_Date.toDate();
                            insert_return.Return_Tote_Size = model.return_Tote_Size;
                            insert_return.Return_Tote_MAX = model.return_Tote_Qty;
                            insert_return.Return_Tote_Qty = model.return_qty;
                            insert_return.Create_By = model.user;
                            insert_return.Create_Date = DateTime.Now;

                            db.im_TruckLoad_ReturnTote.Add(insert_return);
                        }
                        else {
                            if (!string.IsNullOrEmpty(item.return_qty))
                            {
                                updatereturn.Return_Tote_Qty = (int.Parse(updatereturn.Return_Tote_Qty) + int.Parse(item.return_qty) + int.Parse(item.return_qty_damage)).ToString();
                                updatereturn.Retrun_DocReturn = model.return_docReturn_XX.ToString();
                                updatereturn.Update_By = model.user;
                                updatereturn.Update_Date = DateTime.Now;

                                if (int.Parse(item.return_qty) > 0)
                                {
                                    im_TruckLoad_ReturnTote_Tran insert_return_tran = new im_TruckLoad_ReturnTote_Tran();
                                    insert_return_tran.TruckReturn_Tran_Index = Guid.NewGuid();
                                    insert_return_tran.TruckReturn_Index = updatereturn.TruckReturn_Index;
                                    insert_return_tran.TruckLoad_Index = updatereturn.TruckLoad_Index;
                                    insert_return_tran.TruckLoad_No = updatereturn.TruckLoad_No;
                                    insert_return_tran.TruckLoad_Date = updatereturn.TruckLoad_Date;
                                    insert_return_tran.Return_Tote_Size = updatereturn.Return_Tote_Size;
                                    insert_return_tran.Return_Tote_Qty = item.return_qty;
                                    insert_return_tran.Create_By = model.user;
                                    insert_return_tran.Create_Date = DateTime.Now;
                                    insert_return_tran.TruckLoad_Return_Date = model.truckLoad_Return_Date.toDate();
                                    insert_return_tran.TruckLoad_Return_Status = 0;
                                    insert_return_tran.Remark = item.remark;

                                    db.im_TruckLoad_ReturnTote_Tran.Add(insert_return_tran);
                                }
                                if (int.Parse(item.return_qty_damage) > 0)
                                {
                                    im_TruckLoad_ReturnTote_Tran insert_return_tran = new im_TruckLoad_ReturnTote_Tran();
                                    insert_return_tran.TruckReturn_Tran_Index = Guid.NewGuid();
                                    insert_return_tran.TruckReturn_Index = updatereturn.TruckReturn_Index;
                                    insert_return_tran.TruckLoad_Index = updatereturn.TruckLoad_Index;
                                    insert_return_tran.TruckLoad_No = updatereturn.TruckLoad_No;
                                    insert_return_tran.TruckLoad_Date = updatereturn.TruckLoad_Date;
                                    insert_return_tran.Return_Tote_Size = updatereturn.Return_Tote_Size;
                                    insert_return_tran.Return_Tote_Qty = item.return_qty_damage;
                                    insert_return_tran.Create_By = model.user;
                                    insert_return_tran.Create_Date = DateTime.Now;
                                    insert_return_tran.TruckLoad_Return_Date = model.truckLoad_Return_Date.toDate();
                                    insert_return_tran.TruckLoad_Return_Status = 1;
                                    insert_return_tran.Remark = item.remark;

                                    db.im_TruckLoad_ReturnTote_Tran.Add(insert_return_tran);
                                }
                                if (item.re_doc > 0)
                                {
                                    im_TruckLoad_ReturnTote_Tran insert_return_tran = new im_TruckLoad_ReturnTote_Tran();
                                    insert_return_tran.TruckReturn_Tran_Index = Guid.NewGuid();
                                    insert_return_tran.TruckReturn_Index = updatereturn.TruckReturn_Index;
                                    insert_return_tran.TruckLoad_Index = updatereturn.TruckLoad_Index;
                                    insert_return_tran.TruckLoad_No = updatereturn.TruckLoad_No;
                                    insert_return_tran.TruckLoad_Date = updatereturn.TruckLoad_Date;
                                    insert_return_tran.Return_Tote_Size = "Document";
                                    insert_return_tran.Return_Tote_Qty = item.re_doc.ToString();
                                    insert_return_tran.Create_By = model.user;
                                    insert_return_tran.Create_Date = DateTime.Now;
                                    insert_return_tran.TruckLoad_Return_Date = model.truckLoad_Return_Date.toDate();
                                    insert_return_tran.TruckLoad_Return_Status = 0;
                                    insert_return_tran.Remark = item.remark;

                                    db.im_TruckLoad_ReturnTote_Tran.Add(insert_return_tran);
                                }
                            }
                            
                        }
                    }

                    if (model.return_docReturn_XX > 0)
                    {
                        var update_returnDoc = db.im_TruckLoad_ReturnTote.Where(c => c.TruckLoad_Index == model.truckLoad_Index).ToList();
                        if (update_returnDoc.Count > 0)
                        {
                            foreach (var item in update_returnDoc)
                            {
                                item.Retrun_DocReturn = model.return_docReturn_XX.ToString();
                                item.Update_By = model.user;
                                item.Update_Date = DateTime.Now;
                            }
                        }
                    }
                    var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        db.SaveChanges();
                        transactionx.Commit();
                        result.resultIsUse = true;
                    }

                    catch (Exception exy)
                    {
                        result.resultIsUse = false;
                        result.resultMsg = exy.Message;
                        return result;
                    }
                }
                else {
                    result.resultIsUse = false;
                    result.resultMsg = "ไม่สามารถบันทึกการรับคืนได้";
                }
                
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region SearchReturntote
        public List<SearchreturntoteViewModel> SearchReturntote(returntoteViewModel model)
        {
            List<SearchreturntoteViewModel> result = new List<SearchreturntoteViewModel>();
            try
            {
                List<im_TruckLoad> truckLoads = db.im_TruckLoad.Where(c => c.Document_Status != -1).ToList();

                #region Where

                #region truckLoad_Date
                if (model.isDateLoad == "0")
                {
                    if (!string.IsNullOrEmpty(model.truckLoad_Date) && !string.IsNullOrEmpty(model.truckLoad_Date_To))
                    {
                        var dateStart = model.truckLoad_Date.toBetweenDate();
                        var dateEnd = model.truckLoad_Date_To.toBetweenDate();
                        truckLoads = truckLoads.Where(c => c.TruckLoad_Date >= dateStart.start && c.TruckLoad_Date <= dateEnd.end).ToList();
                    }
                }
                #endregion

                #region truckLoad_no
                if (!string.IsNullOrEmpty(model.truckLoad_no))
                {
                    truckLoads = truckLoads.Where(c => c.TruckLoad_No == model.truckLoad_no).ToList();
                }
                #endregion

                

                #region status
                List<View_RPT_ToteReturnTMS> report_truckLoads = db.View_RPT_ToteReturnTMS.Where(c => truckLoads.Select(z => z.TruckLoad_Index).Contains(c.TruckLoad_Index)).ToList();
                var statusModels = new List<int?>();
                if (model.status.Count > 0)
                {
                    if (model.status.Count == 1)
                    {
                        if (model.status[0].value == 1)
                        {
                            report_truckLoads = report_truckLoads.Where(c => c.TruckLoad_Return_Date == null).ToList();
                        }
                        else if (model.status[0].value == 2)
                        {
                            report_truckLoads = report_truckLoads.Where(c => c.TruckLoad_Return_Date != null).ToList();
                        }
                    }
                }
                #endregion

                #region Appointment Date
                if (model.isDateRerurn == "0")
                {
                    if (!string.IsNullOrEmpty(model.truckLoad_Date_Return) && !string.IsNullOrEmpty(model.truckLoad_Date_To_Return))
                    {
                        var dateStart = model.truckLoad_Date_Return.toBetweenDate();
                        var dateEnd = model.truckLoad_Date_To_Return.toBetweenDate();
                        report_truckLoads = report_truckLoads.Where(c => c.TruckLoad_Return_Date >= dateStart.start && c.TruckLoad_Return_Date <= dateEnd.end).ToList();
                    }
                }
                #endregion

                #endregion

                #region check Diff

                #region Diff Doc
                if (model.diff_Doc != null && model.diff_Doc == true)
                {
                    report_truckLoads = report_truckLoads.Where(c => c.DocReturn_Max != c.Return_Doc).ToList();
                }
                #endregion

                #region Diff L
                if (model.diff_L != null && model.diff_L == true)
                {
                    report_truckLoads = report_truckLoads.Where(c => c.Return_Tote_MAX_XL != (c.Return_Tote_Qty_XL + c.Return_Tote_Qty_DMG_XL)).ToList();
                }
                #endregion

                #region Diff M
                if (model.diff_M != null && model.diff_M == true)
                {
                    report_truckLoads = report_truckLoads.Where(c => c.DocReturn_Max != (c.Return_Tote_Qty_M + c.Return_Tote_Qty_DMG_M)).ToList();
                }
                #endregion

                #endregion

                foreach (var item in report_truckLoads)
                {
                    var searchreturntoteView = new SearchreturntoteViewModel();
                    searchreturntoteView.TruckLoad_Return_Date = item.TruckLoad_Return_Date == null ? "-" : item.TruckLoad_Return_Date.GetValueOrDefault().ToString("dd/MM/yyyy");
                    searchreturntoteView.TruckLoad_No = item.TruckLoad_No;
                    searchreturntoteView.Return_Tote_MAX_XL = item.Return_Tote_MAX_XL == null ? "0" : item.Return_Tote_MAX_XL.ToString();
                    searchreturntoteView.Return_Tote_MAX_M = item.Return_Tote_MAX_M == null ? "0" : item.Return_Tote_MAX_M.ToString();
                    searchreturntoteView.DocReturn_Max = item.DocReturn_Max == null ? "0" :item.DocReturn_Max.ToString();
                    searchreturntoteView.Return_Tote_Qty_XL = item.Return_Tote_Qty_XL;
                    searchreturntoteView.Return_Tote_Qty_M = item.Return_Tote_Qty_M;
                    searchreturntoteView.Return_Tote_Qty_DMG_XL = item.Return_Tote_Qty_DMG_XL;
                    searchreturntoteView.Return_Tote_Qty_DMG_M = item.Return_Tote_Qty_DMG_M;
                    searchreturntoteView.Return_Doc = item.Return_Doc;
                    searchreturntoteView.Appointment_date = item.Appointment_date.GetValueOrDefault().ToString("dd/MM/yyyy");
                    searchreturntoteView.VehicleCompany_Name = item.VehicleCompany_Name;
                    result.Add(searchreturntoteView);

                }

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