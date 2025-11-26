import react from 'react'
import {useState}from 'react'
import { Link } from "react-router-dom";

const Login=()=>{
  const[user,setUser] =useState({
   
    email:"",
    password:""

  })
  const[error,seterror]=useState({
    
    emailerror:null,
    passworderror:null
  })
  const handleChange=(ev)=>{
    const fieldname=ev.target.name;
    const fieldvalue=ev.target.value;
    if(fieldname=="password"){
        setUser({
         ...user,password:fieldvalue
        })
        let errors='';
        if(fieldvalue.length==0){
        errors='هذا الحقل مطلوب'
        }
        else if(fieldvalue.length>10){
            errors='طول الاسم يجب ان يكون اقل من 10حروف'
        }
        seterror({
            ...error,passworderror:errors
        })
    }
    if (fieldname === "email") {
        setUser({ ...user, email: fieldvalue });

        let errors = '';
       
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (fieldvalue.length === 0) {
            errors = 'هذا الحقل مطلوب';
        } else if (!emailRegex.test(fieldvalue)) {
            errors = 'الايميل غير صالح';
        }

        seterror({ ...error, emailerror: errors });
    }
  }
 return(
    <div className="login-section ">
        <h1 className='login-h1'>منصة اثر</h1>
        <p className='login-p'>منصةالعطاء والتبرعات</p>
        <div className='login-smallsection'>
    <form>
        <h4 className='login-h4'>تسجيل الدخول</h4>
        <p className='login-p2'>ادخل بياناتك للوصول الى حسابك</p>
      <div className="mb-3">
        <label htmlFor="email" className="form-label">الايميل</label>
     <input 
  type="text"    
  style={{ direction: "rtl" }}    
  className={`form-control login-input ${error.emailerror ? 'border-danger' : ''}`} 
  name="email" 
  value={user.email}
  onChange={handleChange} 
  placeholder="ادخل الايميل" 
/>
         <small className="text-danger">{error.emailerror}</small>
        
      </div>
      <div className="mb-3">
        <label htmlFor="password" className="form-label">كلمة المرور</label>
        <input 
          type="password" 
           style={{ direction: "rtl" }}
          className={`form-control login-input ${error.passworderror ? 'border-danger' : ''}`} 
          name="password" 
          value={user.password}
          onChange={handleChange} 
          placeholder="ادخل كلمة المرور" 
          
        />
         <small className="text-danger">{error.passworderror}</small>
       <button type="submit" class=" login-btn">تسجيل  الدخول</button>
       <hr/>
       <div className='login-div'>
       <span>ليس لديك حساب؟</span>
       <Link to="/register" className="a-login">
  انشاء حساب جديد
</Link>
       </div>
      </div>
    </form>
    </div>
    </div>
 )

 
}
export default Login