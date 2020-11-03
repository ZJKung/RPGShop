using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
namespace CartApi.Infrastructure.ActionResults
{
    public class InternalServerErrorObjectResults : ObjectResult
    {
        public InternalServerErrorObjectResults(object error) : base(error)
        {
            StatusCode=StatusCodes.Status500InternalServerError;
        }
    }
}