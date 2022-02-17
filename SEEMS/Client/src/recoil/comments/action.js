import { get, post, put, remove } from '../../utils/ApiCaller'

const useCommentsAction = () => {
    const loadComments = () => {
        return get({
            endpoint: '/api/comments/1',
        })
    }
    const createComment = (commentData) => {
        return post({
            endpoint: '/api/comments',
            body: commentData,
        })
    }
    const deleteComment = (commentId) => {
        return remove({
            endpoint: `/api/comments/${commentId}`,
        })
    }
    const editComment = (commentId, commentContent) => {
        return put({
            endpoint: `/api/comments/${commentId}`,
            body: { commentContent: commentContent },
        })
    }
    return {
        loadComments,
        createComment,
        deleteComment,
        editComment,
    }
}

export default useCommentsAction
