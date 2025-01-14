import { HubConnectionBuilder } from "@microsoft/signalr";
import { createContext, useContext, useEffect, useState} from "react"
import Chat from "../Chat/Chat";
import { useAuth } from "../Auth/AuthContext";
import useLogout from "../../hooks/useLogOut";
import { unstable_renderSubtreeIntoContainer } from "react-dom";

const SignalRContext = createContext();

export const SignalRProvider = ({children}) => {
  const [connection, setConnection] = useState(null);
  const [chats, setChats] = useState([]);
  const [sellerChats, setSellerChats] = useState(undefined);
  const auth = useAuth();
  const logout = useLogout();

  useEffect(() => {
    if(auth.user == null)
    {
      setConnection(null);
      return;
    }

    const newConnection = new HubConnectionBuilder()
        .withUrl("https://localhost:7045/SupportChat")
        // .withAutomaticReconnect()
        .build(); 

    newConnection.on("ChatCreatedResult", data => {
      if(data.id == "")
      {
        console.log("Chat already exist");
        return;
      }

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
      console.log(data);
    });

    newConnection.on("SellerConnected", data => {
      setSellerChats([...data]);
      console.log(data);
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
  }, [auth.user]);

  useEffect(() => {
    if(connection == null)
    {
      return;
    }

    try
    {
      connection
      .start()
      .then(() => {
        console.log("Connected");
        connection.invoke("UserConnected");
        if(auth.user.role === "Seller" ||
          auth.user.role === "Admin"
        )
        {
          connection.invoke("ConnectSeller");
        }
      });
    }
    catch(error)
    {
      console.log(error);
      console.log(error.message);
    }
  }, [connection]);

  const handleSendMessage = (message, chatId) => {
    if(message.trim() == "")
    {
      return;
    }

    if(auth.user == null)
    {
      logout();
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
    catch(error)
    {
      console.log(error);
      console.log(error.message);
    }
  }

  const handleCloseDispute = (chatId) => {
    if(auth.user == null)
    {
      logout();
    }
    
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
    if(auth.user == null)
    {
      logout();
    }

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
    if(auth.user == null)
    {
      logout();
    }
    
    if(connection === null
    )
    {
      console.log("Error with user identity or signalR connection");
      return;
    }

    connection.invoke("CreateNewChat", {
      consumerEmail: auth.user.email,
      productId: product.id,
      productName: product.name
    });
  }

  return (
    <SignalRContext.Provider value={{ 
      chats,
      sellerChats,
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