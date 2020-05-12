import React, {Component} from 'react';
import {Button, Form} from "react-bootstrap";
import './Loginform.css';
import {BaseUrl} from './FetchHelper'


class Loginform extends Component {

    constructor(props){
        super(props);
        this.state={
            username:"",
            password:"",
            loggedin: false
        }
    }

    handleUsername = (text) => {
        this.setState({username: text.target.value});
    }
    handlePassword = (passwd) => {
        this.setState({password: passwd.target.value});
    }

    login = (username, password) =>{
        const data = {
            "identifier": username,
            "password": password
        };

        //console.log(this.state);
        fetch(BaseUrl + "api/auth/login", {
            method: 'POST',
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        }).then(response =>
            response.json()
        )
        .then((responsedata) => {
            this.setState({ loggedin: true, role: responsedata.role })
            sessionStorage.setItem('role', responsedata.role)
            sessionStorage.setItem('username', responsedata.username)
            sessionStorage.setItem('accessToken', responsedata.accessToken)
            sessionStorage.setItem('refreshToken', responsedata.refreshToken)
            sessionStorage.setItem('email', responsedata.email)
            sessionStorage.setItem('loggedin', this.state.loggedin)
            alert("Successful login")
            window.location.href = '/'
            console.log(sessionStorage.getItem('accessToken'))
            console.log(sessionStorage.getItem('refreshToken'))
        })
        .catch(console.log)
    }

    test = () => {
        fetch(BaseUrl + "api/default/testauthenticate", {
            method: 'GET',
            mode: 'cors',
            headers: {

                "Authorization" : "Bearer " + sessionStorage.getItem('accessToken')
            }
        })
            .then(function (response) {
                if (response.status === 401) {
                    let token = response.headers.get('token-expired');
                    if(token) {
                        console.log(token);
                        this.refresh()
                    }
                }
                else{
                    console.log("token not expired");
                    // adatfeldolgozás tovább
                }
            })
            .catch()
    }

    refresh = () => {
        const data = {
            "refreshToken": sessionStorage.getItem('refreshToken')
        }

        fetch(BaseUrl + "api/auth/refresh", {
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
                sessionStorage.setItem('accessToken', data.accessToken)
                sessionStorage.setItem('refreshToken', data.refreshToken)
                console.log(sessionStorage.getItem('accessToken'))
                console.log(sessionStorage.getItem('refreshToken'))
            })
            .catch(() =>{
                this.setState({
                    loggedin: false
                })
            }) //rossz refresh token esetén kijelentkeztetjük a felhasználót
    }


    render() {
        return (
            <div className="formContainer">
                <Form>
                    <Form.Group controlId="formBasicEmail">
                        <Form.Label>Identifier</Form.Label>
                        <Form.Control type="text" placeholder="Enter identifier" onChange={(text) => {this.handleUsername(text)}}/>
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>Password</Form.Label>
                        <Form.Control type="password" placeholder="Password" onChange={(text) => {this.handlePassword(text)}}/>
                    </Form.Group>
                        <Button variant="outline-dark" block onClick={()=>{this.login(this.state.username, this.state.password)}}>Log In</Button>
                </Form>
            </div>
        );
    }
}

export default Loginform;