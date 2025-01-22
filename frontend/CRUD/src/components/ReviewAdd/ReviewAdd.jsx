import { useRef, useState } from "react";
import EmptyButton from "../buttons/empty_button/EmptyButton";
import InputRating from "../inputs/rating_star/InputRating";
import SimpleArea from "../inputs/simple_area/SimpleArea";
import styles from "./ReviewAdd.module.css";
import useLogout from "../../hooks/useLogOut";
import { useAuth } from "../Auth/AuthContext";
import axios from "axios";

// TODO Add mini message 
const ReviewAdd = ({productId}) => {
  const [feedBack, setFeedBack] = useState({
    rate: null,
    text: "",
    images: []
  });
  const fileInputRef = useRef();
  const logout = useLogout();
  const auth = useAuth();
  const [error, setError] = useState("");

  const handleFeedBackChanged = (newValue, key) => {
    if(error !== "")
    {
      setError("");
    }

    setFeedBack(prev => ({
      ...prev,
      [key]: newValue
    }));
  }

  const handleOpenFileInput = () => {
    if(fileInputRef.current != null)
    {
      fileInputRef.current.click();
    }
  }

  const handleUploadFile = (e) => {
    if(e.target.files && e.target.files[0])
    {
      setFeedBack(prev => ({
        ...prev,
        images: [...e.target.files]
      }));
    }
  } 

  const handleAddFeedBack = async () => {
    if(auth.user == null)
    {
      logout();
    }

    if(feedBack.rate == null)
    {
      setError("Rate is required");
      return;
    }

    try
    {
      const data = new FormData();
      data.append("ProductId", Number.parseInt(productId));
      data.append("EmailAccount", auth.user.email);
      data.append("Text", feedBack.text);
      data.append("Rate", Number.parseInt(feedBack.rate));

      if(feedBack.images.length > 0)
      {
        feedBack.images.forEach(image => {
          data.append("FeedBackImages", image);
        })
      }
      
      const response = await axios.post("https://localhost:5202/api/FeedBack/AddFeedBack", data,
        {
          headers: {
            "Content-Type": "multipart/form-data"
          },
          withCredentials: true
        }
      );

      if(response.data.statusCode !== 0)
      {
        setError(response.data.description);
        return;
      }

      setFeedBack({
        rate: null,
        text: "",
        images: []
      });
    }
    catch(error)
    {
      if(error.status === 401)
      {
        logout();
      }
      else if(error.status === 400)
      {
        setError("Please form correct review");
      }

      console.log(error.message);
    }
  }

  return (
    <div className={styles.ReviewAdd__RatingContent}>
      <InputRating
        rate={feedBack.rate}
        setRate={(rate) => {handleFeedBackChanged(rate, "rate")}}/>
      <div className={styles.ReviewAdd__RatingAddForm}>
        <SimpleArea
          action={(e) => {handleFeedBackChanged(e.target.value, e.target.name)}}
          value={feedBack.text}
          name="text"
          maxLenght={300}
          labelName={"Review"}/>
          <p className={styles.ReviewAdd__Error}>{error}</p>
        <div className={styles.ReviewAdd__RatingAddFromBtn}>
          <div className={styles.ReviewAdd__RatingAddFormBtnWrapper}>
            <input className={styles.ReviewAdd__FileInput} 
              ref={fileInputRef} 
              type="file" 
              multiple={true}
              accept="image/png, image/jpeg"
              onChange={handleUploadFile}/>
            <p>
              {
                feedBack.images.length > 0 &&
                `${feedBack.images.length} files selected`
              }
            </p>
            <EmptyButton
              action={handleOpenFileInput}>
              <div className={styles.ReviewAdd__BtnContent}>
                <svg fill="#222" height="25px" width="25px" version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink" viewBox="0 0 512 512" xmlSpace="preserve" stroke="#222"><g id="SVGRepo_bgCarrier" strokeWidth="0"></g><g id="SVGRepo_tracerCarrier" strokeLinecap="round" strokeLinejoin="round"></g><g id="SVGRepo_iconCarrier"> <g> <g> <path d="M256,0c-54.013,0-97.955,43.943-97.955,97.955v338.981c0,41.39,33.674,75.064,75.064,75.064 c41.39,0,75.064-33.674,75.064-75.064V122.511c0-28.327-23.046-51.375-51.375-51.375c-28.327,0-51.374,23.047-51.374,51.375 v296.911h31.347V122.511c0-11.042,8.984-20.028,20.028-20.028s20.028,8.985,20.028,20.028v314.424 c0,24.106-19.612,43.717-43.718,43.717c-24.106,0-43.717-19.612-43.717-43.717V97.955c0-36.727,29.88-66.608,66.608-66.608 s66.608,29.881,66.608,66.608v321.467h31.347V97.955C353.955,43.943,310.013,0,256,0z"></path> </g> </g> </g></svg>
              </div>
            </EmptyButton>
            <EmptyButton
            action={handleAddFeedBack}>
              <div className={styles.ReviewAdd__BtnContent}>
                <svg fill="#222" height="25px" width="25px" version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink" viewBox="0 0 491.022 491.022" xmlSpace="preserve" stroke="#fff" transform="rotate(0)matrix(-1, 0, 0, 1, 0, 0)"><g id="SVGRepo_bgCarrier" strokeWidth="0"></g><g id="SVGRepo_tracerCarrier" strokeLinecap="round" strokeLinejoin="round"></g><g id="SVGRepo_iconCarrier"> <g> <g> <path d="M490.916,13.991c-0.213-1.173-0.64-2.347-1.28-3.307c-0.107-0.213-0.213-0.533-0.32-0.747 c-0.107-0.213-0.32-0.32-0.533-0.533c-0.427-0.533-0.96-1.067-1.493-1.493c-0.427-0.32-0.853-0.64-1.28-0.96 c-0.213-0.107-0.32-0.32-0.533-0.427c-0.32-0.107-0.747-0.32-1.173-0.427c-0.533-0.213-1.067-0.427-1.6-0.533 c-0.64-0.107-1.28-0.213-1.92-0.213c-0.533,0-1.067,0-1.6,0c-0.747,0.107-1.493,0.32-2.133,0.533 c-0.32,0.107-0.747,0.107-1.067,0.213L6.436,209.085c-5.44,2.347-7.893,8.64-5.547,14.08c1.067,2.347,2.88,4.373,5.227,5.44 l175.36,82.453v163.947c0,5.867,4.8,10.667,10.667,10.667c3.733,0,7.147-1.92,9.067-5.12l74.133-120.533l114.56,60.373 c5.227,2.773,11.627,0.747,14.4-4.48c0.427-0.853,0.747-1.813,0.96-2.667l85.547-394.987c0-0.213,0-0.427,0-0.64 c0.107-0.64,0.107-1.173,0.213-1.707C491.022,15.271,491.022,14.631,490.916,13.991z M190.009,291.324L36.836,219.218 L433.209,48.124L190.009,291.324z M202.809,437.138V321.831l53.653,28.267L202.809,437.138z M387.449,394.898l-100.8-53.013 l-18.133-11.2l-0.747,1.28l-57.707-30.4L462.116,49.298L387.449,394.898z"></path> </g> </g> </g></svg>
              </div>
            </EmptyButton>
          </div>
        </div>
      </div>
    </div>
  )
}

export default ReviewAdd;