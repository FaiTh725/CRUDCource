import { useState } from "react";
import { useAuth } from "../../components/Auth/AuthContext";
import { useNavigate } from "react-router-dom";
import SimpleButton from "../../components/buttons/simple_button/SimpleButton";
import SimpleInput from "../../components/inputs/simple_input/Input";

import styles from "./AuthLogin.module.css"
import api from "../../api/axiosConf";
import decodeJWT from "../../services/JWTService";

const AuthLogin = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const [errorMessage, setErrorMessage] = useState("");

  const auth = useAuth();
  const navigate = useNavigate();

  const handleLogin = async () => {
    try
    {
      const response = await api.post("Authorize/Login", 
        {
          password: password,
          email: email
        }
      );

      if(response.data.statusCode === 0)
      {
        const jwtData = decodeJWT(response.data.data); 

        if(jwtData)
        {
          auth.login(jwtData);
          navigate("/");
        }
        else
        {
          setErrorMessage("Server error please contact admin");
        }
      }
      else
      {
        setErrorMessage(response.data.description);
      }
    }
    catch(error)
    {
      console.log(error.message);
    }

  }
  
  return (
    <div className={styles.AuthLogin__Main}>
      <div className={styles.AuthLogin__Wrapper}>
        <div className={styles.AuthLogin__Header}>
          <p>ACCOUNT LOGIN</p>
        </div>
        <div className={styles.AuthLogin__InputSection}>
          <SimpleInput labelName="EMAIL ADDRESS" action={setEmail}/>
        </div>
        <div className={styles.AuthLogin__InputSection}>
          <SimpleInput labelName="PASSWORD" typeInput="password" action={setPassword}/>
        </div>
        <div className={styles.AuthLogin__RestopePassword}>
          <p>Forgot your password?</p>
        </div>
        <div className={styles.AuthLogin__ErrorSection}>
          <p>{errorMessage}</p>
        </div>
        <div className={styles.AuthLogin__Action}>
          <SimpleButton name="SIGN IN" action={handleLogin}/>
        </div>
        <div className={styles.AuthLogin__NavigateRegister}>
          <p>
            Don't have an account yet? <a href="/account/register">Create Account</a>
          </p>
        </div>
      </div>
    </div>
  )
}

export default AuthLogin;