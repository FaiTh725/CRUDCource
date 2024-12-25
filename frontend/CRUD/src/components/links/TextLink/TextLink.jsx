import styles from "./TextLink.module.css"

const TextLink = ({text, url}) => {

  return (
    <a href={url} className={styles.TextLink__Link}>{text}</a>
  )
}

export default TextLink;