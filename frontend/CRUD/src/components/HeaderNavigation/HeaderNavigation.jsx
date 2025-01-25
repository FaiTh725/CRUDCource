import HeaderNavigationButton from "../buttons/header_navigation/HeaderNavigationButton";
import styles from "./HeaderNavigation.module.css"

const HeaderNavigation = () => {
  return (
    <div className={styles.HeaderNavigation__Main}>
      <div className={styles.HeaderNavigation__Wrapper}>
        <HeaderNavigationButton name="TODO"></HeaderNavigationButton>
        <HeaderNavigationButton name="TODO"></HeaderNavigationButton>
        <HeaderNavigationButton name="TODO"></HeaderNavigationButton>
      </div>
    </div>
  )
}

export default HeaderNavigation;