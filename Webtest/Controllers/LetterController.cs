using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;

namespace Webtest.Controllers
{
    [RoutePrefix("api/letter")]
    public class LetterController : ApiController
    {
        private IService _service;
        public LetterController(IService service)
        {
            _service = service;
        }

        [HttpPost, Route("")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Letter validation failed")]
        [SwaggerResponse(HttpStatusCode.NotFound, "One or more words not found")]
        [SwaggerResponse(HttpStatusCode.OK, "Letter was generated successfully", typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Something wrong")]
        public async Task<IHttpActionResult> DoLetterAsync([FromBody]string value)
        {
            value = value.Trim().ToLowerInvariant();
            if (string.IsNullOrEmpty(value))
                return BadRequest();

            value = Regex.Replace(value,@"\s+", " ");

            try
            {
                var resultLetter = await _service.CreateLetterAsync(value);
                return resultLetter.Equals(value, StringComparison.InvariantCultureIgnoreCase) ?
                    (IHttpActionResult)Ok(resultLetter) : NotFound();

            }
            catch (InvalidOperationException ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
