import styles from "./SimpleArea.module.css"
import React from "react";

const SimpleArea = ({labelName, value, action, name = "", maxLenght = 2000}) => {
  return (
    <div className={styles.SimpleArea__Main}>
      <label className={styles.SimpleArea__Label} htmlFor="field">
        {labelName}
      </label>
      <textarea
        maxLength={maxLenght}
        name={name}
        className={styles.SimpleArea__Input}
        id="field"
        value={value}
        onChange={action}/>
    </div>
  );
};

export default SimpleArea;
