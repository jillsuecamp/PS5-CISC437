using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OCTOBER.EF.Data;
using OCTOBER.Server.Controllers.Base;
using OCTOBER.Shared.DTO;

namespace OCTOBER.Server.Controllers.UD
{
    [Route("api/[controller]")]
    [ApiController]

    public class EnrollmentController : BaseController, GenericRestController<EnrollmentDTO>
    {
        public EnrollmentController(OCTOBEROracleContext context,
                                IHttpContextAccessor httpContextAccessor,
                                IMemoryCache memoryCache)
                : base(context, httpContextAccessor)
        {
        }
    }

