import { Box, Button, FormControl, Modal } from '@mui/material';
import axios from 'axios';
import React, { useState } from 'react';
import '../CreatePost/CreatePost.css'

const CreatePost = (props) => {

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
  const default_isbn = props.isbn;
  const openStatus = props.openStatus
  const [open, setOpen] = React.useState(openStatus);
  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);
  const [postCaption, setPostCaption] = useState('');
  const [picture, setPicture] = useState(null);

  const [userId, setUserId] = useState(sessionStorage.getItem("userId") || '');
  const [isbn, setIsbn] = useState(default_isbn || '');

  const handlePostCaptionChange = (event) => {
    setPostCaption(event.target.value);
  };

  const handlePictureChange = (event) => {
    setPicture(event.target.files[0]);
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    console.log(default_isbn);
    const formData = new FormData();
    formData.append('postCaption', postCaption);
    formData.append('picture', picture);
    formData.append('isbn', isbn);
    formData.append('userId', userId);

    try {
      const res = await axios.post('http://localhost:5278/api/Post/CreatePost', formData, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      });
      console.log(res.data);
      if (res.data.result === true) {
        handleClose();
      }
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <Modal
      open={open}
      onClose={handleClose}
      aria-labelledby="modal-modal-title"
      aria-describedby="modal-modal-description"
    >
      <Box sx={style}>
        <h3>Share Your Thoughts</h3>
        <form onSubmit={handleSubmit} >
          <div>
            <div className='form-container'>
              <div>
                <label htmlFor='userId'>UserId</label>
                <input
                  type='text'
                  id='userId'
                  value={userId}
                  disabled
                />
              </div>
              <div>
              <label htmlFor='isbn'>ISBN</label>
              <input
                type='text'
                id='isbn'
                value={isbn}
                disabled
              />
              </div>
              
              <div>
                <label htmlFor="postCaption">Post Caption:</label>
                <input
                  type="text"
                  id="postCaption"
                  value={postCaption}
                  onChange={handlePostCaptionChange}
                />
              </div>
             
              <br />
              <br />
              <br />
            </div><br />
            <div>
              <label htmlFor="picture">Picture:</label>
              <input
                type="file"
                id="picture"
                accept="image/*"
                onChange={handlePictureChange}
              />
            </div>
            <button type="submit">Create Post</button>
          </div>

        </form>
        <br />
        <Button variant='outlined' color='secondary' onClick={handleSubmit} >Save</Button>
        <Button variant='outlined' color='error' onClick={handleClose}>Cancel</Button>
      </Box>
    </Modal>

  );
};

export default CreatePost;
