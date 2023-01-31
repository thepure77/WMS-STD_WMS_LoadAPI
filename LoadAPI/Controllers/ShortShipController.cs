using LoadBusiness.Load;
using LoadBusiness.ShortShip;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace LoadAPI.Controllers
{
    [Route("api/ShortShip")]
    public class ShortShipController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ShortShipController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #region filter
        [HttpPost("filter")]
        public IActionResult filter([FromBody]JObject body)
        {
            try
            {
                ShortShipService service = new ShortShipService();
                ShortShipModel Models = JsonConvert.DeserializeObject<ShortShipModel>(body.ToString());
                ShortShipModel result = service.filter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region filterV2
        [HttpPost("filterV2")]
        public IActionResult filterV2([FromBody]JObject body)
        {
            try
            {
                var service = new ShortShipService();
                var Models = new ShortShipViewModel();
                Models = JsonConvert.DeserializeObject<ShortShipViewModel>(body.ToString());
                var result = service.filterV2(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region approveStatus
        [HttpPost("approveStatus")]
        public IActionResult approveStatus([FromBody]JObject body)
        {
            try
            {
                var service = new ShortShipService();
                var Models = new ShortShipViewModel();
                Models = JsonConvert.DeserializeObject<ShortShipViewModel>(body.ToString());
                var result = service.approveStatus(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region find
        [HttpPost("find")]
        public IActionResult find([FromBody]JObject body)
        {
            try
            {
                var service = new ShortShipService();
                var Models = new ShortShipItemViewModel();
                Models = JsonConvert.DeserializeObject<ShortShipItemViewModel>(body.ToString());
                var result = service.find(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion
        #region createOrUpdate
        [HttpPost("createOrUpdate")]
        public IActionResult CreateOrUpdate([FromBody]JObject body)
        {
            try
            {
                var service = new ShortShipService();
                var Models = new ShortShipItemViewModel();
                Models = JsonConvert.DeserializeObject<ShortShipItemViewModel>(body.ToString());
                var result = service.CreateOrUpdate(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion
        #region printShortShip
        [HttpPost("printShortShip")]
        public IActionResult printShortShip([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new ShortShipService();
                ShortShipViewModel model = JsonConvert.DeserializeObject<ShortShipViewModel>(body.ToString());
                localFilePath = service.printShortShip(model, _hostingEnvironment.ContentRootPath);
                if (!System.IO.File.Exists(localFilePath))
                {
                    return NotFound();
                }
                return File(System.IO.File.ReadAllBytes(localFilePath), "application/octet-stream");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
                
        }
        #endregion

        #region dropdownProcessStatus
        [HttpPost("dropdownProcessStatus")]
        public IActionResult dropdownProcessStatus([FromBody]JObject body)
        {
            try
            {
                var service = new ShortShipService();
                var Models = new ProcessStatusViewModel();
                Models = JsonConvert.DeserializeObject<ProcessStatusViewModel>(body.ToString());
                var result = service.dropdownProcessStatus(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region AutoProductfilter
        [HttpPost("autoProductfilter")]
        public IActionResult autoProdutfilter([FromBody]JObject body)
        {
            try
            {
                var service = new ShortShipService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoProductfilter(Models);
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
