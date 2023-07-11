import { AddIcon } from '@chakra-ui/icons';
import { Card, CardBody, CardHeader, Heading, SimpleGrid, Text, color } from '@chakra-ui/react';
import { Fab } from '@mui/material'
import { Box, colors } from '@mui/material';
import axios from 'axios';
import React, { useEffect, useState } from 'react'
import { useLocation } from 'react-router-dom';
import '../CommunityComponent/CommunityComponent.css'
import { FavoriteRounded, HeartBroken, ReplyOutlined, SendOutlined } from '@mui/icons-material';
import Alert from '@mui/material/Alert';
import AlertTitle from '@mui/material/AlertTitle';
const CritiqueComponent = (props) => {
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
  const [replyListAlert, setReplyListAlert] = useState(null)
  const [critiqueReply, setCritiqueReply] = useState(null)
  const handleCritiqueReplyList = async (critiqueId) => {
    const criti_repl = await axios.post("http://localhost:5278/api/Critique/GetCritiqueReplyById", critiqueId);
    if (criti_repl.data.result.result === true) {
      setCritiqueReply(criti_repl.data.reply);
      setReplyListAlert(true);
    }
    else {
      setReplyListAlert(false);
    }

  }









  const [alert, setAlert] = useState(null);
  const handleLike = async (likeStatus, critiqueId) => {
    const likeData = {
      critiqueId: critiqueId,
      likeStatus: likeStatus,
      userId: sessionStorage.getItem("userId")
    }
    console.log(likeData);
    const result = await axios.post("http://localhost:5278/api/Critique/LikeDislikeCritique", likeData);
    console.log(result);
    if (result.data.result === true) {
      setAlert(true);
    } else {
      setAlert(false);
    }
  }
  const [critiqueList, setCritiqueList] = useState(null);
  useEffect(() => {
    async function fetchCritiqueList() {
      const res = await axios.post("http://localhost:5278/api/Critique/CritiqueList", book);
      setCritiqueList(res.data);
      console.log(res.data);

    }

    fetchCritiqueList();
  }, []); // Empty dependency array to run the effect only once on page load

  if (critiqueList === null) {
    return <div>Loading...</div>;
  } else if (critiqueList?.[0].critiqueId === 0) {
    return <div>No Critique Found</div>;
  } else {
    return (
      <React.Fragment>
        {alert === true && (
          <Alert severity="success">
            <AlertTitle>Success</AlertTitle>
            Liked the Critique— <strong>check it out by reloading the page!</strong>
          </Alert>
        )}
        {alert === false && (
          <Alert severity="error">
            <AlertTitle>Error</AlertTitle>
            This is an error alert — <strong>You have already Liked this Critique</strong>
          </Alert>
        )}
        <SimpleGrid spacing={4} templateColumns='repeat(auto-fill)' className='grid-container'>
          {critiqueList.map((critique) => (
            <Card
              key={critique.critiqueId}
              variant={"filled"}
              colorScheme='purple'
              style={{ outline: `1px solid ${colors.amber[200]}`, backgroundColor: "#c5bdf9", borderRadius: '5px' }}
            >
              <CardHeader bg="gray.100" p={4}>
                <Heading size='md'>@{critique.userName}</Heading>
              </CardHeader>
              <CardBody>
                <Text>{critique.critiqueDesc}</Text>
                <Box mt={2}>
                  <div className='community-Operation'>
                    <div className='community-details'>
                      <ul>
                        <li><p>Created By: {critique.userName}</p></li>
                        <li><p>Created On: {new Date(critique.createdDate).toLocaleDateString()}</p></li></ul>

                    </div>
                    <div className='button-Container'>
                      <Fab
                        variant='extended'
                        size='small'
                        color='#5BC0F8'
                        aria-label="LikeCritique"
                        onClick={() => handleLike(1, critique.critiqueId)}
                      >
                        <FavoriteRounded color='error' /></Fab>&nbsp;{critique.like}  Like
                      <br />
                      <br />
                      <Fab
                        variant='extended'
                        size='small'
                        color='#5BC0F8'
                        aria-label="DislikeCritique"
                        onClick={() => handleLike(-1, critique.critiqueId)}
                      >
                        <HeartBroken color='error' />
                      </Fab>&nbsp;{critique.dislike}  DisLike
                      <Fab variant='extended' size='small' color='' onClick={() => handleCritiqueReplyList(critique.critiqueId)}>
                        <ReplyOutlined />
                      </Fab> See Replies
                      <Fab variant='extended' size='small' color='' onClick={() => handleCritiqueReplyBox(critique.critiqueId)}>
                        <SendOutlined />
                      </Fab> Send a Reply
                    </div>
                    {replyListAlert === true && (
                      <Alert severity="success">
                        <AlertTitle>Success</AlertTitle>
                        Liked the Critique— <strong>check it out by reloading the page!</strong>
                      </Alert>
                    )}
                    {replyListAlert === false && (
                      <Alert severity="warning">
                        <AlertTitle>No Reply Found</AlertTitle>
                        <strong>NO reply is made under this critique- Be the first to add a Comment</strong>
                      </Alert>
                    )}
                    <div>
                      {
                        critiqueReply.map((reply) => (
                          <div key={reply.critiqueReplyId}>          
                            <p>{reply.reply}</p>
                            <span>Created By: @{reply.userName}</span>&nbsp;&nbsp;<span> Created On:{new Date(reply.createdDate).toLocaleDateString() }</span>
                          </div>
                        ))
                      }
                    </div>
                  </div>
                </Box>
              </CardBody>
            </Card>
          ))}
        </SimpleGrid>
      </React.Fragment >
    );
  }
};

export default CritiqueComponent;
