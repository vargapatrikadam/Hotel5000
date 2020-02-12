import React, {Component} from 'react';
import {Button, Form} from "react-bootstrap";
import './Loginform.css';

class Loginform extends Component {

    constructor(){
        super();
        this.state={
            username:"",
            password:"",
            loggedin: false,
            data: []
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

        console.log(this.state);
        fetch("https://localhost:5000/api/auth/login", {
            method: 'POST',
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(data)
        }).then(response => response.json())
        .then((responsedata) => {
            this.setState({data: responsedata, loggedin: true})
            console.log(this.state.data)
            console.log(this.state.loggedin)
        })
        .catch(console.log)
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
                </Form>
            </div>
        );
    }
}

export default Loginform;