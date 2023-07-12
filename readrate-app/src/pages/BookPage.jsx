import React, { useMemo, useState } from 'react';
import { useLocation } from 'react-router-dom';
import MainLayout from '../layout/MainLayout';
import { Alert, AlertTitle, Button, Fab, FormControl, Rating, TextField, TextareaAutosize, Typography } from '@mui/material';
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
import { AutoStoriesOutlined, Edit, GroupAdd, PostAdd } from '@mui/icons-material';
import Modal from '@mui/material/Modal';
import MenuItem from '@mui/material/MenuItem';
import Select from '@mui/material/Select';


const style = {
  position: 'absolute',
  top: '50%',
  left: '50%',
  transform: 'translate(-50%, -50%)',
  width: 400,
  bgcolor: 'rgba(0,0.5,0.5,1)',
  border: '2px solid #000',
  boxShadow: 24,
  color: 'white',
  justifyContent: 'center',
  align: 'center',
  textAlign: 'center',
  p: 4
};


function BookPage(props) {
  const location = useLocation();
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
  const [alert, setAlert] = useState(null);
  const submitRating = async (newValue) => { // Accept the new value as a parameter
    const data = {
      ISBN: location.state?.bookIsbn,
      userId: userId,
      Rating: newValue // Use the new value here
    };

    const res = await axios.post("http://localhost:5278/api/Rating/AddRating", data);
    console.log(res.data.result.result === true);
    console.log("its is the rating");
    if (res.data.result.result === true) {
      setAlert(true);
    }
    else {
      setAlert(false);
    }
  };

  const [commOpen, setCommunityOpen] = React.useState(false);
  const handleCommunityOpen = () => setCommunityOpen(true);
  const handleCommunityClose = () => setCommunityOpen(false);
  const [communityName, setCommunityName] = useState('');
  const [communityDesc, setCommunityDesc] = useState('');
  const [commAlert, setCommAlert] = useState(null);
  const handleCreateCommunity = async () => {
    const communityData = {
      isbn: location.state?.bookIsbn,
      userId: userId,
      communityName: communityName,
      communityDesc: communityDesc
    }
    if (communityName !== null || communityDesc !== null) {
      const commRes = await axios.post("http://localhost:5278/Book/CreateCommunity", communityData);
      
      if (commRes.data.result.result === true) {
        setCommAlert(true);
        setTimeout(() => {
          window.location.reload();
        }, 1000);
      }
      else {
        setCommAlert(false);
      }
    }
    handleCommunityClose();
  }

  const [open, setOpen] = React.useState(false);
  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);
  const [criAlert, setCriAlert] = useState(null);
  const [critiqueDesc, setCritiqueDesc] = useState('');
  const handleSubmit = () => {
    const data = {
      ISBN: location.state?.bookIsbn,
      userId: userId,
      critiqueDesc: critiqueDesc,
    };
    if (critiqueDesc !== '') {
      axios.post("http://localhost:5278/api/Critique/CreatingCritique", data);
      setCriAlert(true);
      handleClose();
    }
    else{
      setCriAlert(false);
    }
    handleClose();
  };


  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  const handleBookShelf = () => {
    setIsDropdownOpen(!isDropdownOpen);
  };
  const [ShelfReading, setShelfReading] = useState('');
  const [ShelfAlert, setShelfAlert] = useState(null);
  const handleAddingBook = async (shelf) => {
    const shelfData = {
      book: {
        isbn: location.state?.bookIsbn,
        bookName: location.state?.title,        
        genre: location.state?.genre,
        author: location.state?.author,        
        coverUrl: location.state?.coverImage,
        bookDesc: location.state?.desc,
        publisher: location.state?.publisher,
        publishedDate: location.state?.publishedDate,
      },
      userId: userId,
      bookShelfName: shelf
    }
    const bookResult = await axios.post("http://localhost:5278/Book/AddingBook", shelfData)
    setIsDropdownOpen(false);
    if (bookResult.data.result === true) {
      setShelfAlert(true);
    }
    else {
      setShelfAlert(false);
    }
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
              Author: {details.author}
            </Text>
            <Text py="2">Genre: {details.genre}</Text>
            <Rating name="read-only" value={details.rating} size="small" readOnly />
            <Typography variant="body2" align='justify' style={{ wordBreak: 'break-word' }}>{details.desc}</Typography>
            <br />
            <br />
            <div className='rating-button'>
              <div className='button-container'>
                <Modal
                  open={commOpen}
                  onClose={handleCommunityClose}
                  aria-labelledby="modal-modal-title"
                  aria-describedby="modal-modal-description"
                >
                  <Box sx={style}>
                    <h3>Meet Your People By Creating a Community</h3>
                    <FormControl sx={{ margin: '10px' }}>
                      <TextField
                        id="communityName"
                        sx={{
                          bgcolor: 'white',
                          color: 'black',
                          borderRadius: '5px',
                          marginBottom: '20px',
                        }}
                        variant='outlined'
                        placeholder='Name Your Community'
                        onChange={(e) => setCommunityName(e.target.value)}
                      />
                      <TextareaAutosize sx={{ bgcolor: 'white', color: 'black', borderRadius: '5px', width: '380' }} minRows={4} variant='filled' placeholder='Make people to understand about your community' required value={communityDesc}
                        onChange={(e) => setCommunityDesc(e.target.value)} />
                    </FormControl>
                    <br />
                    <Button variant='outlined' color='secondary' onClick={handleCreateCommunity} >Save</Button> &nbsp; &nbsp;
                    <Button variant='outlined' color='error' onClick={handleCommunityClose}>Cancel</Button>
                  </Box>
                </Modal>
                <Fab variant='extended' color='warning' onClick={handleCommunityOpen}><GroupAdd /> &nbsp;Create Community</Fab>
                <Modal
                  open={open}
                  onClose={handleClose}
                  aria-labelledby="modal-modal-title"
                  aria-describedby="modal-modal-description"
                >
                  <Box sx={style}>
                    <h3>Share Your Thoughts</h3>
                    <FormControl>
                      <TextareaAutosize minLength={10} minRows={2} variant='filled' placeholder='Write the Critique' required value={critiqueDesc}
                        onChange={(e) => setCritiqueDesc(e.target.value)} />
                    </FormControl>
                    <br />
                    <Button variant='outlined' color='secondary' onClick={handleSubmit} >Save</Button>
                    <Button variant='outlined' color='error' onClick={handleClose}>Cancel</Button>
                  </Box>
                </Modal>
                <Fab variant='extended' color='info' onClick={handleOpen} ><Edit />&nbsp; Write Critique</Fab>
                {/* 
                <Fab variant='extended' color='secondary'> <PostAdd /> Share a Post</Fab> */}
                <Fab variant='extended' color='error' onClick={handleBookShelf}><AutoStoriesOutlined />Add to Shelf {isDropdownOpen ? "Close" : "Open"}</Fab>

                {isDropdownOpen && (
                  <Select value={ShelfReading} onChange={(event) => {
                    const value = event.target.value;
                    setShelfReading(value);
                    handleAddingBook(value); // Call handleAddingBook without passing any parameter
                  }}
                  
                  >
                    <MenuItem value={"Need To Read"}>Need To Read</MenuItem>
                    <MenuItem value={"Reading"}>Reading</MenuItem>
                    <MenuItem value={"Already Read"}>Already Read</MenuItem>
                  </Select>
                )}
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
              {
                ShelfAlert === true && (
                  <Alert severity="success">
                    <AlertTitle>Success</AlertTitle>
                    Successfully Added the Book to Your shelf <strong>check it out in Your Profile Page!</strong>
                  </Alert>
                )}
              {ShelfAlert === false && (
                <Alert severity="error">
                  <AlertTitle>Error</AlertTitle>
                  This is an error alert — <strong>You were already Added the Book to your shelf. Check it out in the Profile page</strong>
                </Alert>
              )
              }
              {alert === true && (
                <Alert severity="success">
                  <AlertTitle>Success</AlertTitle>
                  Successfully Submitted the the Ratings <strong>check it out in Your Profile Page!</strong>
                </Alert>
              )}
              {alert === false && (
                <Alert severity="error">
                  <AlertTitle>Error</AlertTitle>
                  This is an error alert — <strong>You were already submitted the Ratin. Check it out in the Profile page</strong>
                </Alert>
              )}
            </div>
            <div>
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
                {/* <Tab icon={<WallpaperOutlinedIcon color='info' />} label="Post" value="3" />*/}
                
              </TabList>
            </Box>
            <TabPanel value="1">
              {commAlert === true && (
                <Alert severity="success">
                  <AlertTitle>Success</AlertTitle>
                  Successfully Created the community <strong>check it out in Your Profile Page!</strong>
                </Alert>
              )}
              {commAlert === false && (
                <Alert severity="error">
                  <AlertTitle>Error</AlertTitle>
                  This is an error alert — <strong> There is an error occured while creating a community... Please Try again :&#40;</strong>
                </Alert>
              )}
              <CommunityComponent state={{ bookIsbn: details.isbn, title: details.title, author: details.author, publisher: details.publisher, publishedDate: details.publishedDate, buyLink: details.buyLink, coverImage: details.coverImage, rating: details.rating, genre: details.genre, desc: details.desc }} />
            </TabPanel>
            <TabPanel value="2">
            {criAlert === true && (
                <Alert severity="success">
                  <AlertTitle>Success</AlertTitle>
                  Successfully Created the Critique <strong>check it out in Your Profile Page!</strong>
                </Alert>
              )}
              {criAlert === false && (
                <Alert severity="error">
                  <AlertTitle>Error</AlertTitle>
                  This is an error alert — <strong> There is an error occured while creating a Critique... Please Try again :&#40;</strong>
                </Alert>
              )}
              <CritiqueComponent state={{ bookIsbn: details.isbn, title: details.title, author: details.author, publisher: details.publisher, publishedDate: details.publishedDate, buyLink: details.buyLink, coverImage: details.coverImage, rating: details.rating, genre: details.genre, desc: details.desc }} />
            </TabPanel>
            {/*
            <TabPanel value="3">
              <PostComponent />
            </TabPanel>
            */}
            
          </TabContext>
        </Box>
      </div>
    </MainLayout >

  )
}

export default BookPage