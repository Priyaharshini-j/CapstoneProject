import React, { useState, useEffect } from 'react';
import axios from 'axios';
import CircularProgress from '@mui/material/CircularProgress'
import { Card, CardMedia, Typography, Box, Stack, IconButton } from '@mui/material';

import AddShoppingCartIcon from '@mui/icons-material/AddShoppingCart';
import './BestSellingComponent.css';

const BestSellingComponent = () => {
  const [booksData, setBooksData] = useState(null);  
  const [isLoading, setIsLoading] = useState(true); 

  useEffect(() => {
    const fetchBooksData = async () => {
      try {
        const response = await axios.get(
          'https://api.nytimes.com/svc/books/v3/lists/current/hardcover-fiction.json?api-key=vBHk5mexrXZmzZVrfb69Wbgfczq2iSaw'
        );
        const books = response.data.results.books;

        const filteredBooksData = books.map((book) => ({
          bookdesc: book.description,
          rank: book.rank,
          amazon_url: book.amazon_product_url,
          isbn: book.primary_isbn13,
          author: book.author,
          title: book.title,
          price: book.price,
          book_img: book.book_image,
          publisher: book.publisher,
        }));
        
        setBooksData(filteredBooksData);
        setIsLoading(false);
      } catch (error) {
        console.error(error);
      }
    };    
    fetchBooksData();
  }, []);

  const handleRequest = (book) => {
    window.open(book.amazon_url, '_blank');
  };

  return (
    <React.Fragment>
      <div className='head-best'>
        <h3>Best Selling Books</h3>
      </div>
      <div className="best-selling-container">
        {isLoading && <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%'}}><CircularProgress color='secondary' /></div>}
        {!isLoading && booksData !== null &&
          booksData.map((book, index) => (
            <Card
              key={index}
              variant="outlined"
              sx={{
                p: 1,
                display: 'flex',
                flexDirection: { xs: 'column', sm: 'row' },
              }}
            >
              <CardMedia
                component="img"
                width="124"
                height="auto"
                alt={book.title}
                src={book.book_img}
                sx={{
                  borderRadius: 0.5,
                  width: 'clamp(124px, (304px - 100%) * 999 , 100%)',
                }}
              />
              <Box sx={{ alignSelf: 'center', px: { xs: 0, sm: 2 } }}>
                <Typography
                  variant="body1"
                  color="text.primary"
                  fontWeight={600}
                  sx={{
                    textAlign: { xs: 'center', sm: 'start' },
                    mt: { xs: 1.5, sm: 0 },
                  }}
                >
                  {book.rank}. &nbsp;
                  {book.title} - By {`${book.author}`}
                </Typography>
                <Typography component="div" variant="caption" color="text.secondary">
                  • Publisher: {`${book.publisher}`}
                </Typography>
                <Stack
                  direction="row"
                  spacing={1}
                  sx={{
                    mt: 2,
                    justifyContent: { xs: 'space-between', sm: 'flex-start' },
                  }}
                >
                  <Typography variant="body2">Book Description: {book.bookdesc}</Typography>
                </Stack>
                <br />
                <Typography variant="body2">ISBN: {book.isbn}</Typography>
                <Typography variant="body3">
                  <br />

                  <IconButton color="primary" aria-label="Buy Book" onClick={() => handleRequest(book)}>
                    <AddShoppingCartIcon />
                  </IconButton>
                </Typography>
              </Box>
            </Card>
          ))}
      </div>
    </React.Fragment>
  );
};

export default BestSellingComponent;
