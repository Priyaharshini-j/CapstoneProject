import React, { useEffect, useState } from 'react';
import { Heading, Text } from '@chakra-ui/react';
import { Rating } from '@mui/material';
import axios from 'axios';
import { Alert, AlertTitle, Fab } from '@mui/material';
import { DeleteForeverOutlined } from '@mui/icons-material';


const UserRating = () => {
  const userId = parseInt(sessionStorage.getItem('userId'));
  const [ratingModal, setRatingModal] = useState([]);
  const [alert, setAlert] = useState(null);
  const [bookId, setBookId] = useState(0);
  const [deleteStatus, setDeleteStatus] = useState(null);
  const handleDeleteRating = (bookId) => {
    setBookId(bookId);
    setDeleteStatus(true);
  };
  useEffect(() => {
    async function deleteRating() {
      const data = {
        userId: userId,
        bookId: bookId,
      };
      try {
        const res = await axios.delete('http://localhost:5278/api/Rating/DeleteRating', { data });
        console.log(res.data);
        setAlert(true);
      } catch (error) {
        setAlert(false);
        console.log(error);
      }
    }
    if (deleteStatus) {
      deleteRating();
    }
  }, [deleteStatus, userId, bookId]);



  useEffect(() => {
    async function fetchUserRating() {
      try {
        const res = await axios.post('http://localhost:5278/api/Rating/UsersRating', { userId });
        setRatingModal(res.data);
      } catch (error) {
        console.log(error);
      }
    }

    fetchUserRating();
  }, [userId]);

  return (
    <React.Fragment>
      {alert === true && (
        <Alert severity="success">
          <AlertTitle>Success</AlertTitle>
          <strong>Removed the Book from Shelf...check it out by reloading the page!</strong>
        </Alert>
      )}
      {alert === false && (
        <Alert severity="error">
          <AlertTitle>Error</AlertTitle>
          This is an error alert — <strong>Error removing the book from Shelf</strong>
        </Alert>
      )}
      {ratingModal.length === 0 ? (
        <p>No ratings found.</p>
      ) : (
        ratingModal.map((rate) => (
          <div className='book-holder-1' key={rate.ratingId}>
            <div className='image-container'>
              {rate.bookDetails.coverUrl !== null && (
                <img className='image-holder' src={rate.bookDetails.coverUrl} alt={rate.bookDetails.author} />
              )}
            </div>
           
            <div className='details-container'>
              <Heading size='md'>{rate.bookDetails.bookName}</Heading>
              <Text css={{ color: 'gray.500' }} py='2'>
                Author: {rate.bookDetails.author}
              </Text>
              <Text py='2'>Genre: {rate.bookDetails.genre}</Text>
              <Rating name='read-only' value={rate.rating} size='small' readOnly />
            </div>
            <Fab variant='extended' color='error' onClick={() => handleDeleteRating(rate.bookId)}>
              <DeleteForeverOutlined /> Delete Rating
              &nbsp;
            </Fab>
          </div>
        ))
      )}
    </React.Fragment>
  );
};

export default UserRating;
