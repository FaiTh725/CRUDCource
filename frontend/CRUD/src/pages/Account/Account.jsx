import AccountDetails from "../../components/AccountDetails/AccountDetails";
import AccountOrderHistory from "../../components/AccountOrderHistory/AccountOrderHistory";
import HeaderAccount from "../../components/HeaderAccount/HeaderAccount";
import styles from "./Account.module.css";

const Account = () => {
  return (
    <>
      <HeaderAccount/>
      <div className={styles.Account__Body}>
        <AccountOrderHistory/>
        <AccountDetails/>
      </div>
    </>
  )
}

export default Account;