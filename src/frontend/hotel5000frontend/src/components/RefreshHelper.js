import logout from "./LogoutHelper";
import {BaseUrl} from './FetchHelper'


export function refresh() {
        const data = {
            "refreshToken": sessionStorage.getItem('refreshToken')
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
                sessionStorage.setItem('accessToken', data.accessToken)
                sessionStorage.setItem('refreshToken', data.refreshToken)
                sessionStorage.setItem('loggedin', true)
                console.log(sessionStorage.getItem('accessToken'))
                console.log(sessionStorage.getItem('refreshToken'))
            })
            .catch((error) =>{
                logout().then(
                    console.log('Logged out due to invalid refresh token')
                )
                throw error
            }) //rossz refresh token esetén kijelentkeztetjük a felhasználót

}



