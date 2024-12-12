import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "./AuthContext";
import { useEffect, useState } from "react";
import Cookies from "js-cookie";
import decodeJWT from "../../services/JWTService";

const ProtectedRoutes = ({ roles }) => {
  const auth = useAuth();
  const [currentUser, setCurrentUser] = useState(undefined);

  useEffect(() => {
    const token = Cookies.get("token");

    if (token) {
      const decodeJwt = decodeJWT(token);
      auth.login(decodeJwt);
      setCurrentUser(decodeJwt);
    }
    else
    {
      setCurrentUser(null);
    }

  }, []);

  if (currentUser === undefined) {
    return <p>Loading</p>;
  }

  if (currentUser && roles.includes(currentUser.role)) 
  {
    return <Outlet />;
  } 
  else 
  {
    return <Navigate to="/account/login" />;
  }
};

export default ProtectedRoutes;
