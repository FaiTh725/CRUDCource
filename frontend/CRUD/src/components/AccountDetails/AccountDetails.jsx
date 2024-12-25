import { useEffect, useState } from "react";
import styles from "./AccountDetails.module.css"
import TextLink from "../links/TextLink/TextLink";
import { useAuth } from "../Auth/AuthContext";
import SimpleButton from "../buttons/simple_button/SimpleButton";
import Cookies from "js-cookie";
import axios from "axios";
import MiniMessages from "../MiniMessages/MiniMessages";

const AccountDetails = () => {
  const [accountName, setAccountName] = useState("");
  const [accountRole, setAccoiuntRole] = useState("");
  
  const [miniMessageIsActive, setMiniMessageIsActive] = useState(false);
  const [message, setMessage] = useState("");
  
  const auth = useAuth();

  const sendRequestToStartSell = async () => {
    try
    {
      var token = Cookies.get("token");
      var response = await axios.patch("https://localhost:7080/api/Account/ChangeRole", {
        email: auth.user.email,
        newRole: "Seller"
      }, {
        headers: {
          "Authorization": "Bearer " + token 
        }
      });

      if(response.data.statusCode === 4)
      {
        setMessage(response.data.description);
        setMiniMessageIsActive(true);
        return;
      }

      if(response.data.statusCode !== 0)
      {
        console.log(response.data.description);
        return;
      }

      setMessage("Send request to get permissions");
      setMiniMessageIsActive(true);
    }
    catch (error)
    {
      console.log(error);
      console.log("Error send request to start sell");
    }
  }

  useEffect(() => {
    if(auth.user != null)
    {
      console.log(auth.user);
      setAccountName(auth.user.name);
      setAccoiuntRole(auth.user.role);
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
          {(accountRole !== "User" || accountRole !== "") &&
            <p>You are {accountRole}</p>
          }
        </div>
        <div className={styles.AccountDetails__Action}>
          <TextLink text="Reset Password" url="#"/>
          {
            accountRole === "User" && 
            <SimpleButton name="Start Sell" action={sendRequestToStartSell}/>
          }
        </div>
      </div>
      <MiniMessages message={message} isActive={miniMessageIsActive} setIsActive={setMiniMessageIsActive}/>
    </div>
  )
}

export default AccountDetails;