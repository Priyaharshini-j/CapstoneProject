import { CircularProgress, background } from '@chakra-ui/react';
import styled from '@emotion/styled';
import { Delete, DeleteOutlineOutlined, Favorite, FavoriteBorderOutlined, FavoriteOutlined, FavoriteRounded } from '@mui/icons-material';
import { Alert, AlertTitle, Avatar, Box, Card, CardContent, CardHeader, CardMedia, IconButton, Stack, Typography } from '@mui/material';
import axios from 'axios';
import React, { useEffect, useState } from 'react';
import { useLocation } from 'react-router';
import './PostComponent.css'
const PostComponent = (props) => {
  const location = useLocation();
  const userId = sessionStorage.getItem("userId");
  const userName = sessionStorage.getItem("userName");
  const book = {
    ISBN: location.state?.bookIsbn,
    BookName: location.state?.title,
    Genre: location.state?.genre,
    Author: location.state?.author,
    CoverUrl: location.state?.coverImage,
    BookDesc: location.state?.desc,
    Publisher: location.state?.publisher,
    PublishedDate: location.state?.publishedDate,
  };
  const [postList, setPost] = useState(null);
  const [isFavorite, setIsFavorite] = useState(false);

  const handleFavoriteClick = () => {
    setIsFavorite(!isFavorite);
  };
  useEffect(() => {
    async function fetchPost() {
      try {
        const postResult = await axios.post("http://localhost:5278/api/Post/GetPosts", book);
        setPost(postResult.data);
        console.log(postList);
        console.log(userId);
      } catch (error) {
        console.log(error);
      }
    }
    fetchPost();
  }, []);

  if (postList === null) {
    return <CircularProgress />;
  } else if (postList[0].postId === 0) {
    return (
      <Alert severity="info">
        <AlertTitle>Info</AlertTitle> No Post Found on this book...{' '}
        <strong>Be the first to be trending</strong>
      </Alert>
    );
  } else {
    return (
      <React.Fragment>
        <div className='Postcard-container' style={{ background: 'rgb(229 237 255)' }}>
          {postList.map((post) => {
            const pictureData = post.picture;
            const pictureUrl = `data:image/jpeg;base64,${pictureData}`;

            return (


              <Card key={post.postId} sx={{ maxWidth: 345, height:'auto', background: '#e3c0f1', '&:hover': { transform: 'scale(1.02)' }, alignItems: 'center', margin: '7px 0px', boxShadow: '1px 1px 5px 5px #ddd', justifyContent:'space-evenly' }}>
                <CardHeader
                  style={{ background: 'rgb(245 239 253)', borderRadius: '3px' }}
                  avatar={
                    <Avatar style={{ backgroundColor: 'rgb(134 75 227)' }} aria-label="recipe">
                      {post.userName[0]}
                    </Avatar>
                  }
                  title={post.userName}
                  subheader={new Date(post.createdDate).toLocaleDateString()}
                  action={
                    post.userName === userName? (<IconButton
                      aria-label="bookmark Bahamas Islands"
                      variant="plain"
                      color="neutral"
                      size="sm"
                      sx={{ position: 'relative', right: '0.5rem' }}
                    >
                      <DeleteOutlineOutlined color='error' />
                    </IconButton>): (<IconButton
                      aria-label="bookmark Bahamas Islands"
                      variant="plain"
                      
                      color="neutral"
                      size="sm"
                      style={{display:'none'}}
                      sx={{ position: 'absolute', right: '0.5rem',}}
                    >
                      <DeleteOutlineOutlined color='error' />
                    </IconButton>)}

                />
                

                <CardMedia
                  component="img"
                  style={{ objectFit: 'contain' }}
                  height=""
                  image={pictureUrl}
                  alt="Paella dish"
                />
                <CardContent style={{ borderRadius: '7px', background: '#fff' }}>
                  <Stack direction={'row'} spacing={1} style={{ alignItems: 'center', justifyContent: 'flex-start' }}>
                    <IconButton
                      aria-label="add to favorites"
                      onClick={handleFavoriteClick}
                    >
                      {isFavorite ? <Favorite color='error' /> : <FavoriteBorderOutlined color='error' />}<span>{post.like}</span>
                    </IconButton>
                    <Typography variant="body2" color="text.secondary" style={{ justifyContent: 'center', alignItems: 'items' }}>
                      {post.postCaption}
                    </Typography>
                  </Stack>
                </CardContent>
              </Card>
            );
          })}

        </div>
      </React.Fragment>
    );
  }
};

export default PostComponent;
