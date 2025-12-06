import {
    followCharityPost,
    unFollowCharityDelete,
    isFollowCharityGet,
    followersCountGet,
} from "../Repository/followRepository";

export async function followCharity(id) {
    try {
        const res = await followCharityPost(id);
        return res.data;
    } catch (err) {
        throw err.response?.data || { message: "Unexpected error" };
    }
}

export async function unFollowCharity(id) {
    try {
        const res = await unFollowCharityDelete(id);
        return res.data;
    } catch (err) {
        throw err.response?.data || { message: "Unexpected error" };
    }
}

export async function isCharityFollowed(id) {
    try {
        const res = await isFollowCharityGet(id);
        return res.data;
    } catch (err) {
        return false;
    }
}

export async function getFollowersCount(id) {
    try {
        const res = await followersCountGet(id);
        return res.data;
    } catch (err) {
        return 0;
    }
}
