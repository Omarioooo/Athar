import api from "../Auth/AxiosInstance";

export function followCharityPost(id) {
    return api.post(`/follow/${id}`);
}

export function unFollowCharityDelete(id) {
    return api.delete(`/unfollow/${id}`);
}

export function isFollowCharityGet(id) {
    return api.get(`/isfollowed/${id}`);
}

export function followersCountGet(id) {
    return api.get(`/followerscount/${id}`);
}
