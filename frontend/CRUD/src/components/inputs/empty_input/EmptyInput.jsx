import styles from "./EmptyInput.module.css"

const EmptyInput = ({value, action, type="text", placeHolder = ""}) => {
  return (
    <div className={styles.EmptyInput__Main}>
      <textarea 
        className={styles.EmptyInput__Input}
        onChange={action}
        value={value}
        type={type} 
        placeholder={placeHolder}/>
    </div>
  )
}

export default EmptyInput;

