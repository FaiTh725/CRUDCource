
import styles from "./HomeHeader.module.css"

const HomeHeader = () => {
  return (
    <div className={styles.HomeHeader__Main}>
      <div className={styles.HomeHeader__Wrapper}>
        <div className={styles.HomeHeader__AboutShop}>
          <p>ONLINE DELIVERY</p>
          <p>Order merchandise anywhere on the planet</p>
        </div>
      </div>
    </div>
  )
}

export default HomeHeader;