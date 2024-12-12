import styles from "./HeaderNavigationButton.module.css"

const HeaderNavigationButton = ({name, children}) => {
  return (
    <div className={styles.HeaderNavigation__Main}>
      <button className={styles.HeaderNavigation__Button}>{name}</button>
      <div className={styles.HeaderNavigation__PopUp}> 
        {children}
      </div>
    </div>
  )
}

export default HeaderNavigationButton;