using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TriangleAPI.Models.Enums;
using TriangleAPI.Models.Request;
using TriangleAPI.Models.Response.Response;
using TriangleAPI.Services;
using TriangleAPI.Validations;

namespace TriangleAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TriangleController : Controller
    {
        private readonly TriangleValidator _triangleValidator;
        private readonly TriangleService _triangleService;
        public TriangleController(TriangleValidator triangleValidator,
            TriangleService triangleService)
        {
            _triangleValidator = triangleValidator;
            _triangleService = triangleService;
        }

        [HttpPost]
        [Route("CheckTriangle")]
        public async Task<MVCCommonResponse<string>> CheckTriangle([FromBody] TriangleRequest triangleRequest)
        {
            MVCCommonResponse<string> response = new();
            try
            {
                var validateResult = await _triangleValidator.ValidateAsync(triangleRequest).ConfigureAwait(false);
                if (!validateResult.IsValid)
                {
                    Enum.TryParse(validateResult.Errors.FirstOrDefault().ErrorMessage, out ErrorCodes status);
                    return await Task.FromResult(response.Fail(status)).ConfigureAwait(false);
                }
                response = await _triangleService.CheckTriangleType(triangleRequest).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                response.Exception(ErrorCodes.Exception, JsonConvert.SerializeObject(e));
            }
            return await Task.FromResult(response).ConfigureAwait(false);
        }
    }
}
