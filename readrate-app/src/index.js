import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import {
  createBrowserRouter,
  RouterProvider,
} from "react-router-dom";
import reportWebVitals from './reportWebVitals';
import LoginPage from './pages/LoginPage';
import DashboardPage from './pages/DashboardPage';
import CommunityComponent from './component/CommunityComponent/CommunityComponent';
import CritiqueComponent from './component/CritiqueComponent/CritiqueComponent';
import PostComponent from './component/PostCompoent/PostComponent';
import BookPage from './pages/BookPage';
import ProfilePage from './pages/ProfilePage';

const router = createBrowserRouter([
  {
    path: '/',
    element: <LoginPage />
  },
  {
    path: '/dashboard',
    element: <DashboardPage />
  },
  {
    path: '/community',
    element: <CommunityComponent />
  },
  {
    path: '/critique',
    element: <CritiqueComponent />
  },
  {
    path: '/MyProfile',
    element: <ProfilePage />
  },
  {
    path: '/book',
    element: <BookPage />
  }
])

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
