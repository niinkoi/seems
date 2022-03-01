import { get, post } from '../../utils/ApiCaller'

const useEventAction = () => {
    const getUpcomingEvents = () => get({ endpoint: '/api/events/upcoming' })

    const getEvents = (filterString) => get({ endpoint: '/api/events' + filterString })

    const getMyEvents = (filterString) => get({ endpoint: '/api/Events/my-events' + filterString })

    const getDetailedEvent = (eventId) => get({ endpoint: `/api/events/${eventId}` })

    const createEvent = (eventData) =>
        post({
            endpoint: '/api/events',
            body: eventData,
        })

    const registerEvent = (eventId) =>
        post({
            endpoint: '/api/reservations',
            body: {
                eventId,
            },
        })

    return {
        getUpcomingEvents,
        getEvents,
        getMyEvents,
        createEvent,
        getDetailedEvent,
        registerEvent,
    }
}

export default useEventAction
