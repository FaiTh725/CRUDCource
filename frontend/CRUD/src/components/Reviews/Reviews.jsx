
import FeedBack from "../FeedBack/FeedBack";
import Progress from "../inputs/progress/Progress";
import InputRating from "../inputs/rating_star/InputRating";
import Paginator from "../Paginator/Paginator";
import styles from "./Reviews.module.css"

const Reviews = () => {
  return (
    <div className={styles.Reviews__Main}>
      <h2 className={styles.Reviews__HeaderName}>REVIEWS</h2>
      <div className={styles.Reviews__RatingWrapper}>
        <div className={styles.Reviews__RatingSection}>
          <h3 className={styles.Reviews__RatingSectionHeader}>
            Rating Snapshot
          </h3>
          <div className={styles.Reviews__RatingContent}>
            <Progress label={"5 star"} maxValue={1000} currentValue={800}/>
            <Progress label={"4 star"} maxValue={800} currentValue={400}/>
            <Progress label={"3 star"} maxValue={600} currentValue={200}/>
            <Progress label={"2 star"} maxValue={400} currentValue={200}/>
            <Progress label={"1 star"} maxValue={200} currentValue={20}/>
          </div>
        </div>
        <div className={styles.Reviews__RatingSection}>
          <h3 className={styles.Reviews__RatingSectionHeader}>
            Overall Rating
          </h3>
          <div className={styles.Reviews__RatingContent}>

          </div>
        </div>
        <div className={styles.Reviews__RatingSection}>
          <h3 className={styles.Reviews__RatingSectionHeader}>
            We'd love to hear from you. Provide a review for this product.
          </h3>
          <div className={styles.Reviews__RatingContent}>
            <InputRating/>
          </div>
        </div>
      </div>
      <div className={styles.Reviews__Filters}>

      </div>
      <div className={styles.Reviews__Reviews}>
        <FeedBack/>
        <FeedBack/>
        <FeedBack/>
      </div>
      <div className={styles.Reviews__PaginationControl}>
        <Paginator
          start={1}
          count={10}
          max={100}
          unit={"Reviews"}/>
      </div>
    </div>
  );
}

export default Reviews;