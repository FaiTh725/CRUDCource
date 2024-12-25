import styles from "./ImageSlider.module.css"

import { useEffect, useState } from "react";

import noImage from "../../assets/Empty.jpg"

const ImageSlider = ({images=[]}) => {
  const [currentImage, setCurrentImage] = useState(null);
  const [prevImage, setPrevImage] = useState(null);
  const [nextImage, setNextImage] = useState(null);
  const [centerImageStyles, setCenterImageStyles] = useState(styles.ImageSlider__ImageContainer);
  const [prevImageStyles, setPrevImageStyles] = useState(styles.ImageSlider__PrevImage);
  const [nextImageStyles, setNextImageStyles] = useState(styles.ImageSlider__NextImage);

  useEffect(() => {

    if(images.length > 0)
    {
      setCurrentImage(0);
    }
    else
    {
      images.push(noImage);
      setCurrentImage(0);
    }

    if(images.length > 1)
    {
      setNextImage(1);
    }
  }, [images]);

  const handleAnimationEndCenter = (e) => {
    setCenterImageStyles(styles.ImageSlider__ImageContainer);

    if(e.animationName.includes('swith_left_main'))
    {
      setNextImageStyles(styles.ImageSlider__NextImage);
      
      setPrevImage(currentImage);
      setCurrentImage(currentImage + 1);
      setNextImage(nextImage + 1);
    }
    else
    {
      setPrevImageStyles(styles.ImageSlider__PrevImage);
      
      setNextImage(currentImage);
      setCurrentImage(currentImage - 1);
      setPrevImage(prevImage - 1);
      
    }
  }

  const handleNext = () => {
    if(currentImage + 1 != images.length)
    {
      setCenterImageStyles(`${styles.ImageSlider__ImageContainer} ${styles.ImageSlider__SwithLeft}`);
      setNextImageStyles(`${styles.ImageSlider__NextImage} ${styles.ImageSlider__NextAnim}`);
    }
    else
    {
      setNextImage(null);
    }
  }

  const handlePrev = () => {
    if(currentImage != 0)
    {
      setCenterImageStyles(`${styles.ImageSlider__ImageContainer} ${styles.ImageSlider__SwithRight}`);
      setPrevImageStyles(`${styles.ImageSlider__PrevImage} ${styles.ImageSlider__PrevAnim}`);
    }
    else
    {
      setPrevImage(null);

    }
  }

  return (
    <div className={styles.ImageSlider__Main}>
      <div className={centerImageStyles}
        onAnimationEnd={handleAnimationEndCenter}
        style={{backgroundImage: `url(${currentImage != null ? images[currentImage] : ""})`}}>
      </div>
      <div className={prevImageStyles}
        style={{backgroundImage: `url(${prevImage != null ? images[prevImage] : ""})`}}></div>
      <div className={nextImageStyles}
        style={{backgroundImage: `url(${nextImage != null ? images[nextImage] : ""})`}}></div>
      <div className={styles.ImageSlider__ImageSwitcher} onClick={(e) => {e.stopPropagation()}}>
        <div className={styles.ImageSlider__Button} onClick={handlePrev}></div>
        <div className={`${styles.ImageSlider__Button} ${styles.ImageSlider__CenterButton}`}></div>
        <div className={styles.ImageSlider__Button} onClick={handleNext}></div>
      </div>
    </div>
  )
}

export default ImageSlider;