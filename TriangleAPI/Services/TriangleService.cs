using Newtonsoft.Json;
using TriangleAPI.Models.Request;
using TriangleAPI.Models.Response.Response;

namespace TriangleAPI.Services
{
    public class TriangleService
    {
        public TriangleService()
        {

        }

        public async Task<MVCCommonResponse<string>> CheckTriangleType(TriangleRequest triangleRequest)
        {
            MVCCommonResponse<string> resp = new();
            string triangleType = string.Empty;
            try
            {
                if (triangleRequest.FirstSide == triangleRequest.SecondSide &&
                    triangleRequest.SecondSide == triangleRequest.ThirdSide)
                {
                    triangleType = "EQUILATERAL";
                }
                else if (triangleRequest.FirstSide == triangleRequest.SecondSide ||
                    triangleRequest.SecondSide == triangleRequest.ThirdSide ||
                    triangleRequest.FirstSide == triangleRequest.ThirdSide
                    )
                {
                    triangleType = "ISOSCELES";
                }
                else
                {
                    triangleType = "SCALANE";
                }
                resp.Ok(triangleType);
            }
            catch (Exception e)
            {
                resp.Exception(Models.Enums.ErrorCodes.Exception, JsonConvert.SerializeObject(e));
            }
            return await Task.FromResult(resp).ConfigureAwait(false);
        }
    }
}
