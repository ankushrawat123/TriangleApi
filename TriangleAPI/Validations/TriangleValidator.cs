using FluentValidation;
using TriangleAPI.Models.Enums;
using TriangleAPI.Models.Request;

namespace TriangleAPI.Validations
{
    public class TriangleValidator : AbstractValidator<TriangleRequest>
    {
        public TriangleValidator()
        {
            #region FirstSideValidations
            RuleFor(o => o).MustAsync(async (triangleRequest, CancellationToken) =>
            {
                bool isValid = await CheckSideValidation(triangleRequest.FirstSide).ConfigureAwait(false);
                return isValid;
            }).WithErrorCode(((double)ErrorCodes.InvalidFirstSideInput).ToString()).WithMessage(nameof(ErrorCodes.InvalidFirstSideInput));
            #endregion

            #region SecondSideValidations
            RuleFor(o => o).MustAsync(async (triangleRequest, CancellationToken) =>
            {
                bool isValid = await CheckSideValidation(triangleRequest.SecondSide).ConfigureAwait(false);
                return isValid;
            }).WithErrorCode(((double)ErrorCodes.InvalidSecondSideInput).ToString()).WithMessage(nameof(ErrorCodes.InvalidSecondSideInput));
            #endregion

            #region ThirdSideValidations
            RuleFor(o => o).MustAsync(async (triangleRequest, CancellationToken) =>
            {
                bool isValid = await CheckSideValidation(triangleRequest.ThirdSide).ConfigureAwait(false);
                return isValid;

            }).WithErrorCode(((double)ErrorCodes.InvalidThirdSideInput).ToString()).WithMessage(nameof(ErrorCodes.InvalidThirdSideInput));
            #endregion
        }

        public async Task<bool> CheckSideValidation(double value)
        {
            bool isValid = true;
            try
            {
                if (value <= 0)
                {
                    isValid = false;
                }
            }
            catch (Exception e)
            {
                isValid = false;
            }
            return await Task.FromResult(isValid).ConfigureAwait(false);
        }

    }
}
