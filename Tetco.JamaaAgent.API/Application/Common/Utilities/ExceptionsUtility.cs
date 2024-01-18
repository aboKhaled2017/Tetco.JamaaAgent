using Domain.Common.Patterns;

namespace Application.Common.Utilities
{
    public static class ExceptionsUtility
    {
        public static Result ToResultError(this Exception ex)
        {
            return Result.Failure("Hub_Internal_Error", null, ex.Message);
        }
    }
}
