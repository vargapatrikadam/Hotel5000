import {BaseUrl} from './FetchHelper'
//????????????????????????
function logout() {
    const data = {
        "refreshToken": sessionStorage.getItem('refreshToken')
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
                sessionStorage.setItem('accessToken', "")
                sessionStorage.setItem('refreshToken', "")
                sessionStorage.setItem('loggedin', false)
                sessionStorage.setItem('role', "")
                sessionStorage.setItem('username', "")
                sessionStorage.setItem('email', "")
                window.location.href = '/'
            }
        })
}

export default logout;