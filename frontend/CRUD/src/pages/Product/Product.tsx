import React, { useEffect, useMemo, useState } from "react";
import styles from "./Product.module.css"
import SimpleButton from "../../components/buttons/simple_button/SimpleButton";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import BigImageSlider from "../../components/BigImageSlider/BigImageSlider";
import Counter from "../../components/inputs/counter/Counter";
import Cookies from "js-cookie";
import { useAuth } from "../../components/Auth/AuthContext";
import decodeJWT from "../../services/JWTService";

const Product = () => {
  const auth = useAuth();
  const [product, setProduct] = useState({
    id: null,
    name: "",
    description: "",
    price: 0,
    count: 0,
    images: []
  });
  const [count, setCount] = useState(0);
  const [isProductInCart, setIsProductInCart] = useState(false);
  const navigate = useNavigate();

  const handleDeleteProductrFromCart = async () => {
    if(!isProductInCart)
    {
      return;
    }

    if(auth.user == null)
    {
      console.log("Curent user is invalid");
      return;
    }

    try
    {
      const token = Cookies.get("token");
      const response = await axios.delete("https://localhost:7080/api/Account/DeleteProductFromCart", 
      {  
        headers: {
          "Authorization": "Bearer " + token
        },
        data: {
          productId: product.id,
          email: auth.user.email 
        }
      });

      if(response.data.statusCode === 0)
      {
        setIsProductInCart(false);
      }
    }
    catch (error)
    {
      if(error.status == 401)
      {
        auth.logout();
        navigate("/account/login");
        return;
      }

      console.log("Error delete product from cart");
    }
  }

  const handleAddProduct = async () => {
    if(count == 0)
    {
      return;
    }
    
    if(auth.user == null)
    {
      console.log(auth);
      auth.logout();
      navigate("/account/login");
      return;
    }

    try
    {
      const token = Cookies.get("token");
      var response = await axios.post("https://localhost:7080/api/Account/AddToCartProduct", {
        productId: product.id,
        email: auth.user.email,
        count: count
      }, {
        headers: {
          "Authorization": "Bearer " + token
        }
      });

      if(response.data.statusCode == 0)
      {
        setCount(0);
        setIsProductInCart(true);
      }
      else
      {
        console.log(response.data.description);
      }
    }
    catch (error)
    {
      if(error.status === 401)
      {
        auth.logout();
        navigate("/account/login");
        return;
      }
      console.error("Error added product to cart - " + error.message);
    }
  }

  const fetchProduct = async (productId, abortController) => {
    try
    {
      var response = await axios.get("https://localhost:7080/api/Product/Product?id="+productId, {
        signal: abortController.signal
      });
    
      if(response.data.statusCode === 1)
      {
        navigate("/notfound");
        return;
      }

      if(response.data.statusCode !== 0)
      {
        console.log(response.data.description);
        return;
      }

      setProduct({...response.data.data});
    }
    catch(error)
    {
      console.log("Error fetch product data");
    }
  }

  const fetchIsProductInCart = async (productId, abortController) => {
    try
    {
      
      if(auth.user == null)
      {
        setIsProductInCart(false);
        return;
      }
        
      const token = Cookies.get("token");
      const response = await axios.get(`https://localhost:7080/api/Product/IsProductInCart?productId=${productId}&email=${auth.user.email}`, {
        headers: {
          "Authorization" : "Bearer " + token
        },
        signal: abortController.signal
      });

      if(response.data.statusCode == 1)
      {
        setIsProductInCart(false);
        return;
      }
      else if(response.data.statusCode == 0)
      {
        setIsProductInCart(true);
      }
      else
      {
        console.log(response.data.description);
      }
    }
    catch(error)
    {
      console.log("Error check is product in cart");
    }
  }

  useEffect(() => {
    const params = new URLSearchParams(document.location.search);
    const productId = params.get("product");
    if(productId == null)
    {
      navigate("/notfound");
    }

    const controller = new AbortController();
    const callFetchData = async () => {
      await fetchProduct(productId, controller);
    }

    const callCheckProductInCart = async () => {
      await fetchIsProductInCart(productId, controller);
    }

    callFetchData();
    callCheckProductInCart();

    return () => {
      controller.abort("The Request canceled");
    }
  }, []);

  return (
    <div className={styles.Product__Main}>
      <div className={styles.Product__ImageAndDesc}>
        <p>{product.name}</p>
        <div className={styles.Product__ImageWrapper}>
          <BigImageSlider images={product && product.images}/>
        </div>
        <p >{product.description}</p>
      </div>
      <div className={styles.Product__OrderWrapper}>
        <div className={styles.Product__Order}>
          <div className={styles.Product__OrderSection}>
            <p className={styles.Product__OrderSectionName}>PRICE:</p>
            <p className={styles.Product__OrderSectionContent}>${product.price}</p>
          </div>
          <div className={styles.Product__OrderSection}>
            <p className={styles.Product__OrderSectionName}>QUANTITY:</p>
            <div className={styles.Product__OrderCountWrapper}>
              <Counter value={count} setValue={setCount} min={0} max={product.count}/>
            </div>
          </div>
          <div className={styles.Product__BtnContainer}>
            {
              !isProductInCart ? 
              
              product && product.count > 0 ? 
              <SimpleButton name="ADD TO CART" action={handleAddProduct}/> :
              <div className={styles.Product__SoldOut}>
                <div className={styles.Product__SoldOutHeader}>
                  <p>SOLD OUT</p>
                </div>
                <SimpleButton name="NOTIFY WHEN AVALIABLE"/>
              </div>
              : 
              <div>
                <div className={styles.Product__SoldOut}>
                <div className={styles.Product__SoldOutHeader}>
                  <p>AlREADY IN CART</p>
                </div>
                <SimpleButton name="DELETE FROM CART" action={handleDeleteProductrFromCart}/>
              </div>
              </div>
            }
            <div></div>
          </div>
        </div>
      </div>
    </div>
  )
}

export default Product;