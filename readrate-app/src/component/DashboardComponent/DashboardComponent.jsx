import React from 'react';
import './DashboardComponent.css';
import SearchBarComponent from '../SearchBarComponent/SearchBarComponent';
import BestSellingComponent from '../BestSellingComponent/BestSellingComponent';

function DashboardComponent() { 

  return (
    <React.Fragment>
      <div className='dashboard-container'>
        <SearchBarComponent />
        <div className='books-container'>
          <BestSellingComponent/>
          </div>
      </div>
    </React.Fragment>
  );
}

export default DashboardComponent;
