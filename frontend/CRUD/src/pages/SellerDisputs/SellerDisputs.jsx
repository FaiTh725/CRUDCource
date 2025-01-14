import { useMemo, useState } from "react";
import { useSignalR } from "../../components/SignalR/SignalRContext";
import styles from "./SellerDisputs.module.css"
import ChatRoomCart from "../../components/ChatRoomCart/ChatRoomCart";
import ChatMessages from "../../components/ChatMessages/ChatMessages";
import CompareMessages from "../../services/CompareMessages";

const SellerDisputs = () => {
  const [selectedChatId, setSelectedChatId] = useState(null); 
  const connection = useSignalR();

  const chats = connection.sellerChats;

  const selectedChat = useMemo(() => {
    if(chats)
    {
      return chats
      .find(chat => chat.id == selectedChatId);
    }
    return null;
  }, [selectedChatId, chats]);

  const messages = useMemo(() => {
    if(chats && selectedChat)
    {
      return [...selectedChat
      .messages
      .sort(CompareMessages)];
    }
    else
    {
      return [];
    }
  }, [selectedChat, chats]);

  const handleExecuteCloseDisput = (chatId) => {
    connection.handleCloseSellerDispute(chatId);

    if(chatId == selectedChatId)
    {
      setSelectedChatId(null);
    }
  }

  const handleOpenChat = (chat) => {
    setSelectedChatId(chat.id);
  }

  return (
    <div className={styles.SellerDisputs__Main}>
      <div className={styles.SellerDisputs__Chats}>
        {
          chats && chats.map(chat => (
            <ChatRoomCart key={chat.id}
              chat={chat}
              openChat={handleOpenChat}
              closeDispute={handleExecuteCloseDisput}
              />
          ))
        }
      </div>
      <div className={styles.SellerDisputs__CurrentChat}>
        {
          selectedChat && 
          (<ChatMessages 
          chatId={selectedChat.id}
          messages={messages}/>)
        }
      </div>
    </div>
  );
}

export default SellerDisputs;