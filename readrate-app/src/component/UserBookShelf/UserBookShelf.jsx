import React, { useEffect, useState } from 'react';
import { Heading, Text } from '@chakra-ui/react';
import { Alert, AlertTitle, Fab, Rating } from '@mui/material';
import axios from 'axios';
import { DeleteForeverOutlined } from '@mui/icons-material';

const UserBookShelf = () => {
  const userId = parseInt(sessionStorage.getItem('userId'));
  const [readingModal, setReadingModal] = useState([]);
  const [deleteStatus, setDeleteStatus] = useState(null);
  const [bookId, setBookId] = useState(0);
  const [shelfId, setShelfId] = useState(0);
  const [shelfName, setShelfName] = useState('');
  const [alert, setAlert] = useState(null);

  const handleRemoveBook = (bookShelfId, bookId, bookShelfName) => {
    setBookId(bookId);
    setShelfId(bookShelfId);
    setShelfName(bookShelfName);
    setDeleteStatus(true);
  };

  useEffect(() => {
    async function deleteBook() {
      const data = {
        bookShelfId: shelfId,
        userId: userId,
        bookId: bookId,
        readingStatus: shelfName,
      };

      try {
        const res = await axios.delete('http://localhost:5278/Book/RemoveBook', { data });
        console.log(res.data);
        setAlert(true);
      } catch (error) {
        setAlert(false);
        console.log(error);
      }
    }

    if (deleteStatus) {
      deleteBook();
    }
  }, [deleteStatus, userId, bookId, shelfId, shelfName]);

  useEffect(() => {
    async function fetchUserShelf() {
      try {
        const res = await axios.post('http://localhost:5278/Book/ListBookInShelf', { userId });
        setReadingModal(res.data);
      } catch (error) {
        console.log(error);
      }
    }

    fetchUserShelf();
  }, [userId]);

  const getBookHolderColor = (bookShelfName) => {
    switch (bookShelfName) {
      case 'Need To Read':
        return '#A8D8EA'; // Set the desired background color for 'Currently Reading'
      case 'Reading':
        return '#AA96DA'; // Set the desired background color for 'To Read'
      case 'Already Read':
        return '#FCBAD3'; // Set the desired background color for 'Read'
      default:
        return 'gray'; // Set the default background color for other cases
    }
  };

  return (
    <React.Fragment>{alert === true && (
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
      {readingModal.length === 0 ? (
        <p>No Book Added to Shelf</p>
      ) : (
        readingModal.map((rate) => (
          <div
            className='book-holder-1'
            key={rate.bookShelfId}
            style={{ margin: '10px 10px', backgroundColor: getBookHolderColor(rate.bookShelfName) }}
          >
            <div className='image-container'>
              {rate.book.coverUrl !== null && <img className='image-holder' src={rate.book.coverUrl} alt={rate.book.author} />}
            </div>
            <div className='details-container'>
              <Heading size='md'>{rate.book.bookName}</Heading>
              <Text css={{ color: 'gray.500' }} py='2'>
                Author: {rate.book.author}
                Publisher: {rate.book.publisher}
              </Text>
              <Text py='2'>Genre: {rate.book.genre}</Text>
              <Text py='2'>Reading Status: {rate.bookShelfName}</Text>
              <Rating name='read-only' value={rate.book.rating} size='small' readOnly />

            </div>
            <Fab variant='extended' color='error' onClick={() => handleRemoveBook(rate.bookShelfId, rate.book.bookId, rate.bookShelfName)}>
              <DeleteForeverOutlined />
              &nbsp; Remove Book
            </Fab>
          </div>
        ))
      )}
    </React.Fragment>
  );
};

export default UserBookShelf;
