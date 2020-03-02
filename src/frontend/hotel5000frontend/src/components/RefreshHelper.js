export function refresh() {
        const data = {
            "refreshToken": localStorage.getItem('refreshToken')
        }

        fetch("https://localhost:5000/api/auth/refresh", {
            method: 'POST',
            mode: 'cors',
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        })
            .then(function (response) {
                console.log(response)
                if (response.status === 401) {
                    console.log('Bad refresh token')
                    throw new Error('refresh token invalid');
                }
                else
                    return response.json();
            })
            .then(data => {
                console.log(data)
                localStorage.setItem('accessToken', data.accessToken)
                localStorage.setItem('refreshToken', data.refreshToken)
                localStorage.setItem('loggedin', true)
                console.log(localStorage.getItem('accessToken'))
                console.log(localStorage.getItem('refreshToken'))
            })
            .catch(() =>{
                localStorage.setItem('loggedin', false)
            }) //rossz refresh token esetén kijelentkeztetjük a felhasználót

}



