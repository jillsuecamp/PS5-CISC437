using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OCTOBER.EF.Data;
using OCTOBER.EF.Models;
using OCTOBER.Server.Controllers.Base;
using OCTOBER.Shared.DTO;
using static System.Collections.Specialized.BitVector32;

namespace OCTOBER.Server.Controllers.UD
{
    public class GradeTypeController : BaseController, GenericRestController<CourseDTO>
    {
        public GradeTypeController(OCTOBEROracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public Task<IActionResult> Delete(int KeyVal)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var result = await _context.GradeTypes.Select(sp => new GradeTypeDTO
                {

                     GradeTypeCode = sp.GradeTypeCode,
                     ModifiedDate = sp.ModifiedDate,
                     CreatedBy = sp.CreatedBy,
                     CreatedDate = sp.CreatedDate,
                     Description = sp.Description,
                     ModifiedBy = sp.ModifiedBy,
                     SchoolId = sp.SchoolId

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

        [HttpGet]
        [Route("Get/{SchoolID}/{GradeTypeCode}")]
        public async Task<IActionResult> Get(int SchoolId, char GradeTypeCode)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                GradeTypeDTO? result = await _context
                .Grades
                    .Where(x => x.SchoolId == SchoolId)
                    .Where(x => x.GradeTypeCode.Equals(GradeTypeCode)
                     .Select(sp => new GradeDTO
                     {
                         SchoolId = sp.SchoolId,
                         SectionId = sp.SectionId,
                         GradeCodeOccurrence = sp.GradeCodeOccurrence,
                         GradeTypeCode = sp.GradeTypeCode
                     })
                .SingleOrDefaultAsync();

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

        [HttpPost]
        [Route("Post")]
        public async Task<IActionResult> Post([FromBody] GradeTypeDTO _GradeTypeDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.GradeTypes.Where(x => x.SchoolId == _GradeTypeDTO.SchoolId).FirstOrDefaultAsync();

                if (itm == null)
                {
                    GradeType gt = new GradeType
                    {
                        SchoolId = _GradeTypeDTO.SchoolId,
                        Description = _GradeTypeDTO.Description,
                        GradeTypeCode = _GradeTypeDTO.GradeTypeCode
                         
                    };
                    _context.GradeTypes.Add(gt);
                    await _context.SaveChangesAsync();
                    await _context.Database.CommitTransactionAsync();
                }
                return Ok();
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }

        [HttpPut]
        [Route("Put")]
        public async Task<IActionResult> Put([FromBody] GradeTypeDTO _GradeTypeDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.GradeTypes.Where(x => x.SchoolId == _GradeTypeDTO.SchoolId).FirstOrDefaultAsync();

                itm.Description = _GradeTypeDTO.Description;
                itm.GradeTypeCode = _GradeTypeDTO.GradeTypeCode;

                _context.GradeTypes.Update(itm);
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
    }
}

