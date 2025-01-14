import { useEffect, useMemo, useRef, useState } from "react";
import styles from "./BigImageSlider.module.css";
import HeaderButton from "../buttons/header_button/HeaderButton";

const BigImageSlider = ({images = []}) => {
  const imageContainer = useRef(null);
  const [imageIndex, setImageIndex] = useState(0);
  const [isMouseDown, setIsMouseDown] = useState(false);
  const [mouseDownCoord, setMouseDownCoord] = useState(0);
  
  useEffect(() => {
    if(images.length != 0)
    {
      setImageIndex(1);
    }
  }, [images]);

  const withScrollForOneImage = useMemo(() => {
    if(imageContainer.current != null)
    {
      return imageContainer.current.scrollWidth/images.length;
    }
    
    return 0;
  }, [images]);


  const handleScrollByMouse = (e) => {
    var bounds = e.currentTarget.getBoundingClientRect();
    
    if(imageContainer.current != null && isMouseDown)
      {
        const coord = e.clientX - bounds.left;

        const coordDif = coord > withScrollForOneImage / 2 ? 
          coord - withScrollForOneImage / 2 : 
          coord > 0 ? (-1) * (withScrollForOneImage / 2 - coord) : (-1) * (Math.abs(coord) + withScrollForOneImage / 2);
  
        imageContainer.current.scrollTo({
        left: (mouseDownCoord - coordDif),
        behavior: "smooth"
      });
    }

  }

  const handleStopScrollByMouse = () => {
    setIsMouseDown(false)

    if(imageContainer.current != null)
    {
      const imageStartCoord = imageContainer.current.scrollLeft / withScrollForOneImage
      const postScroll = Math.round(imageStartCoord) * withScrollForOneImage;
      setImageIndex(Math.round(imageStartCoord) + 1);
      
      imageContainer.current.scrollTo({
        left: postScroll,
        behavior: "smooth"
      });
    }
  }

  const handleStartScrollByMouse = () => {
    setIsMouseDown(true);

    if(imageContainer.current != null)
    {
      setMouseDownCoord(imageContainer.current.scrollLeft);
    }
  }

  const handleScrolRight = () => {
    const imageCon = imageContainer.current; 
    if(imageCon != null && imageCon.scrollLeft + withScrollForOneImage <= imageCon.scrollWidth)
    {
      imageCon.scrollTo({
        left: imageCon.scrollLeft + withScrollForOneImage,
        behavior: "smooth"
      });
    }

    if(imageIndex < images.length)
    {
      setImageIndex(imageIndex + 1);
    }
  }

  const handleScrolLeft = () => {
    const imageCon = imageContainer.current; 
    if(imageCon != null && imageCon.scrollLeft - withScrollForOneImage >= 0)
    {
      imageCon.scrollTo({
        left: imageCon.scrollLeft - withScrollForOneImage,
        behavior: "smooth"
      });
    }

    if(imageIndex != 1)
    {
      setImageIndex(imageIndex - 1);
    }
  }
  
  return (
    <div className={styles.BigImageSlider__Main}>
      <div className={styles.BigImageSlider__ImageContainer}
        ref={imageContainer}
        onMouseMove={handleScrollByMouse}
        onMouseDown={handleStartScrollByMouse}
        onMouseUp={handleStopScrollByMouse}>
        {
          images.map((image, index) => (
            <div className={styles.BigImageSlider__Image} key={index} style={{backgroundImage: `url(${image})`}}></div>
          ))
        }
      </div>
      <div className={styles.BigImageSlider__Btn}>
        <HeaderButton action={handleScrolLeft}><svg fill="#000" height="15px" width="15px" version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink" viewBox="0 0 330 330" xmlSpace="preserve" stroke="#000" transform="matrix(-1, 0, 0, 1, 0, 0)"><g id="SVGRepo_bgCarrier" strokeWidth="1"></g><g id="SVGRepo_tracerCarrier" strokeLinecap="roubd" strokeLinejoin="round"></g><g id="SVGRepo_iconCarrier"> <path id="XMLID_222_" d="M250.606,154.389l-150-149.996c-5.857-5.858-15.355-5.858-21.213,0.001 c-5.857,5.858-5.857,15.355,0.001,21.213l139.393,139.39L79.393,304.394c-5.857,5.858-5.857,15.355,0.001,21.213 C82.322,328.536,86.161,330,90,330s7.678-1.464,10.607-4.394l149.999-150.004c2.814-2.813,4.394-6.628,4.394-10.606 C255,161.018,253.42,157.202,250.606,154.389z"></path> </g></svg></HeaderButton>
      </div>
      <div className={`${styles.BigImageSlider__Btn} ${styles.BigImageSlider__RightBtn}`}>
        <HeaderButton action={handleScrolRight}><svg fill="#000" height="15px" width="15px" version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink" viewBox="0 0 330 330" xmlSpace="preserve" stroke="#fff"><g id="SVGRepo_bgCarrier" strokeWidth="1"></g><g id="SVGRepo_tracerCarrier" strokeLinecap="roubd" strokeLinejoin="round"></g><g id="SVGRepo_iconCarrier"> <path id="XMLID_222_" d="M250.606,154.389l-150-149.996c-5.857-5.858-15.355-5.858-21.213,0.001 c-5.857,5.858-5.857,15.355,0.001,21.213l139.393,139.39L79.393,304.394c-5.857,5.858-5.857,15.355,0.001,21.213 C82.322,328.536,86.161,330,90,330s7.678-1.464,10.607-4.394l149.999-150.004c2.814-2.813,4.394-6.628,4.394-10.606 C255,161.018,253.42,157.202,250.606,154.389z"></path> </g></svg></HeaderButton>
      </div>
      {
          images.length > 0 && (
            <p className={styles.BigImageSlider__IndexImage}>{imageIndex}/{images.length}</p>
          )
        }
    </div>
  )
}

export default BigImageSlider;