import { useMemo } from "react";
import styles from "./Message.module.css";
import FormatDate from "../../services/FormatDate";
import { useAuth } from "../Auth/AuthContext";

const Message = ({message}) => {
  const auth = useAuth();

  const formatDate = useMemo(() => {
    return FormatDate(message.sendTime);
  }, [message]);

  const isOwnerMessage = useMemo(() => {
    return message.senderEmail == auth.user.email;
  }, []);

  return (
    <div className={styles.Message__Main} style={{textAlign: `${isOwnerMessage ? "end" : "start"}`}}>
      <div className={styles.Message__Wrapper}>
        <p className={styles.Message__Text}>{message.message}</p>
        <p className={styles.Message__SendTime}>{formatDate}</p>
      </div>
    </div>
  )
}

export default Message;