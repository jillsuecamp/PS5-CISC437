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
	public class GradeController : BaseController, GenericRestController<CourseDTO>
    {
        public GradeController(OCTOBEROracleContext context,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache memoryCache)
        : base(context, httpContextAccessor)
        {
        }

        [HttpDelete]
        [Route("Delete/{StudentId}")]
        public async Task<IActionResult> Delete(int StudentId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Grades.Where(x => x.StudentId == StudentId).FirstOrDefaultAsync();

                if (itm != null)
                {
                    _context.Grades.Remove(itm);
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

                var result = await _context.Grades.Select(sp => new GradeDTO
                {
                     
                     SchoolId = sp.SchoolId,
                     Comments = sp.Comments,
                     CreatedBy = sp.CreatedBy,
                     CreatedDate = sp.CreatedDate,
                     GradeCodeOccurrence = sp.GradeCodeOccurrence,
                     GradeTypeCode = sp.GradeTypeCode,
                     ModifiedBy = sp.ModifiedBy,
                     ModifiedDate = sp.ModifiedDate,
                     NumericGrade = sp.NumericGrade,
                     SectionId = sp.SectionId,
                     StudentId = sp.StudentId
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
        [Route("Get/{SchoolID}/{SectionId}/{StudentId}/{GradeCodeOccurrence}/{GradeTypeCode}")]
        public async Task<IActionResult> Get(int SchoolId, int SectionId, int StudentId, int GradeCodeOccurrence, int GradeTypeCode)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                GradeDTO? result = await _context
                .Grades
                    .Where(x => x.SectionId == SectionId)
                    .Where(x => x.SchoolId == SchoolId)
                    .Where(x => x.StudentId == StudentId)
                    .Where(x => x.GradeCodeOccurrence == SchoolId)
                    .Where(x => x.StudentId == StudentId)
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
        public async Task<IActionResult> Post([FromBody] GradeDTO _GradeDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Grades.Where(x => x.StudentId == _GradeDTO.StudentId).FirstOrDefaultAsync();

                if (itm == null)
                {
                    Grade g = new Grade
                    {
                    Comments = _GradeDTO.Comments,
                    GradeCodeOccurrence = _GradeDTO.GradeCodeOccurrence,
                    GradeTypeCode = _GradeDTO.GradeTypeCode,
                    NumericGrade = _GradeDTO.NumericGrade
                };
                    _context.Grades.Add(g);
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
        public Task<IActionResult> Put([FromBody] CourseDTO _T)
        {
            throw new NotImplementedException();
        }
    }
}

