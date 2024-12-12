import styles from "./Input.module.css"

const SimpleInput = ({labelName, action, typeInput  = "text"}) => {

  return (
    <div className={styles.SimpleInput__Main}>
      <label className={styles.SimpleInput__Label} 
        htmlFor="field">
          {labelName}
      </label>
      <input className={styles.SimpleInput__Input} 
        type={typeInput} 
        id="field" 
        onChange={(e) => action(e.target.value)}/>
    </div>
  )
}

export default SimpleInput;