import api from "../Auth/AxiosInstance";

export const fetchAllContents = async (page = 1, pageSize = 12) => {
    const response = await api.get(`/Content/GetAll`, {
        params: { page, pageSize },
    });
    return {
        contents: response.data.items,
        totalPages: response.data.totalPages,
    };
};

export const searchContents = async (word) => {
    const response = await api.get(`/Content/search`, {
        params: { word },
    });
    return response.data.items || [];
};

export const getMediaOfCampaign = async (id, page = 1, pageSize = 12) => {
    const response = await api.get(`/Content/campaign/${id}/paged`, {
        params: { page, pageSize },
        
    });
    console.log(response)
    return {
        contents: response.data.items,
        totalPages: response.data.totalPages,
    };
};
export const ReacttoMedia=async(contentId, react = true)=>{
    if(react){
        return await api.post(`/Reaction/${contentId}`,{},{ withCredentials: true });
    }
    else{
        return await api.delete(`/Reaction/${contentId}`, { withCredentials: true });
    }
}
export const getcountofreaction=async(contentid)=>{
    const response= await api.get(`/Reaction/${contentid}/reactions/count`, { withCredentials: true });
    return response.data;
}
export const createContent = async (formData) => {
  try {
    const response = await api.post("/Content/create", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
      withCredentials: true, 
    });
    return response.data;
  } catch (error) {
    console.error("خطأ في إنشاء المحتوى:", error.response?.data || error.message);
    throw error;
  }
};
export const deletecontent=async(contentid)=>{
    try{
    const response= await api.delete(`/Content/${contentid}`);
    return response.data;
    }
    catch (error) {
    console.error("خطأ في حذف المحتوى:", error.response?.data || error.message);
    throw error;
  }
}
export const updatecontent = async (contentId, formData) => {
  try {
    const response = await api.put(`/Content/${contentId}`, formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
    return response.data;
  } catch (error) {
    console.error("خطأ في تعديل المحتوى:", error.response?.data || error.message);
    throw error;
  }
};