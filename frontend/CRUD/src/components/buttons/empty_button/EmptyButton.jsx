import styles from "./EmptyButton.module.css"

const EmptyButton = ({action, children}) => {
  return (
    <button className={styles.EmptyButton__Button} onClick={action}>
      {children}
    </button>
  )
}

export default EmptyButton;