namespace Homies.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore;

	using Homies.Data;
	using Homies.Models;
	using Homies.Data.Models;

	public class EventController : BaseController
	{
		private readonly HomiesDbContext context;

		public EventController(HomiesDbContext context)
		{
			this.context = context;
		}

		public async Task<IActionResult> All()
		{
			var events = await context
				.Events
				.Select(e => new AllEventsVM
				{
					Id = e.Id,
					Name = e.Name,
					Start = e.Start.ToString("dd-MM-yyyy HH:mm"),
					Type = e.Type.Name,
					Organiser = e.Organiser.UserName
				}).ToArrayAsync();

			return View(events);
		}

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			var types = await context.Types
				.Select(t => new EventType
				{
					Id = t.Id,
					Name = t.Name,
				})
				.ToListAsync();

			EventFormVM model = new EventFormVM()
			{
				Types = types
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(EventFormVM model)
		{
			Event e = new Event()
			{
				Name = model.Name,
				Description = model.Description,
				Start = model.Start,
				End = model.End,
				TypeId = model.TypeId,
				OrganiserId = GetUserId(),

			};

			await context.Events.AddAsync(e);
			await context.SaveChangesAsync();

			return RedirectToAction("All", "Event");
		}

		public async Task<IActionResult> Joined()
		{
			string userId = GetUserId();

			IEnumerable<AllEventsVM> joinedEvents = await context.EventsParticipants
				.Where(o => o.HelperId == userId)
				.AsNoTracking()
				.Select(e => new AllEventsVM()
				{
					Id = e.EventId,
					Name = e.Event.Name,
					Start = e.Event.Start.ToString("dd-MM-yyyy HH:mm"),
					Type = e.Event.Type.Name,
					Organiser = e.Event.Organiser.UserName
				})
				.ToArrayAsync();

			return View(joinedEvents);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			Event targetEvent = await context.Events
				.FirstAsync(e => e.Id == id);

			EventFormVM model = new EventFormVM()
			{
				Name = targetEvent.Name,
				Description = targetEvent.Description,
				Start = targetEvent.Start,
				End = targetEvent.End,
				TypeId = targetEvent.TypeId,
				Types = await GetAllTypes()
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(EventFormVM model, int id)
		{
			Event targetEvent = await context.Events
				.FirstAsync(e => e.Id == id);

			targetEvent.Name = model.Name;
			targetEvent.Description = model.Description;
			targetEvent.Start = model.Start;
			targetEvent.End = model.End;
			targetEvent.TypeId = model.TypeId;

			await context.SaveChangesAsync();

			return RedirectToAction("All", "Event");
		}

		public async Task<IActionResult> Join(int id)
		{
			var selectedEvent = await context.Events
				.Include(e => e.EventsParticipants)
				.FirstOrDefaultAsync(e => e.Id == id);

			if (selectedEvent == null)
			{
				return BadRequest();
			}

			var userId = base.GetUserId();

			if (!selectedEvent.EventsParticipants.Any(e => e.HelperId == userId))
			{
				selectedEvent.EventsParticipants
					.Add(new EventParticipant()
					{
						HelperId = userId,
					});
				await context.SaveChangesAsync();
				return RedirectToAction("Joined", "Event");
			}

			return RedirectToAction("All", "Event");
		}

		public async Task<IActionResult> Details(int id)
		{
			var selectedEvent = await context.Events
				.Include(e => e.Type)
				.Select(e => new EventDetailVM
				{
					Id = e.Id,
					Name = e.Name,
					Description = e.Description,
					Start = e.Start,
					End = e.End,
					CreatedOn = e.CreatedOn,
					Type = e.Type.Name,
					Organiser = e.Organiser.UserName!
				})
				.FirstAsync(e => e.Id == id);

			return View(selectedEvent);
		}

		public async Task<IActionResult> Leave(int id)
		{
			string userId = base.GetUserId();

			var e = await context
				.EventsParticipants
				.Where(ep => ep.HelperId == userId && ep.EventId == id)
				.FirstAsync();

			context.EventsParticipants.Remove(e);
			await context.SaveChangesAsync();

			return RedirectToAction("All", "Event");
		}

		private async Task<IEnumerable<EventType>> GetAllTypes()
			=> await context.Types
				.Select(t => new EventType
				{
					Id = t.Id,
					Name = t.Name,
				})
				.ToListAsync();

	}
}
