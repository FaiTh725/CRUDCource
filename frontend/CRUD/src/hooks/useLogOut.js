import { useNavigate } from "react-router-dom"
import { useAuth } from "../components/Auth/AuthContext";

function useLogout() {
  const auth = useAuth();
  const navigate = useNavigate();
  return () => {
    auth.logout();
    navigate("/account/login");
    return;
  }
}

export default useLogout;