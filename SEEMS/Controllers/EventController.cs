﻿using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using SEEMS.Authorization;
using SEEMS.Contexts;
using SEEMS.Data.DTO;
using SEEMS.Data.Models;
using SEEMS.Data.ValidationInfo;
using SEEMS.Infrastructures.Commons;
using SEEMS.Models;
using SEEMS.Services;
using SEEMS.Services.Interfaces;
namespace SEEMS.Controller
{
	[Route("api/Events")]
	[ApiController]
	[ApiExplorerSettings(GroupName = "v1")]
	public class EventController : ControllerBase

	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;
		private readonly UserService _userService;

		public EventController(ApplicationDbContext context, IMapper mapper, UserService userService)
		{
			_context = context;
			_mapper = mapper;
			_userService = userService;
		}

		[HttpGet("detail/{id}")]
		public async Task<IActionResult> GetEventDetail()
		{
			try
			{
				var foundEvent = _context.Events.FirstOrDefault(
					e => e.Id == HttpContext.Request.QueryString[]
				);
			}
			catch (Exception ex)
			{
				return BadRequest(
					new Response(
						ResponseStatusEnum.Fail,
						ex.Message
					)
				);
			}
			return null;
		}

		[HttpGet("my-events")]
		[AuthorizationFilter(RoleTypes.ORG, RoleTypes.ADM)]
		public async Task<ActionResult<List<Event>>> GetMyEvents()
		{
			User currentUser = null;
			try
			{
				var user = await _userService.getCurrentUser(HttpContext);
				if (user != null)
				{
					var listEvents = _context.Events.Where(a => a.Client.Id == user.Id).ToList();
					return Ok(
						new Response(ResponseStatusEnum.Success,
						new
						{
							Count = listEvents.Count(),
							Events = listEvents
						})
					);
				}
			}
			catch (Exception e)
			{
				return BadRequest(new Response(ResponseStatusEnum.Error, e.Message));
			}
			return null;

		}
		[HttpGet("upcoming")]
		[AuthorizationFilter(RoleTypes.CUSR, RoleTypes.ORG, RoleTypes.ADM)]
		public async Task<ActionResult<List<Event>>> Get()
		{
			int resultCount;
			try
			{
				var result = _context.Events.ToList().Where(
						e => e.StartDate.Subtract(DateTime.Now).TotalMinutes >= 30
				);
				resultCount = Math.Min(10, result.Count());
				return Ok(new Response(
					ResponseStatusEnum.Success,
					new
					{
						Count = resultCount,
						Events = result.ToList().GetRange(0, resultCount)
					}
				));
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new Response(ResponseStatusEnum.Error, msg: ex.Message));
			}
		}


		[HttpGet()]
		[AuthorizationFilter(RoleTypes.CUSR, RoleTypes.ORG, RoleTypes.ADM)]
		public async Task<ActionResult<List<Event>>> Get(string? search, int? lastEventID, int resultCount = 10)
		{
			try
			{
				var allEvents = _context.Events.ToList();
				var result = allEvents.Where(
					e => Utilitiies.IsAfterMinutes(e.StartDate, DateTime.Now, 30));
				bool failed = false;

				//Filter by title
				if (!string.IsNullOrEmpty(search))
				{
					result = result.Where(e => e.EventTitle.Contains(search, StringComparison.CurrentCultureIgnoreCase));
				}

				//Implement load more
				if (lastEventID != null)
				{
					var lastEventIndex = result.ToList().FindIndex(e => e.Id == lastEventID);
					if (lastEventIndex > 0)
					{
						result = result.ToList().GetRange(
							lastEventIndex + 1,
							Math.Min(resultCount, result.Count() - lastEventIndex - 1));
					}
					else
					{
						failed = true;
					}
				}
				else
				{
					result = result.OrderByDescending(e => e.StartDate).ToList().GetRange(0, Math.Min(result.Count(), resultCount));
				}

				return failed
					? BadRequest(
						new Response(ResponseStatusEnum.Fail, msg: "Invalid Id"))
					: Ok(
						new Response(ResponseStatusEnum.Success,
						new
						{
							Count = result.Count(),
							listEvents = result
						})
				);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new Response(ResponseStatusEnum.Error, msg: ex.Message));
			}
		}

		[HttpPost]
		[AuthorizationFilter(RoleTypes.ORG, RoleTypes.ADM)]
		public async Task<ActionResult> AddEvent(EventDTO eventDTO)
		{
			eventDTO.StartDate = eventDTO.StartDate.ToLocalTime();
			eventDTO.EndDate = eventDTO.EndDate.ToLocalTime();
			EventValidationInfo? eventValidationInfo = EventsServices.GetValidatedEventInfo(eventDTO);

			try
			{
				if (eventValidationInfo != null)
				{
					return BadRequest(
						new Response(ResponseStatusEnum.Fail,
						eventValidationInfo,
						"Some fields didn't match requirements"));
				}
				else
				{
					eventDTO.Active = true;
					if (eventDTO.IsFree) eventDTO.ExpectPrice = 0;
					var newEvent = _mapper.Map<Event>(eventDTO);
					var user = await _userService.getCurrentUser(HttpContext);
					newEvent.Client = user;
					_context.Events.Add(newEvent);
					_context.SaveChanges();
					return Ok(new Response(ResponseStatusEnum.Success, eventDTO));
				}
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new Response(ResponseStatusEnum.Error, msg: ex.Message));
			}
		}
	}
}
