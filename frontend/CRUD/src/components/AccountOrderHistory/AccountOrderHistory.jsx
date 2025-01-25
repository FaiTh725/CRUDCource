import { useEffect, useState } from "react";
import styles from "./AccountOrderHistory.module.css"
import { useAuth } from "../Auth/AuthContext";
import axios from "axios";
import useLogout from "../../hooks/useLogOut";
import ProductOrderCart from "../ProductOrderCart/ProductOrderCart";

const AccountOrderHistory = () => {
  const [products, setProducts] = useState([]);
  const auth = useAuth();
  const logout = useLogout();

  const getOrderHistory = async (emailAccount, abortSignal) => {
    try
    {
      const response = await axios.get(`https://localhost:5202/api/Account/AccountOrderHistory?email=${emailAccount}`, {
        withCredentials: true,
        signal: abortSignal
      });

      if(response.data.statusCode !== 0)
      {
        console.log(response.data.description);
        return;
      }

      setProducts(response.data.data.orderHistory.map(product => ({
        id: product.id,
        image: product.images.length > 0 ? product.images[0] : "",
        price: product.price,
        name: product.name,
        count: product.count
      })));
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
    const abortController = new AbortController();
    const signal = abortController.signal;
    if(auth.user != null)
    {
      const fetchProducts = async () => {
        await getOrderHistory(auth.user.email, signal);
      }

      fetchProducts();
    }
    else
    {
      console.log("Error with user credentials");
    }

    return () => {
      abortController.abort();
    }
  }, []);

  return (
    <div className={styles.AccountOrderHistory__Main}>
      <div className={styles.AccountOrderHistory__Header}>
        <p>ORDER HISTORY</p>
      </div>
      <div className={styles.AccountOrderHistory__Products}>
        {
          products && products.map((product, index) => (
            <ProductOrderCart key={index} 
              product={product}/>
          ))
        }
      </div>  
    </div>
  )
}

export default AccountOrderHistory;