import React, {Component} from 'react';
import {Button, Form} from "react-bootstrap";
import './Loginform.css';

class Loginform extends Component {

    constructor(){
        super();
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
            "username": username,
            "password": password
        };

        //console.log(this.state);
        fetch("https://localhost:5000/api/auth/login", {
            method: 'POST',
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        }).then(response => response.json())
        .then((responsedata) => {
            this.setState({ loggedin: true})
            localStorage.setItem('accessToken', responsedata.accessToken)
            localStorage.setItem('refreshToken', responsedata.refreshToken)
            console.log(localStorage.getItem('accessToken'))
            console.log(localStorage.getItem('refreshToken'))
            //console.log(this.state.data)
            //console.log(this.state.loggedin)
        })
        .catch(console.log)
    }

    test = () => {
        fetch("https://localhost:5000/api/default/testauthenticate", {
            method: 'GET',
            mode: 'cors',
            headers: {

                "Authorization" : "Bearer " + localStorage.getItem('accessToken')
            }
        })
            .then(function (response) {
                if (response.status === 401) {
                    let token = response.headers.get('token-expired');
                    if(token) {
                        console.log(token);
                        throw new Error("token needs to be refreshed") //itt ugrunk a catchbe, meghívjuk a refresht
                    }
                }
                else{
                    console.log("token not expired");
                    // adatfeldolgozás tovább
                }
            })
            .catch(this.refresh)
    }

    refresh = () => {
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
                console.log(localStorage.getItem('accessToken'))
                console.log(localStorage.getItem('refreshToken'))
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
                        <Form.Label>Username</Form.Label>
                        <Form.Control type="text" placeholder="Enter username" onChange={(text) => {this.handleUsername(text)}}/>
                    </Form.Group>
                    <Form.Group>
                        <Form.Label>Password</Form.Label>
                        <Form.Control type="password" placeholder="Password" onChange={(text) => {this.handlePassword(text)}}/>
                    </Form.Group>
                        <Button variant="outline-dark" block onClick={()=>{this.login(this.state.username, this.state.password)}}>Log In</Button>
                        <Button variant="outline-dark" block onClick={()=>{this.test()}}>Test auth</Button>
                </Form>
            </div>
        );
    }
}

export default Loginform;