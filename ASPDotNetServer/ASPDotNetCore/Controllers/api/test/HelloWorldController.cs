using ASPDotNetCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASPDotNetCore.Controllers
{
    [Route("api/test/HelloWorld")]
    [ApiController]
    public class HelloWorldController : ControllerBase
    {
        public HelloWorld Get()
        {
            return new HelloWorld() { str = "Hello, World" };
        }

    }
}
