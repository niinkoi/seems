import { useEffect } from 'react'

import { Box } from '@mui/material'

import useEventAction from '../../recoil/event/action'
import EventsList from './EventsList'

const events = [
    {
        id: 1,
        src: 'https://res.cloudinary.com/dq7l8216n/image/upload/v1642134779/Techpeek.png',
        time: '20-10-2022',
        mode: 'Offline',
        title: 'Nulla nisl tellus hendrerit nec dignissim pellentesqu posu in est Suspendisse ',
        content:
            'Cras semper, massa vel aliquam luctus, eros odio tempor turpis, ac placerat metus tortor eget magna. Donec mattis posuere pharetra. Donec vestibulum ornare velit ut sollicitudin ut sollicitudin.',
    },
    {
        id: 2,
        src: 'https://res.cloudinary.com/dq7l8216n/image/upload/v1642134779/Techpeek.png',
        time: '20-10-2022',
        mode: 'Online',
        title: 'Nulla nisl tellus hendrerit nec dignissim pellentesqu posu in est Suspendisse ',
        content:
            'Cras semper, massa vel aliquam luctus, eros odio tempor turpis, ac placerat metus tortor eget magna. Donec mattis posuere pharetra. Donec vestibulum ornare velit ut sollicitudin ut sollicitudin.',
    },
]

const MyEvents = () => {
    const eventAction = useEventAction()

    useEffect(() => {
        eventAction
            .getMyEvents()
            .then((response) => console.log(response))
            .catch((error) => console.log(error))
    })

    return (
        <Box component="main" sx={{ mt: { sx: 0, sm: 8.5 } }} px={3} pt={10}>
            <EventsList events={events} />
        </Box>
    )
}

export default MyEvents
