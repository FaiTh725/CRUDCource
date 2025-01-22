import { useEffect } from "react";
import { ShortFormatDate } from "../../services/FormatDate";
import styles from "./FeedBack.module.css";

const FeedBack = ({feedBack}) => {
  
  return (
    <div className={styles.FeedBack__Main}>
      <p className={styles.FeedBack__Owner}>{feedBack.owner}</p>
      <div className={styles.FeedBack__RatingWrapper}>
        {
          Array.from({length: feedBack.rate})
          .map((_, index) => (
            <div key={index} className={styles.FeedBack__UnitRating}>
              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path d="m21.5 9.757-5.278 4.354 1.649 7.389L12 17.278 6.129 21.5l1.649-7.389L2.5 9.757l6.333-.924L12 2.5l3.167 6.333z"/></svg>
            </div>
          ))
        }
      </div>
      <p className={styles.FeedBack__DataSend}>{ShortFormatDate(feedBack.sendTime)}</p>
      <div className={styles.FeedBack__Images}>
        {
          feedBack.images.map((image, index) => (
            <div className={styles.FeedBack__Image} 
              key={index} 
              style={{backgroundImage: `url(http://localhost:10000/faith725/images/FeedBacks/4/a8aa89eb-313d-459e-a0f1-4898b1a8b0e7Logo.png?sv=2025-01-05&se=2025-02-22T19%3A25%3A59Z&sr=b&sp=r&sig=QDGesjYmYIDn33FpfOEnFXT8Ezrds%2B43Z5hiA9%2BgD1A%3D)`}}>
            </div>
          ))
        }
      </div>
      <div className={styles.FeedBack__Text}>
        {feedBack.text}
      </div>
    </div>
  )
}

export default FeedBack;