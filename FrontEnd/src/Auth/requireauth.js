import { UseAuth } from "../Auth/Auth"
import { useNavigate } from "react-router-dom";
import { Navigate } from "react-router-dom";
export const RequireAuth=({children})=>{
const auth=UseAuth();
if(!auth.user){
    return <Navigate to="/login"/>
}
return children
}
