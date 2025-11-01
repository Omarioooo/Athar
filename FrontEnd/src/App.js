import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import './App.css';
import 'bootstrap/dist/css/bootstrap.rtl.min.css';
import { motion } from "framer-motion";
import Home from './home/home.js'
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
                <Link className="nav-link" to="/projects">الميديا</Link>
              </li>
               <li className="nav-item">
                <Link className="nav-link" to="/">الحملات</Link>
              </li>
                  <li className="nav-item">
                <Link className="nav-link" to="/skills">تسجيل الدخول</Link>
              </li>
            </ul>
          </div>
        </nav>
       <Routes>
          
              <Route path="/" element={<Home/>}/>
                
   
       </Routes>
       </Router>
    
    </div>
  );
}

export default App;
