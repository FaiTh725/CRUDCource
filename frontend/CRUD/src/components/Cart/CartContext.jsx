import { createContext, useContext, useState } from "react";
import Cart from "./Cart";
import styles from "./Cart.module.css"

const CartContext = createContext();

export const CartContextProvider = ({children}) => {
  const [cart, setCart] = useState([]);
  const [cartIsOpen, setCartIsOpen] = useState(false);
  
  const addCartitem = (cartItem) => {
    setCart(prev => {
      return [...prev, cartItem]
    });
  }

  const setCartItems = (cartItems) => {
    setCart([...cartItems]);
  }

  const removeCartItem = (indexCartItem) => {
    setCart(prev => {
      return prev.filter(cartItem => cartItem.id != indexCartItem)
    });
  }

  return <CartContext.Provider value={{
    cart, 
    addCartitem, 
    setCartItems, 
    removeCartItem,
    setCartIsOpen,
    cartIsOpen}}>
    {children}
    <Cart isOpen={cartIsOpen} setIsOpen={setCartIsOpen}/>
  </CartContext.Provider>
}

export const useCart = () => useContext(CartContext);