import styles from "./SimpleArea.module.css"
import React from "react";

const SimpleArea = ({labelName, value, action, name = ""}) => {
  return (
    <div className={styles.SimpleArea__Main}>
      <label className={styles.SimpleArea__Label} htmlFor="field">
        {labelName}
      </label>
      <textarea
        name={name}
        className={styles.SimpleArea__Input}
        id="field"
        value={value}
        onChange={action}/>
    </div>
  );
};

export default SimpleArea;
