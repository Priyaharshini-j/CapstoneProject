import React from 'react'
import './NavigationComponent.css'
import { Link, Routes, Route} from 'react-router-dom'
import DashboardComponent from '../DashboardComponent/DashboardComponent'
import CommunityComponent from '../CommunityComponent/CommunityComponent'
import CritiqueComponent from '../CritiqueComponent/CritiqueComponent'
import PostComponent from '../PostCompoent/PostComponent'
const NavigationComponent = () => {
  return (
    <React.Fragment>
      <div className='navigation'>
        <header class="heading-container-1">
          <div class="dot"></div><h3 class="heading">READ & RATE</h3>
        </header>
        <nav>
          <ul><Link className='link' to="/" >Home</Link></ul>
          <ul><Link className='link' to='/community'> Community</Link></ul>
          <ul><Link className='link' to='/critique'>Critique</Link></ul>
          <ul><Link className='link' to='/post'>Post</Link></ul>
        </nav>
      </div>
      <div className=''>
        <div className=''>

        </div>
      </div>
    </React.Fragment>
  )
}

export default NavigationComponent