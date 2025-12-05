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