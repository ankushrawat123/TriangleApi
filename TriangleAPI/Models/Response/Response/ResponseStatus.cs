using TriangleAPI.Models.Enums;

namespace TriangleAPI.Models.Response.Response
{
    public static class ResponseStatus
    {
        public static readonly Dictionary<ErrorCodes, string>? Library = new();

        static ResponseStatus() 
        {
            Library.Add(ErrorCodes.Success, "Request Processed Successfully");
            Library.Add(ErrorCodes.Failure, "Something Went Wrong While Processing Request");
            Library.Add(ErrorCodes.Exception, "We're Sorry , But We're Experiencing Some Technical Issues");
            Library.Add(ErrorCodes.InvalidFirstSideInput, "FirstSide Input Value Is Either Zero Or Less Than Zero");
            Library.Add(ErrorCodes.InvalidSecondSideInput, "SecondSide Input Value Is Either Zero Or Less Than Zero");
            Library.Add(ErrorCodes.InvalidThirdSideInput, "ThirdSide Input Value Is Either Zero Or Less Than Zero");
        }
    }
}
