import React, { useState } from "react";
import axios from "axios";
import "./LoginComponent.css"
import { Card, CardBody, CardFooter, CardHeader, Heading, Link } from "@chakra-ui/react";
const LoginComponent = () => {
  const [UserEmail, setUserEmail] = useState("");
  const [Password, setPassword] = useState("");

  const handleLogin = async () => {
    const data = {
      userEmail: UserEmail,
      password: Password,
    };

    const res = await axios.post("http://localhost:5278/User/Login", data);
    console.log(res.data.result.result);
  }

  return (
    <React.Fragment>
      <Heading>READ & RATE</Heading>
      <Card>
        <CardHeader>Login To Your Account</CardHeader>
        <CardBody>
          <div>
            <input
              type="email"
              placeholder="Email"
              onChange={(e) => setUserEmail(e.target.value)}
            />
            <input
              type="password"
              placeholder="Password"
              onChange={(e) => setPassword(e.target.value)}
            />
            <button onClick={handleLogin}>Login</button>
          </div>
        </CardBody>
        <CardFooter>
          <Link>Forgot Password?</Link>
        </CardFooter>
      </Card>

    </React.Fragment>









  );
};
export default LoginComponent;
