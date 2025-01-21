import { useMemo, useState } from "react";
import styles from "./InputRating.module.css";
import RatingStar from "./RatingStar";

const InputRating = () => {
  const [stars, setStars] = useState([
    {position:0, isSelected: false},
    {position:1, isSelected: false},
    {position:2, isSelected: false},
    {position:3, isSelected: false},
    {position:4, isSelected: false}
  ]);

  const handleMouseEnter = (position) => {
    const updatedStars = stars.map((cur, index) => 
      position >= index ? {...cur, isSelected: true} : {...cur, isSelected: false}
    );

    setStars(updatedStars);
  }

  const handleMouseLeave = () => {
    const updatedStars = stars.map(cur => 
      ({...cur, isSelected: false})
    );

    setStars(updatedStars);
  }

  
  return (
    <div className={styles.InputRating__Main}>
      {
        stars && stars.map(star => (
          <RatingStar 
            key={star.position} 
            position={star.position} 
            isActive={star.isSelected}
            handleMouseEnter={handleMouseEnter}
            handleMouseLeave={handleMouseLeave}/>
        ))
      }
    </div>
  )
}

export default InputRating;