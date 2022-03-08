import { useEffect, useState } from 'react'

import queryString from 'query-string'
import { useHistory, useLocation } from 'react-router-dom'

import SearchField from '../../../components/SearchField'
import { Box, Divider } from '@mui/material'

import EventActiveFilter from './EventActiveFilter'
import EventPeriodFilter from './EventPeriodFilter'

const Filters = ({ page }) => {
    const history = useHistory()
    const { search: queries } = useLocation()
    const { search, upcoming, active } = queryString.parse(queries)
    const [nameFilter, setNameFilter] = useState('')
    const [periodFilter, setPeriodFilter] = useState(upcoming)
    const [activeFilter, setActiveFilter] = useState(active)

    const searchSubmitHandler = (searchText) => {
        searchText = searchText.trim()
        if (searchText !== '') {
            setNameFilter(searchText)
        }
    }

    const periodSubmitHandler = (periodOption) => {
        setPeriodFilter(periodOption)
    }

    const activeSubmitHandler = (activeOption) => {
        setActiveFilter(activeOption)
    }

    const searchHandler = () => {
        let route = page + '?'

        if (nameFilter) {
            route += '&search=' + nameFilter
        }
        if (periodFilter) {
            route += '&upcoming=' + periodFilter
        }
        if (activeFilter) {
            route += '&active=' + activeFilter
        }

        history.push(route)
    }

    useEffect(() => {
        searchHandler()
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [nameFilter, periodFilter, activeFilter])

    return (
        <Box mr={8} width="500px">
            <SearchField submitHandler={searchSubmitHandler} defaultText={search} />
            <Divider sx={{ width: '100%', backgroundColor: 'secondary.main', my: 3 }} />
            <EventPeriodFilter submitHandler={periodSubmitHandler} defaultPeriod={upcoming} />
            <EventActiveFilter submitHandler={activeSubmitHandler} defaultPeriod={active} />
        </Box>
    )
}

export default Filters