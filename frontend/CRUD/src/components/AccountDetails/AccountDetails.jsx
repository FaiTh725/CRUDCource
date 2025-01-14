import { useEffect, useState } from "react";
import styles from "./AccountDetails.module.css"
import { useAuth } from "../Auth/AuthContext";
import useLogout from "../../hooks/useLogOut";
import TextLink from "../links/TextLink/TextLink";
import SimpleButton from "../buttons/simple_button/SimpleButton";
import MiniMessages from "../MiniMessages/MiniMessages";
import { useNavigate } from "react-router-dom";

const AccountDetails = () => {
  const [accountName, setAccountName] = useState("");
  const [accountRole, setAccountRole] = useState("");
  
  const [miniMessageIsActive, setMiniMessageIsActive] = useState(false);
  const [message, setMessage] = useState("");
  
  const auth = useAuth();
  const logout = useLogout();
  const navigate = useNavigate();

  //TODO use call back
  const sendRequestToStartSell = async () => {
    try
    {
      var response = await axios.patch("https://localhost:5202/api/Account/ChangeRole", {
        email: auth.user.email,
        newRole: "Seller"
      }, {
        withCredentials: true
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
      <MiniMessages message={message} isActive={miniMessageIsActive} setIsActive={setMiniMessageIsActive}/>
    </div>
  )
}

export default AccountDetails;