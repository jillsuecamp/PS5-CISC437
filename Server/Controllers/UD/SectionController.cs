using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OCTOBER.EF.Data;
using OCTOBER.EF.Models;
using OCTOBER.Shared;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis;
using AutoMapper;
using OCTOBER.Server.Controllers.Base;
using OCTOBER.Shared.DTO;
using System.Linq;

namespace OCTOBER.Server.Controllers.UD
{
	public class StudentController : BaseController, GenericRestController<CourseDTO>
    {
        public StudentController(OCTOBEROracleContext context,
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

                var itm = await _context.Students.Where(x => x.StudentId == StudentId).FirstOrDefaultAsync();

                if (itm != null)
                {
                    _context.Students.Remove(itm);
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

                var result = await _context.Students.Select(sp => new StudentDTO
                {
                    Capacity = sp.Capacity,
                    CourseNo = sp.CourseNo,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    InstructorId = sp.InstructorId,
                    Location = sp.Location,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                    SchoolId = sp.SchoolId,
                    StudentId = sp.StudentId,
                    StudentNo = sp.StudentNo,
                    StartDateTime = sp.StartDateTime
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
        [Route("Get/{SchoolID}")]
        public async Task<IActionResult> Get(int SchoolId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                StudentDTO? result = await _context.Students.Where(x =>x.SchoolId == SchoolId).Where(x=>x.StudentId == SchoolId).Select(sp => new StudentDTO
                {
                    Capacity = sp.Capacity,
                    CourseNo = sp.CourseNo,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    InstructorId = sp.InstructorId,
                    Location = sp.Location,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                    SchoolId = sp.SchoolId,
                    StudentId = sp.StudentId,
                    StudentNo = sp.StudentNo,
                    StartDateTime = sp.StartDateTime
                })
                .SingleAsync();
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
        public async Task<IActionResult> Post([FromBody] StudentDTO _StudentDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Students.Where(x => x.StudentId == _StudentDTO.StudentId).FirstOrDefaultAsync();

                if (itm == null)
                {
                    Student s = new Student
                    {
                        StudentId = _StudentDTO.StudentId,
                        CourseNo = _StudentDTO.CourseNo,
                        StudentNo = _StudentDTO.StudentNo,
                        StartDateTime = _StudentDTO.StartDateTime,
                        Location = _StudentDTO.Location,
                        InstructorId = _StudentDTO.InstructorId,
                        Capacity = _StudentDTO.Capacity,
                        SchoolId = _StudentDTO.SchoolId
                    };
                    _context.Students.Add(s);
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
        public async Task<IActionResult> Put([FromBody] StudentDTO _StudentDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Students.Where(x => x.CourseNo == _StudentDTO.CourseNo).FirstOrDefaultAsync();

                itm.Capacity = _StudentDTO.Capacity;
                itm.Location = _StudentDTO.Location;

                _context.Students.Update(itm);
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

