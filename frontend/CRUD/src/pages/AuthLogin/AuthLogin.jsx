import { useState } from "react";
import { useAuth } from "../../components/Auth/AuthContext";
import { useNavigate } from "react-router-dom";
import styles from "./AuthLogin.module.css"
import axios from "axios";
import SimpleInput from "../../components/inputs/simple_input/Input";
import TextLink from "../../components/links/TextLink/TextLink";
import SimpleButton from "../../components/buttons/simple_button/SimpleButton";

const AuthLogin = () => {
  const [loginForm, setLoginForm] = useState({
    email: "",
    password: ""
  });

  const [errorMessage, setErrorMessage] = useState("");

  const auth = useAuth();
  const navigate = useNavigate();

  const handleChangeForm = (e) => {
    const key = e.target.name;
    const newValue = e.target.value;

    setLoginForm(prev => ({
      ...prev,
      [key]: newValue
    }));
  }

  const handleLogin = async () => {
    try
    {
      const response = await axios.post("https://localhost:5402/api/Authorize/Login", 
        {
          password: loginForm.password,
          email: loginForm.email
        },
        {
          headers: {
            "Content-Type": "application/json"
          },
          withCredentials: true
        }
      );

      if(response.data.statusCode === 0)
      {
        localStorage.setItem("authData", JSON.stringify(response.data.data));
        auth.login(response.data.data);
        navigate("/");
      }
      else
      {
        setErrorMessage(response.data.description);
      }
    }
    catch(error)
    {
      console.log(error);
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
          <SimpleInput labelName="EMAIL ADDRESS" 
            name="email" 
            action={handleChangeForm}/>
        </div>
        <div className={styles.AuthLogin__InputSection}>
          <SimpleInput labelName="PASSWORD" 
          typeInput="password" 
          name="password" 
          action={handleChangeForm}/>
        </div>
        <div>
          <TextLink url="#" text="Forgot your password?"/>
        </div>
        <div className={styles.AuthLogin__ErrorSection}>
          <p>{errorMessage}</p>
        </div>
        <div className={styles.AuthLogin__Action}>
          <SimpleButton name="SIGN IN" action={handleLogin}/>
        </div>
        <div className={styles.AuthLogin__NavigateRegister}>
          <p>
            Don't have an account yet? <TextLink url="/account/register" text="Create Account"/>
          </p>
        </div>
      </div>
    </div>
  )
}

export default AuthLogin;