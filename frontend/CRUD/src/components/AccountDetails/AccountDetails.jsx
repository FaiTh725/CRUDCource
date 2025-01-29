import { useEffect, useState } from "react";
import styles from "./AccountDetails.module.css"
import { useAuth } from "../Auth/AuthContext";
import useLogout from "../../hooks/useLogOut";
import TextLink from "../links/TextLink/TextLink";
import SimpleButton from "../buttons/simple_button/SimpleButton";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { useNotification } from "../Notification/NotificationContext";

const AccountDetails = () => {
  const [accountName, setAccountName] = useState("");
  const [accountRole, setAccountRole] = useState("");
  
  const auth = useAuth();
  const logout = useLogout();
  const navigate = useNavigate();
  const notification = useNotification();

  const sendRequestToStartSell = async () => {
    try
    {
      var response = await axios.patch("https://localhost:5402/api/Account/ChangeRole", {
        email: auth.user.email,
        newRole: "Seller"
      }, {
        withCredentials: true
      });

      if(response.data.statusCode === 4)
      {
        console.log(response.data.description);
        return;
      }

      if(response.data.statusCode !== 0)
      {
        console.log(response.data.description);
        return;
      }

      notification("Request to change role sended");
    }
    catch (error)
    {
      if(error.status === 401)
      {
        logout();
      }
      console.log("Error send request to start sell");
    }
  }

  useEffect(() => {
    if(auth.user != null)
    {
      setAccountName(auth.user.name);
      setAccountRole(auth.user.role);
    }
    else
    {
      console.log("Error user credentials");
    }
  }, []);

  return (
    <div className={styles.AccountDetails__Main}>
      <div className={styles.AccountDetails__Header}>
        <p>ACCOUNT DETAILS</p>
      </div>
      <div  className={styles.AccountDetails__Body}>
        <div className={styles.AccountDetails__AccountName}>
          <p>Hello {accountName}!!</p>
          {(accountRole !== "User" && accountRole !== "") &&
            <p>You are {accountRole}</p>
          }
        </div>
        <div className={styles.AccountDetails__Action}>
          <TextLink text="Reset Password" url="#"/>
          {
            accountRole === "User" && 
            <SimpleButton name="Start Sell" action={sendRequestToStartSell}/>
          }
          {
            accountRole === "Seller" || accountRole === "Admin" && 
            <SimpleButton name="Open Disputs" action={() => {navigate("/disputs_cats")}}/>
          }
        </div>
      </div>
    </div>
  )
}

export default AccountDetails;