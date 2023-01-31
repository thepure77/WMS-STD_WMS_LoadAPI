using LoadBusiness.Load;
using MasterDataBusiness.VehicleCompany;
using MasterDataBusiness.VehicleCompanyType;
using MasterDataBusiness.VehicleType;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlanGIBusiness.Reports.PrintOutShipment;
using PlanGIBusiness.Reports.Trace_loading;
using PlanGIBusiness.Reports.Trace_picking;
using System;
using static LoadBusiness.Load.LoadViewModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LoadAPI.Controllers
{
    [Route("api/TruckLoad")]
    public class LoadController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public LoadController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #region checkPlanGI
        [HttpPost("checkPlanGI")]
        public IActionResult checkGI([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.checkPlanGI(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region find
        [HttpGet("find/{id}")]
        public IActionResult find(Guid id)
        {
            try
            {
                var service = new LoadService();
                var result = service.find(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region filter
        [HttpPost("filter")]
        public IActionResult filter([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = new SearchDetailModel();
                Models = JsonConvert.DeserializeObject<SearchDetailModel>(body.ToString());
                var result = service.filter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region CreateOrUpdate
        [HttpPost("createOrUpdate")]
        public IActionResult CreateOrUpdate([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = new LoadViewModel();
                Models = JsonConvert.DeserializeObject<LoadViewModel>(body.ToString());
                var result = service.CreateOrUpdate(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region delete
        [HttpPost("delete")]
        public IActionResult delete([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = new LoadViewModel();
                Models = JsonConvert.DeserializeObject<LoadViewModel>(body.ToString());
                var result = service.delete(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region comfirmStatus
        [HttpPost("comfirmStatus")]
        public IActionResult comfirmStatus([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = new LoadViewModel();
                Models = JsonConvert.DeserializeObject<LoadViewModel>(body.ToString());
                var result = service.comfirmStatus(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region listcomfirmStatus
        [HttpPost("listcomfirmStatus")]
        public IActionResult listcomfirmStatus([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = new listTruckload();
                Models = JsonConvert.DeserializeObject<listTruckload>(body.ToString());
                var result = service.listcomfirmStatus(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion


        #region scanLoadNo
        [HttpPost("scanLoadNo")]
        public IActionResult scanLoadNo([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = new LoadViewModel();
                Models = JsonConvert.DeserializeObject<LoadViewModel>(body.ToString());
                var result = service.scanLoadNo(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region scanSoNo
        [HttpPost("scanSoNo")]
        public IActionResult scanSoNo([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = new ScanLoadItemViewModel();
                Models = JsonConvert.DeserializeObject<ScanLoadItemViewModel>(body.ToString());
                var result = service.scanSoNo(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region scanProductId
        [HttpPost("scanProductId")]
        public IActionResult scanProductId([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = new ScanLoadItemViewModel();
                Models = JsonConvert.DeserializeObject<ScanLoadItemViewModel>(body.ToString());
                var result = service.scanProductId(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion



        #region scanSN
        [HttpPost("scanSN")]
        public IActionResult scanSN([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = new ScanLoadItemViewModel();
                Models = JsonConvert.DeserializeObject<ScanLoadItemViewModel>(body.ToString());
                var result = service.scanSN(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region filterPlanSN
        [HttpPost("filterPlanSN")]
        public IActionResult filterPlanSN([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = new ScanLoadItemViewModel();
                Models = JsonConvert.DeserializeObject<ScanLoadItemViewModel>(body.ToString());
                var result = service.filterplanSN(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region confirmScan
        [HttpPost("confirmScan")]
        public IActionResult confirmScan([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = new ScanLoadItemViewModel();
                Models = JsonConvert.DeserializeObject<ScanLoadItemViewModel>(body.ToString());
                var result = service.confirmScan(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion


        #region deleteItem
        [HttpPost("deleteItem")]
        public IActionResult deleteItem([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = new ScanLoadItemViewModel();
                Models = JsonConvert.DeserializeObject<ScanLoadItemViewModel>(body.ToString());
                var result = service.deleteItem(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion


        #region dropdownVehicleCompanyType
        [HttpPost("dropdownVehicleCompanyType")]
        public IActionResult dropdownVehicleCompanyType([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = new VehicleCompanyTypeViewModel();
                Models = JsonConvert.DeserializeObject<VehicleCompanyTypeViewModel>(body.ToString());
                var result = service.dropdownVehicleCompanyType(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownVehicleCompany
        [HttpPost("dropdownVehicleCompany")]
        public IActionResult dropdownVehicleCompany([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = new VehicleCompanyViewModel();
                Models = JsonConvert.DeserializeObject<VehicleCompanyViewModel>(body.ToString());
                var result = service.dropdownVehicleCompany(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownVehicleType
        [HttpPost("dropdownVehicleType")]
        public IActionResult dropdownVehicleType([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = new VehicleTypeViewModel();
                Models = JsonConvert.DeserializeObject<VehicleTypeViewModel>(body.ToString());
                var result = service.dropdownVehicleType(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region ConfirmCutStock
        [HttpPost("ConfirmCutStock")]
        public IActionResult ConfirmCutStock([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = JsonConvert.DeserializeObject<SearchDetailModel>(body.ToString());
                var result = service.ConfirmCutStock(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region closeDocument
        [HttpPost("closeDocument")]
        public IActionResult CloseDocument([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = JsonConvert.DeserializeObject<SearchDetailModel>(body.ToString());
                var result = service.CloseDocument(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region ReportPrintOutShipment
        [HttpPost("ReportPrintOutShipment")]
        public IActionResult ReportPrintOutShipment([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new LoadService();
                var Models = new PrintOutShipmentModel();
                Models = JsonConvert.DeserializeObject<PrintOutShipmentModel>(body.ToString());
                //localFilePath = service.ReportPrintOutShipment(Models, _hostingEnvironment.ContentRootPath);
                localFilePath = service.ReportPrintOutHandover(Models, _hostingEnvironment.ContentRootPath);
                if (!System.IO.File.Exists(localFilePath))
                {
                    return NotFound();
                }
                return File(System.IO.File.ReadAllBytes(localFilePath), "application/octet-stream");
                //return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                System.IO.File.Delete(localFilePath);
            }
        }
        #endregion

        #region printOutTruckMenifest
        [HttpPost("printOutTruckMenifest")]
        public IActionResult printOutTruckMenifest([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new LoadService();
                var Models = new PrintOutShipmentModel();
                Models = JsonConvert.DeserializeObject<PrintOutShipmentModel>(body.ToString());
                //localFilePath = service.ReportPrintOutShipment(Models, _hostingEnvironment.ContentRootPath);
                localFilePath = service.printOutTruckMenifest(Models, _hostingEnvironment.ContentRootPath);
                if (!System.IO.File.Exists(localFilePath))
                {
                    return NotFound();
                }
                return File(System.IO.File.ReadAllBytes(localFilePath), "application/octet-stream");
                //return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                System.IO.File.Delete(localFilePath);
            }
        }
        #endregion

        #region printOutTracePicking
        [HttpPost("printOutTracePicking")]
        public IActionResult printOutTracePicking([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = new Trace_picking();
                Models = JsonConvert.DeserializeObject<Trace_picking>(body.ToString());
                var result = service.printOutTracePicking(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region printOutTraceLoading
        [HttpPost("printOutTraceLoading")]
        public IActionResult printOutTraceLoading([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = new Trace_loading();
                Models = JsonConvert.DeserializeObject<Trace_loading>(body.ToString());
                var result = service.printOutTraceLoading(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region printOutDeliveryNote
        [HttpPost("printOutDeliveryNote")]
        public IActionResult printOutDeliveryNote([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new LoadService();
                var Models = new PrintOutShipmentModel();
                Models = JsonConvert.DeserializeObject<PrintOutShipmentModel>(body.ToString());
                //localFilePath = service.ReportPrintOutShipment(Models, _hostingEnvironment.ContentRootPath);
                localFilePath = service.printOutDeliveryNote(Models, _hostingEnvironment.ContentRootPath);
                if (!System.IO.File.Exists(localFilePath))
                {
                    return NotFound();
                }
                return File(System.IO.File.ReadAllBytes(localFilePath), "application/octet-stream");
                //return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                System.IO.File.Delete(localFilePath);
            }
        }
        #endregion

        #region Taskreturntote
        [HttpPost("Taskreturntote")]
        public IActionResult Taskreturntote()
        {
            try
            {
                var service = new LoadService();
                var result = service.Taskreturntote();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region CheckCancle_shipment
        [HttpPost("CheckCancle_shipment")]
        public IActionResult CheckCancle_shipment([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = new SearchDetailModel();
                //Models = JsonConvert.DeserializeObject<SearchDetailModel>(body.ToString());
                var result = service.CheckCancle_shipment_bymapRound(body.ToString());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion
    }
}
