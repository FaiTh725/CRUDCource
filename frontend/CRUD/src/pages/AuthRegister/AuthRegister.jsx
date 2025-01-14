import { useEffect, useState } from "react";
import styles from "./AuthRegister.module.css"
import { useAuth } from "../../components/Auth/AuthContext";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import SimpleInput from "../../components/inputs/simple_input/Input";
import TextLink from "../../components/links/TextLink/TextLink";
import SimpleButton from "../../components/buttons/simple_button/SimpleButton";

const AuthRegister = () => {
  const [formRegister, setFormRegister] = useState({
    firstName: "",
    email: "",
    password: "",
    repeatPassword: ""
  });

  const [formError, setFormError] = useState({
    firstNameError: "",
    emailError: "",
    passwordError: "",
    repeatPasswordError: ""
  });

  const auth = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    setFormError({
      firstNameError: "",
      emailError: "",
      passwordError: "",
      repeatPasswordError: ""
    });
  }, [formRegister]);

  const handleFormChange = (e) => {
    const key = e.target.name;
    const newValue = e.target.value;

    setFormRegister(prev => ({
      ...prev,
      [key]: newValue
    }));
  }

  const handleChangeErrorForm = (key, newValue) => {
    setFormError(prev => ({
      ...prev,
      [key]: newValue
    }));
  }

  const handleRegister = async () => {
    let isValidForm = true;
    if(!/^[^@\s]+@[^@\s]+\.[^@\s]+$/.exec(formRegister.email))
    {
      handleChangeErrorForm("emailError", "Invalid Email");
      isValidForm = false;
    }
    if(!/^(?=.*[A-Za-z])(?=.*\d).+$/.exec(formRegister.password) || 
    formRegister.password.length < 5 || 
    formRegister.password.length > 20)
    {
      handleChangeErrorForm("passwordError", "Password should have 1 letter and character\n" + 
        "And has be in the ranпe from 5 to 20"
      );
      isValidForm = false;
    }
    if(formRegister.password !== formRegister.repeatPassword)
    {
      handleChangeErrorForm("repeatPasswordError", "Please enter the same passwords");
      isValidForm = false;
    }

    if(formRegister.firstName.trim() === "")
    {
      handleChangeErrorForm("firstNameError", "This Files Is Required");
      isValidForm = false;
    }

    if(!isValidForm)
    {
      return;
    }

    try
    {
      const response = await axios.post("https://localhost:5102/api/Authorize/Register", 
        {
          userName: formRegister.firstName,
          password: formRegister.password,
          email: formRegister.email
        },{
          withCredentials: true
        })
      
      if(response.data.statusCode === 0)
      {
        localStorage.setItem("authData", JSON.stringify(response.data.data));
        auth.login(response.data.data);
        navigate("/");
      }
      else
      {
        console.log(response.data.description);
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
          <SimpleInput labelName="FIRST NAME"
          name="firstName"
          action={handleFormChange}/>
          <span className={styles.AuthRegister__ErrorInput}>{formError.firstNameError}</span>
        </div>
        <div className={styles.AuthRegister__InputSection}>
          <SimpleInput labelName="EMAIL"
          name="email" 
          action={handleFormChange}/>
          <span className={styles.AuthRegister__ErrorInput}>{formError.emailError}</span>
        </div>
        <div className={styles.AuthRegister__InputSection}>
          <SimpleInput labelName="PASSWORD" 
          typeInput="password"
          name="password" 
          action={handleFormChange}/>
          <span className={styles.AuthRegister__ErrorInput}>{formError.passwordError}</span>
        </div>
        <div className={styles.AuthRegister__InputSection}>
          <SimpleInput labelName="REPEAT PASSWORD" 
          typeInput="password" 
          name="repeatPassword"
          action={handleFormChange}/>
          <span className={styles.AuthRegister__ErrorInput}>{formError.repeatPasswordError}</span>
        </div>
        <div className={styles.AuthRegister__InputSection}>
          <SimpleButton name="CREATE" action={() => handleRegister()}/>
        </div>
        <div className={styles.AuthRegister__NavigateRegister}>
          <p>
            Already have an account ? <TextLink url="/account/login" text="Sign in"/>
          </p>
        </div>
      </div>
    </div>
  )
}

export default AuthRegister;