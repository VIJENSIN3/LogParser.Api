using Microsoft.AspNetCore.Mvc;

namespace LogParser.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogParserService _logParserService;

        public LogController(ILogParserService logParserService)
        {
            _logParserService = logParserService;
        }

        [HttpPost("upload")]
        public IActionResult ParseLog([FromBody] string[] logLines)
        {
            var entries = _logParserService.ParseLogEntries(logLines).ToList();
            var hostAccessCounts = _logParserService.GetHostAccessCounts(entries);
            var resourceAccessCounts = _logParserService.GetSuccessfulResourceAccessCounts(entries);

            return Ok(new { HostAccessCounts = hostAccessCounts, ResourceAccessCounts = resourceAccessCounts });
        }
    }

}
