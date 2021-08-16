import React, { FormEvent, useContext, useState } from 'react';
import { Page } from "../Page/Page";
import { LoginContext } from "../../Components/LoginManager/LoginManager";
import "./Login.scss";

export function Login(): JSX.Element {
    const loginContext = useContext(LoginContext);

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    function tryLogin(event: FormEvent) {
        console.log(username)
        console.log(password)
        event.preventDefault();

//         fetch('https://example.com/path', {method:'GET', 
// headers: {'Authorization': 'Basic ' + btoa('login:password')}})

        fetch('https://localhost:5001/login', { 
            method: 'GET', 
            headers: new Headers({
              'Authorization': 'Basic '+ btoa(`${username}:${password}`), 
              'Content-Type': 'application/json'
            }), 
            //body: 'A=1&B=2'
          })
          .then(response => response.json())
          .then(response => {
              if (response.success == true) {
                  loginContext.logIn();
                  loginContext.btoaString = 'Basic ' + btoa(`${username}:${password}`);
              } else {
                  setPassword("");
              }
          }).catch(error => {
              console.log(error)
            });
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