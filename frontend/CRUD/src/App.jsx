import { BrowserRouter, Route, Routes } from 'react-router-dom'
import './App.css'
import Home from './pages/Home/Home'
import NotFound from './pages/NotFound/NotFound'
import AuthLogin from './pages/AuthLogin/AuthLogin'
import AuthRegister from './pages/AuthRegister/AuthRegister'
import { AuthProvider } from './components/Auth/AuthContext'
import ProtectedRoutes from './components/Auth/ProtectedRoutes'
import ShopingCart from './pages/ShopingCart/ShopingCart'
import Account from './pages/Account/Account'
import AddProduct from './pages/AddProduct/AddProduct'
import Header from './components/Header/Header'
import Product from './pages/Product/Product'
import AuthAcccount from './components/Auth/AuthAccount'

const App = () => {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route element={<Header/>}>
            <Route path='/' element={<Home/>}/>
            <Route path='/account/*' element={<AuthLogin/>}/>
            <Route path='/account/register' element={<AuthRegister/>}/>
            <Route path='/*' element={<NotFound/>}/>
            <Route element={<AuthAcccount/>}>
              <Route path='/products/*' element={<Product/>}/>
            </Route>
            <Route element={<ProtectedRoutes roles={["User"]}/>}>
              <Route path='/account/shopingcart' element={<ShopingCart/>}/>
            </Route>
            <Route element={<ProtectedRoutes roles={["Admin", "User", "Seller"]}/>}>
              <Route path='/user' element={<Account/>}/>
            </Route>
            <Route element={<ProtectedRoutes roles={["Admin", "Seller"]}/>}>
              <Route path='/add_product' element={<AddProduct/>}/>
            </Route>
          </Route>
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  )
}

export default App
