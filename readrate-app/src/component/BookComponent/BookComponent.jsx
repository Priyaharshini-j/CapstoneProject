import { Heading, Text } from '@chakra-ui/react';
import { Rating } from '@mui/material';
import React from 'react';

const BookComponent = ({ book }) => {
  return (
    <React.Fragment>
      <div>
        <div className='book-holder'>
          <div className='image-container'>
            {book.volumeInfo.imageLinks?.thumbnail !== null && (
              <img
                src={book.volumeInfo.imageLinks?.thumbnail}
                alt={book.volumeInfo.title}
              />
            )}
          </div>
          <div className='details-container'>
            <Heading size="md">{book.volumeInfo.title}</Heading>
            <Text
              css={{
                color: 'gray.500',
              }}
              py="2"
            >
              {book.volumeInfo.authors?.[0]}
            </Text>
            <Text py="2">{book.volumeInfo.categories?.[0]}</Text>
            {console.log(book.volumeInfo.averageRating)}
            <Rating name="read-only" value={book.volumeInfo.averageRating} readOnly /> <span>&#91;{book.volumeInfo.ratingsCount} &#93;</span>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};

export default BookComponent;
