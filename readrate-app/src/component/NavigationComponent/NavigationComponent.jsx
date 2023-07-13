import React, { useState, useEffect } from 'react';
import './NavigationComponent.css';
import { Link, useNavigate } from 'react-router-dom';
import { Paper } from '@mui/material';
import { BottomNavigation, BottomNavigationAction } from '@mui/material';
import { Dashboard, Logout, PeopleAltTwoTone } from '@mui/icons-material';

const NavigationComponent = () => {
  const [value, setValue] = useState(0);
  const navigate = useNavigate();
  const [showPaper, setShowPaper] = useState(true);
  const userName = sessionStorage.getItem('userName');

  useEffect(() => {
    const handleResize = () => {
      const maxWidth = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
      setShowPaper(maxWidth <= 1000);
    };

    handleResize();
    window.addEventListener('resize', handleResize);

    return () => {
      window.removeEventListener('resize', handleResize);
    };
  }, []);

  useEffect(() => {
    if (value === 1) {
      navigate('/MyProfile');
    } else if (value === 2) {
      navigate('/dashboard');
    } else if (value === 3) {
      sessionStorage.clear();
      navigate('/');
    }
  }, [value, navigate]);

  return (
    <React.Fragment>
      <div className='navigation'>
        <header className="heading-container-1">
          <div className="dot"></div>
          <h3 className="heading">READ & RATE</h3>
        </header>
        <nav className={showPaper ? 'navigation-bar' : 'navigation-bar hidden'}>
          <ul className='link'>
            <Link className='link' to='/dashboard'>
              Dashboard
            </Link>
          </ul>
          {/* Add more navigation links here */}
          <ul className='link'>
            <Link className='link' to='/MyProfile'>
              My Profile
            </Link>
          </ul>
          <ul className='link'>
            <Link className='link' to="/">
              Log Out
            </Link>
          </ul>
          <ul>
            <span>@{userName}</span>
          </ul>
        </nav>
        {showPaper && (
          <Paper
            style={{
              display: 'block',
              maxWidth: '1000px',
              position: 'fixed',
              margin: '20px 0px 0px 0px',
              bottom: '0',
              left: '0',
              right: '0',
              elevation: 3,
            }}
            sx={{ position: 'fixed', bottom: 0, left: 0, right: 0 }}
            elevation={3}
          >
            <BottomNavigation showLabels value={value} onChange={(event, newValue) => setValue(newValue)}>
              <BottomNavigationAction label="Profile" icon={<PeopleAltTwoTone />} />
              <BottomNavigationAction label="Dashboard" icon={<Dashboard />} />
              <BottomNavigationAction label="LogOut" icon={<Logout />} />
            </BottomNavigation>
          </Paper>
        )}
      </div>
    </React.Fragment>
  );
};

export default NavigationComponent;
