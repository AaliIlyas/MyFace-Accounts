import React, {createContext, ReactNode, useState} from "react";

export const LoginContext = createContext({
    isLoggedIn: false,
    isAdmin: false,
    logIn: (basicString: string) => {},
    logOut: () => {},
    authString: ""
});

interface LoginManagerProps {
    children: ReactNode
}

export function LoginManager(props: LoginManagerProps): JSX.Element {
    const [loggedIn, setLoggedIn] = useState(false);
    const [admin, setAdmin] = useState(false);
    const [authString, setAuthString] = useState("");
    
    function logIn(basicString: string) {
        setAuthString(basicString);
        setLoggedIn(true);
    }
    
    function logOut() {
        setLoggedIn(false);
        setAuthString("");
    }
    
    const context = {
        isLoggedIn: loggedIn,
        isAdmin: false,
        logIn: logIn,
        logOut: logOut,
        authString: authString
    };
    
    return (
        <LoginContext.Provider value={context}>
            {props.children}
        </LoginContext.Provider>
    );
}