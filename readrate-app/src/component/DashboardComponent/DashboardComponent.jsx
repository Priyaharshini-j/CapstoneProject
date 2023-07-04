import React, { useState, useEffect } from 'react';
import './DashboardComponent.css';
import { Search2Icon } from '@chakra-ui/icons';
import axios from 'axios';
import { Card, CardMedia, Typography, Box, Stack } from '@mui/material';
import NavigationComponent from '../NavigationComponent/NavigationComponent';

function DashboardComponent() {
  const [searchTerm, setSearchTerm] = useState("");
  const [booksData, setBooksData] = useState(null);

  useEffect(() => {
    const fetchBooksData = async () => {
      try {
        const response = await axios.get('https://api.nytimes.com/svc/books/v3/lists/current/hardcover-fiction.json?api-key=vBHk5mexrXZmzZVrfb69Wbgfczq2iSaw');
        const books = response.data.results.books;

        const filteredBooksData = books.map(book => ({
          bookdesc: book.description,
          rank: book.rank,
          amazon_url: book.amazon_product_url,
          isbn: book.primary_isbn13,
          author: book.author,
          title: book.title,
          price: book.price,
          book_img: book.book_image,
          publisher: book.publisher
        }));

        setBooksData(filteredBooksData);
      } catch (error) {
        console.error(error);
      }
    };

    fetchBooksData();
  }, []);

  return (
    <React.Fragment>
      
      <NavigationComponent />
      <div className='dashboard-container'>
        <div className='searchBox'>
          <input className="searchInput" type="text" onChange={(e) => setSearchTerm(e.target.value)} name="" placeholder="Search" />
          <button className="searchButton" href="#">
            <Search2Icon boxSize={8} color='pink.600' />
          </button>
        </div>
        <div className='books-container'>
        <div className='best-selling-container'>
          {booksData !== null && booksData.map((book, index) => (
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
                height="124"
                alt={book.title}
                src={book.book_image}
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
                  {book.title}
                </Typography>
                <Typography
                  component="div"
                  variant="caption"
                  color="text.secondary"
                  fontWeight={500}
                  sx={{ textAlign: { xm: 'center', sm: 'start' } }}
                >
                  {`${book.publisher} • ${book.author}`}
                </Typography>
                <Stack
                  direction="row"
                  spacing={1}
                  sx={{
                    mt: 2,
                    justifyContent: { xs: 'space-between', sm: 'flex-start' },
                  }}
                >
                  <Typography variant="body2">Rank: {book.rank}</Typography>
                  <Typography variant="body2">Price: ${book.price}</Typography>
                </Stack>
              </Box>
            </Card>
          ))}
        </div>
        </div>
        
      </div>
    </React.Fragment>
  );
}

export default DashboardComponent;
