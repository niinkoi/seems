import { useEffect, useState } from 'react'

import { useParams } from 'react-router-dom'
import { useRecoilValue } from 'recoil'

import EventPoster from '../../components/EventPoster'
import { Festival } from '@mui/icons-material'
import { Box, Button, Card, CardContent, Container, Grid, Typography } from '@mui/material'
import { blueGrey } from '@mui/material/colors'

import atom from '../../recoil/auth'
import useEventAction from '../../recoil/event/action'
import CommentsSection from './Comments/index'
import EventDate from './EventDate'

const EventDetailed = () => {
    const auth = useRecoilValue(atom)
    const { id } = useParams()
    const { getDetailedEvent } = useEventAction()
    const [error, setError] = useState(null)
    const [detailedEvent, setDetailedEvent] = useState({
        numberComments: 0,
        event: {},
        numberRootComments: 0,
    })
    useEffect(() => {
        getDetailedEvent(id)
            .then((response) => {
                const { event: responseEvent } = response.data.data
                setDetailedEvent({
                    numberComments: responseEvent.commentsNum,
                    event: responseEvent,
                    numberRootComments: responseEvent.rootCommentsNum,
                })
            })
            .catch((errorResponse) => {
                const errorMessage = errorResponse.response.data.data
                setError(errorMessage)
            })
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])
    if (error)
        return (
            <Box
                sx={{
                    height: '85vh',
                    display: 'flex',
                    justifyContent: 'center',
                    alignItems: 'center',
                }}
            >
                <Typography variant="h4">{error}</Typography>
            </Box>
        )
    return (
        <Container fixed sx={{ mt: 15, px: 0, mb: 8 }}>
            <Grid container>
                <Grid item xs={12} sm={4}>
                    <EventPoster src={detailedEvent.event.imageUrl} size="contain" />
                </Grid>
                <Grid item xs={12} sm={8} component={Card} sx={{ position: 'relative' }}>
                    <CardContent sx={{ p: 5 }}>
                        <Typography variant="h4" color="primary" fontWeight={700}>
                            {detailedEvent.event.eventTitle}
                        </Typography>
                        <Box display="flex" alignItems="center" sx={{ my: 2 }}>
                            <Festival color="primary" fontSize="medium" />
                            <Typography
                                fontWeight={600}
                                variant="h5"
                                sx={{ ml: 1.5, color: blueGrey[900] }}
                            >
                                {detailedEvent.event.location}
                            </Typography>
                        </Box>
                        <EventDate
                            startDate={new Date(detailedEvent.event.startDate)}
                            endDate={new Date(detailedEvent.event.endDate)}
                        />
                        <Typography paragraph sx={{ color: blueGrey[900], my: 1.5 }} variant="h6">
                            {detailedEvent.event.eventDescription}
                        </Typography>
                        {!auth.role === 'Admin' && (
                            <Box sx={{ position: 'absolute', bottom: 30, right: 30 }}>
                                <Button variant="contained">Register</Button>
                            </Box>
                        )}
                    </CardContent>
                </Grid>
            </Grid>
            <CommentsSection
                eventId={id}
                numberComments={detailedEvent.numberComments}
                numberRootComments={detailedEvent.numberRootComments}
            />
        </Container>
    )
}
export default EventDetailed
