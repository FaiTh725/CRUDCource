import styles from "./HeaderButton.module.css"

const HeaderButton = ({children, action}) => {
  return (
    <button className={styles.HeaderButton__Button}
      onClick={action}>
      {children}  
    </button>
  )
}

export default HeaderButton; 