using Microsoft.AspNetCore.Mvc;
using PortiaNet.HealthCheck.Reporter;
using PortiaNet.HealthCheck.Sample.API.Models;
using System.Diagnostics;
using System.Text.Json;
using RequestDetail = PortiaNet.HealthCheck.Sample.API.Models.RequestDetail;

namespace PortiaNet.HealthCheck.Sample.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ReportWriterEmulatorController : ControllerBase
    {
        private readonly string _sampleToken = "A very hard and long super secret token!!!";

        #region Without Authentication
        [HttpPost]
        [HealthCheckIgnore]
        public IActionResult SaveReportWithoutAuthentication([FromBody] RequestDetail report)
        {
            Debugger.Log(0, null, JsonSerializer.Serialize(report));
            return Ok();
        }
        #endregion Without Authentication

        #region Client Secret
        [HttpPost]
        [HealthCheckIgnore]
        public IActionResult AuthenticateByClientSecret([FromBody]string clientSecret)
        {
            if(string.IsNullOrEmpty(clientSecret))
                return Unauthorized();

            return Ok(_sampleToken);
        }

        [HttpPost]
        [HealthCheckIgnore]
        public IActionResult SaveReportByClientSecret([FromBody]RequestDetail report)
        {
            if (Request.Headers.ContainsKey("Authorization") &&
                Request.Headers["Authorization"][0].StartsWith("Bearer "))
            {
                var token = Request.Headers["Authorization"][0]["Bearer ".Length..];

                if (token != _sampleToken)
                    return Unauthorized();
            }
            else
                return Unauthorized();

            Debugger.Log(0, null, JsonSerializer.Serialize(report));
            return Ok();
        }
        #endregion Client Secret

        #region Static Bearer Token
        [HttpPost]
        [HealthCheckIgnore]
        public IActionResult SaveReportByStaticBearerToken([FromBody] RequestDetail report)
        {
            if (Request.Headers.ContainsKey("Authorization") &&
                Request.Headers["Authorization"][0].StartsWith("Bearer "))
            {
                var token = Request.Headers["Authorization"][0]["Bearer ".Length..];

                if (token != _sampleToken)
                    return Unauthorized();
            }
            else
                return Unauthorized();

            Debugger.Log(0, null, JsonSerializer.Serialize(report));
            return Ok();
        }
        #endregion Static Bearer Token

        #region Username and Password Authentication
        [HttpPost]
        [HealthCheckIgnore]
        public IActionResult AuthenticateByUsernamePassword([FromBody] UsernamePasswordModel credential)
        {
            if (credential == null || credential.Username != "TestUser" && credential.Password != "P@ssvor3d")
                return Unauthorized();

            return Ok(_sampleToken);
        }

        [HttpPost]
        [HealthCheckIgnore]
        public IActionResult SaveReportByAuthenticateByUsernamePassword([FromBody] RequestDetail report)
        {
            if (Request.Headers.ContainsKey("Authorization") &&
                Request.Headers["Authorization"][0].StartsWith("Bearer "))
            {
                var token = Request.Headers["Authorization"][0]["Bearer ".Length..];

                if (token != _sampleToken)
                    return Unauthorized();
            }
            else
                return Unauthorized();

            Debugger.Log(0, null, JsonSerializer.Serialize(report));
            return Ok();
        }
        #endregion Username and Password Authentication

        #region Bulk Data Dumping
        [HttpPost]
        [HealthCheckIgnore]
        public IActionResult SaveBulkReport([FromBody] List<RequestDetail> report)
        {
            Debugger.Log(0, null, JsonSerializer.Serialize(report));
            return Ok();
        }
        #endregion Bulk Data Dumping
    }
}
