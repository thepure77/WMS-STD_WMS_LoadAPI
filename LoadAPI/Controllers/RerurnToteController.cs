using LoadBusiness.Load;
using MasterDataBusiness.VehicleCompany;
using MasterDataBusiness.VehicleCompanyType;
using MasterDataBusiness.VehicleType;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlanGIBusiness.Reports.PrintOutShipment;
using System;
using static LoadBusiness.Load.LoadViewModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LoadAPI.Controllers
{
    [Route("api/RerurnTote")]
    public class RerurnToteController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public RerurnToteController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        
        #region scanShipment
        [HttpPost("scanShipment")]
        public IActionResult scanShipment([FromBody]JObject body)
        {
            try
            {
                var service = new ReturntoteService();
                var Models = new returntoteViewModel();
                Models = JsonConvert.DeserializeObject<returntoteViewModel>(body.ToString());
                var result = service.filter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region SavereturnTote
        [HttpPost("SavereturnTote")]
        public IActionResult SavereturnTote([FromBody]JObject body)
        {
            try
            {
                var service = new ReturntoteService();
                var Models = new returntoteViewModel();
                Models = JsonConvert.DeserializeObject<returntoteViewModel>(body.ToString());
                var result = service.SavereturnTote(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region SearchReturntote
        [HttpPost("SearchReturntote")]
        public IActionResult SearchReturntote([FromBody]JObject body)
        {
            try
            {
                var service = new ReturntoteService();
                var Models = new returntoteViewModel();
                Models = JsonConvert.DeserializeObject<returntoteViewModel>(body.ToString());
                var result = service.SearchReturntote(Models);
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
