import styles from "./SimpleButton.module.css"

const SimpleButton = ({name, action}) => {

  return (
    <button className={styles.SimpleButton__Button} 
      onClick={action}>
        {name}
    </button>
  )
}

export default SimpleButton;