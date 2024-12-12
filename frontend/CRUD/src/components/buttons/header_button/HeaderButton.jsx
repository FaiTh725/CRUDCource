import styles from "./HeaderButton.module.css"

const HeaderButton = ({children}) => {
  return (
    <button className={styles.HeaderButton__Button}>
      {children}  
    </button>
  )
}

export default HeaderButton; 