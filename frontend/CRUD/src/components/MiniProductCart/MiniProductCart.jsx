import { useEffect, useState } from "react";
import styles from "./MiniProductCart.module.css"
import EmptyButton from "../buttons/empty_button/EmptyButton";
import Counter from "../inputs/counter/Counter";

const MiniProductCart = ({product, changeProductCount, deleteProduct}) => {
  const [count, setCount] = useState(product.count);

  useEffect(() => {
    changeProductCount(product.id, count)
  }, [count])

  return (
    <div className={styles.MiniProductCart__Main}>
      <div className={styles.MiniProductCart__Image} style={{backgroundImage: `url(${product.images.length > 0 ? product.images[0] : ""})`}}></div>
      <div className={styles.MiniProductCart__ProductData}>
        <div className={styles.MiniProductCart__ProductSection}>
          <p className={styles.MiniProductCart__Name}>{product.name}</p>
          <div>
            <EmptyButton action={() => {deleteProduct(product.id)}}>
              <svg viewBox="0 0 24 24" width={25} height={25} fill="none" xmlns="http://www.w3.org/2000/svg"><g id="SVGRepo_bgCarrier" strokeWidth="0"></g><g id="SVGRepo_tracerCarrier" strokeLinecap="round" strokeLinejoin="round"></g><g id="SVGRepo_iconCarrier"> <path className={styles.MiniProductCart__DeleteBtn} d="M5 6.77273H9.2M19 6.77273H14.8M9.2 6.77273V5.5C9.2 4.94772 9.64772 4.5 10.2 4.5H13.8C14.3523 4.5 14.8 4.94772 14.8 5.5V6.77273M9.2 6.77273H14.8M6.4 8.59091V15.8636C6.4 17.5778 6.4 18.4349 6.94673 18.9675C7.49347 19.5 8.37342 19.5 10.1333 19.5H13.8667C15.6266 19.5 16.5065 19.5 17.0533 18.9675C17.6 18.4349 17.6 17.5778 17.6 15.8636V8.59091M9.2 10.4091V15.8636M12 10.4091V15.8636M14.8 10.4091V15.8636" stroke="#222" strokeLinecap="round" strokeLinejoin="round"></path> </g></svg>
            </EmptyButton>
          </div>
        </div>
        <div className={styles.MiniProductCart__ProductSection}>
          <Counter min={0} 
            max={product.maxCountProduct} 
            value={count} 
            setValue={setCount}/>
          <p>${product.price}</p>
        </div>
      </div>
    </div>
  )
}

export default MiniProductCart;