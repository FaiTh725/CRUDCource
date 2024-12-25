import styles from "./Input.module.css"

const SimpleInput = ({labelName, value, action, name = "", typeInput  = "text"}) => {

  return (
    <div className={styles.SimpleInput__Main}>
      <label className={styles.SimpleInput__Label} 
        htmlFor="field">
          {labelName}
      </label>
      <input className={styles.SimpleInput__Input} 
        type={typeInput} 
        id="field" 
        value={value}
        name={name}
        onChange={action}/>
    </div>
  )
}

export default SimpleInput;