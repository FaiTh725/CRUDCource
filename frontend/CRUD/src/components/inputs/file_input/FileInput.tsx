import React, { useRef, useState } from "react";
import styles from "./FileInput.module.css"

const FileInput = ({setFiles, name = ""}) => {
  const inputFile = useRef(null);
  const [isDragEnter, setIsDragEnter] = useState(false); 
  
  const handleOpenFileUploading = (e) => {
    if(inputFile.current != null)
    {
      inputFile.current.click();
    }
  }

  const handleChange = (e) => {
    if(e.target.files && e.target.files[0])
    {
      setFiles([...e.target.files])
    }
  }

  const handleDrag = (e) => {
    e.preventDefault();
    setIsDragEnter(true);
  }

  const handleDragLeave = (e) => {
    e.preventDefault();
    setIsDragEnter(false);
  }

  const handleDrop = (e) => {
    e.preventDefault();
    if(e.dataTransfer.files && e.dataTransfer.files[0])
    {
      setFiles([...e.dataTransfer.files]);
    }
  }

  return (
    <div className={`${styles.FileInput__Main} ${isDragEnter ? styles.FileInput__DragEnter : ""}`}
      onDragEnter={handleDrag}
      onDragOver={handleDrag}
      onDragLeave={handleDragLeave}
      onDrop={handleDrop}>
      <p>Drop Files Here</p>
      <p>or</p>
      <p className={styles.FileInput__BtnUpload} onClick={handleOpenFileUploading}>Upload Files</p>
      <input className={styles.FileInput__InputFile} 
        name={name}
        ref={inputFile} 
        type="file" 
        multiple={true} 
        accept="image/png, image/jpeg"
        onChange={handleChange}/>
    </div>
  )
}

export default FileInput;