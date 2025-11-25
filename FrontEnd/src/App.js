import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import './App.css';
import 'bootstrap/dist/css/bootstrap.rtl.min.css';
import { motion } from "framer-motion";
import Home from './pages/home/home'
import Campaign from './pages/campaign/campaign';
import Content from './pages/content/content';
import Register from './pages/register/register';
import Login from './pages/login/login';
import CharityRegister from './pages/charityregister/charityregister';
import DonorRegister from './pages/donorregister/donorregister';
import { Link } from "react-router-dom";
import '@fortawesome/fontawesome-free/css/all.min.css';
function App() {
  return (
    <div className="App">
      <Router>
          <nav className="navbar navbar-expand-lg navbar-light bg-light">
          <div className="container-fluid">
            <Link className="navbar-brand" to="/">اثر</Link>
             
            <ul className="navbar-nav">
           
              <li className="nav-item">
                <Link className="nav-link" to="/skills">الجمعيات</Link>
              </li>
             
              <li className="nav-item">
                <Link className="nav-link" to="/content">الميديا</Link>
              </li>
               <li className="nav-item">
                <Link className="nav-link" to="/Campaign">الحملات</Link>
                  </li>
              
                  <li className="nav-item">
                <Link className="nav-link" to="/register">انشاء حساب</Link>
              </li>
              
                  <li className="nav-item">
                <Link className="nav-link" to="/login">تسجيل الدخول</Link>
              </li>
            </ul>
          </div>
        </nav>
       <Routes>
          
              <Route path="/" element={<Home/>}/>
              <Route path="/Campaign" element={<Campaign/>}/>
               <Route path="/content" element={<Content/>}/>
                  <Route path="/login" element={<Login/>}/>
                   <Route path="/register" element={<Register/>}/>
                      <Route path="/charityregister" element={<CharityRegister/>}/>
                        <Route path="/donorregister" element={<DonorRegister/>}/>
                
   
       </Routes>
       </Router>
    
    </div>
  );
}

export default App;
