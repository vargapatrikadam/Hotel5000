import logout from "./LogoutHelper";
import {BaseUrl} from './FetchHelper'


export function refresh() {
        const data = {
            "refreshToken": localStorage.getItem('refreshToken')
        }

        return fetch(BaseUrl + "api/auth/refresh", {
            method: 'POST',
            mode: 'cors',
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        })
            .then(function (response) {
                console.log(response)
                if (response.status === 200) {
                    return response.json();
                }
                else{
                    console.log('Bad refresh token')
                    throw new Error('refresh token invalid');
                }
            })
            .then(data => {
                console.log(data)
                localStorage.setItem('accessToken', data.accessToken)
                localStorage.setItem('refreshToken', data.refreshToken)
                localStorage.setItem('loggedin', true)
                console.log(localStorage.getItem('accessToken'))
                console.log(localStorage.getItem('refreshToken'))
            })
            .catch((error) =>{
                logout().then(
                    console.log('Logged out due to invalid refresh token')
                )
                throw error
            }) //rossz refresh token esetén kijelentkeztetjük a felhasználót

}



