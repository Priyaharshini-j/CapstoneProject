import React, { useState } from "react";
import axios from "axios";
import "./LoginComponent.css"
import { SignupComponent } from "../SignupComponent/SignupComponent";
import { useNavigate } from "react-router";

const LoginComponent = () => {
  const [UserEmail, setUserEmail] = useState("");
  const [Password, setPassword] = useState("");
  const [signUp, setSignUp] = useState(false);
  const [isLoggedIn, setIsLoggedIn] = useState(false);  
  const navigate = useNavigate();
  const handleLogin = async () => {
    const data = {
      userEmail: UserEmail,
      password: Password,
    };

    const res = await axios.post("https://readrate-app.azurewebsites.net/User/Login", data);
    console.log(res.data.result.result);
    if (res.data.result.result === true) {
      sessionStorage.setItem("userId", res.data.userId);
      sessionStorage.setItem("userName", res.data.userName);
      sessionStorage.setItem("userEmail", res.data.userEmail);
      setIsLoggedIn(true);
    }
    else {
      setIsLoggedIn(false);
      alert(res.data.result.message);
    }
  }

  if (isLoggedIn) {
    navigate('/dashboard')
  }

  if (signUp) {
    return <SignupComponent />;
  }

  return (
    <React.Fragment>
      <div class='login-container'>
        <div class="card">
          <h3 class="card-heading">Login To Your Account</h3>
          <div class="card-body">
            <label class="label-body">Email:</label>
            <input
              class="input-box"
              type="email"
              placeholder="Email"
              onChange={(e) => setUserEmail(e.target.value)}
            />

            <label class="label-body">Password:</label>
            <input
              class="input-box"
              type="password"
              placeholder="Password"
              onChange={(e) => setPassword(e.target.value)}
            />
            <div class="button-container">
              <button class="button" onClick={handleLogin}>Login</button>
              <button class="button" onClick={(e) => setSignUp(true)}>SignUp</button>
            </div>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};
export default LoginComponent;