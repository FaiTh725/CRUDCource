import React, { useEffect, useState } from "react"
import ProductCart from "../ProductCart/ProductCart";
import styles from "./ProductList.module.css"
import axios from "axios";


const ProductList = () => {
  const [products, setProducts] = useState([]);

  const fetchProducts = async () => {
    try
    {
      const response = await axios.get("https://localhost:7080/api/Product/Products");

      if(response.data.statusCode !== 0)
      {
        console.log(response.data.description);
        return;
      }

      setProducts(response.data.data);
    }
    catch (error)
    {
      console.log(error);
    }
  } 

  useEffect(() => {
    const dataProducts = async () => {
      await fetchProducts();
    }

    dataProducts();
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