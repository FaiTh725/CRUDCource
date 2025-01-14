import { useEffect, useState } from "react";
import { createContext, useContext } from "react";
import Loading from "../Loading/Loading";

const AuthContext = createContext();

export const AuthProvider = ({children}) => {
  const [user, setUser] = useState(null);
  const [currentUser, setCurrentUser] = useState(undefined);
  
  const login = (userData) => {
    setUser(userData);
  }
  
  const logout = () => {
    setUser(null);
    localStorage.removeItem("authData");
  }

  useEffect(() => {
    try
    {
      const authData = JSON.parse(localStorage.getItem("authData"));
      if(authData)
      {
        setUser(authData);
        setCurrentUser(authData);
      }
      else
      {
        setUser(null);
        setCurrentUser(null);
      }
    }
    catch
    {
      console.log("parse localstorage error");
      setUser(null);
      setCurrentUser(null);
    }

  }, []);

  if(currentUser === undefined)
  {
    return (
      <div style={{"width": "100vw", "height": "100vh", "display": "flex", "justifyContent": "center", "alignItems": "center"}}>
        <Loading/>
      </div>
    )
  }
  else
  {
    return (
      <AuthContext.Provider value={{user, login, logout}}>
        {children}
      </AuthContext.Provider>
    )
  }
}

export const useAuth = () => useContext(AuthContext);
