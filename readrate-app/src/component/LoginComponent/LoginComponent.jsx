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

    const res = await axios.post("http://localhost:5278/User/Login", data);
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
      <div className='login-container'>

        <div className="card">
          <h3 className="card-heading">Login To Your Account</h3>
          <div className="card-body">
            <label className="label-body">Email:</label>
            <input
              className="input-box"
              type="email"
              style={{height: '35px', width: '300px'}}
              placeholder="Email"
              onChange={(e) => setUserEmail(e.target.value)}
            />

            <label className="label-body">Password:</label>
            <input
              className="input-box"
              type="password"
              style={{height: '35px', width: '300px'}}
              placeholder="Password"
              onChange={(e) => setPassword(e.target.value)}
            />
            <div className="button-container">
              <button className="button" onClick={handleLogin}>Login</button>
              <button className="button" onClick={(e) => setSignUp(true)}>SignUp</button>
            </div>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};
export default LoginComponent;