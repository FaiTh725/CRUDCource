import { useEffect, useRef} from "react";
import styles from "./Cart.module.css"
import { useMemo } from "react";
import useLogout from "../../hooks/useLogOut";
import axios from "axios";
import { useAuth } from "../Auth/AuthContext";
import { useCart } from "./CartContext";
import EmptyButton from "../buttons/empty_button/EmptyButton";
import MiniProductCart from "../MiniProductCart/MiniProductCart";
import SimpleButton from "../buttons/simple_button/SimpleButton";
import { useNavigate } from "react-router-dom";

const Cart = ({isOpen, setIsOpen}) => {
  const cartItems = useCart();
  const auth = useAuth();
  const cart = useRef(null);
  const logout = useLogout();
  const navigate = useNavigate();

  const cartClass = useMemo(() => {
    if(cart.current !== null)
    {
      cart.current.className = `${styles.Cart__Wrapper} ${isOpen ? 
        styles.Cart__WrapperAnimationShow :
        styles.Cart__WrapperAnimationHide }`; 
    }
  }, [isOpen]);

  const subTotal = useMemo(() => {
    return cartItems.cart.reduce((currentSum, product) =>
      (product.count * product.price) + currentSum , 0);
  }, [cartItems]);

  const handleHandleChangeProductCount = (productId, newCount) => {
    cartItems.setCartItems(cartItems.cart.map(product => {
      if(product.id === productId)
      {
        product.count = newCount;
      }

      return product;
    }));
  }

  const handleBuyProucts = async () => {
    if(auth.user == null)
    {
      logout();
    }
    
    try
    {
      const response = await axios.post("https://localhost:5202/api/Account/BuyProducts", {
        email: auth.user.email,
        products: cartItems.cart
          .filter(product => product.count > 0)
          .map(product => {
            return ({
              productId: product.id,
              count: product.count
            })
          })
      }, {
        withCredentials: true
      });

      if(response.data.statusCode !== 0)
      {
        console.log(response.data.description);
        return;
      }

      cartItems.setCartItems(cartItems.cart
        .filter(product => product.count === 0));
    
          
    }
    catch(error)
    {
      if(error.status === 401)
      {
        logout();
      }
      console.log(error);
    }
  }

  const handleGetCart = async (signal) => {
    try
    {
      const response = await axios.get(`https://localhost:5202/api/Account/GetAccountCart?email=${auth.user.email}`, {
        withCredentials: true,
        signal: signal
      });

      if(response.data.statusCode === 0)
      {
        cartItems.setCartItems(response.data.data.cartProducts);
      }
      else
      {
        console.log(response.data.description);
      }
    }
    catch(error)
    {
      if(error.status === 401)
      {
        logout();
      }
      console.log(error.message);
    }
  }

  useEffect(() => {
    if(auth.user == null)
    { 
      return;
    }

    const abortController = new AbortController();
    const signal = abortController.signal;
    if(auth.user !== null)
    {
      const executeGetCart = async () => {
        await handleGetCart(signal);
      }

      executeGetCart();
    }

    return () => 
    {
      abortController.abort();
    }
  }, [auth.user]);

  const handleDeleteFromCart = async (productId) => {
    console.log("1");
    try
    {
      if(auth.user == null)
      {
        logout();
      }
      const response = await axios.delete("https://localhost:5202/api/Account/DeleteProductFromCart", {
        withCredentials: true,
        data: {
          productId: productId,
          email: auth.user.email
        }
      });

      if(response.data.statusCode === 0)
      {
        cartItems.removeCartItem(productId);
      }
      else
      {
        console.log(response.data.description);

      }
    }
    catch(error)
    {
      if(error.status === 401)
        {
          logout();
        }
        console.log("Error delete from cart");

    }
  }

  return (
    <div className={`${styles.Cart__Main} ${isOpen ? 
      "" : styles.Cart__Disable + " " + styles.Cart__MainForClosing}`}
      onClick={() => {setIsOpen(false)}}>
      <div className={cartClass}
        onClick={(e) => {e.stopPropagation()}}
        ref={cart}>
        <div className={styles.Cart__Header}>
          <p>CART 
            <span className={styles.Cart__CountInCart}>
              {cartItems.cart.filter(product => product.count !==0).length}
            </span> 
          </p>
          <EmptyButton action={() => {setIsOpen(false)}}>
            <svg width={25} height={25} viewBox="0 0 28 28" fill="none" xmlns="http://www.w3.org/2000/svg"><g id="SVGRepo_bgCarrier" strokeWidth="0"></g><g id="SVGRepo_tracerCarrier" strokeLinecap="round" strokeLinejoin="round"></g><g id="SVGRepo_iconCarrier"><path d="M2.32129 2.32363C2.72582 1.9191 3.38168 1.9191 3.78621 2.32363L25.6966 24.234C26.1011 24.6385 26.1011 25.2944 25.6966 25.6989C25.2921 26.1035 24.6362 26.1035 24.2317 25.6989L2.32129 3.78854C1.91676 3.38402 1.91676 2.72815 2.32129 2.32363Z" fill="#000000"></path><path d="M25.6787 2.30339C25.2742 1.89887 24.6183 1.89887 24.2138 2.30339L2.30339 24.2138C1.89887 24.6183 1.89887 25.2742 2.30339 25.6787C2.70792 26.0832 3.36379 26.0832 3.76831 25.6787L25.6787 3.76831C26.0832 3.36379 26.0832 2.70792 25.6787 2.30339Z" fill="#000000"></path></g></svg>          
          </EmptyButton>
        </div>
        {
          cartItems.cart.length > 0 ?
          <div className={styles.Cart__NotEmpty}>
            <div className={styles.Cart__Products}>
              {
                cartItems.cart.map(product => (
                  <MiniProductCart key={product.id} 
                    product={product} 
                    changeProductCount={handleHandleChangeProductCount}
                    deleteProduct={handleDeleteFromCart}/>
                ))
              }
            </div>
            <div className={styles.Cart__Footer}>
              <div className={styles.Cart__Subtotal}>
                <p>SUBTOTAL</p>
                <p>${subTotal}</p>
              </div>
              <SimpleButton name="CHECK OUT" action={handleBuyProucts}/>
            </div>
          </div> : 
          auth.user == null ? 
          <div className={styles.Cart__CartIsUnAvaliable}>
            <p className={styles.Cart__CartUnAvaliableHeader}>SHOPING CART IS AVALIABLE ONLY FOR REGISTERED USERS</p>
            <SimpleButton 
              name="Sign In"
              action={() => {navigate("/account/login"); setIsOpen(false);}}  
            />
          </div> :
          <div className={styles.Cart__EmptyCart}>
            <p className={styles.Cart__EmptyHeader}>YOUR CART IS EMPTY</p>
            <p className={styles.Cart__EmptyBody}>Add your favorite items to your cart</p>
            <SimpleButton 
              name="SHOP NOW"
              action={() => {navigate("/"); setIsOpen(false);}}
            />
          </div>
        }
      </div>
    </div>
  )
}

export default Cart;