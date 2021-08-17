import React, {createContext, ReactNode, useState} from "react";

export const LoginContext = createContext({
    isLoggedIn: false,
    isAdmin: false,
    logIn: (basicString: string) => {},
    logOut: () => {},
    btoaString: ""
});

interface LoginManagerProps {
    children: ReactNode
}

export function LoginManager(props: LoginManagerProps): JSX.Element {
    const [loggedIn, setLoggedIn] = useState(false);
    const [admin, setAdmin] = useState(false);
    const [btoaString, setBtoaString] = useState("")
    
    function logIn(basicString: string) {
        setLoggedIn(true);
        setBtoaString(basicString);
    }
    
    function logOut() {
        setLoggedIn(false);
        setBtoaString("");
    }
    
    const context = {
        isLoggedIn: loggedIn,
        isAdmin: false,
        logIn: logIn,
        logOut: logOut,
        btoaString: btoaString
    };
    
    return (
        <LoginContext.Provider value={context}>
            {props.children}
        </LoginContext.Provider>
    );
}