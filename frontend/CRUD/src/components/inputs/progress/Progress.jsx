import { useEffect, useRef } from "react"
import styles from "./Progress.module.css"

const Progress = ({label, 
                currentValue, 
                maxValue,
                handleClick,
                isSelected=false}) => {
  
  const scaleRef = useRef();

  useEffect(() => {
    if(scaleRef.current != null)
    {
      scaleRef.current.style.setProperty("--progressWidth", 
        parseFloat((currentValue / maxValue) * scaleRef.current.offsetWidth) + "px");
    }


  }, [currentValue, maxValue])

  return (
    <div className={`${styles.Progress__Main} 
      ${isSelected ? styles.Progress__Selected : ""}`}
      onClick={handleClick}>
      <div className={styles.Progress__Wrapper}>
        <div className={styles.Progress__Label}>{label}</div>
        <div className={styles.Progress__Scale} ref={scaleRef}></div>
        <div className={styles.Progress__MaxValue}>{currentValue}</div>
      </div>
    </div>
  )
}

export default Progress