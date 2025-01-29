import { useEffect, useState } from "react"
import styles from "./ProductList.module.css"
import axios from "axios";
import ProductCart from "../ProductCart/ProductCart";
import Paginator from "../Paginator/Paginator";


const ProductList = () => {
  const [page, setPage] = useState(1);
  const [count, setCount] = useState(12);
  const [maxCount, setMaxCount] = useState(0);
  const [products, setProducts] = useState([]);

  const handleNextPage = () => {
    if(page * count < maxCount)
    {
      fetchProducts(page + 1, count);
    }
  }

  const handlePrevPage = () => {
    if(page - 1 > 0)
    {
      fetchProducts(page - 1, count);
    }
  }

  const fetchProducts = async (page, count, abortSignal) => {
    try
    {
      const response = await axios.get(`https://localhost:5402/api/Product/ProductsPagination?page=${page}&count=${count}`, {
        signal: abortSignal
      });

      if(response.data.statusCode !== 0)
      {
        console.log(response.data.description);
        return;
      }

      setPage(response.data.data.page);
      setCount(response.data.data.count);
      setMaxCount(response.data.data.maxCount);
      setProducts([...response.data.data.products]);
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
      await fetchProducts(page, count, signal);
    }

    dataProducts();

    return () => {
      abortController.abort();
    }
  }, []);

  return (
    <div className={styles.ProductList__Main}>
      <div className={styles.ProductList__Products}>
        { products && 
            products.map(product => (
              <ProductCart key={product.id} product={product}/>
            ))
        }
      </div>
      <div className={styles.ProductList__Pagination}>
        <Paginator
          start={page}
          count={count}
          max={maxCount}
          unit={"Products"}
          handleNext={handleNextPage}
          handlePrev={handlePrevPage}
          />
      </div>
    </div>
  )
}

export default ProductList; 