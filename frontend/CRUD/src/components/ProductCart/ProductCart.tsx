import React, { useRef } from "react"
import ImageSlider from "../ImageSlider/ImageSlider"
import styles from "./ProductCart.module.css"
import { useNavigate } from "react-router-dom";

const ProductCart = ({product}) => {

  const cart = useRef(null);
  const navigate = useNavigate();

  const handleMouseMove = (e) => {
    if(cart.current != null)
    {
      const x = e.pageX - cart.current.offsetLeft;
      const y = e.pageY - cart.current.offsetTop;

      cart.current.style.setProperty("--top-ofset-x", x + "px");
      cart.current.style.setProperty("--top-ofset-y", y + "px");
    }
  }

  const handleClickCart = (e) => {
    
    navigate("/products?product=" + product.id);
  }

  return (
    <div className={styles.ProductCart__Main}
      onMouseMove={handleMouseMove}
      ref={cart}
      onClick={handleClickCart}>
      <div className={styles.ProductCart__Content}>
        <div className={styles.ProductCart__ImageWrapper}>
          <ImageSlider images={product.images}/>
        </div>
        <p className={styles.ProductCart__Name}>{product.name}</p>
        <p className={styles.ProductCart__Price}>${product.price}</p>
      </div>
    </div>
  )
}

export default ProductCart