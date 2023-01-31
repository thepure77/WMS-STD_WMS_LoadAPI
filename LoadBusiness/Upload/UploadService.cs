using DataAccess;
using GIDataAccess.Models;
using LoadBusiness.Upload;
using LoadDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using PlanGIBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using static LoadBusiness.Load.LoadViewModel;
using static LoadBusiness.Upload.TruckLoadImageViewModel;

namespace LoadBusiness.Load
{
    public class UploadService
    {

        private LoadDbContext db;

        public UploadService()
        {
            db = new LoadDbContext();
        }

        public UploadService(LoadDbContext db)
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

        public actionResult UploadImg(TruckLoadImageViewModel data)
        {
            var actionResult = new actionResult();
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                byte[] img = Convert.FromBase64String(data.base64);
                var path = Directory.GetCurrentDirectory();
                path += "\\" + "ImageFolder" + "\\";
                if (!System.IO.Directory.Exists(path)) 
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                System.IO.File.WriteAllBytes(path + data.name , img);

                im_TruckLoadImages result = new im_TruckLoadImages();

                result.TruckLoadImage_Index = Guid.NewGuid();
                result.TruckLoad_Index = data.truckLoad_Index;
                result.ImageUrl = "http://kascoit.ddns.me:99/ImageFolder/" + data.name.ToString();
                result.ImageType = data.type;
                result.Create_By = data.create_By;
                result.Create_Date = DateTime.Now;
                db.im_TruckLoadImages.Add(result);


                var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transactionx.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("SaveTruckLoadImages", msglog);
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

        public List<TruckLoadImageViewModel> findImg(TruckLoadImageViewModel data)
        {
            try
            {
                var result = new List<TruckLoadImageViewModel>();

                var query = db.im_TruckLoadImages.Where(c => c.TruckLoad_Index == data.truckLoad_Index).ToList();

                foreach (var item in query)
                {
                    var resultItem = new TruckLoadImageViewModel();

                    resultItem.src = item.ImageUrl;
                    resultItem.type = "image";
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