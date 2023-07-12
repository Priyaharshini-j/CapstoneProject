import React from 'react'
import Accordion from '@mui/material/Accordion';
import AccordionSummary from '@mui/material/AccordionSummary';
import AccordionDetails from '@mui/material/AccordionDetails';
import Typography from '@mui/material/Typography';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import MainLayout from '../layout/MainLayout'
import UserComponent from '../component/UserComponent/UserComponent'
import UserCritique from '../component/UserCritique/UserCritique'
import UserRating from '../component/UserRating/UserRating';
import UserBookShelf from '../component/UserBookShelf/UserBookShelf';
const ProfilePage = () => {
  return (
    <MainLayout>
      <UserComponent />
      <Accordion>
        <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          aria-controls="panel1a-content"
          id="panel1a-header" 
        ><Typography>User Critique</Typography>
        </AccordionSummary>
        <AccordionDetails>
          <UserCritique />
        </AccordionDetails>
      </Accordion>
      <Accordion>
        <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          aria-controls="panel1a-content"
          id="panel1a-header" 
          style={{backgroundColor:"warning"}}
        ><Typography>User Rating</Typography>
        </AccordionSummary>
        <AccordionDetails>
         <UserRating />
        </AccordionDetails>
      </Accordion>
      <Accordion>
        <AccordionSummary
          expandIcon={<ExpandMoreIcon />}
          aria-controls="panel1a-content"
          id="panel1a-header" 
          style={{backgroundColor:"warning"}}
        ><Typography>User Shelf</Typography>
        </AccordionSummary>
        <AccordionDetails>
         <UserBookShelf />
        </AccordionDetails>
      </Accordion>
    </MainLayout>
  )
}

export default ProfilePage