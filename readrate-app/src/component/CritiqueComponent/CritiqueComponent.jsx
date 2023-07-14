import { Card, CardBody, CardFooter, CardHeader, Heading, SimpleGrid, Text, color } from '@chakra-ui/react';
import { Button, Divider, Fab, FormControl, IconButton, Modal, TextareaAutosize } from '@mui/material'
import { Box } from '@mui/material';
import axios from 'axios';
import React, { useEffect, useRef, useState } from 'react'
import { useLocation } from 'react-router-dom';
import '../CommunityComponent/CommunityComponent.css'
import { DeleteOutlineOutlined, FavoriteRounded, HeartBroken, ReplyOutlined, SendOutlined } from '@mui/icons-material';
import Alert from '@mui/material/Alert';
import AlertTitle from '@mui/material/AlertTitle';

const CritiqueComponent = (props) => {
  const location = useLocation();
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
  const book = {
    ISBN: location.state?.bookIsbn,
    BookName: location.state?.title,
    Genre: location.state?.genre,
    Author: location.state?.author,
    CoverUrl: location.state?.coverImage,
    BookDesc: location.state?.desc,
    Publisher: location.state?.publisher,
    PublishedDate: location.state?.publishedDate,
  };
  const userName = sessionStorage.getItem("userName");
  const [deleteAlert, setDeleteAlert] = useState(null);
  const handleDeleteCritique = async (critiqueId) => {
    console.log(critiqueId)
    console.log(sessionStorage.userId);
    try {
      const deleteData = {
        critiqueId: parseInt(critiqueId),
        userId: parseInt(sessionStorage.getItem("userId"))
      }
      const deleteResponse = await axios.delete("http://localhost:5278/api/Critique/DeleteReply", { data: deleteData });

      if (deleteResponse.data.result === true) {
        setDeleteAlert(true)
      }
      else {
        setDeleteAlert(false)
      }
    }
    catch (error) {
      console.log(error)
    }
  }









  const [open, setOpen] = React.useState(false);
  const [ReplycritiqueId, setCritiqueId] = useState(null); // Track the critiqueId for sending the reply
  const handleOpen = (critiqueId) => {
    setCritiqueId(critiqueId); // Set the critiqueId for sending the reply
    setOpen(true);
  };
  const handleClose = () => {
    setCritiqueId(null); // Clear the critiqueId when closing the modal
    setOpen(false);
  };
  const [criAlert, setCriAlert] = useState(null);
  const [Reply, setCriReply] = useState('');
  const handleSubmit = async () => {
    console.log(ReplycritiqueId);
    if (Reply !== '') {
      const replyData = {
        critiqueId: ReplycritiqueId,
        userId: sessionStorage.getItem("userId"),
        reply: Reply,
      };

      try {
        console.log(replyData);
        const response = await axios.post("http://localhost:5278/api/Critique/CreatingCritiqueReply", replyData);
        const data = response.data;
        if (data.result.result === true) {
          handleClose();
          setCriAlert(true);
        } else {
          setCriAlert(false);
        }
        handleClose();
      } catch (error) {
        console.error("Error creating critique reply:", error);
        setCriAlert(false);
      }
    }
  };

  const [alert, setAlert] = useState(null);
  const [critiqueList, setCritiqueList] = useState([]);
  const [replyAlert, setAlertReply] = useState(null);
  const [critiqueReplies, setCritiqueReplies] = useState({});

  const handleCritiqueReplyList = async (critiqueId) => {
    if (replyAlert === true || replyAlert === false) {
      setAlertReply(null);
    } else {
      const id = { critiqueId };
      const result = await axios.post("http://localhost:5278/api/Critique/GetCritiqueReplyById", id);
      console.log(result.data);
      if (result.data !== null) {
        console.log(result.data.reply);
        setAlertReply(true);
        setCritiqueReplies((prevReplies) => ({
          ...prevReplies,
          [critiqueId]: result.data.reply,
        }));
      } else {
        setAlertReply(false);
      }
    }
  };

  const handleLike = async (likeStatus, critiqueId) => {
    const likeData = {
      critiqueId: critiqueId,
      likeStatus: likeStatus,
      userId: sessionStorage.getItem("userId"),
    };
    const result = await axios.post("http://localhost:5278/api/Critique/LikeDislikeCritique", likeData);
    if (result.data.result === true) {
      setAlert(true);
    } else {
      setAlert(false);
    }
  };
  const [noCritiqueAlert, setNoCritique] = useState(null);
  useEffect(() => {
    async function fetchCritiqueList() {
      const res = await axios.post("http://localhost:5278/api/Critique/CritiqueList", book);
      console.log(res.data);
      if (res.data[0].critiqueId === 0) {
        setNoCritique(true);
      } else {
        setCritiqueList(res.data);
      }

    }

    fetchCritiqueList();
  }, [criAlert, alert,deleteAlert]); // Empty dependency array to run the effect only once on page load

  return (
    <React.Fragment>
      {deleteAlert === true && (
        <Alert onClose={() => { setDeleteAlert(null) }} severity="success">
          <AlertTitle>Success</AlertTitle>
          Deleted the Critique— <strong>check it out by reloading the page!</strong>
        </Alert>
      )}
      {deleteAlert === false && (
        <Alert onClose={() => { setDeleteAlert(null) }} severity="error">
          <AlertTitle>Error</AlertTitle>
          This is an error alert — <strong>Can't delete this critique</strong>
        </Alert>
      )}
      {criAlert === false && (
        <Alert onClose={() => { setCriAlert(null) }} severity="success">
          <AlertTitle>Success</AlertTitle>
          Replied to the Critique— <strong>check it out by reloading the page!</strong>
        </Alert>
      )}
      {criAlert === true && (
        <Alert onClose={() => { setCriAlert(null) }} severity="error">
          <AlertTitle>Error</AlertTitle>
          This is an error alert — <strong>Can't send the reply to this critique</strong>
        </Alert>
      )}
      {alert === true && (
        <Alert onClose={() => { setAlert(null) }} severity="success">
          <AlertTitle>Success</AlertTitle>
          Liked the Critique— <strong>check it out by reloading the page!</strong>
        </Alert>
      )}
      {alert === false && (
        <Alert onClose={() => { setAlert(null) }} severity="error">
          <AlertTitle>Error</AlertTitle>
          This is an error alert — <strong>You have already Liked this Critique</strong>
        </Alert>
      )}
      <SimpleGrid spacing={4} templateColumns="repeat(auto-fill)" className="grid-container">
        {
          noCritiqueAlert === true && (
            <Alert onClose={() => { setNoCritique(null) }} severity="info">
              <AlertTitle>Info</AlertTitle>
              No critique Found for this Book <strong>Be the first to state the facts about book...</strong>
            </Alert>
          )
        }
        {critiqueList.map((critique) => (
          <Card
            key={critique.critiqueId}
            variant="filled"
            style={{ outline: `1px solid primary`, backgroundColor: "#c5bdf9", borderRadius: '5px' }}
          >
            <CardHeader bg="gray.100" p={4} sx={{ display: 'flex', flexDirection: 'row', justifyContent: 'space-between' }}>
              <Heading size="md">@{critique.userName}
              </Heading>
              {
                critique.userName === userName ? (<IconButton
                  aria-label="bookmark Bahamas Islands"
                  variant="plain"
                  color="neutral"
                  size="sm"
                  onClick={() => handleDeleteCritique(critique.critiqueId)}
                  sx={{ position: 'relative', right: '0.5rem' }}
                >
                  <DeleteOutlineOutlined color='error' />
                </IconButton>) : (<IconButton
                  aria-label="bookmark Bahamas Islands"
                  variant="plain"

                  color="neutral"
                  size="sm"
                  style={{ display: 'none' }}
                  sx={{ position: 'absolute', right: '0.5rem', }}
                >
                  <DeleteOutlineOutlined color='error' />
                </IconButton>)}
            </CardHeader>
            <CardBody>
              <Text>{critique.critiqueDesc}</Text>
              <Box mt={2}>
                <div className="community-Operation">
                  <div className="community-details">
                    <ul>
                      <li>
                        <p>Created By: {critique.userName}</p>
                      </li>
                      <li>
                        <p>Created On: {new Date(critique.createdDate).toLocaleDateString()}</p>
                      </li>
                    </ul>
                  </div>
                  <div className="button-Container">
                    <Fab
                      variant="extended"
                      size="small"
                      color="#5BC0F8"
                      aria-label="LikeCritique"
                      onClick={() => handleLike(1, critique.critiqueId)}
                    >
                      <FavoriteRounded color="error" />
                    </Fab>
                    &nbsp;{critique.like} Like
                    {/* <Fab
                      variant="extended"
                      size="small"
                      color="#5BC0F8"
                      aria-label="DislikeCritique"
                      onClick={() => handleLike(-1, critique.critiqueId)}
                    >
                      <HeartBroken color="error" />
                    </Fab>*/}
                    <br />
                    <br />
                    <Fab
                      variant="extended"
                      size="small"
                      color=""
                      onClick={() => handleCritiqueReplyList(critique.critiqueId)}
                    >
                      <ReplyOutlined /> See Replies
                    </Fab>
                    <br />
                    <br />
                    <Fab variant="extended" size="small" color="" onClick={() => handleOpen(critique.critiqueId)}>
                      <SendOutlined /> Send Reply
                    </Fab>
                  </div>
                </div>
                {critiqueReplies[critique.critiqueId] && critiqueReplies[critique.critiqueId].length > 0 && (
                  <Box sx={{ justifyContent: 'center', alignItems: 'center', display: 'flex', flexDirection: 'column', flexWrap: 'wrap', borderRadius: '5px' }}>
                    {critiqueReplies[critique.critiqueId].map((reply) => (
                      <Card key={reply.critiqueReplyId} variant={'outline'} boxShadow={'2px 2px rgba(0,0,0,0.7)'} style={{ borderRadius: '5px', backgroundColor: "#EEEEEE", padding: '0px 10px', margin: '3px 0px 10px 20px', width: '450px', }}>
                        <CardBody><p style={{ fontSize: '17px', fontFamily: 'IBM Plex Sans' }}>{reply.reply}</p></CardBody>
                        <CardFooter style={{ fontSize: '14px' }}><span>Created By: @{reply.userName}</span>&nbsp;&nbsp;
                          <span style={{ fontSize: '14px' }}>Created On: {new Date(reply.createdDate).toLocaleDateString()}</span>
                        </CardFooter>
                        <Divider variant="middle" />
                      </Card>
                    ))}
                  </Box>
                )}
                {replyAlert === false && (
                  <Alert onClose={() => { setAlertReply(null) }} severity="warning">
                    <AlertTitle>No Reply Found</AlertTitle>
                    <strong>Be the First to Reply tp the Critique</strong>
                  </Alert>
                )}
              </Box>
            </CardBody>
            <Modal
              open={open}
              onClose={handleClose}
              aria-labelledby="modal-modal-title"
              aria-describedby="modal-modal-description"
            >
              <Box sx={style}>
                <h3>Share Your Comment</h3>
                <FormControl>
                  <TextareaAutosize style={{ width: '300px' }} minRows={7} variant='filled' placeholder='Write Critique Reply' required value={Reply}
                    onChange={(e) => setCriReply(e.target.value)} />
                </FormControl>
                <br />
                <Button variant='outlined' color='secondary' style={{ margin: '10px 20px' }} onClick={handleSubmit} >Save</Button>
                <Button variant='outlined' color='error' style={{ margin: '10px 20px' }} onClick={handleClose}>Cancel</Button>
              </Box>
            </Modal>

          </Card>
        ))}
      </SimpleGrid>
    </React.Fragment>
  );
};

export default CritiqueComponent;
