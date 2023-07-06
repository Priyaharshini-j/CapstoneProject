import React from 'react';
import { useLocation } from 'react-router-dom';
import MainLayout from '../layout/MainLayout';
import { Rating, Typography } from '@mui/material';
import { Heading, Text } from '@chakra-ui/react';
import '../component/BookComponent/BookComponent.css';
import Box from '@mui/material/Box';
import Tab from '@mui/material/Tab';
import TabContext from '@mui/lab/TabContext';
import TabList from '@mui/lab/TabList';
import TabPanel from '@mui/lab/TabPanel';

function BookPage(props) {
  const location = useLocation();
  console.log(props, 'props');
  console.log(location, 'location')
  const details = {
    isbn: location.state?.bookIsbn,
    title: location.state?.title,
    author: location.state?.author,
    publisher: location.state?.publisher,
    buyLink: location.state?.buyLink,
    coverImage: location.state?.coverImage,
    rating: location.state?.rating,
    genre: location.state?.genre,
    desc: location.state?.desc
  }
  const [value, setValue] = React.useState('1');

  const handleChange = (event, newValue) => {
    setValue(newValue);
  };

  return (
    <MainLayout>
      <div>
        <div className='book-holder'>
          <div className='image-container'>
            {details.coverImage !== null && (
              <img className='image-holder'
                src={details.coverImage}
                alt={details.title}
              />
            )}
          </div>
          <div className='details-container'>
            <Heading size="md">{details.title}</Heading>
            <Text
              css={{
                color: 'gray.500',
              }}
              py="2"
            >
              {details.author}
            </Text>
            <Text py="2">{details.genre}</Text>
            {console.log(details.rating)}
            <Rating name="read-only" value={details.rating} size="small" readOnly />
            <Typography variant="body2">Book Description: {details.desc}</Typography>
          </div>
        </div>
        <Box sx={{ width: '100%', typography: 'body1' }}>
          <TabContext value={value}>
            <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
              <TabList onChange={handleChange} aria-label="lab API tabs example">
                <Tab label="Item One" value="1" />
                <Tab label="Item Two" value="2" />
                <Tab label="Item Three" value="3" />
              </TabList>
            </Box>
            <TabPanel value="1">Item One</TabPanel>
            <TabPanel value="2">Item Two</TabPanel>
            <TabPanel value="3">Item Three</TabPanel>
          </TabContext>
        </Box>
      </div>
    </MainLayout>

  )
}

export default BookPage