import React, { FormEvent, useContext, useState } from 'react';
import { Page } from "../Page/Page";
import { LoginContext, userDetailsContext } from "../../Components/LoginManager/LoginManager";
import "./Login.scss";

export function Login(): JSX.Element {
    const loginContext = useContext(LoginContext);
    const userDetails = useContext(userDetailsContext)

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    function tryLogin(event: FormEvent) {
        event.preventDefault();
        fetch('https://localhost:5001/login', { 
            method: 'post', 
            headers: new Headers({
              'Authorization': 'Basic '+btoa(`${username}:${password}`), 
              'Content-Type': 'application/x-www-form-urlencoded'
            }), 
            body: 'A=1&B=2'
          })
          .then(response => response.json())
          .then(response => {
              if (response.success == true) {
                  loginContext.logIn();
                  userDetails.username = username;
                  userDetails.password = password;
              } else {
                  setPassword("");
              }
          })
    }

    return (
        <Page containerClassName="login">
            <h1 className="title">Log In</h1>
            <form className="login-form" onSubmit={tryLogin}>
                <label className="form-label">
                    Username
                    <input className="form-input" type={"text"} value={username} onChange={event => setUsername(event.target.value)} />
                </label>

                <label className="form-label">
                    Password
                    <input className="form-input" type={"password"} value={password} onChange={event => setPassword(event.target.value)} />
                </label>

                <button className="submit-button" type="submit">Log In</button>
            </form>
        </Page>
    );
}