import HeaderNavigation from "../HeaderNavigation/HeaderNavigation";
import styles from "./Header.module.css"

import logo from "../../assets/Logo.png"
import { Outlet } from "react-router-dom";
import HeaderNews from "../HeaderNews/HeaderNews";

const Header = () => {
  return (
    <div className={styles.Header__Main}>
      <HeaderNews/>
      <div className={styles.Header__StickyHeader}>
        <div className={styles.Header__Body}>
          <a href="/" className={styles.Header__Logo}><img src={logo} alt="logo" height={100}/></a>
          <div className={styles.Header__UserNavigation}>
            <a href="/add_product"><svg height="30px" width="30px" viewBox="0 0 48 48" fill="none" xmlns="http://www.w3.org/2000/svg"><g id="SVGRepo_bgCarrier" strokeWidth="0"></g><g id="SVGRepo_tracerCarrier" strokeLinecap="round" strokeLinejoin="round"></g><g id="SVGRepo_iconCarrier"> <path d="M16 6H8C6.89543 6 6 6.89543 6 8V16" stroke="#222" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round"></path> <path d="M16 42H8C6.89543 42 6 41.1046 6 40V32" stroke="#222" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round"></path> <path d="M32 42H40C41.1046 42 42 41.1046 42 40V32" stroke="#222" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round"></path> <path d="M32 6H40C41.1046 6 42 6.89543 42 8V16" stroke="#222" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round"></path> <path d="M32 24L16 24" stroke="#222" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round"></path> <path d="M24 32L24 16" stroke="#222" strokeWidth="4" strokeLinecap="round" strokeLinejoin="round"></path> </g></svg></a>
            <a href="#"><svg fill="#222" height="30px" width="30px" version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink" viewBox="0 0 488.4 488.4" xmlSpace="preserve"><g id="SVGRepo_bgCarrier" strokeWidth="0"></g><g id="SVGRepo_tracerCarrier" strokeLinecap="round" strokeLinejoin="round"></g><g id="SVGRepo_iconCarrier"> <g> <g> <path d="M0,203.25c0,112.1,91.2,203.2,203.2,203.2c51.6,0,98.8-19.4,134.7-51.2l129.5,129.5c2.4,2.4,5.5,3.6,8.7,3.6 s6.3-1.2,8.7-3.6c4.8-4.8,4.8-12.5,0-17.3l-129.6-129.5c31.8-35.9,51.2-83,51.2-134.7c0-112.1-91.2-203.2-203.2-203.2 S0,91.15,0,203.25z M381.9,203.25c0,98.5-80.2,178.7-178.7,178.7s-178.7-80.2-178.7-178.7s80.2-178.7,178.7-178.7 S381.9,104.65,381.9,203.25z"></path> </g> </g> </g></svg></a>
            <a href="/user"><svg fill="#222" height="30px" width="30px" viewBox="0 0 32 32" version="1.1" xmlns="http://www.w3.org/2000/svg" stroke="#222"><g id="SVGRepo_bgCarrier" strokeWidth="0"></g><g id="SVGRepo_tracerCarrier" strokeLinecap="round" strokeLinejoin="round"></g><g id="SVGRepo_iconCarrier"> <title>user</title> <path d="M16 16.75c4.28 0 7.75-3.47 7.75-7.75s-3.47-7.75-7.75-7.75c-4.28 0-7.75 3.47-7.75 7.75v0c0.005 4.278 3.472 7.745 7.75 7.75h0zM16 2.75c3.452 0 6.25 2.798 6.25 6.25s-2.798 6.25-6.25 6.25c-3.452 0-6.25-2.798-6.25-6.25v0c0.004-3.45 2.8-6.246 6.25-6.25h0zM30.41 29.84c-1.503-6.677-7.383-11.59-14.41-11.59s-12.907 4.913-14.391 11.491l-0.019 0.099c-0.011 0.048-0.017 0.103-0.017 0.16 0 0.414 0.336 0.75 0.75 0.75 0.357 0 0.656-0.25 0.731-0.585l0.001-0.005c1.351-5.998 6.633-10.41 12.945-10.41s11.594 4.413 12.929 10.322l0.017 0.089c0.076 0.34 0.374 0.59 0.732 0.59 0 0 0.001 0 0.001 0h-0c0.057-0 0.112-0.007 0.165-0.019l-0.005 0.001c0.34-0.076 0.59-0.375 0.59-0.733 0-0.057-0.006-0.112-0.018-0.165l0.001 0.005z"></path> </g></svg></a>
            <a href="/account/shopingcart"><svg fill="#222" height="30px" width="30px" version="1.1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" xmlnsXlink="http://www.w3.org/1999/xlink" enableBackground="new 0 0 512 512" stroke="#222"><g id="SVGRepo_bgCarrier" strokeWidth="0"></g><g id="SVGRepo_tracerCarrier" strokeLinecap="round" strokeLinejoin="round"></g><g id="SVGRepo_iconCarrier"> <g> <g> <path d="m480.6,11h-89.9c-8.9,0-16.8,5.8-19.5,14.3l-82.9,264.2h-178l-50.4-148.5h187.6c11.3,0 20.4-9.1 20.4-20.4 0-11.3-9.1-20.4-20.4-20.4h-216c-6.6,0-12.8,3.2-16.6,8.5-3.8,5.3-4.9,12.2-2.7,18.4l64.2,189.4c2.8,8.3 10.6,13.9 19.3,13.9h207.7c8.9,0 16.8-5.8 19.5-14.3l82.9-264.2h74.9c11.3,0 20.4-9.1 20.4-20.4-0.1-11.4-9.2-20.5-20.5-20.5z"></path> <path d="m114.3,368.2c-36.5,0-66.3,29.8-66.3,66.4 0,36.6 29.7,66.4 66.3,66.4 36.6,0 66.3-29.8 66.3-66.4 0-36.6-29.7-66.4-66.3-66.4z"></path> <path d="m277.4,368.2c-36.6,0-66.3,29.8-66.3,66.4 0,36.6 29.7,66.4 66.3,66.4 36.6,0 66.3-29.8 66.3-66.4 0-36.6-29.8-66.4-66.3-66.4z"></path> </g> </g> </g></svg></a>
          </div>
        </div>
        <HeaderNavigation/>
      </div>
      <Outlet/>
    </div>
  )
}

export default Header;