using LoadBusiness.Load;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LoadAPI.Controllers
{
    [Route("api/AutoLoad")]
    public class AutoLoadController : Controller
    {
        #region autoGoodIssueNo
        [HttpPost("autoGoodIssueNo")]
        public IActionResult autoGoodIssueNo([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoGoodIssueNo(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region autoPlanGoodIssueNo
        [HttpPost("autoPlanGoodIssueNo")]
        public IActionResult autoPlanGoodIssueNo([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoPlanGoodIssueNo(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region autoTruckloadNo
        [HttpPost("autoTruckloadNo")]
        public IActionResult autoTruckloadNo([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoTruckloadNo(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region dockfilter
        [HttpPost("dockfilter")]
        public IActionResult dockfilter([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = new trace_loading_model();
                Models = JsonConvert.DeserializeObject<trace_loading_model>(body.ToString());
                var result = service.Dockfillter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region Rollcagefilter
        [HttpPost("Rollcagefilter")]
        public IActionResult Rollcagefilter([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = new trace_loading_model();
                Models = JsonConvert.DeserializeObject<trace_loading_model>(body.ToString());
                var result = service.Rollcagefillter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region Appointtimefilter
        [HttpPost("Appointtimefilter")]
        public IActionResult Appointtimefilter([FromBody]JObject body)
        {
            try
            {
                var service = new LoadService();
                var Models = new trace_loading_model();
                Models = JsonConvert.DeserializeObject<trace_loading_model>(body.ToString());
                var result = service.Appointtimefilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion
    }


}
