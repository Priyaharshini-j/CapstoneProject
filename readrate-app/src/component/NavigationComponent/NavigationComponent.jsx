import React, { useState } from 'react';
import './NavigationComponent.css';
import { Link } from 'react-router-dom';
import { Menu, MenuItem, MenuList, MenuButton, IconButton} from '@chakra-ui/react';
import { HamburgerIcon } from '@chakra-ui/icons';


const NavigationComponent = () => {
  const [isMenuOpen, setMenuOpen] = useState(false);
  const userName = sessionStorage.getItem("userName");

  const toggleMenu = () => {
    setMenuOpen(!isMenuOpen);
  };

  return (
    <React.Fragment>
      <div className='navigation'>
        <header className="heading-container-1">
          <div className="dot"></div>
          <h3 className="heading">READ & RATE</h3>
        </header>
        <nav className={isMenuOpen ? 'navigation-bar open' : 'navigation-bar'}>
          <ul className='link'><Link className='link' to="/">Home</Link></ul>
          <ul className='link'><Link className='link' to='/dashboard'>Dashboard</Link></ul>
          <ul className='link'><Link className='link' to='/community'>Community</Link></ul>
          <ul className='link'><Link className='link' to='/critique'>Critique</Link></ul>
          <ul className='link'><Link className='link' to='/post'>Post</Link></ul>
          <ul ><span>{userName}</span></ul>
        </nav>
        <div className="hamburger-menu">
          <Menu>
            <MenuButton
              onClick={toggleMenu}
              className={`menu-icon${isMenuOpen ? 'open' : ''}`}
              as={IconButton}
              aria-label='Options'
              icon={<HamburgerIcon />}
              variant='outline'
            />
            <MenuList>
              <MenuItem command='⌘T'>
                <Link className='link' to="/">Home</Link>
              </MenuItem>
              <MenuItem command='⌘N'>
                <Link className='link' to='/dashboard'>Dashboard</Link>
              </MenuItem>
              <MenuItem command='⌘⇧N'>
                <Link className='link' to='/community'>Community</Link>
              </MenuItem>
              <MenuItem command='⌘O'>
                <Link className='link' to='/critique'>Critique</Link>
              </MenuItem>
              <MenuItem command='⌘L'>
                <Link className='link' to='/post'>Post</Link>
              </MenuItem>
            </MenuList>
          </Menu>
        </div>
      </div>
    </React.Fragment>
  );
};

export default NavigationComponent;
