using FluentValidation;

namespace IztekCafe.WebApi.Filters
{
    public class ValidationFilter<T> : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
            if (validator is null)
            {
                return await next(context);
            }
            var model = context.Arguments.OfType<T>().FirstOrDefault();
            if (model is null)
            {
                return await next(context);
            }
            var validateResult = await validator.ValidateAsync(model);
            if (!validateResult.IsValid)
            {
                return Results.ValidationProblem(validateResult.ToDictionary());
            }
            return await next(context);
        }
    }
}