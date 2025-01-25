import { useRef } from "react";
import styles from "./ProductOrderCart.module.css"
import EmptyButton from "../buttons/empty_button/EmptyButton";
import { useSignalR } from "../SignalR/SignalRContext";
import { useNavigate } from "react-router-dom";

const ProductOrderCart = ({product}) => {
  const cart = useRef(null);
  const connection = useSignalR();
  const navigate = useNavigate();

  const handleMouseMove = (e) => {
    if(cart.current != null)
    {
      const x = e.pageX - cart.current.offsetLeft;
      const y = e.pageY - cart.current.offsetTop;

      cart.current.style.setProperty("--top-ofset-x", y + "px");
      cart.current.style.setProperty("--left-ofset-y", x + "px");
    }
  }


  
  return (
    <div className={styles.ProductOrderCart__Main}
      onMouseMove={handleMouseMove}
      ref={cart}
      onClick={() => {navigate(`/products?product=${product.id}`);}}>
      <div className={styles.ProductOrderCart__Wrapper}>
        <div className={styles.ProductOrderCart__Image}
          style={{backgroundImage: `url(${product.image})`}}>
  
        </div>
        <div className={styles.ProductOrderCart__Body}>
          <div className={styles.ProductOrderCart__ProductInfo}>
            <p className={styles.ProductOrderCart__ProductName}>
              {product.name}
            </p>
            <p className={styles.ProductOrderCart__ProductPrice}>
              ${product.price}
            </p>
          </div>
          <div className={styles.ProductOrderCart__DisputeBtnWrapper}
            onClick={(e) => {e.stopPropagation()}}>
            <p>{product.count}x</p>
            <EmptyButton
              action={() => {connection.handleOpenDispute(product);}}>
              <span className={styles.ProductOrderCart__DisputeText}>
                open dispute
              </span>
            </EmptyButton>
          </div>
        </div>
      </div>
    </div>
  )
}

export default ProductOrderCart;