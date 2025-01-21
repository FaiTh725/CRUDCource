import styles from "./FeedBack.module.css";

const FeedBack = () => {
  return (
    <div className={styles.FeedBack__Main}>
      <p className={styles.FeedBack__Owner}>Name Owner</p>
      <div className={styles.FeedBack__RatingWrapper}>
        <div className={styles.FeedBack__UnitRating}>
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path d="m21.5 9.757-5.278 4.354 1.649 7.389L12 17.278 6.129 21.5l1.649-7.389L2.5 9.757l6.333-.924L12 2.5l3.167 6.333z"/></svg>
        </div>
        <div className={styles.FeedBack__UnitRating}>
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path d="m21.5 9.757-5.278 4.354 1.649 7.389L12 17.278 6.129 21.5l1.649-7.389L2.5 9.757l6.333-.924L12 2.5l3.167 6.333z"/></svg>
        </div>
        <div className={styles.FeedBack__UnitRating}>
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path d="m21.5 9.757-5.278 4.354 1.649 7.389L12 17.278 6.129 21.5l1.649-7.389L2.5 9.757l6.333-.924L12 2.5l3.167 6.333z"/></svg>
        </div>
      </div>
      <p className={styles.FeedBack__DataSend}>Data</p>
      <div className={styles.FeedBack__Text}>
        Lorem ipsum dolor sit amet consectetur adipisicing elit. Doloremque, iusto. Magnam sit veniam rem laborum perspiciatis tenetur possimus illo accusamus!
      </div>
    </div>
  )
}

export default FeedBack;