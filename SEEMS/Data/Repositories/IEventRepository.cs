﻿using SEEMS.Models;

namespace SEEMS.Data.Repositories
{
	public interface IEventRepository
	{
		IEnumerable<Event> GetAllEvents(bool trackChanges = false);
		Event GetEvent(int id, bool trackChanges = false);

		bool CanRegister(int id);

		bool CanTakeAttendance(int id);

		Task<IEnumerable<Event>> GetAllEventsAboutToStartIn30Min(DateTime from, bool trackChanges);

		Task<Event> GetEventAsync(int id, bool trackChanges);

		bool CanUnregister(int id, int minHourToUnregister);
	}
}
