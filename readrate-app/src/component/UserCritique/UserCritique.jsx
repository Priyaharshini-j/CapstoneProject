import { AddIcon } from '@chakra-ui/icons';
import { Card, CardBody, CardFooter, CardHeader, Heading, SimpleGrid, Text, color } from '@chakra-ui/react';
import { Button, Divider, Fab, FormControl, Modal, TextareaAutosize } from '@mui/material'
import { Box, colors } from '@mui/material';
import axios from 'axios';
import React, { useEffect, useRef, useState } from 'react'
import { useLocation } from 'react-router-dom';
import '../CommunityComponent/CommunityComponent.css'
import { FavoriteRounded, HeartBroken, ReplyOutlined, SendOutlined } from '@mui/icons-material';
import Alert from '@mui/material/Alert';
import AlertTitle from '@mui/material/AlertTitle';
import CreateCritiqueComponent from '../CreateCritiqueComponent/CreateCritiqueComponent';

const UserCritique = () => {

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

  const [open, setOpen] = React.useState(false);
  const handleOpen = (critiqueId) => {
    setCritiqueId(critiqueId); // Set the critiqueId for sending the reply
    setOpen(true);
  };
  const handleClose = () => {
    setCritiqueId(null); // Clear the critiqueId when closing the modal
    setOpen(false);
  };
  const [ReplycritiqueId, setCritiqueId] = useState(null);
  const userId = parseInt(sessionStorage.getItem("userId"))
  const [alert, setAlert] = useState(null);
  const [critiqueList, setCritiqueList] = useState([]);
  const [replyAlert, setAlertReply] = useState(null);
  const [critiqueReplies, setCritiqueReplies] = useState({});
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
          setCriAlert(true);
          handleClose();
        } else {
          setCriAlert(false);
        }
        handleClose();
      } catch (error) {
        console.error("Error creating critique reply:", error);
      }
    }
  };


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

  useEffect(() => {
    async function fetchCritiqueList() {
      const data = {
        userId: userId
      }
      const res = await axios.post("http://localhost:5278/api/Critique/UsersCritiqueList", data);
      if(res.data[0].critiqueId === 0){
        setCritiqueList([]);
      }
      else{
        setCritiqueList(res.data);
      }
      console.log(res.data);
    }

    fetchCritiqueList();
  }, []);
  return (
    <React.Fragment>
      <SimpleGrid spacing={4} templateColumns="repeat(auto-fill)" className="grid-container">
        {critiqueList.map((critique) => (
          <Card
            key={critique.critiqueId}
            variant="filled"
            style={{ outline: `1px solid primary`, backgroundColor: "#c5bdf9", borderRadius: '5px' }}
          >
            <CardHeader bg="gray.100" p={4}>
              <Heading size="md">@{sessionStorage.getItem("userName")}</Heading>
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
                  <div>
                    {critiqueReplies[critique.critiqueId].map((reply) => (
                      <Card key={reply.critiqueReplyId} variant={'outline'} boxShadow={'2px 4px rgba(0,0,0,0.7)'} style={{ backgroundColor: "#EEEEEE", padding: '8px 10px', margin: '9px 0px 10px 20px', borderRadius: '7px'}}>
                        <CardBody><p style={{ fontSize: '20px' }}>{reply.reply}</p></CardBody>
                        <CardFooter><span>Created By: @{reply.userName}</span>&nbsp;&nbsp;
                          <span>Created On: {new Date(reply.createdDate).toLocaleDateString()}</span>
                        </CardFooter>
                        <Divider variant="middle" />
                      </Card>
                    ))}
                  </div>
                )}
                {replyAlert === false && (
                  <Alert severity="warning">
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
                  <TextareaAutosize minLength={10} minRows={2} variant='filled' placeholder='Write Critique Reply' required value={Reply}
                    onChange={(e) => setCriReply(e.target.value)} />
                </FormControl>
                <br />
                <Button variant='outlined' color='secondary' onClick={handleSubmit} >Save</Button>
                <Button variant='outlined' color='error' onClick={handleClose}>Cancel</Button>
              </Box>
            </Modal>

          </Card>
        ))}
      </SimpleGrid>
    </React.Fragment>
  )
}

export default UserCritique