import React, { useState, useEffect } from 'react';
import './SearchBarComponent.css';

import { Search2Icon } from '@chakra-ui/icons';
import axios from 'axios';
import { Card, CardBody, Heading, Image, Stack, Text } from '@chakra-ui/react';
import { CircularProgress, IconButton } from '@mui/material';
import { AddShoppingCart } from '@mui/icons-material';



const SearchBarComponent = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const [searchBook, setSearchBookData] = useState([]);
  const apiUrl = 'https://www.googleapis.com/books/v1/volumes';
  const apiKey = 'AIzaSyB3F3n7M4ArN6gtRzReJkAML_M7oauPtoo'; // Replace with your actual API key
  const handleChange = (e) => {
    setSearchTerm(e.target.value);
  };
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async () => {
    try {
      setIsLoading(true);
      const response = await axios.get(apiUrl, {
        params: {
          key: apiKey,
          q: searchTerm,
        },
      });
      const books = response.data.items;
      setSearchBookData(books);
    } catch (error) {
      console.log(error);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    if (searchTerm) {
      console.log(searchTerm);
      handleSubmit();
    }
  }, [searchTerm]);

  const handleRequest = (book) => {
    window.open(book.saleInfo.buyLink, '_blank');
  };

  return (
    <React.Fragment>
      <div className='search-component-container'>
        <div className="searchBox">
          <input className="searchInput" type="text" onChange={handleChange} placeholder="Search for books" />
          <button className="searchButton" onClick={handleSubmit}>
            <Search2Icon boxSize={9} />
          </button>
        </div>
        <div className="search-result-container">
          {isLoading ? (
            <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%' }}>
              <CircularProgress color="secondary" />
            </div>
          ) : (
            searchBook.length > 0 && (
              <div>
                <h2 class='search-term'>Search Term is '{searchTerm}'</h2>
                <br />
                <div className='search-container'>

                  {searchBook.map((book, index) => (
                    <Card css={{
                      backgroundColor: '#d1dcf7',
                      border: '0.5mm solid gray',
                      borderRadius: '5px'
                    }} direction={{ base: 'column', sm: 'row' }} overflow="hidden" variant="outline" key={index}>
                      <Image objectFit="cover" css={{ borderRadius: '4px' }} maxW={{ base: '100%', sm: '150px' }} src={book.volumeInfo.imageLinks?.thumbnail} alt="Book Cover" />
                      <Stack>
                        <CardBody>
                          <Heading size="md">{book.volumeInfo.title}</Heading>
                          <Text css={{
                            color: 'gray.500',
                            
                          }} py="2">{book.volumeInfo.authors?.[0]}</Text>
                          <Text py="2">{book.volumeInfo.categories?.[0]}</Text>
                          <IconButton css={{
                            color: 'primary',
                            cursor: 'pointer',
                            '&:hover': {
                              color: 'primary.500',
                            },
                          }} size='large' color="primary" variant='outline' aria-label="Buy Book" onClick={() => handleRequest(book)}>
                            <AddShoppingCart />
                          </IconButton>
                        </CardBody>
                      </Stack>
                    </Card>
                  ))}
                </div>
              </div>
            )

          )}
        </div>
      </div>
    </React.Fragment>
  );
};

export default SearchBarComponent;
