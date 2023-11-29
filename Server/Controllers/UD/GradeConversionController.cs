using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OCTOBER.EF.Data;
using OCTOBER.EF.Models;
using OCTOBER.Server.Controllers.Base;
using OCTOBER.Shared.DTO;

namespace OCTOBER.Server.Controllers.UD
{
	public class GradeConversionController : BaseController, GenericRestController<CourseDTO>
    {
        public GradeConversionController(OCTOBEROracleContext context,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache memoryCache)
        : base(context, httpContextAccessor)
        {
        }

        [HttpDelete]
        [Route("Delete/{SchoolId}")]
        public async Task<IActionResult> Delete(int SchoolId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.GradeConversions.Where(x => x.SchoolId == SchoolId).FirstOrDefaultAsync();

                if (itm != null)
                {
                    _context.GradeConversions.Remove(itm);
                }
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                return Ok();
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }

        public async Task<IActionResult> Get()
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var result = await _context.GradeConversions.Select(sp => new GradeConversionDTO
                {
                    
                    
                })
                .ToListAsync();
                await _context.Database.RollbackTransactionAsync();
                return Ok(result);
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }

        public Task<IActionResult> Get(int KeyVal)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Post([FromBody] CourseDTO _T)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Put([FromBody] CourseDTO _T)
        {
            throw new NotImplementedException();
        }
    }
}

