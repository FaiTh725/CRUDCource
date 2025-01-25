
import { useEffect, useState } from "react";
import FeedBack from "../FeedBack/FeedBack";
import Progress from "../inputs/progress/Progress";
import Paginator from "../Paginator/Paginator";
import styles from "./Reviews.module.css"
import axios from "axios";
import ReviewAdd from "../ReviewAdd/ReviewAdd";
import useLogout from "../../hooks/useLogOut";
import { useAuth } from "../Auth/AuthContext";

const Reviews = ({productId}) => {
  const [feedBacks, setFeedBacks] = useState([]);
  const [feedBacksStat, setFeedBackStat] = useState();
  const [filterRate, setFilterRate] = useState(null);
  const [page, setPage] = useState(1);
  const [count, setCount] = useState(3);
  const [maxCount, setMaxCount] = useState(0);
  const [userFeedBack, setUserFeedBack] = useState(undefined);

  const auth = useAuth();
  const logout = useLogout();

  const fetchProductMetrics = async (productId, signal) => {
    try
    {
      const response = await axios.get(`https://localhost:5202/api/Product/GetProductMetrics?productId=${productId}`, {
        signal: signal
      });
    
      if(response.data.statusCode !== 0)
      {
        console.log(response.data.description);
        return;
      }

      const resData = response.data.data;
      setFeedBackStat({
        count: resData.maxCount,
        generalRating: resData.generalRating,
        extentionRating: [...resData.partRatingCount.map(x => ({
          countStars: x.key,
          count: x.value
        }))]
      });
    }
    catch
    {
      console.log("Error Get Product Review Statistics");
    }
  }

  const fetchUserFeedBack = async (productId, signal) => {
    if(auth.user == null)
    {
      return;
    }
    
    try
    {
      const response = await axios.get("https://localhost:5202/api/FeedBack/FeedBackAccount?" + 
          `productId=${productId}&accountEmail=${auth.user.email}`, {
        withCredentials: true,
        signal: signal
      });

      if(response.data.statusCode === 1)
      {
        setUserFeedBack(null);
        return;
      }

      if(response.data.statusCode === 0)
      {
        setUserFeedBack({...response.data.data});
        return;
      }

      console.log("Error get user feedBack " + response.data.description);
    }
    catch(error)
    {
      if(error.status === 401)
      {
        logout();
      }

      console.log("error get user feedback");
    }
  }

  const fetchFeedBacks = async (start, countItems, rate, abortSignal) => {
    try
    {
      const response = await axios.get(
        "https://localhost:5202/api/FeedBack/ProductFeedBacksPagination?" + 
        `productId=${productId}&start=${start}&count=${countItems}${rate == null ? "" : `&rating=${rate}`}`, {
        signal: abortSignal
      });

      if(response.data.statusCode !== 0)
      {
        console.log("Error get product reviews - " + 
          response.data.description);
        return;
      }

      setFeedBacks([...response.data.data.feedBacks]);
      setMaxCount(response.data.data.maxCount);
      setPage(response.data.data.page);
      setCount(response.data.data.count);
    }
    catch(error)
    {
      console.log("Error get product review" + error.message);
    }
  }

  const handleNextPage = () => {
    if(page * count < maxCount)
    {
      fetchFeedBacks(page + 1, count, filterRate);
    }
  }

  const handlePrevPage = () => {
    if(page - 1 > 0)
    {
      fetchFeedBacks(page - 1, count, filterRate);
    }
  } 

  const handleSortReviewByRate = (rate) => {
    const newRate = rate == filterRate ?
      null :
      rate;
    
    setFilterRate(newRate);
    fetchFeedBacks(page, count, newRate);
  }

  const sortProductRatings = (left, right) => {
    return left.countStars > right.countStars ?
      -1 : 1;
  } 

  useEffect(() => {
    const abortController = new AbortController();
    const signal = abortController.signal;
    const executeFetchFeedBacks = async () => {
      await fetchFeedBacks(page, count, filterRate, signal);
    }
    const executeFetchUserFeedBack = async () => {
      await fetchUserFeedBack(productId, signal);
    }
    const executeFethcProductMetrics = async () => {
      await fetchProductMetrics(productId, signal);
    }

    executeFetchFeedBacks();
    executeFetchUserFeedBack();
    executeFethcProductMetrics();

    return () => {
      abortController.abort();
    }
  }, []);
  
  return (
    <div className={styles.Reviews__Main}>
      <h2 className={styles.Reviews__HeaderName}>REVIEWS</h2>
      <div className={styles.Reviews__RatingWrapper}>
        <div className={styles.Reviews__RatingSection}>
          <h3 className={styles.Reviews__RatingSectionHeader}>
            Rating Snapshot
          </h3>
          <div className={styles.Reviews__RatingContent}>
            {
              feedBacksStat && 
              feedBacksStat.extentionRating
                .sort(sortProductRatings)
                .map(rating => (
                <Progress 
                  key={rating.countStars}
                  label={`${rating.countStars} star${rating.countStars == 1 ? "": "s"}`}
                  maxValue={feedBacksStat.count}
                  currentValue={rating.count}
                  isSelected={rating.countStars == filterRate}
                  handleClick={() => {handleSortReviewByRate(rating.countStars)}}/>
              ))
            }
          </div>
        </div>
        <div className={styles.Reviews__RatingSection}>
          <h3 className={styles.Reviews__RatingSectionHeader}>
            Overall Rating
          </h3>
          {
            feedBacksStat &&
            feedBacksStat.count !== 0 ? 
            <div className={`${styles.Reviews__RatingContent} ${styles.Reviews__GeneralRatingSection}`}>
              <div className={styles.Reviews__GeneralRating}>
                {feedBacksStat.generalRating}
              </div>
              <div className={styles.Reviews__RatingContentTotal}>
                <p></p>
                <p>{feedBacksStat.count} reviews</p>
              </div>
            </div> :
            <div className={styles.Reviews__RatingIsEmpty}>
              <p>This product has no reviews</p>
            </div>
          }
        </div>
        <div className={styles.Reviews__RatingSection}>
          <h3 className={styles.Reviews__RatingSectionHeader}>
            We'd love to hear from you. Provide a review for this product.
          </h3>
          {
            userFeedBack ? 
            <p>Thank you for you review it is very important for us!!!</p> :
            <ReviewAdd
              productId={productId}/>
          }
        </div>
      </div>
      {
        userFeedBack &&
        <div className={styles.Reviews__UserFeedBack}>
          <p>Your Review</p>
          <FeedBack
            feedBack={{
              rate: userFeedBack.rate,
              text: userFeedBack.feedBackText,
              sendTime: userFeedBack.sendTime,
              owner: userFeedBack.ownerFeedBack.name,
              images: [...userFeedBack.images]}}/>
        </div>
      }
      <div className={styles.Reviews__Filters}>

      </div>
      <div className={styles.Reviews__Reviews}>
        {
          feedBacks.map(feedBack => (
            <FeedBack key={feedBack.id}
              feedBack={{
                rate: feedBack.rate,
                text: feedBack.feedBackText,
                sendTime: feedBack.sendTime,
                owner: feedBack.ownerFeedBack.name,
                images: [...feedBack.images]
              }}/>
          ))
        }
      </div>
      <div className={styles.Reviews__PaginationControl}>
        <Paginator
          start={page}
          count={count}
          max={maxCount}
          unit={"Reviews"}
          handleNext={handleNextPage}
          handlePrev={handlePrevPage}/>
      </div>
    </div>
  );
}

export default Reviews;