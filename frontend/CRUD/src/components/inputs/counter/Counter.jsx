import EmptyButton from "../../buttons/empty_button/EmptyButton";
import styles from "./Counter.module.css"

const Counter = ({value , setValue, min = 0, max, range = 1}) => {
  
  const handleChangeRangeUp = () => {
    console.log(max);
    setValue((prev) => {
      if(max >= prev + range)
      {
        return prev + range
      }
      return prev
    });
  }
  const handleChangeRangeDown = () => {
    setValue((prev) => {
      if(min <= prev - range)
      {
        return prev - range
      }
      return prev;
    });
  }
  
  return (
    <div className={styles.Counter__Main}>
      <div className={styles.Counter__MinusBtn}>
        <EmptyButton action={handleChangeRangeDown}>
          <svg width="21" height="21" viewBox="0 0 21 21" fill="none" xmlns="http://www.w3.org/2000/svg" aria-hidden="true">
          <path fillRule="evenodd" clipRule="evenodd" d="M20 10.75C20 11.1642 19.6642 11.5 19.25 11.5L1.75 11.5C1.33579 11.5 1 11.1642 1 10.75C1 10.3358 1.33579 10 1.75 10L19.25 10C19.6642 10 20 10.3358 20 10.75Z" fill="currentColor"></path>
          </svg>
        </EmptyButton>
      </div>
      <div className={styles.counter__Value}>{value}</div>
      <div className={styles.Counter__PlusBtn}>
        <EmptyButton action={handleChangeRangeUp}>
          <svg width="21" height="21" viewBox="0 0 21 21" fill="none" xmlns="http://www.w3.org/2000/svg" aria-hidden="">
          <path fillRule="evenodd" clipRule="evenodd" d="M10.75 1C11.1642 1 11.5 1.33579 11.5 1.75L11.5 19.25C11.5 19.6642 11.1642 20 10.75 20C10.3358 20 10 19.6642 10 19.25L10 1.75C10 1.33579 10.3358 1 10.75 1Z" fill="currentColor"></path>
          <path fillRule="evenodd" clipRule="evenodd" d="M20 10.75C20 11.1642 19.6642 11.5 19.25 11.5L1.75 11.5C1.33579 11.5 1 11.1642 1 10.75C1 10.3358 1.33579 10 1.75 10L19.25 10C19.6642 10 20 10.3358 20 10.75Z" fill="currentColor"></path>
          </svg>
        </EmptyButton>
      </div>
    </div>
  )
}

export default Counter;