import { useEffect, useMemo, useState } from "react";
import styles from "./Product.module.css"
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { useAuth } from "../../components/Auth/AuthContext";
import useLogout from "../../hooks/useLogOut";
import { useCart } from "../../components/Cart/CartContext";
import SimpleButton from "../../components/buttons/simple_button/SimpleButton";
import Counter from "../../components/inputs/counter/Counter";
import BigImageSlider from "../../components/BigImageSlider/BigImageSlider";
import Reviews from "../../components/Reviews/Reviews";

const Product = () => {
  const auth = useAuth();
  const cart = useCart();
  const [product, setProduct] = useState({
    id: null,
    name: "",
    description: "",
    price: 0,
    count: 0,
    images: []
  });
  const [count, setCount] = useState(0);
  const navigate = useNavigate();
  const logout = useLogout();

  const isProductInCart = useMemo(() => {
    const params = new URLSearchParams(document.location.search);
    const productId = Number.parseInt(params.get("product"));
    if(cart.cart.length !== 0 && !Number.isNaN(productId))
    {
      return cart.cart.find(cartItem => 
        cartItem.id === productId) !== undefined ?
        true : false;
    }
    else
    {
      return false;
    }
  }, [cart]);

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
      const response = await axios.delete("https://localhost:5202/api/Account/DeleteProductFromCart", 
      {  
        withCredentials: true,
        data: {
          productId: product.id,
          email: auth.user.email 
        }
      });

      if(response.data.statusCode === 0)
      {
        cart.removeCartItem(product.id);
      }
    }
    catch (error)
    {
      if(error.status == 401)
      {
        logout();
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
      logout();
    }

    try
    {
      var response = await axios.post("https://localhost:5202/api/Account/AddToCartProduct", {
        productId: product.id,
        email: auth.user.email,
        count: count
      }, { 
        withCredentials: true
      });

      if(response.data.statusCode == 0)
      {
        setCount(0);
        cart.addCartitem({
          id: product.id,
          description: product.description,
          price: product.price,
          name: product.name,
          images: product.images,
          maxCountProduct: product.count,
          count: count
        });
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
        logout();
      }
      console.error("Error added product to cart - " + error.message);
    }
  }

  const fetchProduct = async (productId, abortController) => {
    try
    {
      var response = await axios.get("https://localhost:5202/api/Product/Product?id="+productId, {
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
      console.log(error.message);
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

    callFetchData();

    return () => {
      controller.abort("The Request canceled");
    }
  }, []);


  return (
    <div className={styles.Product__Main}>
      <div className={styles.Product__ProductView}>
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
            </div>
          </div>
        </div>
      </div>
      <div>
        <Reviews/>
      </div>
    </div>
  )
}

export default Product;