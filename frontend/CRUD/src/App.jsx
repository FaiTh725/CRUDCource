import { BrowserRouter, Route, Routes } from 'react-router-dom'
import './App.css'
import Home from './pages/Home/Home'
import NotFound from './pages/NotFound/NotFound'
import AuthLogin from './pages/AuthLogin/AuthLogin'
import AuthRegister from './pages/AuthRegister/AuthRegister'
import { AuthProvider } from './components/Auth/AuthContext'
import ProtectedRoutes from './components/Auth/ProtectedRoutes'
import ShopingCart from './pages/ShopingCart/ShopingCart'

const App = () => {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path='/' element={<Home/>}/>
          <Route path='/account/*' element={<AuthLogin/>}/>
          <Route path='/account/register' element={<AuthRegister/>}/>
          <Route path='/*' element={<NotFound/>}/>
          <Route element={<ProtectedRoutes roles={["User"]}/>}>
            <Route path='/account/shopingcart' element={<ShopingCart/>}/>
          </Route>
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  )
}

export default App
