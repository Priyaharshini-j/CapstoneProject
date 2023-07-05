import React from 'react'
import NavigationComponent from '../component/NavigationComponent/NavigationComponent'

const MainLayout = ({ children }) => {
  return (
    <React.Fragment>
      <NavigationComponent />
      <div>{children}</div>
    </React.Fragment>
  )
}

export default MainLayout