using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OCTOBER.EF.Data;
using OCTOBER.Server.Controllers.Base;
using OCTOBER.Shared.DTO;

using System;
using Microsoft.EntityFrameworkCore;
using OCTOBER.EF.Models;

namespace OCTOBER.Server.Controllers.UD
{
    public class EnrollmentController : BaseController, GenericRestController<CourseDTO>
    {
        public EnrollmentController(OCTOBEROracleContext context,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache memoryCache)
        : base(context, httpContextAccessor)
        {
        }

        [HttpDelete]
        [Route("Delete/{SectionId}")]
        public async Task<IActionResult> Delete(int SectionId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Enrollments.Where(x => x.SectionId == SectionId).FirstOrDefaultAsync();

                if (itm != null)
                {
                    _context.Enrollments.Remove(itm);
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

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var result = await _context.Enrollments.Select(sp => new EnrollmentDTO
                {
                     SchoolId = sp.SchoolId,
                     SectionId = sp.SectionId,
                     CreatedBy = sp.CreatedBy,
                     CreatedDate = sp.CreatedDate,
                     EnrollDate = sp.EnrollDate,
                     FinalGrade = sp.FinalGrade,
                     ModifiedBy = sp.ModifiedBy,
                     ModifiedDate = sp.ModifiedDate,
                     SectionId = sp.SectionId
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
        [Route("Get/{SchoolID}/{SectionId}/{SectionId}")]
        public async Task<IActionResult> Get(int SchoolId, int SectionId, int SectionId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                EnrollmentDTO? result = await _context
                .Enrollments
                    .Where(x => x.SectionId == SectionId)
                    .Where(x => x.SchoolId == SchoolId)
                    .Where(x => x.SectionId == SectionId)
                     .Select(sp => new EnrollmentDTO
                     {
                         SchoolId = sp.SchoolId,
                         SectionId = sp.SectionId,
                         CreatedBy = sp.CreatedBy,
                         CreatedDate = sp.CreatedDate,
                         EnrollDate = sp.EnrollDate,
                         FinalGrade = sp.FinalGrade,
                         ModifiedBy = sp.ModifiedBy,
                         ModifiedDate = sp.ModifiedDate,
                         SectionId = sp.SectionId
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
        public async Task<IActionResult> Post([FromBody] EnrollmentDTO _EnrollmentDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Enrollments.Where(x => x.SectionId == _EnrollmentDTO.SectionId).FirstOrDefaultAsync();

                if (itm == null)
                {
                    Enrollment e = new Enrollment
                    {
                        EnrollDate = _EnrollmentDTO.EnrollDate,
                        FinalGrade = _EnrollmentDTO.FinalGrade,
                        SectionId = _EnrollmentDTO.SectionId,
                        SectionId = _EnrollmentDTO.SectionId,
                        SchoolId = _EnrollmentDTO.SectionId

                    };
                    _context.Enrollments.Add(e);
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
        public async Task<IActionResult> Put([FromBody] EnrollmentDTO _EnrollmentDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Enrollments.Where(x => x.SectionId == _EnrollmentDTO.SectionId).FirstOrDefaultAsync();

                itm.FinalGrade = _EnrollmentDTO.FinalGrade;

                _context.Enrollments.Update(itm);
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
