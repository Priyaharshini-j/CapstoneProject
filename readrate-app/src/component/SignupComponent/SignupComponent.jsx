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
    if (password === confirmPassword) {
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
    }
    else{
      console.log("Wrong Password and Confirm Password");
    }
  }
  
  if (logIn) {
    return <LoginComponent />;
  }


  return (
    <React.Fragment>
      <div class="card">
        <h3 class="card-heading">Sign Up Your Account</h3>
        <div class="card-body">
          <label class="label-body">UserName:</label>
          <input
            class="input-box"
            type="text"
            placeholder="User Name"
            onChange={(e) => setUserName(e.target.value)}
            required
          />
          <label class="label-body">Email:</label>
          <input
            class="input-box"
            type="email"
            placeholder="Email"
            onChange={(e) => setUserEmail(e.target.value)}
            required
          />

          <label class="label-body">Password:</label>
          <input
            class="input-box"
            type="password"
            placeholder="Password"
            onChange={(e) => setPassword(e.target.value)}
            required
          />
          <label class="label-body">Confirm Password:</label>
          <input
            class="input-box"
            type="password"
            placeholder="Confirm Password"
            onChange={(e) => setConfirmPassword(e.target.value)}
            required
          />
          <label class="label-body">Security Question:</label>
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
          <label class="label-body">Security Answer:</label>
          <input
            class="input-box"
            type="text"
            placeholder="Secuirty Answer"
            onChange={(e) => setSecuirtyAns(e.target.value)}
            required
          />
          <div class="button-container">
            <button class="button" onClick={handleSignUp}>Sign Up</button>
            <button class="button" onClick= {(e) => setLogIn(true)}>Login... </button>
          </div>

        </div>

      </div>

    </React.Fragment>
  )
}
