import { HttpTransportType, HubConnectionBuilder} from "@microsoft/signalr";
import { createContext, useContext, useEffect, useState} from "react"
import Chat from "../Chat/Chat";
import { useAuth } from "../Auth/AuthContext";
import { useNotification } from "../Notification/NotificationContext";

const SignalRContext = createContext();

export const SignalRProvider = ({children}) => {
  const [connection, setConnection] = useState(null);
  const [chats, setChats] = useState([]);
  const [sellerChats, setSellerChats] = useState(undefined);
  const [chatIsAvaliable, setChatIsAvaliable] = useState(false);
  const auth = useAuth();
  const notification = useNotification();

  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
        .withUrl("https://localhost:5402/Chat", {
          skipNegotiation: true,
          transport: HttpTransportType.WebSockets
        })
        // .withAutomaticReconnect()
        .build(); 

    newConnection.on("ChatCreatedResult", data => {
      if(data.id == "")
      {
        notification.notify("Chat already exist");
        // console.log("Chat already exist");
        return;
      }

      notification.notify("Chat had already been created, look chats");
      if(data.customerEmail == auth.user.email)
      {
        setChats(prev => [
          ...prev,
          {...data}
        ]);
      }
      else
      {
        setSellerChats(prev => [
          ...prev,
          {...data}
        ]);
      }

    });

    newConnection.on("MessageSent", data => {
      setChats(prev => {
        return prev.map(chat => {
          if(chat.id === data.chatRoomId)
          {
            return { ...chat, messages: [...chat.messages, { ...data }] };
          }

          return chat;
        });
      });
      
      setSellerChats(prev => {
        if (!prev) return prev;
        if(prev)
        {
          return prev.map(chat => {
            if(chat.id === data.chatRoomId)
            {
              return { ...chat, messages: [...chat.messages, { ...data }] };
            }
            return chat;
          });
        }
        else
        {
          return undefined;
        }
      });
    });

    newConnection.on("UserConnected", data => {
      setChats([...data]);
    });

    newConnection.on("SellerConnected", data => {
      setSellerChats([...data]);
    });

    newConnection.on("ChatDeleted", data => {
      if(sellerChats)
      {
        setSellerChats(prev => prev
          .filter(chat => chat.id !== data));
      }

      setChats(prev => prev
        .filter(chat => chat.id !== data));
    });

    setConnection(newConnection);

    return () => {
      if(newConnection)
      {
        newConnection.stop();
      }
    }
  }, []);

  useEffect(() => {
    if(auth.user == null)
    {
      setConnection(null);
      return;
    }

    if((connection != null && 
      connection.connectionState == 1) ||
      connection == null)
    {
      return;
    }

    try
    {
      connection
      .start()
      .then(() => {
        console.log("Connected");

        setChatIsAvaliable(true);

        connection.invoke("UserConnected");
        if(auth.user.role === "Seller" ||
          auth.user.role === "Admin"
        )
        {
          connection.invoke("ConnectSeller");
        }
      });
    }
    catch
    {
      console.error("Error connect to hub");
    }
  }, [connection]);

  const handleSendMessage = (message, chatId) => {
    if(message.trim() == "")
    {
      return;
    }

    if(connection == null)
    {
      console.error("do not connected to hub");
      return;
    }

    try
    {
      connection.invoke("SendMessages", {
        text: message,
        senderEmail: auth.user.email,
        chatId: chatId
      });
    }
    catch
    {
      console.log("Error send message");
    }
  }

  const handleCloseDispute = (chatId) => {
    if(connection == null)
    {
      console.log("connection is null");
      return;
    }

    connection
    .invoke("CloseDispute", chatId)
    .then(() => {
      setChats(chats.filter(chat => chat.id !== chatId));
    });
  }

  const handleCloseSellerDispute = (chatId) => {
    if(connection == null)
    {
      console.log("connection is null");
      return;
    }

    connection
    .invoke("CloseDispute", chatId)
    .then(() => {
      setSellerChats(sellerChats.filter(chat => chat.id !== chatId))
    });
  }

  const handleOpenDispute = (product) => {
    if(connection === null)
    {
      console.log("Error with user identity or signalR connection");
      return;
    }

    try
    {
      connection.invoke("CreateNewChat", {
        consumerEmail: auth.user.email,
        productId: product.id,
        productName: product.name
      });
    }
    catch(error)
    {
      console.log(error);
    }
    
  }

  useEffect(() => {
    setChatIsAvaliable(connection !== null);
  }, [connection, auth.user]);

  return (
    <SignalRContext.Provider value={{ 
      chats,
      sellerChats,
      chatIsAvaliable,
      handleSendMessage,
      handleCloseDispute,
      handleCloseSellerDispute,
      handleOpenDispute}}>
      {children}
      <Chat/>
    </SignalRContext.Provider>
  )
}

export const useSignalR = () => useContext(SignalRContext);