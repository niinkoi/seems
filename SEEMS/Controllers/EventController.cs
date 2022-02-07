﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SEEMS.Data.DTO;
using SEEMS.Database;
using SEEMS.Models;

namespace SEEMS.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class EventController : ControllerBase

    {
        private readonly IdentityDbContext _context;
        private readonly IMapper _mapper;
        public EventController(IdentityDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        [HttpPost("add-event")]
        public IActionResult Post([FromBody] EventDTO eventDTO)
        {
            //try
            //{
            if (!ModelState.IsValid)
            {
                //throw new InvalidOperationException();
                return BadRequest();
            }
            else
            {
                var _event = _mapper.Map<Event>(eventDTO);
                _context.Events.Add(_event);
                _context.SaveChanges();
                return Ok(eventDTO);
            }
            //}
            //catch (Exception ex)
            //{
            //}

        }

        [HttpGet()]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
