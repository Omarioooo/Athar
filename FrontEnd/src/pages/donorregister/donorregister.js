import react from 'react'
import {useState}from 'react'
import { Link } from "react-router-dom";

export default function DonorRegister(){
    const[user,setUser] =useState({
   
    email:"",
    firstname:"",
    lastname:"",
    password:"",
    confirmpassword:"", 
    country:"",
    city:"",  
    img:""

  })
  const[error,seterror]=useState({
    
    emailerror:null,
    passworderror:null,
   firstnamerror:null,
   lastnameerror:null,
    confirmpassworderror:null,
   
    
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
      if(fieldname=="firstname"){
        setUser({
         ...user,firstname:fieldvalue
        })
        let errors='';
        if(fieldvalue.length==0){
        errors='هذا الحقل مطلوب'
        }
        else if(fieldvalue.length>10){
            errors='طول الاسم يجب ان يكون اقل من 10حروف'
        }
        seterror({
            ...error,firstnamerror:errors
        })
    }
      if(fieldname=="lastname"){
        setUser({
         ...user,lastname:fieldvalue
        })
        let errors='';
        if(fieldvalue.length==0){
        errors='هذا الحقل مطلوب'
        }
        else if(fieldvalue.length>10){
            errors='طول الاسم يجب ان يكون اقل من 10حروف'
        }
        seterror({
            ...error,lastnamerror:errors
        })
    }
    if(fieldname=="password"){
        setUser({
            ...user,password:fieldvalue
        })
    }
        
     if (fieldname === "img") {
        setUser({
            ...user,
            document: ev.target.files[0]   
        });
    }
     if(fieldname=="confirmpassword"){
        setUser({
            ...user,confirmpassword:fieldvalue
        })
    }
      
       if(fieldname=="country"){
        setUser({
            ...user,country:fieldvalue
        })
    }
       if(fieldname=="city"){
        setUser({
            ...user,city:fieldvalue
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
<>
<div className='charityregister-section'>
  <div className="register-idiv2">
                <i class="fa-solid fa-circle-user register-i"></i>
          </div>
  <div className='charityregister-media'>
  <h3>تسجيل حساب متبرع</h3>
  <p>ادخل معلوماتك لانشاء حساب جديد</p>
    </div>
  <div className='charityregister-smallsection'>
    {/* //////////////// */}
    <form >

    <div  className='mt-4' >
      <label htmlFor="email" className="form-label">البريد الالكترونى*</label>
     <input 
     type="email"    
    style={{ direction: "rtl" }}    
    className={`form-control login-input ${error.emailerror ? 'border-danger' : ''} email-charityregister`} 
    name="email" 
    value={user.email}
    onChange={handleChange} 
    placeholder="ادخل الايميل" 
   />
         <small className="text-danger">{error.emailerror}</small>
        
   </div>
     {/* ////////////////// */}
   <div className='row'>
    <div className="col-md-6 mt-4">
      <label htmlFor="firstname" className="form-label">الاسم الاول*</label>
     <input 
     type="text"    
    style={{ direction: "rtl" }}    
    className={`form-control login-input ${error.passworderror ? 'border-danger' : ''} `} 
    name="firstname" 
    value={user.firstname}
    onChange={handleChange} 
    placeholder="اكتب اسمك" 
   />
         <small className="text-danger">{error.firstnamerror}</small>
        
   </div>
     {/* ////////////////// */}
      <div className="col-md-6 mt-4">
      <label htmlFor="lastname" className="form-label">الاسم الاخير*</label>
     <input 
     type="text"    
    style={{ direction: "rtl" }}    
    className={`form-control login-input ${error.charitynameerror ? 'border-danger' : ''} `} 
    name="lastname" 
    value={user.lastname}
    onChange={handleChange} 
    placeholder="اكتب اسمك الاخير" 
   />
         <small className="text-danger">{error.lastnameerror}</small>
        
   </div>

</div>


{/* ///////////////// */}
<div className='row'>
    <div className="col-md-6 mt-4">
      <label htmlFor="password" className="form-label">كلمة المرور*</label>
     <input 
     type="password"    
    style={{ direction: "rtl" }}    
    className={`form-control login-input ${error.passworderror ? 'border-danger' : ''} `} 
    name="password" 
    value={user.password}
    onChange={handleChange} 
    placeholder="......." 
   />
         <small className="text-danger">{error.passworderror}</small>
        
   </div>
     {/* ////////////////// */}
      <div className="col-md-6 mt-4">
      <label htmlFor="confirmpassword" className="form-label">تاكيد كلمة المرور*</label>
     <input 
     type="password"    
    style={{ direction: "rtl" }}    
    className={`form-control login-input ${error.charitynameerror ? 'border-danger' : ''} `} 
    name="confirmpassword" 
    value={user.confirmpassword}
    onChange={handleChange} 
    placeholder="........" 
   />
         <small className="text-danger">{error.confirmpassworderror}</small>
        
   </div>

</div>

{/* ///////////////// */}
<div className='row'>
    <div className="col-md-6 mt-4">
      <label htmlFor="country" className="form-label">الدولة*</label>
     <input 
     type="text"    
    style={{ direction: "rtl" }}    
    className={`form-control login-input `} 
    name="country" 
    value={user.country}
    onChange={handleChange} 
    placeholder="مصر" 
   />
        
        
   </div>
     {/* ////////////////// */}
      <div className="col-md-6 mt-4">
      <label htmlFor="city" className="form-label">المدينة*</label>
     <input 
     type="text"    
    style={{ direction: "rtl" }}    
    className={`form-control login-input `} 
    name="city" 
    value={user.city}
    onChange={handleChange} 
    placeholder="القاهرة" 
   />
        
        
   </div>

</div>
{/* ///////////////// */}
 
   {/* ///////////////// */}
 <div  className='mt-4'>
      <label htmlFor="document" className="form-label">صورة شخصية(اختيارى)</label>
     <input 
     type="file"    
    style={{ direction: "rtl" }}    
    className={`form-control login-input `} 
    name="document" 
   
    onChange={handleChange} 
   
   />
     {user.document && <small>الملف المختار: {user.img.name}</small>}   
        
   </div>
     <button type="submit" class=" login-btn">انشاء حساب</button>
 

</form>
  </div>
</div>
</>
    );
}