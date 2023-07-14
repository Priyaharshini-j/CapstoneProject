import { AddIcon } from '@chakra-ui/icons';
import { Card, CardBody, CardHeader, Heading, SimpleGrid, Text, color } from '@chakra-ui/react';
import {Alert, AlertTitle, CircularProgress, Fab} from '@mui/material'
import { Box, colors } from '@mui/material';
import axios from 'axios';
import React, { useEffect, useState } from 'react'
import { useLocation } from 'react-router-dom';
import './CommunityComponent.css'


const CommunityComponent = (props) => {
  const location = useLocation();
  const bookDetails = {
    isbn: location.state?.bookIsbn,
    bookName: location.state?.title,
    genre: location.state?.genre,
    author: location.state?.author,
    coverImage: location.state?.coverImage,
    bookDesc: location.state?.desc,
    publisher: location.state?.publisher,
    publishedDate: location.state?.publishedDate,
    buyLink: location.state?.buyLink,
    rating: location.state?.rating,
  }
  const book = {
    ISBN: location.state?.bookIsbn,
    BookName: location.state?.title,
    Genre: location.state?.genre,
    Author: location.state?.author,
    CoverUrl: location.state?.coverImage,
    BookDesc: location.state?.desc,
    Publisher: location.state?.publisher,
    PublishedDate: location.state?.publishedDate,
  }
  const [alert, setAlert] = useState(null);
  const handleAdd= async(communityId)=>{
    const addMemberData = {
      communityId: communityId,
      userId: sessionStorage.getItem("userId")
    }
    const addResult = await axios.post("http://localhost:5278/Book/AddMember",addMemberData);
    console.log(addResult);
    if (addResult.data.result === true) {
      setAlert(true);
    } else {
      setAlert(false);
    }
  }
  const [communityList, setCommunityList] = useState(null);
  console.log(bookDetails)
  useEffect(() => {
    async function fetchCommunityList() {
      const res = await axios.post("http://localhost:5278/Book/CommunityList", book);
      setCommunityList(res.data);
    }
    fetchCommunityList();
  }, [alert]); // Empty dependency array to run the effect only once on page load

  if (communityList === null) {
    return <CircularProgress/>;
  } else if (communityList[0].communityId === 0) {
    return (<Alert onClose={()=>{setAlert(null)}} severity='info'><AlertTitle>Info</AlertTitle> No Community Found on this book... <strong>Be the first to create your turf</strong></Alert>);
  } else {
    return (
      <React.Fragment>
        {alert === true && (
          <Alert onClose={()=>{setAlert(null)}} severity="success">
            <AlertTitle>Success</AlertTitle>
            Successfully Followed the Community <strong>check it out by reloading the page!</strong>
          </Alert>
        )}
        {alert === false && (
          <Alert onClose={()=>{setAlert(null)}} severity="error">
            <AlertTitle>Error</AlertTitle>
            This is an error alert — <strong>You were already a member of this community</strong>
          </Alert>
        )}
        <SimpleGrid spacing={4} templateColumns='repeat(auto-fill)' className='grid-container'>
          {communityList.map((community) => (
            <Card
              key={community.communityId}
              variant={"filled"}
              colorScheme='purple'
              style={{ outline: `1px solid ${colors.amber[200]}`, backgroundColor: "#c5bdf9", borderRadius: '5px' }}
            >
              <CardHeader bg="gray.100" p={4}>
                <Heading size='md'>{community.communityName}</Heading>
              </CardHeader>
              <CardBody>
                <Text>{community.communityDesc}</Text>
                <Box mt={2}>
                  <div className='community-Operation'>
                    <div className='community-details'>
                      <ul><li><p>TotalMembers: {community.communityMembers}</p></li>
                        <li><p>Created By: {community.communityAdmin}</p></li>
                        <li><p>Created On: {new Date(community.createdDate).toLocaleDateString()}</p></li></ul>
                    </div>
                    <div className='button-Container'>
                    <Fab variant='extended' color='#5BC0F8' aria-label="addMember" onClick={() => handleAdd(community.communityId)}><AddIcon /> Follow</Fab>                   
                    </div>
                  </div>

                </Box>
                <p style={{ display: 'none' }}>{community.communityId}</p>
                <p style={{ display: 'none' }}>{community.bookId}</p>
                <p style={{ display: 'none' }}>Created By: {community.communityAdmin}</p>
              </CardBody>
            </Card>
          ))}
        </SimpleGrid>
      </React.Fragment >
    );
  }
};

export default CommunityComponent;
