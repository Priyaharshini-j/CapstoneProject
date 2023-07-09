import React, { useMemo, useState } from 'react';
import { useLocation } from 'react-router-dom';
import MainLayout from '../layout/MainLayout';
import { Fab, Rating, Typography } from '@mui/material';
import { Heading, Text } from '@chakra-ui/react';
import '../component/BookComponent/BookComponent.css';
import Box from '@mui/material/Box';
import Tab from '@mui/material/Tab';
import TabContext from '@mui/lab/TabContext';
import TabList from '@mui/lab/TabList';
import TabPanel from '@mui/lab/TabPanel';
import CommunityComponent from '../component/CommunityComponent/CommunityComponent';
import CritiqueComponent from '../component/CritiqueComponent/CritiqueComponent';
import PostComponent from '../component/PostCompoent/PostComponent';
import PeopleIcon from '@mui/icons-material/People';
import ReviewsOutlinedIcon from '@mui/icons-material/ReviewsOutlined';
import WallpaperOutlinedIcon from '@mui/icons-material/WallpaperOutlined';
import axios from 'axios';
import { Edit, GroupAdd, PostAdd } from '@mui/icons-material';

function BookPage(props) {
  const location = useLocation();
  console.log(props, 'props');
  console.log(location, 'location')

  const [value, setValue] = React.useState('1');
  const [rate, setRating] = useState(0);
  const userId = sessionStorage.getItem("userId");
  const details = {
    isbn: location.state?.bookIsbn,
    title: location.state?.title,
    author: location.state?.author,
    publisher: location.state?.publisher,
    publishedDate: location.state?.publishedDate,
    buyLink: location.state?.buyLink,
    coverImage: location.state?.coverImage,
    rating: location.state?.rating,
    genre: location.state?.genre,
    desc: location.state?.desc,
  }
  const handleChange = (event, newValue) => {
    setValue(newValue);

  };

  const data = useMemo(() => ({
    ISBN: location.state?.bookIsbn,
    userId: userId,
    Rating: rate
  }), [rate]);

  const handleRate = (event, newValue) => {
    setRating(newValue);
    submitRating(newValue); // Pass the new value to submitRating()
  };

  const submitRating = async (newValue) => { // Accept the new value as a parameter
    const data = {
      ISBN: location.state?.bookIsbn,
      userId: userId,
      Rating: newValue // Use the new value here
    };

    const res = await axios.post("http://localhost:5278/api/Rating/AddRating", data);
    console.log(res.data?.[0]?.result?.result);
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
            {console.log(details.desc)}
            <Typography variant="body2" align='justify' style={{ wordBreak: 'break-word' }}>{details.desc}</Typography>
            <br />
            <br />

            <div className='rating-button'>
              <div className='button-container'>
                <Fab variant='extended' color='warning'><GroupAdd /> &nbsp;Create Community</Fab>
                <Fab variant='extended' color='info'><Edit />&nbsp; Write Critique</Fab>
                <Fab variant='extended' color='secondary'> <PostAdd /> Share a Post</Fab>
              </div>
              <div>
                <br />
                Add Your Rating:
                <br />
                <Rating
                  name="simple-controlled"
                  value={rate}
                  onChange={handleRate}
                  size="large"
                  align="center"
                  justifyContent="center"
                />

              </div>
            </div>
          </div>
        </div>
        <Box sx={{ width: '100%', typography: 'body1' }}>
          <TabContext value={value}>
            <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
              <TabList onChange={handleChange} aria-label=" API tabs example"
                textColor="secondary"
                indicatorColor="secondary" centered >
                <Tab icon={<PeopleIcon color='primary' />} label="Community" value="1" />
                <Tab icon={<ReviewsOutlinedIcon color='warning' />} label="Critique" value="2" />
                <Tab icon={<WallpaperOutlinedIcon color='info' />} label="Post" value="3" />
              </TabList>
            </Box>
            <TabPanel value="1">
              <CommunityComponent state={{ bookIsbn: details.isbn, title: details.title, author: details.author, publisher: details.publisher, publishedDate: details.publishedDate, buyLink: details.buyLink, coverImage: details.coverImage, rating: details.rating, genre: details.genre, desc: details.desc }} />
            </TabPanel>
            <TabPanel value="2">
              <CritiqueComponent state={{ bookIsbn: details.isbn, title: details.title, author: details.author, publisher: details.publisher, publishedDate: details.publishedDate, buyLink: details.buyLink, coverImage: details.coverImage, rating: details.rating, genre: details.genre, desc: details.desc }}/>
            </TabPanel>
            <TabPanel value="3">
              <PostComponent />
            </TabPanel>
          </TabContext>
        </Box>
      </div>
    </MainLayout>

  )
}

export default BookPage