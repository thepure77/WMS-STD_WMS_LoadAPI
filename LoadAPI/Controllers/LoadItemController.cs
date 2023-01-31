using LoadBusiness.Load;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LoadAPI.Controllers
{
    [Route("api/TruckLoadItem")]
    public class LoadItemController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public LoadItemController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #region find
        [HttpGet("find/{id}")]
        public IActionResult find(Guid id)
        {
            try
            {
                var service = new LoadItemService();
                var result = service.find(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion


        [HttpGet("findDetail/{id}")]
        public IActionResult findDetail(Guid id)
        {
            try
            {
                var service = new LoadItemService();
                var result = service.findDetail(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }



    }
}
