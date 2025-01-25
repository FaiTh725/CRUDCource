import { BrowserRouter, Route, Routes } from 'react-router-dom'
import './App.css'
import Home from './pages/Home/Home'
import AuthLogin from './pages/AuthLogin/AuthLogin'
import AuthRegister from './pages/AuthRegister/AuthRegister'
import NotFound from './pages/NotFound/NotFound'
import Product from './pages/Product/Product'
import { CartContextProvider } from './components/Cart/CartContext'
import ProtectedRoutes from './components/Auth/ProtectedRoutes'
import Account from './pages/Account/Account'
import { AuthProvider } from './components/Auth/AuthContext'
import AddProduct from './pages/AddProduct/AddProduct'
import Header from './components/Header/Header'
import { SignalRProvider } from './components/SignalR/SignalRContext'
import SellerDisputs from './pages/SellerDisputs/SellerDisputs'
import { NotificationUserProvider } from './components/Notification/NotificationContext'

const App = () => {
  return (
  <BrowserRouter>
    <AuthProvider>
      <CartContextProvider>
        <NotificationUserProvider>
          <SignalRProvider>
            <Routes>
              <Route element={<Header/>}>
                <Route path='/' element={<Home/>}/>
                <Route path='/account/*' element={<AuthLogin/>}/>
                <Route path='/account/register' element={<AuthRegister/>}/>
                <Route path='/*' element={<NotFound/>}/>
                <Route path='/products/*' element={<Product/>}/>
                <Route element={<ProtectedRoutes roles={["User"]}/>}>
                </Route>
                <Route element={<ProtectedRoutes roles={["Admin", "User", "Seller"]}/>}>
                  <Route path='/user' element={<Account/>}/>
                </Route>
                <Route element={<ProtectedRoutes roles={["Admin", "Seller"]}/>}>
                  <Route path='/add_product' element={<AddProduct/>}/>
                  <Route path='/disputs_cats' element={<SellerDisputs/>}/>
                </Route>
              </Route>
            </Routes>
          </SignalRProvider>
        </NotificationUserProvider>
      </CartContextProvider>
    </AuthProvider>
  </BrowserRouter>
  )
}

export default App
