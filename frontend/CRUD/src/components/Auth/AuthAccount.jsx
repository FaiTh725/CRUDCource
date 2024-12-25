import { useEffect, useState } from "react";
import { useAuth } from "./AuthContext"
import Cookies from "js-cookie";
import decodeJWT from "../../services/JWTService";
import { Outlet } from "react-router-dom";
import Loading from "../Loading/Loading";

const AuthAcccount = () => {
  const auth = useAuth();
  const [currentUser, setCurrentUser] = useState(null);
  
  useEffect(() => {
    const token = Cookies.get("token");
    
    if(token)
    {
      var tokenData = decodeJWT(token);

      if(tokenData !== null)
      {
        setCurrentUser(tokenData);
        auth.login(tokenData);
      }
    }
  }, [])

  if (currentUser === null) {
    return (
    <div style={{"width": "100vw", "height": "1000vh", "display": "flex", "justifyContent": "center", "alignItems": "center"}}>
      <Loading/>
    </div>
    );
  }
  
  if(currentUser)
  {
    return (
      <Outlet/>
    )
  }
}

export default AuthAcccount;