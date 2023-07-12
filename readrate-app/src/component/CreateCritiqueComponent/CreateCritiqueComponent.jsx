import { Box, Button, FormControl, Modal, TextareaAutosize } from '@mui/material';
import axios from 'axios';
import React, { useState } from 'react'
import { useLocation } from 'react-router';


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

const CreateCritiqueComponent = (props) => {
  const location = useLocation();
  const critiqueId = location.state?.critiqueId;
  console.log(critiqueId);
  const userId = sessionStorage.getItem("userId");
  const [open, setOpen] = React.useState(false);
  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);
  const [criAlert, setCriAlert] = useState(null);
  const [Reply, setCriReply] = useState('');
  const handleSubmit = (critiqueId) => {
    const Reply_data = {
      critiqueId: critiqueId,
      userId: userId,
      critiqueReply: Reply,
    };
    if(Reply !== '') {
      axios.post("http://localhost:5278/api/Critique/CreatingCritiqueReply", Reply_data);
      setCriAlert(true);
      handleClose();
    }
    else {
      setCriAlert(false);
    }
    handleClose();
  };

  return (
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
  )
}

export default CreateCritiqueComponent