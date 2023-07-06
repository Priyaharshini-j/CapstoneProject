import axios from 'axios'
import React, { useState } from 'react'
import './SignupComponent.css'
import LoginComponent from '../LoginComponent/LoginComponent'

export const SignupComponent = () => {
  const [userName, setUserName] = useState("")
  const [userEmail, setUserEmail] = useState("")
  const [password, setPassword] = useState("")
  const [confirmPassword, setConfirmPassword] = useState("")
  const [securityQn, setSecuirtyQn] = useState("")
  const [securityAns, setSecuirtyAns] = useState("")
  const [logIn, setLogIn] = useState(false)

  const handleSignUp = async () => {
    if(userName !== null || userEmail !== null || password!== null || confirmPassword !== null || securityQn !== null ||securityAns !== null)
    {
      alert("Fill all the fields");
    }
    if (password === confirmPassword && password !== null && confirmPassword !== null) {
      const data = {
        userName: userName,
        userEmail: userEmail,
        password: password,
        securityQn: securityQn,
        securityAns: securityAns
      };
      console.log(data);
      const res = await axios.post("http://localhost:5278/User/SignUp", data);
      console.log(res.data.result);
      if(res.data.result === true){
        return(<LoginComponent/>)
      }
      else{
        alert(res.data.message);
      }
    }
    else {
      console.log("Wrong Password and Confirm Password");
      alert("Wrong Password and Confirm Password")
    }
  }

  if (logIn) {
    return <LoginComponent />;
  }



  return (
    <React.Fragment>
      <div className='signup-container'>
        <div className="card">
          <h3 className="card-heading">Sign Up Your Account</h3>
          <div className="card-body">
            <label className="label-body">UserName:</label>
            <input
              className="input-box"
              type="text"
              placeholder="User Name"
              onChange={(e) => setUserName(e.target.value)}
              required
            />
            <label className="label-body">Email:</label>
            <input
              className="input-box"
              type="email"
              placeholder="Email"
              onChange={(e) => setUserEmail(e.target.value)}
              required
            />

            <label className="label-body">Password:</label>
            <input
              className="input-box"
              type="password"
              placeholder="Password"
              onChange={(e) => setPassword(e.target.value)}
              required
            />
            <label className="label-body">Confirm Password:</label>
            <input
              className="input-box"
              type="password"
              placeholder="Confirm Password"
              onChange={(e) => setConfirmPassword(e.target.value)}
              required
            />
            <label className="label-body">Security Question:</label>
            <select
              id="security-question"
              className="input-box-select"
              onChange={(e) => setSecuirtyQn(e.target.value)}
              required
            >
              <option value="" disabled selected>Select a security question</option>
              <option value="What is your mother's maiden name?">What is your mother's maiden name?</option>
              <option value="What was the name of your first pet?">What was the name of your first pet?</option>
              <option value="What is your favorite book?">What is your favorite book?</option>
              <option value="In what city were you born?">In what city were you born?</option>
              <option value="What is the name of your best friend?">What is the name of your best friend?</option>
            </select>
            <label className="label-body">Security Answer:</label>
            <input
              className="input-box"
              type="text"
              placeholder="Secuirty Answer"
              onChange={(e) => setSecuirtyAns(e.target.value)}
              required
            />

            <div className="button-container">
              <button className="button" onClick={handleSignUp}>Sign Up</button>
              <button className="button" onClick={(e) => setLogIn(true)}>Login... </button>
            </div>
          </div>
        </div>
      </div>
    </React.Fragment>
  )
}
