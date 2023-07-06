import React from 'react';
import { useLocation } from 'react-router-dom';
import MainLayout from '../layout/MainLayout';
import { Rating, Typography, colors } from '@mui/material';
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
    desc: location.state?.desc,
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
            {console.log(details.desc)}
            <Typography variant="body2">{details.desc}</Typography>
          </div>
        </div>
        <Box sx={{ width: '100%', typography: 'body1' }}>
          <TabContext value={value}>
            <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
              <TabList onChange={handleChange} aria-label=" API tabs example"
                textColor="secondary"
                indicatorColor="secondary" centered >
                <Tab icon={<PeopleIcon color='primary'/>} label="Community" value="1" />
                <Tab icon={<ReviewsOutlinedIcon color='warning'/>} label="Critique" value="2" />
                <Tab icon={<WallpaperOutlinedIcon color='info'/> } label="Post" value="3" />
              </TabList>
            </Box>
            <TabPanel value="1">
              <CommunityComponent/>
            </TabPanel>
            <TabPanel value="2">
              <CritiqueComponent />
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