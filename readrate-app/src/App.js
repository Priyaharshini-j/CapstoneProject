import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import LoginComponent from './component/LoginComponent/LoginComponent';
import DashboardComponent from './component/DashboardComponent/DashboardComponent';

const App = () => {
  return (
    <Router>
      <Routes>
        <Route exact path="/" component={LoginComponent} element= {<LoginComponent/>}/>
        <Route path="/dashboard" component={DashboardComponent} element = {<DashboardComponent />} />
      </Routes>
    </Router>
  );
};

export default App;
