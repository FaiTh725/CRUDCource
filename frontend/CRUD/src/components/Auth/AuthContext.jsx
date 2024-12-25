import { useState } from "react";
import Cookies from "js-cookie";

import { createContext, useContext } from "react";

const AuthContext = createContext();

export const AuthProvider = ({children}) => {
  const [user, setUser] = useState(null);
  
  const login = (userData) => {
    setUser(userData);
  }
  
  const logout = () => {
    setUser(null);
    Cookies.set("token", "");
  }

  return (
    <AuthContext.Provider value={{user, login, logout}}>
      {children}
    </AuthContext.Provider>
  )
}

export const useAuth = () => useContext(AuthContext);
