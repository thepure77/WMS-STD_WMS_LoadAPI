using LoadBusiness.Load;
using LoadBusiness.Upload;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LoadAPI.Controllers
{
    [Route("api/Upload")]
    public class UploadController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }


        [HttpPost("UploadImg")]
        public IActionResult UploadImg([FromBody]JObject body)
        {
            try
            {
                var service = new UploadService();
                var Models = new TruckLoadImageViewModel();
                Models = JsonConvert.DeserializeObject<TruckLoadImageViewModel>(body.ToString());
                var result = service.UploadImg(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("findImg")]
        public IActionResult findImg([FromBody]JObject body)
        {
            try
            {
                var service = new UploadService();
                var Models = new TruckLoadImageViewModel();
                Models = JsonConvert.DeserializeObject<TruckLoadImageViewModel>(body.ToString());
                var result = service.findImg(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


    }
}
