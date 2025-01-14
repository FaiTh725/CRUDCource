import { useEffect, useState } from "react"
import styles from "./ProductList.module.css"
import axios from "axios";
import ProductCart from "../ProductCart/ProductCart";


const ProductList = () => {
  const [products, setProducts] = useState([]);

  const fetchProducts = async (abortSignal) => {
    try
    {
      const response = await axios.get("https://localhost:5202/api/Product/Products", {
        signal: abortSignal
      });

      if(response.data.statusCode !== 0)
      {
        console.log(response.data.description);
        return;
      }

      setProducts(response.data.data);
    }
    catch (error)
    {
      console.log(error.message);
    }
  } 

  useEffect(() => {
    const abortController = new AbortController();
    const signal = abortController.signal;
      const dataProducts = async () => {
      await fetchProducts(signal);
    }

    dataProducts();

    return () => {
      abortController.abort();
    }
  }, []);

  return (
    <div className={styles.ProductList__Main}>
      { products && 
          products.map(product => (
            <ProductCart key={product.id} product={product}/>
          ))
      }
    </div>
  )
}

export default ProductList; 