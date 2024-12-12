import { useEffect, useState } from "react";
import SimpleButton from "../../components/buttons/simple_button/SimpleButton";
import SimpleInput from "../../components/inputs/simple_input/Input";

import styles from "./AuthRegister.module.css"
import api from "../../api/axiosConf";
import decodeJWT from "../../services/JWTService";
import { useAuth } from "../../components/Auth/AuthContext";
import { useNavigate } from "react-router-dom";


const AuthRegister = () => {
  const [firstName, setFirstName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [repeatPassword, setRepeatPassword] = useState("");

  const [errorMessage, setErrorMessage] = useState("");

  const auth = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    if(errorMessage !== "")
    {
      setErrorMessage("");
    }
  }, [firstName, email, password, repeatPassword]);

  const handleRegister = async () => {
    let isValidForm = true;
    if(!/^[^@\s]+@[^@\s]+\.[^@\s]+$/.exec(email))
    {
      setErrorMessage(errorMessage + "\nInvalid Email");
      isValidForm = false;
    }
    if(!/^(?=.*[A-Za-z])(?=.*\d).+$/.exec(password) || 
      password.length < 5 || password.length > 20)
    {
      setErrorMessage(errorMessage + 
        "\nPassword should have 1 letter and character" +
        "\nAnd has be in the ranпe from 5 to 20"
      );
      isValidForm = false;
    }
    if(password !== repeatPassword)
    {
      setErrorMessage(errorMessage + 
        "\nPlease enter the same passwords"
      );
      isValidForm = false;
    }

    if(!isValidForm)
    {
      return;
    }

    try
    {
      const response = await api.post("/Authorize/Register", 
      {
        password: password,
        email: email
      })
      
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
      console.log(error);
    }
    
  }
  return (
    <div className={styles.AuthRegister__Main}>
      <div className={styles.AuthRegister__Wrapper}>
        <div className={styles.AuthRegister__Header}>
          <p>CREATE ACCOUNT</p>
        </div>
        <div className={styles.AuthRegister__InputSection}>
          <SimpleInput labelName="FIRST NAME" action={setFirstName}/>
        </div>
        <div className={styles.AuthRegister__InputSection}>
          <SimpleInput labelName="EMAIL" action={setEmail}/>
        </div>
        <div className={styles.AuthRegister__InputSection}>
          <SimpleInput labelName="PASSWORD" typeInput="password" action={setPassword}/>
        </div>
        <div className={styles.AuthRegister__InputSection}>
          <SimpleInput labelName="REPEAT PASSWORD" typeInput="password" action={setRepeatPassword}/>
        </div>
        <div className={styles.AuthRegister__ErrorSection}>
          <p>{errorMessage}</p>
        </div>
        <div className={styles.AuthRegister__InputSection}>
          <SimpleButton name="CREATE" action={() => handleRegister()}/>
        </div>
        <div className={styles.AuthRegister__NavigateRegister}>
          <p>
            Already have an account ? <a href="/account/login">Sign in</a>
          </p>
        </div>
      </div>
    </div>
  )
}

export default AuthRegister;