using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrderApp.Application.Dtos.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Application.Filters
{
    public class ValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            if (!context.ModelState.IsValid)
            {
                ErrorMessage errorMessage = new();
                errorMessage.ErrorCode = ((int)HttpStatusCode.BadRequest).ToString();

                IEnumerable<ModelError> modelErrors = context.ModelState.Values.SelectMany(v => v.Errors);

                modelErrors.ToList().ForEach(x =>
                {
                    errorMessage.ErrorDescription = x.ErrorMessage;
                });

                var response = new CustomResponseDto().Error((int)HttpStatusCode.BadRequest, errorMessage);

                context.Result = new BadRequestObjectResult(response);
            }
        }
    }
}
