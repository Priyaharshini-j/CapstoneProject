import React, { useState } from 'react';
import { DeleteForeverOutlined, Edit, Password } from '@mui/icons-material';
import { Alert, AlertTitle, Box, Button, Fab, FormControl, Modal } from '@mui/material';
import './UserComponent.css'
import avatar1 from '../images/woman.png'
import avatar3 from '../images/man1.png'
import avatar4 from '../images/man2.png'
import avatar2 from '../images/female2.png'

import axios from 'axios';
import { useNavigate } from 'react-router';




const UserComponent = () => {
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
  const avatarSources = [
    avatar1,
    avatar2,
    avatar3,
    avatar4,
    // Add more image sources as needed
  ];
  const getRandomAvatarIndex = () => {
    return Math.floor(Math.random() * avatarSources.length);
  };
  const randomAvatarIndex = getRandomAvatarIndex();
  const randomAvatarSrc = avatarSources[randomAvatarIndex];


  const userId = sessionStorage.getItem("userId");
  const navigate = useNavigate();
  const [deleteAlert, setDeleteAlert] = useState(null);
  const handleDeleteProfile = async () => {
    const re = await axios.delete("http://localhost:5278/User/DeleteProfile", {
      data: { userId: userId },
      headers: { 'Content-Type': 'application/json' }
    });

    if (re.data.result === true) {
      sessionStorage.clear();
      navigate('/');
    }
    else {
      setDeleteAlert(false);
    }
  }
  const [open, setOpen] = React.useState(false);
  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);
  const [password, setPassword] = useState('');
  const [securityQn, setsecQN] = useState('');
  const [securityAns, setsecAns] = useState('');
  const [updateAlert, setUpdateAlert] = useState(null);
  const handleSubmit = async () => {
    const updateUserId = parseInt(sessionStorage.getItem("userId"))
    const data = {
      userId: updateUserId,
      password: password,
      securityQn: securityQn,
      securityAns: securityAns
    };
    console.log(data);
    if (data !== '') {
      const response = await axios.put("http://localhost:5278/User/UpdateProfile", data);
      console.log(response);
      if (response.data.result === true) {
        setUpdateAlert(true);
      }
      else {
        setUpdateAlert(false);
        handleClose();
      }
    }
    else {
      setUpdateAlert(false);
    }
    handleClose();
  };

  return (
    <React.Fragment>
      <div className='user-container'>
        <div className='image-container'>
          <img src={randomAvatarSrc} alt='Avatar' width={'200px'} border={'0.25mm dashed #000'} />
        </div>

        <div>
          <div className='user-details-container'>
            <h2>@{sessionStorage.getItem("userName")}</h2>
            <h4>{sessionStorage.getItem("userEmail")}</h4>

          </div>
          <div className='button-contain'>

            <Fab variant='extended' color='secondary' onClick={handleOpen} ><Edit />&nbsp; Update Profile</Fab>

            <Fab variant='extended' color='error' onClick={() => { handleDeleteProfile() }}><DeleteForeverOutlined />&nbsp; Delete Profile</Fab>
          </div>
        </div>
        <Modal
          open={open}
          onClose={handleClose}
          aria-labelledby="modal-modal-title"
          aria-describedby="modal-modal-description"
        >
          <Box sx={style}>
            <h3>Update Profile</h3>
            <FormControl>
              <label className="label-body">Password:</label>
              <input
                className="input-box"
                type="password"
                placeholder="Password"
                onChange={(e) => setPassword(e.target.value)}
                required
              />

              <label className="label-body">Security Question:</label>
              <select
                id="security-question"
                className="input-box-select"
                onChange={(e) => setsecQN(e.target.value)}
                required
              >
                <option value="" disabled selected>Select a security question</option>
                <option value="What is your mother's maiden name?">What is your mother's maiden name?</option>
                <option value="What is your pet name?">What is your pet name?</option>
                <option value="What is your favorite book?">What is your favorite book?</option>
                <option value="In what city were you born?">In what city were you born?</option>
                <option value="What is the name of your best friend?">What is the name of your best friend?</option>
                <option value="What is your favorite color?">What is your favorite color?</option>
              </select>
              <label className="label-body">Security Answer:</label>
              <input
                className="input-box"
                type="text"
                placeholder="Secuirty Answer"
                onChange={(e) => setsecAns(e.target.value)}
                required
              />
            </FormControl>
            <br />
            <Button variant='outlined' color='secondary' onClick={handleSubmit} >Save</Button>
            <Button variant='outlined' color='error' onClick={handleClose}>Cancel</Button>
          </Box>
        </Modal>
        {deleteAlert === false && (
          <Alert severity="error">
            <AlertTitle>Error</AlertTitle>
            This is an error alert — <strong>Error Deleting the Profile</strong>
          </Alert>
        )}
        {
          updateAlert === true && (
            <Alert severity="success">
              <AlertTitle>Success</AlertTitle>
              <strong>Profile Updated Successfully</strong>
            </Alert>
          )
        }
        {updateAlert === false && (
          <Alert severity="error">
            <AlertTitle>Error</AlertTitle>
            This is an error alert — <strong>Error Updating Profile</strong>
          </Alert>
        )}
      </div>
    </React.Fragment >
  );
};

export default UserComponent;
