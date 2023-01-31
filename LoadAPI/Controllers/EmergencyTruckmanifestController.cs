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
    [Route("api/EmergencyTruckmanifest")]
    public class EmergencyTruckmanifestController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public EmergencyTruckmanifestController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #region filter
        [HttpPost("filter")]
        public IActionResult filter([FromBody]JObject body)
        {
            try
            {
                var service = new EmergencyTruckmanifestService();
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

        #region EmergencyTruckMenifest
        [HttpPost("EmergencyTruckMenifest")]
        public IActionResult printOutTruckMenifest([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new EmergencyTruckmanifestService();
                var Models = new PrintOutShipmentModel();
                Models = JsonConvert.DeserializeObject<PrintOutShipmentModel>(body.ToString());
                //localFilePath = service.ReportPrintOutShipment(Models, _hostingEnvironment.ContentRootPath);
                localFilePath = service.EmergencyTruckMenifest(Models, _hostingEnvironment.ContentRootPath);
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
        
    }
}
