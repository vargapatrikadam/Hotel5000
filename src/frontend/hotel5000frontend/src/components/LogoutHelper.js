import {BaseUrl} from './FetchHelper'
//????????????????????????
function logout() {
    const data = {
        "refreshToken": localStorage.getItem('refreshToken')
    }
    return fetch(BaseUrl+"api/auth/logout", {
        method: 'POST',
        mode: 'cors',
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(data)
    })
        .then(response => {
            if(response.status === 200){
                localStorage.setItem('accessToken', "")
                localStorage.setItem('refreshToken', "")
                localStorage.setItem('loggedin', false)
                localStorage.setItem('role', "")
                localStorage.setItem('username', "")
                localStorage.setItem('email', "")
                window.location.href = '/'
            }
        })
}

export default logout;