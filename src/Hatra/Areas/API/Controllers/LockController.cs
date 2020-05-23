using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Hatra.Entities;
using Hatra.Services.Contracts;
using Hatra.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hatra.Areas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LockController : ControllerBase
    {
        private readonly IHardwareLockService _hardwareLockService;

        public LockController(IHardwareLockService hardwareLockService)
        {
            _hardwareLockService = hardwareLockService;
        }


        // POST api/<LockController>
        [HttpPost]
        public async Task<HardwareLockResponseViewModel> Post([FromBody] HardwareLockViewModel input)
        {
            if (!ModelState.IsValid) return new HardwareLockResponseViewModel() { Status = 3 };

            return await _hardwareLockService.InsertOrUpdateAsync(input);
        }

        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}
        //// GET: api/<LockController>
        //[HttpGet]
        //public HardwareLockViewModel Get()
        //{
        //    return new HardwareLockViewModel()
        //    {
        //        IsBlocked = true,
        //        LockSerialNumber = "Test Lock Serial",
        //        CpuSerialNumber = "Test Cpu Serial",
        //        ComputerName = "Test PC",
        //        FinancialYears = new List<HardwareLockFinancialYearViewModel>()
        //    {
        //        new HardwareLockFinancialYearViewModel()
        //        {
        //            Id = 0,
        //            CompanyId = Guid.NewGuid(),
        //            CompanyName = "TH724",
        //            FinancialYearId = Guid.NewGuid(),
        //            FinancialYearName = "1399",
        //            IsArchive = false
        //        }
        //    }
        //    };
        //}
        // GET api/<LockController>/5
        //// PUT api/<LockController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}
        //// DELETE api/<LockController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
