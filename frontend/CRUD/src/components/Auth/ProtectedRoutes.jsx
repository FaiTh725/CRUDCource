import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "./AuthContext";
import { useEffect, useState } from "react";
import Loading from "../Loading/Loading";

const ProtectedRoutes = ({ roles }) => {
  const auth = useAuth();
  const [currentUser, setCurrentUser] = useState(undefined);

  useEffect(() => {
    const user = auth.user
    if (user != null) {
      setCurrentUser(user);
    }
    else
    {
      setCurrentUser(null);
    }

  }, []);

  if (currentUser === undefined) {
    return (
    <div style={{"width": "100vw", "height": "1000vh", "display": "flex", "justifyContent": "center", "alignItems": "center"}}>
      <Loading/>
    </div>
    );
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
