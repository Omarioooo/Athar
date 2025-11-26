import axios from "axios";

const API_URL = "https://localhost:44389/api/Campaign";

export const fetchAllCampaigns=async(page=1,pagesize=12)=>{
    const response=await axios.get(`${API_URL}/GetAll`,{
params:{page,pagesize}

    });
    console.log(response.data);
    return{
        compaign:response.data.items,
        totalpage:response.data.totalPages

        
    }
}

export const fetchCampaignsByType = async (type) => {
  const response = await axios.get(`${API_URL}/GetByType`, {
    params: { type },
  });
  return response.data.items || response.data;
};
export const SearchCompaign = async (keyword) => {
  try {
    const response = await axios.get(`${API_URL}/Search`, {
      params: { keyword },
    });
    
    return response.data.items || response.data;
  } catch (error) {
    
    
        alert("no compaigns found"); 
         return [];
    
  }
};
